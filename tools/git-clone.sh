#!/bin/bash
REPOSITORIES=(Naos.Sample.ApiGateway Naos.Sample.Services.Availability Naos.Sample.Services.Customers Naos.Sample.Services.Deliveries Naos.Sample.Services.Identity Naos.Sample.Services.Operations Naos.Sample.Services.Orders Naos.Sample.Services.OrderMaker Naos.Sample.Services.Parcels Naos.Sample.Services.Pricing Naos.Sample.Services.Vehicles)

for REPOSITORY in ${REPOSITORIES[*]}
do
	 echo ========================================================
	 echo Cloning the repository: $REPOSITORY
	 echo ========================================================
	 REPO_URL=https://github.com/vip32/$REPOSITORY.git
	 git clone $REPO_URL .\services
	 cd $REPOSITORY && cd ..
done