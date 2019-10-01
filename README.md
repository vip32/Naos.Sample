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
- https://blog.docker.com/2019/08/deploy-dockerized-apps-without-being-a-devops-guru/
- https://docs.microsoft.com/en-us/azure/virtual-machines/linux/docker-compose-quickstart
- https://buildazure.com/how-to-setup-an-ubuntu-linux-vm-in-azure-with-remote-desktop-rdp-access/
- https://azure.github.io/AppService/2018/06/27/How-to-use-Azure-Container-Registry-for-a-Multi-container-Web-App.html
- https://docs.microsoft.com/en-us/aspnet/core/security/docker-https?view=aspnetcore-2.2
- 
##### setup linux docker vm (azure console)
- `az account set --subscription [SUBSCRIPTIONID]`
- `az group create --name globaldocker --location westeurope`
- `az vm create --resource-group globaldocker --name globaldockervm --image UbuntuLTS --admin-username [USERNAME] --generate-ssh-keys --custom-data cloud-init.txt`
- `az vm open-port --port 80 --priority 900 --nsg-name globaldockervmNSG --resource-group globaldocker --name globaldockervm`
- `az vm open-port --port 443 --priority 901 --nsg-name globaldockervmNSG --resource-group globaldocker --name globaldockervm`
- `az vm open-port --port 6000 --priority 1100 --nsg-name globaldockervmNSG --resource-group globaldocker --name globaldockervm`
- `az vm open-port --port 6100 --priority 1101 --nsg-name globaldockervmNSG --resource-group globaldocker --name globaldockervm`
- `az vm open-port --port 9000 --priority 1102 --nsg-name globaldockervmNSG --resource-group globaldocker --name globaldockervm`

##### connect with ssh or azure vm serial console (terminal)
- `sudo apt install gnupg2 pass` # due to issue https://github.com/docker/cli/issues/1136
- `sudo apt install mc`
- `sudo apt install docker-compose`
- create docker-compose.yml FILE
```
version: '3.4'

services:
  apigateway.application.web:
    image: globaldockerregistry.azurecr.io/naos/apigateway.application.web
    ports:
      - 80:80
      - 433:433

  customers.application.web:
    image: globaldockerregistry.azurecr.io/naos/customers.application.web
    ports:
      - 6001:80

  orders.application.web:
    image: globaldockerregistry.azurecr.io/naos/orders.application.web
    ports:
      - 6002:80

#  web:
#    image: nginxdemos/hello
#    ports:
#      - 80:80
```

- `sudo docker-compose up -d`
- remote client: browse to [vmIP]:80 (nginx demo)

##### setup portainer (terminal) 
- `docker volume create portainer_data`
- `docker run -d -p 8000:8000 -p 9000:9000 -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer`
- remote client: browse to [vmIP]:9000 (portainer)

##### setup rdp (terminal)
- `sudo apt-get install lxde -y`
- `sudo apt-get install xrdp -y`
- `/etc/init.d/xrdp start`
- remote client: rdp into [vmIP]:3389

sudo docker login -u [USERNAME] -p [PASSWORD] globaldockerregistry.azurecr.io
sudo docker pull globaldockerregistry.azurecr.io/naos/orders.application.web
sudo docker rmi globaldockerregistry.azurecr.io/naos/orders.application.web
sudo docker-compose up -d