/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026
 * Purpose: Concrete report that summarizes the payroll run for all employees.
 *
 * ABSTRACTION:
 *   Extends the abstract ReportGenerator base class and provides a concrete
 *   implementation of GenerateReport() with payroll-specific formatting.
 *   The base class header/footer helpers are reused; only the body is unique here.
 *
 * ACCESS SPECIFIERS:
 *   - _results and _totalPayroll: private — internal data passed at construction.
 *     External code cannot read or modify the raw results through this class;
 *     only the formatted report string (GenerateReport()) is publicly accessible.
 */

namespace EmployeeManagementSystem.Models
{
    public class PayrollSummaryReport : ReportGenerator
    {
        // ACCESS SPECIFIER — private readonly:
        // The list of payroll results is internal to this report.
        private readonly List<PayrollResult> _results;

        // ACCESS SPECIFIER — private:
        // The pre-calculated total is used only when formatting the report footer line.
        private readonly decimal _totalPayroll;

        // CONSTRUCTOR — parameterized, chains to ReportGenerator base:
        public PayrollSummaryReport(List<PayrollResult> results, decimal totalPayroll)
            : base("Payroll Summary Report")
        {
            _results      = results;
            _totalPayroll = totalPayroll;
        }

        // ABSTRACTION — concrete implementation of the abstract method:
        // Formats each PayrollResult as a line in the report.
        public override string GenerateReport()
        {
            var lines = new List<string>
            {
                GetReportHeader(),
                $"  Tax Year: {PayrollResult.TaxYear}   |   Employees processed: {_results.Count}",
                string.Empty,
                $"  {"TYPE",-12} {"NAME",-25} {"PAY STRUCTURE",-32} {"PERIOD PAY"}",
                $"  {"────",-12} {"────",-25} {"─────────────",-32} {"──────────"}"
            };

            foreach (var r in _results)
            {
                lines.Add($"  {r.EmployeeType,-12} {r.EmployeeName,-25} {r.PaySummary,-32} {r.PayAmount:C}");
            }

            lines.Add(string.Empty);
            lines.Add($"  {"TOTAL GROSS PAYROLL THIS PERIOD:",-69} {_totalPayroll:C}");
            lines.Add(string.Empty);
            lines.Add(GetReportFooter());
            return string.Join("\n", lines);
        }
    }
}
