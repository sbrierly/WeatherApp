using System.CommandLine;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using WeatherApp.Cli;
using WeatherApp.Cli.Enums;
using WeatherApp.Domain.Enums;
using WeatherApp.Services.DTOs;

var weatherApiConfig = GetWeatherApiConfig();
var provider = GetServiceProvider();
var logger = provider.GetRequiredService<ILogger<Program>>();
var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
var client = httpClientFactory.CreateClient("WeatherApi");

var zipcodeArg = new Argument<string>("zipcode") { Description = "Zip code to fetch weather for" };
var unitsArg = new Argument<TemperatureUnit>("units") { Description = "Temperature units (fahrenheit or celsius)" };
var daysArg = new Argument<int>("days") { Description = "Number of days for average weather (2-5)" };
var outputOption = new Option<OutputFormat>("--output") { Description = "Output format (json, yaml, text)" };

var getCurrentWeather = new Command("get-current-weather", "Get current weather for a zip code")
{
    zipcodeArg,
    unitsArg,
    outputOption
};

getCurrentWeather.SetAction(async parseResult =>
{
    var zipcode = parseResult.GetRequiredValue(zipcodeArg);
    var units = parseResult.GetRequiredValue(unitsArg);
    var output = parseResult.GetValue(outputOption);

    var url = $"{weatherApiConfig?.BaseUrl}Weather/Current/{zipcode}?units={units}";
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var formattedOutput = FormatHelper.GetFormattedOutput<GetWeatherCurrentResponse>(zipcode, content, output);
    Console.Out.WriteLine(formattedOutput);

    return ExitCode.Success;
});

var getAverageWeather = new Command("get-average-weather", "Get average weather for a zip code over a 2-5 day period")
{
    zipcodeArg,
    unitsArg,
    daysArg,
    outputOption
};

getAverageWeather.SetAction(async parseResult =>
{
    var zipcode = parseResult.GetRequiredValue(zipcodeArg);
    var units = parseResult.GetRequiredValue(unitsArg);
    var days = parseResult.GetRequiredValue(daysArg);
    var output = parseResult.GetValue(outputOption);

    var url = $"{weatherApiConfig?.BaseUrl}Weather/Average/{zipcode}?timePeriod={days}";
    var response = await client.GetAsync(url);
    response.EnsureSuccessStatusCode();

    var content = await response.Content.ReadAsStringAsync();
    var formattedOutput = FormatHelper.GetFormattedOutput<GetWeatherAverageResponse>(zipcode, content, output);
    Console.Out.WriteLine(formattedOutput);

    return ExitCode.Success;
});

var rootCommand = new RootCommand("WeatherApp CLI tool for fetching weather data")
{
    getCurrentWeather,
    getAverageWeather
};

ParseResult parseResult = rootCommand.Parse(args);

return parseResult.Invoke();



static WeatherApiConfig? GetWeatherApiConfig()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddEnvironmentVariables(); // override via env vars

    var configuration = builder.Build();
    return configuration.GetSection("WeatherApi").Get<WeatherApiConfig>();
}

static ServiceProvider GetServiceProvider()
{
    var services = new ServiceCollection();

    services.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.SetMinimumLevel(LogLevel.Warning);
    });

    services.AddHttpClient();

    var provider = services.BuildServiceProvider();
    return provider;
}
