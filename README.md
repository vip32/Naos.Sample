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
- api gateway: https://localhost:5100/customers/api/values
- direct:  http://localhost:5002/api/values (debugging only)

#### Orders
- api gateway: https://localhost:5100/customers/api/values
- direct:  http://localhost:5006/api/values (debugging only)

```

                                    http:5002     http:5006
               .-------------------------|----------|-----------------. 
               | Docker host             |          |                 |
               |                         V          |                 |
               |                    .------------.  |                 |
               |         .----> http| Customers  |  |                 |
               |         |        80|  Service   |  |                 |
               |         |          "------------"  |                 |
            .----------. |                          |                 |
       https| Api      |-"                          |                 |
        5100| Gateway  |                            V                 |
        http| (ocelot) |-.                .------------.              |
        5000"----------" "----------> http| Customers  |              |
               |                        80|  Service   |              |
               |                          "------------"              |
               |                                                      |
               "------------------------------------------------------"

```	