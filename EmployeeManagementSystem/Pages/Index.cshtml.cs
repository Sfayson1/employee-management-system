using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        public List<Employee> Employees { get; set; } = new();

        public void OnGet()
        {
            Employees.Add(new HourlyEmployee(
                101,
                "Maya",
                "Johnson",
                new Address("123 Pine Street", "Orlando", "FL", "32801"),
                "Human Resources",
                "HR Coordinator",
                "m.johnson@company.com",
                "(407) 555-0101",
                22.50m
            ));

            Employees.Add(new SalariedEmployee(
                102,
                "David",
                "Carter",
                new Address("850 Lakeview Drive", "Longwood", "FL", "32750"),
                "Information Technology",
                "Software Engineer",
                "d.carter@company.com",
                "(321) 555-0202",
                68000m
            ));

            Employees.Add(new CommissionEmployee(
                103,
                "Sarah",
                "Williams",
                new Address("456 Oak Avenue", "Tampa", "FL", "33602"),
                "Sales",
                "Sales Representative",
                "s.williams@company.com",
                "(813) 555-0303",
                15m
            ));
        }
    }
}
