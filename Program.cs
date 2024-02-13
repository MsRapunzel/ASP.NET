using ASP.NET;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

builder.Configuration.
    AddIniFile("Configuration/ConfigGoogle.ini").
    AddJsonFile("Configuration/ConfigApple.json").
    AddXmlFile("Configuration/ConfigMicrosoft.xml").
    AddJsonFile("Configuration/PersonalInfo.json");

app.Map("/task1", (IConfiguration config) =>
{
    var companies = new List<Company>();

    foreach (var company in config.GetSection("Company").GetChildren())
    {
        var name = company.Key;
        var amount = int.Parse(company.GetSection("employees").Value);

        companies.Add(new Company(name, amount));
    }

    var companyWithMostEmployees = companies.OrderByDescending(c => c.Employees).FirstOrDefault();

    return $"The company with the most employees is {companyWithMostEmployees?.Name} ({companyWithMostEmployees?.Employees})";
});

app.MapGet("/task2", (IConfiguration config) =>
{
    var person = config.GetSection("Person");
    var name = person.GetSection("name").Value;
    var age = person.GetSection("age").Value;

    var languagesList = person.GetSection("languages").GetChildren().Select(x => x.Value).ToList();
    var languages = string.Join(", ", languagesList);

    return $"My name is {name}, I am {age} years old, and I know {languagesList.Count} languages: {languages}.";
});

app.Run();
