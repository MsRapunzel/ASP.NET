using ASP.NET;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Company company = new Company { Name = "Apple Inc.", HeadquartersAddress = "1 Apple Park Way, Cupertino, California, U.S.", FoundedYear = 1976 };

app.MapGet("/task1", () =>
{
    return $"Company name: \"{company.Name}\"," +
    $" its headquarters address: \"{company.HeadquartersAddress}\"," +
    $" and its foundation year: \"{company.FoundedYear}\".";
});

app.MapGet("/task2", () =>
{
    return $"Random number from 1 to 100 is: {new Random().Next(0, 101)}";
});

app.Use(async (context, next) =>
{
    var token = context.Request.Query["token"];
    if (token != "12345678")
    {
        context.Response.StatusCode = 403;
        await context.Response.WriteAsync("Token is invalid");
    }
    else
    {
        await next.Invoke(context);
    }

});

app.Run();
