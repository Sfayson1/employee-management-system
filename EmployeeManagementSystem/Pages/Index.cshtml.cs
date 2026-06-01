/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Page model for the Employee Management System — Phase 3 home page.
 *          Builds sample employee data, runs payroll, generates reports, and
 *          demonstrates multiple constructors with a "new hire" intake queue.
 *
 * PHASE 3 ADDITIONS:
 *   - Employee is now abstract; all three employee types provide GetDetailedSummary().
 *   - ReportGenerator (abstract) hierarchy: EmployeeDirectoryReport and
 *     PayrollSummaryReport are instantiated and their output exposed to the view.
 *   - NewHires list: three employees created with the simplified 4-parameter
 *     constructors to demonstrate multiple constructor support.
 *   - PayrollResult now uses a parameterized constructor (private-set properties).
 */

using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        // ── Week 1 & 2: Employee directory and payroll ────────────────────────
        public List<Employee>      Employees      { get; set; } = new();
        public List<PayrollResult> PayrollResults { get; set; } = new();
        public decimal             TotalPayroll   { get; set; }
        public decimal             HoursThisPeriod { get; set; } = 80m;

        // ── Phase 3: Generated report text ───────────────────────────────────
        // ABSTRACTION: the view receives plain strings from the abstract
        // ReportGenerator hierarchy; it has no knowledge of the concrete type used.
        public string DirectoryReportText { get; set; } = string.Empty;
        public string PayrollReportText   { get; set; } = string.Empty;

        // ── Phase 3: New hire intake queue (simplified constructor demo) ──────
        // These employees were created with the 4-parameter simplified constructors,
        // demonstrating that multiple constructors exist for different use cases.
        public List<Employee> NewHires { get; set; } = new();

        public void OnGet()
        {
            // ── Build main employee roster ────────────────────────────────────
            // CONSTRUCTOR used: full parameterized (all fields known at hire).
            // ABSTRACTION: Employee is abstract — these variables hold concrete subtypes.
            // COMPOSITION: each Employee is composed with a fully specified Address.

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
                commissionRate: 15m,
                salesAmount: 42000m);

            var commission2 = new CommissionEmployee(
                106, "James", "Thompson",
                new Address("900 Citrus Road", "Clearwater", "FL", "33755"),
                "Sales", "Senior Account Executive",
                "j.thompson@company.com", "(727) 555-0106",
                commissionRate: 18m,
                salesAmount: 61500m);

            Employees.AddRange(new Employee[] { hourly1, hourly2, salaried1, salaried2, commission1, commission2 });

            // ── Run payroll (Phase 2 feature, preserved) ──────────────────────
            var payableEmployees = new List<IPayable>
                { hourly1, hourly2, salaried1, salaried2, commission1, commission2 };

            var processor    = new PayrollProcessor(payableEmployees);
            PayrollResults   = processor.ProcessPayroll(HoursThisPeriod);
            TotalPayroll     = processor.GetTotalPayroll(HoursThisPeriod);

            // ── Generate reports using the abstract ReportGenerator hierarchy ─
            // ABSTRACTION: both variables are typed as ReportGenerator (abstract).
            // GenerateReport() dispatches to the correct subclass at runtime.
            ReportGenerator directoryReport = new EmployeeDirectoryReport(Employees);
            ReportGenerator payrollReport   = new PayrollSummaryReport(PayrollResults, TotalPayroll);

            DirectoryReportText = directoryReport.GenerateReport();
            PayrollReportText   = payrollReport.GenerateReport();

            // ── New hire intake queue — simplified constructor demo ────────────
            // CONSTRUCTOR used: 4-parameter simplified constructor (name + pay rate only).
            // Department, address, and contact details are "Pending" — filled in later
            // via HR onboarding. This shows WHY multiple constructors are useful:
            // not all data is available at every stage of the employee lifecycle.
            NewHires.Add(new HourlyEmployee(    201, "Alex",   "Turner",  21.00m));
            NewHires.Add(new SalariedEmployee(  202, "Jordan", "Lee",     58000m));
            NewHires.Add(new CommissionEmployee(203, "Kim",    "Park",    14m));
        }
    }
}
