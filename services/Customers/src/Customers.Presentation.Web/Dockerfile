FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["services/Customers/src/Customers.Presentation.Web/Customers.Presentation.Web.csproj", "services/Customers/src/Customers.Presentation.Web/"]
RUN dotnet restore "services/Customers/src/Customers.Presentation.Web/Customers.Presentation.Web.csproj" -verbosity:normal
COPY . .
WORKDIR "/src/services/Customers/src/Customers.Presentation.Web"
RUN dotnet build "Customers.Presentation.Web.csproj" -c Release -o /app/build -verbosity:normal

FROM build AS publish
RUN dotnet publish "Customers.Presentation.Web.csproj" -c Release -o /app/publish -verbosity:normal

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Naos.Sample.Customers.Presentation.Web.dll"]