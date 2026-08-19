FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/SeniorDeveloperTest.Api/SeniorDeveloperTest.Api.csproj", "src/SeniorDeveloperTest.Api/"]
COPY ["src/SeniorDeveloperTest.Application/SeniorDeveloperTest.Application.csproj", "src/SeniorDeveloperTest.Application/"]
COPY ["src/SeniorDeveloperTest.Domain/SeniorDeveloperTest.Domain.csproj", "src/SeniorDeveloperTest.Domain/"]
COPY ["src/SeniorDeveloperTest.Infrastructure/SeniorDeveloperTest.Infrastructure.csproj", "src/SeniorDeveloperTest.Infrastructure/"]

RUN dotnet restore "src/SeniorDeveloperTest.Api/SeniorDeveloperTest.Api.csproj"

COPY . .

WORKDIR "/src/src/SeniorDeveloperTest.Api"

RUN dotnet publish "SeniorDeveloperTest.Api.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "SeniorDeveloperTest.Api.dll"]