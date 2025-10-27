using Microsoft.AspNetCore.SignalR.Client;
using System.Text.Json;

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5240/notifications")
    .Build();

connection.On<object>("ReceiveNotification", msg =>
{
    Console.WriteLine("Notification recieved: " + JsonSerializer.Serialize(msg));
});

await connection.StartAsync();

Console.WriteLine("Connected to notification hub");

Console.ReadLine();
