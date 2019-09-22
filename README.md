# Naos.Sample

A mildly opiniated modern cloud service architecture blueprint + reference implementation

## Docker

- docker build -t naos/naos.sample.services.customers .
- docker image ls
- docker run naos/naos.sample.services.customers

- docker network create naos-network
- docker-compose -f .\infrastructure.yml up -d
- docker-compose -f .\services.yml -f .\services-local.yml build
- docker-compose -f .\services.yml -f .\services-local.yml up -d

## Services

#### Customers

http://localhost:5002/api/values

#### Orders

http://localhost:5006/api/values
