using System;
namespace ASP.NET
{
    public class Company
    {
        public string Name { get; set; } = " ";
        public int Employees { get; set; } = 0;

        public Company(string name, int employees)
        {
            Name = name;
            Employees = employees;
        }
    }
}