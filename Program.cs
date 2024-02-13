using labWork;
using labWork.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<CalcController>();
builder.Services.AddTransient<TimeController>();

var app = builder.Build();

app.MapGet("/task1", async (context) =>
{
    var calcController = app.Services.GetService<CalcController>();

    var a = float.Parse(context.Request.Query["a"]);
    var b = float.Parse(context.Request.Query["b"]);
    var operation = context.Request.Query["operation"].ToString();

    var answer = "None";

    switch (operation)
    {
        case "add":
            answer = $"{a} + {b} = {calcController?.Add(a, b)}";
            break;
        case "substract":
            answer = $"{a} - {b} = {calcController?.Substract(a, b)}";
            break;
        case "multiply":
            answer = $"{a} * {b} = {calcController?.Multiply(a, b)}";
            break;
        case "divide":
            try
            {
                answer = $"{a} / {b} = {calcController?.Divide(a, b)}";
            }
            catch (Exception ex)
            {
                answer = ex.Message;
            }
            break;
        default:
            break;
    }

    await context.Response.WriteAsync(answer);
});

app.MapGet("/task2", async (context) =>
{
    var timeController = app.Services.GetService<TimeController>();

    var time = timeController?.GetTime().ToShortTimeString();
    var hour = timeController?.GetTime().Hour;

    var message = $"it's {time} o'clock now.";

    switch (hour)
    {
        case int t when (hour >= 12 && hour <= 17):
            message = $"Good day, " + message;
            break;
        case int t when (hour >= 18 && hour <= 23):
            message = $"Good evening, " + message;
            break;
        case int t when (hour >= 00 && hour <= 5):
            message = $"Good night, " + message;
            break;
        case int t when (hour >= 6 && hour <= 11):
            message = $"Good morning, " + message;
            break;
        default:
            break;
    }

    await context.Response.WriteAsync(message);
});

app.Run();