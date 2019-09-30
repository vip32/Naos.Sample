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
 | I -|   6100 |    433 | Api      | |                          |              |
 | E -|---------------->| Gateway  |-`                          |              |
 | N -|   http |     80 |==========|                            V              |
 | T -|   6000 |        | (ocelot) |-.                .------------.           |
 | S -|        |        `----------`  `-------------->| Customers  |           |
 |    |        |                              http:80 |  Service   |           |
 `----`        |                                      `------------`           |
               |                                                               |
               `---------------------------------------------------------------`

```	

## Services

#### ApiGateway
- health: https://localhost:6100/health
    - https://customers.application.web/health (port 80)
    - https://orders.application.web/health (port 80)

#### Customers
- api gateway: https://localhost:6100/customers/api/values -> https://customers.application.web/api/values (port 80)
- local:  http://localhost:5002/api/values (debugging only)

#### Orders
- api gateway: https://localhost:6100/customers/api/values -> https://orders.application.web/api/values (port 80)
- local:  http://localhost:5006/api/values (debugging only)

## Docker

- docker build -t naos/naos.sample.services.customers .
- docker image ls
- docker run naos/naos.sample.services.customers

- docker network create naos-network
- docker-compose -f .\compose\infrastructure\docker.compose.yml up -d
- docker-compose -f .\docker.compose.yml -f .\docker.compose.override.yml build
- docker-compose -f .\docker.compose.yml -f .\docker.compose.override.yml up -d

### Docker VM
https://blog.docker.com/2019/08/deploy-dockerized-apps-without-being-a-devops-guru/
https://docs.microsoft.com/en-us/azure/virtual-machines/linux/docker-compose-quickstart

- `az account set --subscription [SUBSCRIPTIONID]`
- `az group create --name globaldocker --location westeurope`
- `az vm create --resource-group globaldocker --name globaldockervm --image UbuntuLTS --admin-username [USERNAME] --generate-ssh-keys --custom-data cloud-init.txt`
- `az vm open-port --port 80 --resource-group globaldocker --name globaldockervm`

connect with ssh or azure vm serial console
- `sudo apt install docker-compose`

- create docker-compose.yml
```
web:
  image: nginxdemos/hello
  ports:
    - 80:80

```

- `sudo docker-compose up -d`