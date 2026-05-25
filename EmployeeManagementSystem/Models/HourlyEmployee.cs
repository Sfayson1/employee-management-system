/*
 * Name: Sherika Fayson
 * Date: May 24, 2026 (updated from May 15, 2026)
 * Purpose: Represents an hourly employee.
 *
 *   INHERITANCE: HourlyEmployee extends (inherits from) the Employee base class,
 *   reusing all shared properties (name, address, department, etc.) and
 *   overriding GetEmployeeType() and GetPayInformation() with hourly-specific behavior.
 *
 *   INTERFACE (IPayable): HourlyEmployee implements the IPayable interface,
 *   providing a concrete CalculatePay() implementation that multiplies
 *   the hours worked by the hourly rate. This makes HourlyEmployee objects
 *   eligible to be processed by PayrollProcessor without the processor
 *   needing to know the concrete type.
 *
 *   POLYMORPHISM: When PayrollProcessor calls CalculatePay() through an
 *   IPayable reference, the runtime dispatches to THIS method.
 */

using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    // INHERITANCE: derives from Employee base class
    // INTERFACE:   implements IPayable contract
    public class HourlyEmployee : Employee, IPayable
    {
        /// <summary>Pay rate in dollars per hour</summary>
        public decimal HourlyRate { get; set; }

        public HourlyEmployee(
            int employeeId,
            string firstName,
            string lastName,
            Address employeeAddress,
            string department,
            string jobTitle,
            string email,
            string phone,
            decimal hourlyRate)
            : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
        {
            HourlyRate = hourlyRate;
        }

        // INHERITANCE: overrides base class virtual method
        public override string GetEmployeeType()
        {
            return "Hourly";
        }

        // INHERITANCE: overrides base class virtual method
        public override string GetPayInformation()
        {
            return $"${HourlyRate:F2}/hour";
        }

        // INTERFACE IMPLEMENTATION (IPayable):
        // Pay = hours worked × hourly rate
        // POLYMORPHISM: PayrollProcessor calls this through an IPayable reference
        public decimal CalculatePay(decimal hoursWorked)
        {
            return hoursWorked * HourlyRate;
        }

        // INTERFACE IMPLEMENTATION (IPayable):
        // Returns a readable description of the pay structure
        public string GetPaySummary()
        {
            return $"${HourlyRate:F2}/hr × hours worked";
        }
    }
}
