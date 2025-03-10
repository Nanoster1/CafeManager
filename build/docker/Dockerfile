# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /source

COPY CafeManager.sln ./
COPY ./src ./src
RUN dotnet restore

WORKDIR /source/src/CafeManager.Server
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0

WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["dotnet", "CafeManager.Server.dll"]