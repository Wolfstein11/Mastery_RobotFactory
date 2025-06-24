// Ruta: TuFabricaDDD.API/Program.cs
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TuFabricaDDD.Application;
using TuFabricaDDD.Contracts.Repositories;
using TuFabricaDDD.GrpcContracts.Units; // Asegúrate de que este using esté para UnitService
using TuFabricaDDD.Persistence;
using TuFabricaDDD.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// =========================================================
// 1. Configuración de la base de datos (DbContext)
// =========================================================
var connectionString = builder.Configuration.GetConnectionString("TuFabricaDb") ??
                       throw new InvalidOperationException("Connection string 'TuFabricaDb' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

// =========================================================
// 2. Registro de Repositorios y Unidad de Trabajo
// =========================================================
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// =========================================================
// 3. Configuración de MediatR (Patrón Mediador y CQRS)
// =========================================================
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssemblies(
        typeof(TuFabricaDDD.Application.AssemblyReference).Assembly,
        Assembly.GetExecutingAssembly()
    );
});

// =========================================================
// 4. Configuración de AutoMapper
// =========================================================
builder.Services.AddAutoMapper(
    typeof(TuFabricaDDD.Application.AssemblyReference).Assembly,
    Assembly.GetExecutingAssembly()
);

// =========================================================
// 5. Configuración de los servicios gRPC
// =========================================================
builder.Services.AddGrpc();

var app = builder.Build();

// Configure the HTTP request pipeline.
// Esta línea seguirá dando error hasta que creemos UnitGrpcService
app.MapGrpcService<TuFabricaDDD.API.Services.UnitGrpcService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();