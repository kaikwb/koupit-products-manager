FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["koupit-products-manager/koupit-products-manager.csproj", "koupit-products-manager/"]
RUN dotnet restore "koupit-products-manager/koupit-products-manager.csproj"
COPY . .
WORKDIR "/src/koupit-products-manager"
RUN dotnet build "koupit-products-manager.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "koupit-products-manager.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "koupit-products-manager.dll"]
