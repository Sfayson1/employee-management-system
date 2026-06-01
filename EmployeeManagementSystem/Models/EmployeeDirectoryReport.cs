/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026
 * Purpose: Concrete report that lists all employees in the Employee Management System.
 *
 * ABSTRACTION:
 *   Extends the abstract ReportGenerator base class and provides a concrete
 *   implementation of GenerateReport(). This class also demonstrates polymorphism
 *   within the report: it calls emp.GetDetailedSummary() through an abstract Employee
 *   reference — each concrete employee type (Hourly, Salaried, Commission) returns
 *   its own formatted summary at runtime without this class knowing the concrete type.
 *
 * ACCESS SPECIFIERS:
 *   - _employees: private — the data source is an internal detail.
 *     External code provides the list at construction time and cannot alter it afterward.
 */

namespace EmployeeManagementSystem.Models
{
    public class EmployeeDirectoryReport : ReportGenerator
    {
        // ACCESS SPECIFIER — private readonly:
        // The employee list is the internal data source for this report.
        // readonly prevents reassignment after the constructor runs.
        // private prevents external code from accessing or mutating it.
        private readonly List<Employee> _employees;

        // CONSTRUCTOR — parameterized, chains to ReportGenerator base:
        // Accepts the employee list and passes the report title to the base class.
        public EmployeeDirectoryReport(List<Employee> employees)
            : base("Employee Directory Report")
        {
            _employees = employees;
        }

        // ABSTRACTION — concrete implementation of the abstract method:
        // Uses GetReportHeader() and GetReportFooter() from the protected base-class helpers.
        // Calls emp.GetDetailedSummary() through abstract Employee references —
        // each call dispatches to the correct concrete subclass at runtime (polymorphism).
        public override string GenerateReport()
        {
            var lines = new List<string>
            {
                GetReportHeader(),
                $"  Total employees on file: {_employees.Count}",
                string.Empty
            };

            foreach (Employee emp in _employees)
            {
                // POLYMORPHISM: GetDetailedSummary() resolves to the correct
                // HourlyEmployee, SalariedEmployee, or CommissionEmployee implementation.
                lines.Add($"  {emp.GetDetailedSummary()}");
            }

            lines.Add(string.Empty);
            lines.Add(GetReportFooter());
            return string.Join("\n", lines);
        }
    }
}
