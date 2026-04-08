using EmployeesTasksTracker.TasksTrackerService.Application.Extensions;
using EmployeesTasksTracker.TasksTrackerService.Application.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Application.Services;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Extensions;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.ReportGeneration;
using MassTransit;
using Shared.Extensions;
using Shared.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

builder.Services.AddMassTransit(config =>
{
    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQHost"], h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
    });
});

builder.Services.AddScoped<DbInitializer>();

builder.Services.AddScoped<ITaskReportService, TaskReportService>();
builder.Services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();
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

app.UseGlobalExceptionHandler();

app.MapControllers();

await app.Services.AddDatabaseInitialization();

app.Run();
