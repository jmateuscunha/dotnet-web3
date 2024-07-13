FROM mcr.microsoft.com/dotnet/sdk:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./presentation.api/application.api.csproj", "./presentation.api/"]
RUN dotnet restore "./presentation.api/application.api.csproj"

COPY . .
WORKDIR "./presentation.api"
RUN dotnet build "application.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "application.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "application.api.dll"]
