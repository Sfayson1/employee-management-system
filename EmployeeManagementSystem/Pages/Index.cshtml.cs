/*
 * Name: Sherika Fayson
 * Date: May 24, 2026
 * Purpose: Page model for the Employee Management System home page.
 *          Builds sample employee data, runs payroll processing, and
 *          provides both the employee directory (Week 1) and payroll
 *          results (Week 2) to the Razor view.
 *
 *          Demonstrates:
 *            - Inheritance  : three concrete Employee subclasses
 *            - Composition  : Employee contains Address; PayrollProcessor
 *                             contains List<IPayable>
 *            - Interface    : employees cast to IPayable and passed to
 *                             PayrollProcessor
 *            - Polymorphism : PayrollProcessor.ProcessPayroll() calls
 *                             CalculatePay() through IPayable references;
 *                             each subtype computes pay differently
 */

using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;
using EmployeeManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EmployeeManagementSystem.Pages
{
    public class IndexModel : PageModel
    {
        // ─── Week 1: Employee directory ───────────────────────────────────────
        /// <summary>All employees displayed in the directory table</summary>
        public List<Employee> Employees { get; set; } = new();

        // ─── Week 2: Payroll results ──────────────────────────────────────────
        /// <summary>
        /// Per-employee payroll outcomes produced by PayrollProcessor.
        /// Populated by running ProcessPayroll() — demonstrates polymorphism.
        /// </summary>
        public List<PayrollResult> PayrollResults { get; set; } = new();

        /// <summary>Sum of all employee pay amounts this period</summary>
        public decimal TotalPayroll { get; set; }

        /// <summary>Hours used for this pay run (80 = standard 2-week period)</summary>
        public decimal HoursThisPeriod { get; set; } = 80m;

        public void OnGet()
        {
            // ── Build sample employees ────────────────────────────────────────
            // INHERITANCE: each variable holds a concrete subtype of Employee
            // COMPOSITION: each Employee object is composed with an Address object

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

            // ── Populate employee directory (Week 1) ──────────────────────────
            Employees.AddRange(new Employee[] { hourly1, hourly2, salaried1, salaried2, commission1, commission2 });

            // ── Run payroll processing (Week 2) ───────────────────────────────
            // INTERFACE: each employee object is passed as IPayable
            // The List<IPayable> holds objects of three different concrete types.
            var payableEmployees = new List<IPayable>
            {
                hourly1, hourly2, salaried1, salaried2, commission1, commission2
            };

            // COMPOSITION: PayrollProcessor is composed of the IPayable list
            var processor = new PayrollProcessor(payableEmployees);

            // POLYMORPHISM: ProcessPayroll() calls CalculatePay() on each element
            // through IPayable — the runtime resolves the correct implementation.
            PayrollResults  = processor.ProcessPayroll(HoursThisPeriod);
            TotalPayroll    = processor.GetTotalPayroll(HoursThisPeriod);
        }
    }
}
