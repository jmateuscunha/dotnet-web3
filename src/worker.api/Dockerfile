FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./worker.api/worker.api.csproj", "./worker.api/"]
RUN dotnet restore "./worker.api/worker.api.csproj"

COPY . .
WORKDIR "./worker.api"
RUN dotnet build "worker.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "worker.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "worker.api.dll"]
