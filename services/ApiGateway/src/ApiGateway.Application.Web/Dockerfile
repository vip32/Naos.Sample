FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["services/ApiGateway/src/ApiGateway.Application.Web/ApiGateway.Application.Web.csproj", "services/ApiGateway/src/ApiGateway.Application.Web/"]
RUN dotnet restore "services/ApiGateway/src/ApiGateway.Application.Web/ApiGateway.Application.Web.csproj"
COPY . .
WORKDIR "/src/services/ApiGateway/src/ApiGateway.Application.Web"
RUN dotnet build "ApiGateway.Application.Web.csproj" -c Release -o /app/build --no-restore -verbosity:normal

FROM build AS publish
RUN dotnet publish "ApiGateway.Application.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Naos.Sample.ApiGateway.Application.Web.dll"]