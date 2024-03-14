#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Services/Ordering/Ordering.Logger/Ordering.Logger.csproj", "src/Services/Ordering/Ordering.Logger/"]
COPY ["BuildingBlocks/Shared/Shared.csproj", "BuildingBlocks/Shared/"]
COPY ["src/Services/Ordering/Infrastructure/Ordering.Persistence/Ordering.Persistence.csproj", "src/Services/Ordering/Infrastructure/Ordering.Persistence/"]
COPY ["src/Services/Ordering/Core/Ordering.Application/Ordering.Application.csproj", "src/Services/Ordering/Core/Ordering.Application/"]
COPY ["BuildingBlocks/EventBus.Message/EventBus.Message.csproj", "BuildingBlocks/EventBus.Message/"]
COPY ["src/Services/Ordering/Core/Ordering.Domain/Ordering.Domain.csproj", "src/Services/Ordering/Core/Ordering.Domain/"]
RUN dotnet restore "./src/Services/Ordering/Ordering.Logger/Ordering.Logger.csproj"
COPY . .
WORKDIR "/src/src/Services/Ordering/Ordering.Logger"
RUN dotnet build "./Ordering.Logger.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ordering.Logger.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Logger.dll"]