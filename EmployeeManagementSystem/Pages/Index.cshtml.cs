/*
 * Name:    Sherika Fayson
 * Date:    June 7, 2026 (Phase 4 — updated from Phase 3)
 * Purpose: Page model for the Employee Management System home page.
 *          Phase 4 adds SQLite database integration. The Employee Directory
 *          section now loads live data from the database rather than in-memory
 *          objects, demonstrating the READ operation from the data layer.
 *          The Phase 3 OOP concept sections (abstraction, constructors, payroll)
 *          are preserved using in-memory data so those demos remain self-contained.
 *
 * PHASE 4 ADDITIONS:
 *   - EmployeeDatabase injected via constructor (dependency injection).
 *   - Employees list now populated from SQLite via _db.GetAllEmployees().
 *   - DbEmployeeCount exposed so the view can show a live record count.
 *
 * PHASE 3 FEATURES (preserved):
 *   - Employee is abstract; subtypes implement GetDetailedSummary().
 *   - ReportGenerator abstract hierarchy: EmployeeDirectoryReport and
 *     PayrollSummaryReport generate formatted text reports.
 *   - NewHires: simplified 4-parameter constructors demonstration.
 *   - PayrollResult: parameterized constructor with private-set properties.
 *
 * ABSTRACTION (Phase 3/4):
 *   All Employee references are typed as abstract Employee. The concrete subtype
 *   is transparent to this page model — all interaction is through abstract methods.
 *
 * CONSTRUCTORS (Phase 3/4):
 *   EmployeeDatabase is injected via the constructor. In-memory demo employees
 *   use full parameterized constructors; new-hire demos use simplified constructors.
 *
 * ACCESS SPECIFIERS (Phase 3/4):
 *   _db is private readonly; all public properties expose data to the Razor view.
 */

using EmployeeManagementSystem.Data;
using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        // ACCESS SPECIFIER — private readonly:
        // The database service is set at construction and never reassigned.
        private readonly EmployeeDatabase _db;

        // CONSTRUCTOR — parameterized (dependency injection):
        // ASP.NET Core resolves EmployeeDatabase from the singleton registration
        // in Program.cs and passes it here. The page model never creates the DB directly.
        public IndexModel(EmployeeDatabase db) => _db = db;

        // ── Phase 4: Live database data ───────────────────────────────────────
        // Employees is loaded from SQLite on every GET; changes made through
        // the /Employees CRUD pages are immediately reflected here.
        // ABSTRACTION: typed as abstract Employee — concrete types resolved in DB layer.
        public List<Employee> Employees { get; set; } = new();

        // Total record count for display in the header.
        public int DbEmployeeCount { get; set; }

        // ── Phase 3: In-memory OOP demo data (preserved) ─────────────────────
        public List<PayrollResult> PayrollResults  { get; set; } = new();
        public decimal             TotalPayroll    { get; set; }
        public decimal             HoursThisPeriod { get; set; } = 80m;
        public string              DirectoryReportText { get; set; } = string.Empty;
        public string              PayrollReportText   { get; set; } = string.Empty;
        public List<Employee>      NewHires        { get; set; } = new();

        public void OnGet()
        {
            // ── Phase 4: Load live employee list from SQLite ───────────────────
            Employees      = _db.GetAllEmployees();
            DbEmployeeCount = Employees.Count;

            // ── Phase 3: Build in-memory demo employees for OOP sections ─────
            // These mirror the seeded DB data; the payroll and report demos
            // use in-memory objects so they remain self-contained regardless of
            // what the user adds or deletes through the /Employees CRUD pages.

            // CONSTRUCTOR: full parameterized (all fields known at hire).
            // ABSTRACTION: variables typed as abstract Employee.
            // COMPOSITION: each Employee composed with a fully specified Address.
            var hourly1 = new HourlyEmployee(
                101, "Maya", "Johnson",
                new Address("123 Pine Street", "Orlando", "FL", "32801"),
                "Human Resources", "HR Coordinator",
                "m.johnson@company.com", "(407) 555-0101",
                hourlyRate: 22.50m);

            var hourly2 = new HourlyEmployee(
                104, "Carlos", "Rivera",
                new Address("77 Harbor Blvd", "Kissimmee", "FL", "34741"),
                "Facilities", "Maintenance Technician",
                "c.rivera@company.com", "(407) 555-0104",
                hourlyRate: 18.75m);

            var salaried1 = new SalariedEmployee(
                102, "David", "Carter",
                new Address("850 Lakeview Drive", "Longwood", "FL", "32750"),
                "Information Technology", "Software Engineer",
                "d.carter@company.com", "(321) 555-0202",
                annualSalary: 68000m);

            var salaried2 = new SalariedEmployee(
                105, "Priya", "Patel",
                new Address("210 Magnolia Lane", "Winter Park", "FL", "32789"),
                "Finance", "Financial Analyst",
                "p.patel@company.com", "(407) 555-0105",
                annualSalary: 74500m);

            var commission1 = new CommissionEmployee(
                103, "Sarah", "Williams",
                new Address("456 Oak Avenue", "Tampa", "FL", "33602"),
                "Sales", "Sales Representative",
                "s.williams@company.com", "(813) 555-0303",
                commissionRate: 15m, salesAmount: 42000m);

            var commission2 = new CommissionEmployee(
                106, "James", "Thompson",
                new Address("900 Citrus Road", "Clearwater", "FL", "33755"),
                "Sales", "Senior Account Executive",
                "j.thompson@company.com", "(727) 555-0106",
                commissionRate: 18m, salesAmount: 61500m);

            // ── Run payroll (Phase 2 feature, preserved) ──────────────────────
            var payableEmployees = new List<IPayable>
                { hourly1, hourly2, salaried1, salaried2, commission1, commission2 };
            var processor  = new PayrollProcessor(payableEmployees);
            PayrollResults = processor.ProcessPayroll(HoursThisPeriod);
            TotalPayroll   = processor.GetTotalPayroll(HoursThisPeriod);

            // ── Generate reports using abstract ReportGenerator hierarchy ─────
            // ABSTRACTION: both variables typed as abstract ReportGenerator.
            ReportGenerator directoryReport = new EmployeeDirectoryReport(
                new List<Employee> { hourly1, hourly2, salaried1, salaried2, commission1, commission2 });
            ReportGenerator payrollReport   = new PayrollSummaryReport(PayrollResults, TotalPayroll);
            DirectoryReportText = directoryReport.GenerateReport();
            PayrollReportText   = payrollReport.GenerateReport();

            // ── New hire intake queue — simplified constructor demo ────────────
            // CONSTRUCTOR: 4-parameter simplified constructor (name + pay rate only).
            NewHires.Add(new HourlyEmployee(    201, "Alex",   "Turner", 21.00m));
            NewHires.Add(new SalariedEmployee(  202, "Jordan", "Lee",    58000m));
            NewHires.Add(new CommissionEmployee(203, "Kim",    "Park",   14m));
        }
    }
}
