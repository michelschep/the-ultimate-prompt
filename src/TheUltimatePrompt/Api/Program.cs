var builder = WebApplication.CreateBuilder(args);

// The following line enables Application Insights telemetry collection.
// We need to add configuration here to be sure the settings from the secrets file are used.
// Somehow without passing in the configuration it only works when the connection string is in appsettings.
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cors is needed for Blazor client to call the api
builder.Services.AddCors(policy =>
{
    policy.AddPolicy("CorsPolicy", opt => opt
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

var app = builder.Build();
// Need to use CORS for Blazor client.
// NOTE does order matter for the Use pipeline?
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
await app.RunAsync();

// To be able to create integration tests:
#pragma warning disable S1118
// ReSharper disable once ClassNeverInstantiated.Global
public partial class Program;
#pragma warning restore S1118
