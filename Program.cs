using labWork.Logging;
using System.Text;

WebApplicationBuilder builder = WebApplication.CreateBuilder();
builder.Logging.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "log.txt"));

var app = builder.Build();
var logger = app.Logger;


app.Use(async (HttpContext context, RequestDelegate next) =>
{
    try
    {
        await next.Invoke(context);
    }
    catch (Exception ex)
    {
        var log = new StringBuilder();

        log.AppendLine($"\nTime:\t\t\t\t\t{DateTime.Now.ToString()}");
        log.AppendLine($"Request Path:\t\t\t{context.Request.Path}");
        log.AppendLine($"Request Method:\t\t\t{context.Request.Method}");

        if (context.Request.HasFormContentType && context.Request.Form.Any())
        {
            foreach (var form in context.Request.Form)
            {
                log.AppendLine($"Form Data: {form.Key} = {form.Value}");
            }
        }

        log.AppendLine($"{ex}");

        logger.LogError(message: log.ToString());

        throw;
    }
});

app.MapGet("/", async (context) =>
{
    var htmlFilePath = Path.Combine(Directory.GetCurrentDirectory(), "index.html");

    if (File.Exists(htmlFilePath))
    {
        await context.Response.WriteAsync(File.ReadAllText(htmlFilePath));
    }
    else
    {
        await context.Response.WriteAsync("<p>File's path is wrong.</p>");
        throw new Exception("File's path is wrong.");
    }
});

app.MapPost("/set-cookies", async context =>
{
    var value = context.Request.Form["value"];
    var expiration = context.Request.Form["expiration"];

    if (DateTime.Parse(expiration) < DateTime.Now)
    {
        await context.Response.WriteAsync(
            $"<p>Value \"{value}\" is expired.</p>" +
            $"<a href='/'>Try again.</a>");

        throw new Exception($"Value \"{value}\" is expired.");
    }

    if (DateTime.TryParse(expiration, out DateTime _expiration))
    {
        context.Response.Cookies.Append(
            "MyCookie",
            value,
            new CookieOptions
            {
                Expires = _expiration
            });

        await context.Response.WriteAsync(
            "<p>Value is in Cookies. "
            + "<a href='/check-cookies/'>Check Cookie</a></p>");
    }

    else
    {
        await context.Response.WriteAsync("<p>Error with expiration date." +
            "<a href='/'>Try again.</p>");
        throw new Exception("Error with expiration date");
    }
});

app.MapGet("/check-cookies", async context =>
{
    var cookie = context.Request.Cookies["MyCookie"];

    if (!string.IsNullOrEmpty(cookie))
    {
        await context.Response.WriteAsync($"<p>Value: {cookie}</p>");
    }
    else
    {
        await context.Response.WriteAsync("<p>There is no value in Cookies</p>");
        throw new Exception("There is no value in Cookies");
    }
});

app.Run();