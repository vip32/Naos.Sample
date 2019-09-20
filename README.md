# Naos.Sample
A mildly opiniated modern cloud service architecture blueprint + reference implementation

docker build -t naos/naos.sample.services.customers .
docker image ls
docker run naos/naos.sample.services.customers

docker network create naos-network
docker-compose -f .\services.yml -f .\services-local.yml up -d
