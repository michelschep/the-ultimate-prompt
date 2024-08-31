using System.Net.Http.Json;
using Api.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using XUnit.Extensions.IntegrationTests;

namespace IntegrationTests;

public class IntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public IntegrationTest(ITestOutputHelper testOutputHelper, CustomWebApplicationFactory<Program> factory)
    {
        _client = factory
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureLogging(loggingBuilder =>
                {
                    loggingBuilder.ClearProviders();
                    loggingBuilder.SetMinimumLevel(LogLevel.Information);
                    loggingBuilder.Services.AddSingleton<ILoggerProvider>(new XUnitLoggerProvider(testOutputHelper));
                });

                builder.ConfigureAppConfiguration(configurationBuilder =>
                {
                    var integrationConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Integrationtest.json").Build();
                    configurationBuilder.AddConfiguration(integrationConfig);
                });
                
            })
            .CreateClient(new WebApplicationFactoryClientOptions 
            {
                AllowAutoRedirect = false
            });
    }
    
    [Fact]
    public async Task GetWeatherForecast()
    {
        var result = await _client.GetFromJsonAsync<IList<WeatherForecast>>("WeatherForecast");

        result.Should().NotBeNull();
    }
}