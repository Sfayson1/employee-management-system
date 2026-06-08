/*
 * Name:    Sherika Fayson
 * Date:    June 7, 2026 (Phase 4)
 * Purpose: Page model for the Employee database management page.
 *          Handles READ (list all / search) and DELETE operations.
 *          The Create and Update operations each have their own pages.
 *
 * ABSTRACTION (Phase 3/4):
 *   Employees is typed as List<Employee> — an abstract base-class reference.
 *   All display logic (GetEmployeeType, GetPayInformation) is called through
 *   the abstract interface; the page model never casts to a concrete type.
 *
 * CONSTRUCTORS (Phase 3/4):
 *   EmployeeDatabase is injected via the constructor (dependency injection),
 *   which is itself a form of parameterized construction supported by ASP.NET Core.
 *
 * ACCESS SPECIFIERS (Phase 3/4):
 *   _db is private readonly — only this class calls the database service.
 *   Employees and SearchTerm are public properties so the Razor view can bind to them.
 */

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages.Employees
{
    public class IndexModel : PageModel
    {
        // ACCESS SPECIFIER — private readonly:
        // The database service is assigned at construction and never reassigned.
        private readonly EmployeeDatabase _db;

        // CONSTRUCTOR — parameterized (dependency injection):
        // ASP.NET Core resolves EmployeeDatabase from the service container and passes it here.
        public IndexModel(EmployeeDatabase db) => _db = db;

        // READ — list of employees shown in the table.
        // ABSTRACTION: typed as abstract Employee; concrete types resolved in the DB layer.
        public List<Employee> Employees { get; set; } = new();

        // Bound from the query string so the search value persists across round trips.
        [BindProperty(SupportsGet = true)]
        public string? SearchTerm { get; set; }

        // Confirmation message shown after a successful add, update, or delete.
        [TempData]
        public string? StatusMessage { get; set; }

        // READ — loads employees from the database (filtered if a search term is provided).
        public void OnGet()
        {
            Employees = string.IsNullOrWhiteSpace(SearchTerm)
                ? _db.GetAllEmployees()
                : _db.SearchEmployees(SearchTerm);
        }

        // DELETE — removes the employee with the given ID and redirects back to this page.
        public IActionResult OnPostDelete(int id)
        {
            _db.DeleteEmployee(id);
            StatusMessage = $"Employee #{id} was deleted successfully.";
            return RedirectToPage();
        }
    }
}
