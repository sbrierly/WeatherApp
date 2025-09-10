FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src
COPY src ./src

RUN dotnet publish ./src/WeatherApp.Api/WeatherApp.Api.csproj -c Release -o /app/publish /p:UseAppHost=false


FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

ENTRYPOINT ["dotnet", "WeatherApp.Api.dll"]
