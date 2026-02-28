using FxWallet.Api;
using FxWallet.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLayers(builder.Configuration);

var app = builder.Build();

await app.Services.EnsureDatabaseCreatedAsync();

await app.RunAsync();