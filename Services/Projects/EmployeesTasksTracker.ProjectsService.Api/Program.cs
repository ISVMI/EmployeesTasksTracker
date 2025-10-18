using EmployeesTasksTracker.ProjectsService.Application.Extensions;
using EmployeesTasksTracker.ProjectsService.Application.Interfaces;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Clients;
using EmployeesTasksTracker.ProjectsService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.ProjectsService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();


builder.Services.AddScoped<IEmployeesClient, EmployeesClient>();
builder.Services.AddScoped<ProjectsGenerator>();
builder.Services.AddScoped<DbInitializer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.AddDatabaseInitialization();

app.Run();
