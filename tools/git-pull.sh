#!/bin/bash
REPOSITORIES=(Naos.Sample.ApiGateway Naos.Sample.Services.Availability Naos.Sample.Services.Customers Naos.Sample.Services.Deliveries Naos.Sample.Services.Identity Naos.Sample.Services.Operations Naos.Sample.Services.Orders Naos.Sample.Services.OrderMaker Naos.Sample.Services.Parcels Naos.Sample.Services.Pricing Naos.Sample.Services.Vehicles)

for REPOSITORY in ${REPOSITORIES[*]}
do
	 echo ========================================================
	 echo Updating the repository: $REPOSITORY
	 echo ========================================================
	 cd $REPOSITORY && git checkout develop && git pull && git checkout master && git pull && cd ..
done