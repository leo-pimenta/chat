# start localstack
docker-compose up -d localstack

# start kafka
docker-compose up -d zookeeper
docker-compose up -d kafka

# start ws
docker-compose up -d --build ws-server

# start ws client
docker-compose up -d --build ws-api

# start reader
