using Microsoft.EntityFrameworkCore;
using SEAA.Astrodex.Core.Interfaces;
using SEAA.Astrodex.Data.Context;
using SEAA.Astrodex.Data.Repositories;
using SEAA.Astrodex.Infrastructure.External;
using SEAA.Astrodex.Infrastructure.Formatters;
using SEAA.Astrodex.Infrastructure.Services;
using SEAA.Astrodex.Infrastructure.Structures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition =
            System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<SolarSystemApiService>(client =>
{
    client.BaseAddress = new Uri("https://api.le-systeme-solaire.net/");
    client.DefaultRequestHeaders.Authorization =
        new System.Net.Http.Headers.AuthenticationHeaderValue(
            "Bearer",
            "e0df2949-d0ae-45d3-8f3d-2f0a39149033"
        );
});

builder.Services.AddDbContext<AstronomiaContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Estructuras de memoria como Singleton
builder.Services.AddSingleton<MemoriaService>();
builder.Services.AddSingleton<HistorialMemoriaService>();
builder.Services.AddSingleton<CachePaginas>();

// Repository
builder.Services.AddScoped<ICuerpoCelesteRepository, CuerpoCelesteRepository>();

// Services
builder.Services.AddScoped<ICuerpoCelesteService, CuerpoCelesteService>();
builder.Services.AddScoped<ICaracteristicasFisicasFormatter,
    CaracteristicasFisicasFormatter>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider
        .GetRequiredService<AstronomiaContext>();
    context.Database.EnsureCreated();
}

app.Run();