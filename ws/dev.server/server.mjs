const ws = require('aws-lambda-ws-server');
const axios = require('axios');

exports.handler = ws(
  ws.handler({
    async connect ({ id }) {
      console.log('connection %s', id)
      return { statusCode: 200 }
    },
    async disconnect ({ id }) {
      console.log('disconnect %s', id)
      return { statusCode: 200 }
    },
    async default ({ message, id }) {
      // console.log('default message', message, id)
      // return { statusCode: 200 }
      var response = await axios.post('http://localhost:8080/api/messages', message);
      console.log(response)
      return { statusCode: response.status }
    },
    async message ({ message, id, context }) {
      var response = await axios.post('http://localhost:8080/api/messages', message);
      console.log(response)
      return { statusCode: response.status }
    }
  })
)