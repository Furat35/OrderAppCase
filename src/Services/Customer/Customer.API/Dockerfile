#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Customer/Customer.API/Customer.API.csproj", "src/Services/Customer/Customer.API/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["src/Services/Customer/Customer.Business/Customer.Business.csproj", "src/Services/Customer/Customer.Business/"]
COPY ["src/Services/Customer/Customer.DataAccess/Customer.DataAccess.csproj", "src/Services/Customer/Customer.DataAccess/"]
COPY ["src/Services/Customer/Customer.Entity/Customer.Entity.csproj", "src/Services/Customer/Customer.Entity/"]
RUN dotnet restore "./src/Services/Customer/Customer.API/Customer.API.csproj"
COPY . .
WORKDIR "/src/src/Services/Customer/Customer.API"
RUN dotnet build "./Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Customer.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Customer.API.dll"]