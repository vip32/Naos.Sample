# Naos.Sample

A mildly opiniated modern cloud service architecture blueprint + reference implementation

### Infrastructure
```


                                             http:5002  http:5006
               +==============+-------------------|-------------|--------------. 
               | DOCKER HOST  |                   |             |              |
               +==============+                   V             |              |
 .----.        |                                .------------.  |              |
 |    |        |                     .--------->| Customers  |  |              |
 | C -|        |                     |  http:80 |  Service   |  |              |
 | L -|  https |        .----------. |          `------------`  |              |
 | I -|   5100 |    433 | Api      | |                          |              |
 | E -|---------------->| Gateway  |-`                          |              |
 | N -|   http |     80 |==========|                            V              |
 | T -|   5000 |        | (ocelot) |-.                .------------.           |
 | S -|        |        `----------`  `-------------->| Customers  |           |
 |    |        |                              http:80 |  Service   |           |
 `----`        |                                      `------------`           |
               |                                                               |
               `---------------------------------------------------------------`

```	

## Services

#### ApiGateway
- health: https://localhost:5100/health
    - https://customers.application.web/health (port 80)
    - https://orders.application.web/health (port 80)

#### Customers
- api gateway: https://localhost:5100/customers/api/values -> https://customers.application.web/api/values (port 80)
- local:  http://localhost:5002/api/values (debugging only)

#### Orders
- api gateway: https://localhost:5100/customers/api/values -> https://orders.application.web/api/values (port 80)
- local:  http://localhost:5006/api/values (debugging only)

## Docker

- docker build -t naos/naos.sample.services.customers .
- docker image ls
- docker run naos/naos.sample.services.customers

- docker network create naos-network
- docker-compose -f .\compose\infrastructure\docker.compose.yml up -d
- docker-compose -f .\docker.compose.yml -f .\docker.compose.override.yml build
- docker-compose -f .\docker.compose.yml -f .\docker.compose.override.yml up -d
