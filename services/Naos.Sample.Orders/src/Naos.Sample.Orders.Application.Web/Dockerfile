FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Naos.Sample.Orders/src/Naos.Sample.Orders.Application.Web/Naos.Sample.Orders.Application.Web.csproj", "Naos.Sample.Orders/src/Naos.Sample.Orders.Application.Web/"]
RUN dotnet restore "Naos.Sample.Orders/src/Naos.Sample.Orders.Application.Web/Naos.Sample.Orders.Application.Web.csproj"
COPY . .
WORKDIR "/src/Naos.Sample.Orders/src/Naos.Sample.Orders.Application.Web"
RUN dotnet build "Naos.Sample.Orders.Application.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Naos.Sample.Orders.Application.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Naos.Sample.Orders.Application.Web.dll"]
