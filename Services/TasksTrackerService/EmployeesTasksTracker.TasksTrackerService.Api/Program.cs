using Confluent.Kafka;
using EmployeesTasksTracker.TasksTrackerService.Api;
using EmployeesTasksTracker.TasksTrackerService.Application.Extensions;
using EmployeesTasksTracker.TasksTrackerService.Application.Interfaces;
using EmployeesTasksTracker.TasksTrackerService.Application.Services;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.DataSeeding;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.Extensions;
using EmployeesTasksTracker.TasksTrackerService.Infrastructure.ReportGeneration;
using MassTransit;
using Serilog;
using Shared.Extensions;
using Shared.Interfaces;
using Shared.InterfacesImplementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    Log.Information("Starting web host");

    builder.Services.AddControllers();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    builder.Services.AddInfrastructure(builder.Configuration);
    builder.Services.AddCaching(builder.Configuration, builder.Environment.IsDevelopment());
    builder.Services.AddObservability(builder.Configuration, "tasks-tracker-service");
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

    builder.Services.AddSingleton<IProducer<string, string>>(sp =>
    {
        var config = new ProducerConfig
        {
            BootstrapServers = builder.Configuration["Kafka:Host"]
        };

        return new ProducerBuilder<string, string>(config).Build();
    });

    builder.Services.AddSingleton<IKafkaProducer, KafkaProducer>();

    builder.Services.AddScoped<ProjectsGenerator>();
    builder.Services.AddScoped<TasksGenerator>();
    builder.Services.AddScoped<DbInitializer>();

    builder.Services.AddScoped<ITasksGroupReportService, TasksGroupReportService>();
    builder.Services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();
    builder.Services.AddScoped<ITaskReportService, TaskReportService>();
    builder.Services.AddScoped<IPdfReportGenerator, PdfReportGenerator>();

    builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
    builder.Services.AddProblemDetails();

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

    app.UseExceptionHandler();

    app.MapControllers();

    await app.Services.AddDatabaseInitialization();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host has terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
