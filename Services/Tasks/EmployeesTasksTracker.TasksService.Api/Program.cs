using EmployeesTasksTracker.TasksService.Infrastructure.Extensions;
using EmployeesTasksTracker.TasksService.Application.Extensions;
using EmployeesTasksTracker.TasksService.Application.Interfaces;
using EmployeesTasksTracker.TasksService.Infrastructure.Clients;
using EmployeesTasksTracker.TasksService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.TasksService.Application.Services;
using EmployeesTasksTracker.TasksService.Infrastructure.Interfaces;
using EmployeesTasksTracker.TasksService.Infrastructure.ReportGeneration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddHttpClient<IEmployeesClient, EmployeesClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServicesUrls:Employees"]);
});

builder.Services.AddHttpClient<ITasksGroupsClient, TasksGroupsClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServicesUrls:TasksGroups"]);
});

builder.Services.AddHttpClient<IProjectsClient, ProjectsClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServicesUrls:Projects"]);
});

builder.Services.AddScoped<TasksGenerator>();
builder.Services.AddScoped<DbInitializer>();
builder.Services.AddScoped<ITaskReportService, TaskReportService>();
builder.Services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();

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
