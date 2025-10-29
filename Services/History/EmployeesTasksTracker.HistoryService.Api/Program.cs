using EmployeesTasksTracker.HistoryService.Infrastructure.Extensions;
using EmployeesTasksTracker.HistoryService.Application.Extensions;
using MassTransit;
using EmployeesTasksTracker.HistoryService.Infrastructure.Consumers;

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
    config.AddConsumer<TaskDataChangedConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQHost"], h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("task-data-changed", e =>
        {
            e.ConfigureConsumer<TaskDataChangedConsumer>(context);
        });
    });
});

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
