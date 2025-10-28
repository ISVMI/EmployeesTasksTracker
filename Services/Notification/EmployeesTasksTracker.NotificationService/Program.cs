using EmployeesTasksTracker.NotificationService.Consumers;
using EmployeesTasksTracker.NotificationService.Hubs;
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSignalR();
builder.Services.AddMassTransit(config => {

    config.AddConsumer<TaskStatusChangedConsumer>();
    config.AddConsumer<EmployeeAssignedConsumer>();

    config.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQHost"], h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint("task-status-changed", e =>
        {
            e.ConfigureConsumer<TaskStatusChangedConsumer>(context);
        });

        cfg.ReceiveEndpoint("employee-assigned", e =>
        {
            e.ConfigureConsumer<EmployeeAssignedConsumer>(context);
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

app.MapHub<NotificationHub>("/notifications");

app.Run();
