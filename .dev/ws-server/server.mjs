const ws = require('aws-lambda-ws-server');
const axios = require('axios');

exports.handler = ws(
  ws.handler({
    async connect ({ id, event, context }) {
      const user_id_header = event.multiValueHeaders["user_id"];

      if (!user_id_header || user_id_header.length < 1) {
        return { statusCode: 401 };
      }

      var user_id = user_id_header[0];

      var response = await axios.post(`http://localhost:8080/api/ws/${id}`, {}, {
        headers: {
          user_id: user_id
        }
      });
      
      console.log(response)

      return { statusCode: response.status }
    },
    async disconnect ({ id }) {
      var response = await axios.delete(`http://localhost:8080/api/ws/${id}`);
      console.log(response);
      return { statusCode: 200 }
    },
    async default ({ message, id }) {
      var response = await axios.post('http://localhost:8080/api/messages', message);
      console.log(response)
      return { statusCode: response.status }
    },
  })
)