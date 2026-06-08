/*
 * Name:    Sherika Fayson
 * Date:    June 7, 2026 (Phase 4)
 * Purpose: Page model for the Edit Employee form.
 *          Handles the UPDATE database operation — overwrites an existing employee row.
 *
 * ABSTRACTION (Phase 3/4):
 *   GetEmployee returns an abstract Employee reference. The page model queries it
 *   through GetEmployeeType() to know which pay field to pre-populate, and then
 *   calls GetPayFields() on the database service (which returns raw pay values
 *   stored in SQLite) to fill the form without casting. The database layer handles
 *   all type-specific reconstruction.
 *
 * CONSTRUCTORS (Phase 3/4):
 *   EmployeeDatabase injected via constructor (dependency injection).
 *
 * ACCESS SPECIFIERS (Phase 3/4):
 *   _db private readonly; Input and Employee public for Razor binding and display.
 */

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class EditModel : PageModel
    {
        // ACCESS SPECIFIER — private readonly.
        private readonly EmployeeDatabase _db;

        // CONSTRUCTOR — parameterized (dependency injection).
        public EditModel(EmployeeDatabase db) => _db = db;

        // The employee being edited (used by the view for display).
        // ABSTRACTION: abstract Employee reference.
        public Employee? Employee { get; set; }

        // Bound from the POST body.
        [BindProperty]
        public InputModel Input { get; set; } = new();

        public class InputModel
        {
            public int EmployeeId { get; set; }
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

        // READ — loads the employee and pre-populates the form.
        public IActionResult OnGet(int id)
        {
            Employee = _db.GetEmployee(id);
            if (Employee is null) return RedirectToPage("/Employees/Index");

            // GetPayFields returns the raw stored pay values for this employee.
            var pay = _db.GetPayFields(id);

            Input = new InputModel
            {
                EmployeeId     = Employee.EmployeeId,
                FirstName      = Employee.FirstName,
                LastName       = Employee.LastName,
                Street         = Employee.EmployeeAddress.Street,
                City           = Employee.EmployeeAddress.City,
                State          = Employee.EmployeeAddress.State,
                ZipCode        = Employee.EmployeeAddress.ZipCode,
                Department     = Employee.Department,
                JobTitle       = Employee.JobTitle,
                Email          = Employee.Email,
                Phone          = Employee.Phone,
                EmployeeType   = pay.Type,
                HourlyRate     = pay.HourlyRate,
                AnnualSalary   = pay.AnnualSalary,
                CommissionRate = pay.CommissionRate,
                SalesAmount    = pay.SalesAmount,
            };
            return Page();
        }

        // UPDATE — validates and saves the modified employee record.
        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Employee = _db.GetEmployee(Input.EmployeeId);
                return Page();
            }

            _db.UpdateEmployee(
                Input.EmployeeId,
                Input.FirstName, Input.LastName,
                Input.Street, Input.City, Input.State, Input.ZipCode,
                Input.Department, Input.JobTitle, Input.Email, Input.Phone,
                Input.EmployeeType,
                Input.HourlyRate, Input.AnnualSalary,
                Input.CommissionRate, Input.SalesAmount);

            TempData["StatusMessage"] = $"Employee #{Input.EmployeeId} — {Input.FirstName} {Input.LastName} was updated successfully.";
            return RedirectToPage("/Employees/Index");
        }
    }
}
