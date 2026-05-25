/*
 * Name: Sherika Fayson
 * Date: May 24, 2026
 * Purpose: Represents a single employee's payroll calculation result.
 *          PayrollResult is a data-carrying model populated by PayrollProcessor
 *          and passed to the view for display. It is one piece of the composition
 *          used in PayrollProcessor — the processor IS composed of a list of
 *          PayrollResult objects after processing runs.
 */

namespace EmployeeManagementSystem.Models
{
    /// <summary>
    /// Stores the outcome of processing pay for one employee.
    /// Produced by PayrollProcessor.ProcessPayroll() and consumed by the view layer.
    /// </summary>
    public class PayrollResult
    {
        /// <summary>Employee's unique identifier</summary>
        public int EmployeeId { get; set; }

        /// <summary>Employee's full name (First + Last)</summary>
        public string EmployeeName { get; set; } = string.Empty;

        /// <summary>Employment type: Hourly, Salaried, or Commission</summary>
        public string EmployeeType { get; set; } = string.Empty;

        /// <summary>
        /// Human-readable description of how pay was calculated.
        /// Sourced from IPayable.GetPaySummary() — polymorphically resolved.
        /// </summary>
        public string PaySummary { get; set; } = string.Empty;

        /// <summary>
        /// The computed pay amount for this period.
        /// Sourced from IPayable.CalculatePay() — polymorphically resolved.
        /// </summary>
        public decimal PayAmount { get; set; }
    }
}
