/*
 * Name:    Sherika Fayson
 * Date:    June 7, 2026 (Phase 4)
 * Purpose: Page model for the Add New Employee form.
 *          Handles the CREATE database operation — inserts a new employee row.
 *
 * ABSTRACTION (Phase 3/4):
 *   The EmployeeDatabase.AddEmployee method internally selects the correct
 *   concrete Employee subtype from the EmployeeType value; this page model
 *   does not need to know about HourlyEmployee, SalariedEmployee, or
 *   CommissionEmployee — it passes flat form data to the data layer.
 *
 * CONSTRUCTORS (Phase 3/4):
 *   EmployeeDatabase is injected via the constructor (dependency injection).
 *   InputModel uses a default parameterized pattern where properties are
 *   initialized to safe defaults inline.
 *
 * ACCESS SPECIFIERS (Phase 3/4):
 *   _db is private readonly — inaccessible outside this class.
 *   Input is a public nested class so model binding can populate it from the form.
 */

using EmployeeManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class CreateModel : PageModel
    {
        // ACCESS SPECIFIER — private readonly: only this class uses the database service.
        private readonly EmployeeDatabase _db;

        // CONSTRUCTOR — parameterized (dependency injection):
        public CreateModel(EmployeeDatabase db) => _db = db;

        // Bound from the POST body — populated by the HTML form.
        [BindProperty]
        public InputModel Input { get; set; } = new();

        // Flat data-transfer class for form binding.
        // All fields are public so Razor model binding can set them.
        public class InputModel
        {
            [Required] public string FirstName      { get; set; } = "";
            [Required] public string LastName       { get; set; } = "";
            public string Street         { get; set; } = "";
            public string City           { get; set; } = "";
            public string State          { get; set; } = "";
            public string ZipCode        { get; set; } = "";
            [Required] public string Department     { get; set; } = "";
            [Required] public string JobTitle       { get; set; } = "";
            public string Email          { get; set; } = "";
            public string Phone          { get; set; } = "";
            [Required] public string EmployeeType   { get; set; } = "Hourly";
            public decimal HourlyRate     { get; set; }
            public decimal AnnualSalary   { get; set; }
            public decimal CommissionRate { get; set; }
            public decimal SalesAmount    { get; set; }
        }

        // GET — displays the empty create form.
        public void OnGet() { }

        // CREATE — validates, inserts the new employee, then shows their Details page.
        // Redirecting to Details gives the user immediate, unambiguous confirmation
        // that the save worked by displaying the record they just created.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            int newId = _db.AddEmployee(
                Input.FirstName, Input.LastName,
                Input.Street, Input.City, Input.State, Input.ZipCode,
                Input.Department, Input.JobTitle, Input.Email, Input.Phone,
                Input.EmployeeType,
                Input.HourlyRate, Input.AnnualSalary,
                Input.CommissionRate, Input.SalesAmount);

            TempData["StatusMessage"] = $"Employee {Input.FirstName} {Input.LastName} was added successfully.";
            return RedirectToPage("/Employees/Details", new { id = newId });
        }
    }
}
