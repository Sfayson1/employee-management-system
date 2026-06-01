/*
 * Name: Sherika Fayson
 * Date: May 31, 2026 (Phase 3 — reviewed; no changes to interface contract required)
 * Purpose: Defines the IPayable interface for the Employee Management System.
 *          This interface establishes a contract that any payable entity must fulfill,
 *          enabling polymorphic payroll processing across different employee types.
 *
 * INTERFACE DEMONSTRATION:
 *   IPayable is defined as an interface (rather than a base class) because:
 *   - Calculating pay is a capability, not an identity — it describes what something CAN DO.
 *   - Different employee types have fundamentally different pay calculation logic
 *     with no shared implementation worth inheriting.
 *   - Any future payable entity (e.g., Contractor, Intern) can implement this
 *     interface without being forced into the Employee inheritance hierarchy.
 *   - It allows the PayrollProcessor to work with any IPayable object without
 *     knowing the concrete type — the foundation of polymorphism in this system.
 */

namespace EmployeeManagementSystem.Interfaces
{
    // INTERFACE: Defines the contract all payable employee types must fulfill.
    // Any class implementing IPayable must provide concrete implementations
    // for both CalculatePay and GetPaySummary.
    public interface IPayable
    {
        /// <summary>
        /// Calculates the pay amount for a given number of hours worked.
        /// Each implementing class computes this differently:
        ///   - HourlyEmployee:     hoursWorked × hourlyRate
        ///   - SalariedEmployee:   annualSalary ÷ 26 (bi-weekly, hours ignored)
        ///   - CommissionEmployee: salesAmount × commissionRate (hours ignored)
        /// </summary>
        /// <param name="hoursWorked">Number of hours worked in the pay period</param>
        /// <returns>The calculated pay amount for the period</returns>
        decimal CalculatePay(decimal hoursWorked);

        /// <summary>
        /// Returns a human-readable summary of how this employee is compensated.
        /// Used by PayrollProcessor to display a per-employee pay description.
        /// </summary>
        /// <returns>A string describing the pay structure for this employee</returns>
        string GetPaySummary();
    }
}
