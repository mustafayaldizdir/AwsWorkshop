#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/AwsWorkshop.Administration.Api/AwsWorkshop.Administration.Api.csproj", "Presentation/AwsWorkshop.Administration.Api/"]
COPY ["Infrastructure/AwsWorkshop.Persistence/AwsWorkshop.Persistence.csproj", "Infrastructure/AwsWorkshop.Persistence/"]
COPY ["Core/AwsWorkshop.Application/AwsWorkshop.Application.csproj", "Core/AwsWorkshop.Application/"]
COPY ["Core/AwsWorkshop.Domain/AwsWorkshop.Domain.csproj", "Core/AwsWorkshop.Domain/"]
RUN dotnet restore "./Presentation/AwsWorkshop.Administration.Api/AwsWorkshop.Administration.Api.csproj"
COPY . .
WORKDIR "/src/Presentation/AwsWorkshop.Administration.Api"
RUN dotnet build "./AwsWorkshop.Administration.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AwsWorkshop.Administration.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AwsWorkshop.Administration.Api.dll"]