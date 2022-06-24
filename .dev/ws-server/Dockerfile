FROM node:16
WORKDIR /usr/src/app

COPY package*.json ./
COPY server.js ./

RUN npm i
EXPOSE 5000

CMD ["node", "server.js"]