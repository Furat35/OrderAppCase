using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json", true, true);

builder.Services
    .AddOcelot()
    .AddCacheManager(x => x.WithDictionaryHandle());

var app = builder.Build();

await app.UseOcelot();

app.Run();
