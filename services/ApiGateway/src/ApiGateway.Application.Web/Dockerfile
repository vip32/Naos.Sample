FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["services/Naos.Sample.ApiGateway/src/Naos.Sample.ApiGateway.Application.Web/Naos.Sample.ApiGateway.Application.Web.csproj", "services/Naos.Sample.ApiGateway/src/Naos.Sample.ApiGateway.Application.Web/"]
RUN dotnet restore "services/Naos.Sample.ApiGateway/src/Naos.Sample.ApiGateway.Application.Web/Naos.Sample.ApiGateway.Application.Web.csproj"
COPY . .
WORKDIR "/src/services/Naos.Sample.ApiGateway/src/Naos.Sample.ApiGateway.Application.Web"
RUN dotnet build "Naos.Sample.ApiGateway.Application.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Naos.Sample.ApiGateway.Application.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Naos.Sample.ApiGateway.Application.Web.dll"]