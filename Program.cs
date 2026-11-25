using BrewBench.Components;
using BrewBench.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<IngredientCatalog>();
builder.Services.AddSingleton<StyleCatalog>();
builder.Services.AddSingleton<RecipeStore>();
builder.Services.AddSingleton<BrewSession>();

await builder.Build().RunAsync();
