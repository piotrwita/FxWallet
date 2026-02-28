using FxWallet.Api;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

builder.Services.AddLayers(builder.Configuration);

await app.RunAsync();