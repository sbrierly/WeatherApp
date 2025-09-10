# WeatherApp

WeatherApp is a .NET 9.0 project that includes a **CLI tool** and a **Web API** for fetching weather data.

- [Requirements](#requirements)
- [Auth](#auth)
  - [Setting environment variable](#setting-environment-variable)
- [Build Instructions](#build-instructions)
  - [Build C# Project](#build-c-project)
  - [Build Docker Image](#build-docker-image)
- [Publish Instructions](#publish-instructions)
- [Running Tests](#running-tests)
- [Running the Web API](#running-the-web-api)
- [Launching Docker Container](#launching-docker-container)
- [Accessing the Swagger Documentation](#accessing-the-swagger-documentation)
- [WeatherApp CLI](#weatherapp-cli)
  - [Examples](#examples)
- [TODO](#todo)
- [Data Source](#data-source)


## Requirements

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)  
- [Docker](https://www.docker.com/) (optional, for containerized runs)  
- Internet connection for weather API access
- OpenWeather Free API key - https://home.openweathermap.org/users/sign_up

## Auth

This project requires an OpenWeather Free API key for calls to the geolocation and weather services.

https://home.openweathermap.org/users/sign_up

### Setting environment variable

The environment variable 'OPENWEATHER_API_KEY' must be set.

* For vscode debugging and running via `dotnet` command, it can be set in `src/WeatherApp.Api/Properties/launchSettings.json`
* For docker, it can be passed with the `run` command by replacing `<REPLACE_ME>`
```bash
docker run -e OPENWEATHER_API_KEY=<REPLACE_ME> -p 5000:5000 --rm weatherapp:latest
```

## Build Instructions

### Build C# Project

From the repo root:

```bash
dotnet build
```

### Build Docker Image

```bash
docker build -t weatherapp:latest .
```

## Publish Instructions

Publish the Web API or CLI app for deployment:

```bash
dotnet publish -c Release
```

## Running Tests

```bash
dotnet test
```

## Running the Web API

By default, the API runs on port 5000:

```bash
dotnet run --project src/WeatherApp.Api
```

Or, if published:

```
dotnet src/WeatherApp.Api/bin/Release/net9.0/WeatherApp.Api.dll
```

## Launching Docker Container

Run the container and expose port 5000:

```bash
docker run -e OPENWEATHER_API_KEY=<REPLACE_ME> --rm -p 5000:5000 weatherapp:latest
```

* ensure you replace <REPLACE_ME> with your OPENWEATHER_API_KEY.
* --rm ensures the container is removed when stopped.
* Port 5000 on host maps to port 5000 in container.

## Accessing the Swagger Documentation

Once the Web API is running, open: http://localhost:5000/swagger

## WeatherApp CLI

Note, WeatherApp.Api must be running as the CLI makes requests to the WeatherApp.Api.

```bash
Description:
  WeatherApp CLI tool for fetching weather data

Usage:
  WeatherApp.Cli [command] [options]

Options:
  -?, -h, --help  Show help and usage information
  --version       Show version information

Commands:
  get-current-weather <zipcode> <Celsius|Fahrenheit>         Get current weather for a zip code
  get-average-weather <zipcode> <Celsius|Fahrenheit> <days>  Get average weather for a zip code over a 2-5 day period
```

### Examples

```bash
WeatherApp.Cli get-current-weather 01440 Fahrenheit --output JSON
WeatherApp.Cli get-average-weather 01440 Celsius 3 --output TEXT
```

## TODO

1. CLI validation
2. CLI error handling
3. Enable OAuth
4. Add additional unit tests
5. Add end to end integration tests
6. Shared secret store for API key
7. Review and update docstrings

## Data Source

This project uses weather data provided by [OpenWeather](https://openweathermap.org/). 
Data is fetched via the OpenWeather Free API. All weather data is © OpenWeather.
