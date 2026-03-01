using FxWallet.Api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLayers(builder.Configuration);

var app = builder.Build();

await app.UseInfrastructure();

await app.RunAsync();