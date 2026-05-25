/*
 * Name: Sherika Fayson
 * Date: May 24, 2026 (updated from May 15, 2026)
 * Purpose: Represents a salaried employee.
 *
 *   INHERITANCE: SalariedEmployee extends the Employee base class, reusing all
 *   shared properties and overriding type/pay methods with salary-specific behavior.
 *
 *   INTERFACE (IPayable): SalariedEmployee implements IPayable, providing a
 *   CalculatePay() that divides the annual salary by 26 pay periods (bi-weekly).
 *   Hours worked are not a factor for salaried employees.
 *
 *   POLYMORPHISM: When PayrollProcessor calls CalculatePay() through an IPayable
 *   reference, the runtime dispatches to THIS method — producing a different
 *   result from HourlyEmployee or CommissionEmployee despite the identical call.
 */

using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    // INHERITANCE: derives from Employee base class
    // INTERFACE:   implements IPayable contract
    public class SalariedEmployee : Employee, IPayable
    {
        /// <summary>Annual salary in US dollars</summary>
        public decimal AnnualSalary { get; set; }

        public SalariedEmployee(
            int employeeId,
            string firstName,
            string lastName,
            Address employeeAddress,
            string department,
            string jobTitle,
            string email,
            string phone,
            decimal annualSalary)
            : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
        {
            AnnualSalary = annualSalary;
        }

        // INHERITANCE: overrides base class virtual method
        public override string GetEmployeeType()
        {
            return "Salaried";
        }

        // INHERITANCE: overrides base class virtual method
        public override string GetPayInformation()
        {
            return $"${AnnualSalary:N0}/year";
        }

        // INTERFACE IMPLEMENTATION (IPayable):
        // Pay = annual salary ÷ 26 bi-weekly pay periods (hours are not a factor)
        // POLYMORPHISM: PayrollProcessor calls this through an IPayable reference
        public decimal CalculatePay(decimal hoursWorked)
        {
            // Salaried employees are paid regardless of hours — divide annual by 26 periods
            return AnnualSalary / 26m;
        }

        // INTERFACE IMPLEMENTATION (IPayable):
        // Returns a readable description of the pay structure
        public string GetPaySummary()
        {
            return $"${AnnualSalary:N0}/yr ÷ 26 periods";
        }
    }
}
