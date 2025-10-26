using EmployeesTasksTracker.TasksGroupsService.Application.Extensions;
using EmployeesTasksTracker.TasksGroupsService.Application.Interfaces;
using EmployeesTasksTracker.TasksGroupsService.Application.Services;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Clients;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.Extensions;
using EmployeesTasksTracker.TasksGroupsService.Infrastructure.ReportGeneration;
using Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<ITasksClient, TasksClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServicesUrls:Tasks"]);
});

builder.Services.AddHttpClient<IProjectsClient, ProjectsClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServicesUrls:Projects"]);
});

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddScoped<ITasksGroupReportService, TasksGroupReportService>();
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
