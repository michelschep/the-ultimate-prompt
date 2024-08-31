using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Web;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseAddress = builder.Configuration["ApiBaseAddress"];
if (baseAddress == null)
{
    throw new ArgumentException("Api address not found in appsettings.json");
}
builder.Services.AddScoped(_ => new HttpClient
{
    BaseAddress = new Uri(baseAddress)
});

await builder.Build().RunAsync();