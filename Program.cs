using labWork.Models;

var builder = WebApplication.CreateBuilder();

builder.Services.AddTransient<Book>();
builder.Services.AddTransient<User>();

builder.Configuration.
    AddJsonFile("Configuration/Books.json").
    AddJsonFile("Configuration/Users.json");

var app = builder.Build();

app.Map("/Library", () => "Greetings!");

app.Map("/Library/Books", (HttpContext context, IConfiguration config) =>
{
    context.Response.ContentType = "text/html; charset=utf-8";
    var stringBuilder = new System.Text.StringBuilder("<table style=\"width:450px;\">");

    var books = config.GetSection("library:books").Get<Book[]>();

    stringBuilder.Append("" +
        "<tr>" +
            "<th>Title</th>" +
            "<th>Author</th>" +
            "<th>Date Of Publishing</th>" +
        "</tr>");

    foreach (var item in books)
    {
        stringBuilder.Append("" +
            "<tr>" +
                $"<td>{item.Title}</td>" +
                $"<td>{item.Author}</td>" +
                $"<td>{item.DateOfRelease}</td>" +
            "</tr>");
    }

    stringBuilder.Append("</table>");

    return $"{stringBuilder}";
});

app.Map("/Library/Profile/{id:range(0,5):int?}", (int? id, IConfiguration config) =>
{
    if (!id.HasValue)
    {
        return $"There is no user, you did not add user id.";
    }

    var users = config.GetSection("profile:users").Get<User[]>();
    var user = users.FirstOrDefault(user => user.Id == id);

    return $"User is {user.Name}";
});

app.Run();