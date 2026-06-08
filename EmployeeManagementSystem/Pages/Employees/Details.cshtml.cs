/*
 * Name:    Sherika Fayson
 * Date:    June 7, 2026 (Phase 4)
 * Purpose: Page model for the Employee Details (read-only) view.
 *          Handles a single READ operation — retrieves one employee by ID.
 *
 * ABSTRACTION (Phase 3/4):
 *   Employee is an abstract reference. GetDetailedSummary(), GetEmployeeType(),
 *   and GetPayInformation() are all called through the abstract interface —
 *   the page model never casts to a concrete employee type.
 *
 * ACCESS SPECIFIERS (Phase 3/4):
 *   _db private readonly; Employee public for Razor view display.
 */

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        // ACCESS SPECIFIER — private readonly.
        private readonly EmployeeDatabase _db;

        // CONSTRUCTOR — parameterized (dependency injection).
        public DetailsModel(EmployeeDatabase db) => _db = db;

        // READ — the employee to display.
        // ABSTRACTION: abstract Employee reference — view uses abstract methods for display.
        public Employee? Employee { get; set; }

        // Success message passed from Create or Edit via TempData.
        [TempData]
        public string? StatusMessage { get; set; }

        // READ — loads one employee record from the database.
        public IActionResult OnGet(int id)
        {
            Employee = _db.GetEmployee(id);
            if (Employee is null) return RedirectToPage("/Employees/Index");
            return Page();
        }
    }
}
