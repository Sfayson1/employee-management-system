/*
 * Name: Sherika Fayson
 * Date: May 24, 2026 (updated from May 15, 2026)
 * Purpose: Represents a commission-based employee.
 *
 *   INHERITANCE: CommissionEmployee extends the Employee base class, reusing all
 *   shared properties and overriding type/pay methods with commission-specific behavior.
 *
 *   INTERFACE (IPayable): CommissionEmployee implements IPayable, providing a
 *   CalculatePay() that computes earnings as a percentage of total sales.
 *   Neither annual salary nor hours worked apply — only sales volume matters.
 *
 *   POLYMORPHISM: When PayrollProcessor calls CalculatePay() through an IPayable
 *   reference, the runtime dispatches to THIS method — producing a completely
 *   different calculation from HourlyEmployee or SalariedEmployee, yet called
 *   with the exact same syntax: payable.CalculatePay(hoursWorked).
 */

using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    // INHERITANCE: derives from Employee base class
    // INTERFACE:   implements IPayable contract
    public class CommissionEmployee : Employee, IPayable
    {
        /// <summary>Commission percentage (e.g., 15 = 15%)</summary>
        public decimal CommissionRate { get; set; }

        /// <summary>Total sales generated this pay period</summary>
        public decimal SalesAmount { get; set; }

        public CommissionEmployee(
            int employeeId,
            string firstName,
            string lastName,
            Address employeeAddress,
            string department,
            string jobTitle,
            string email,
            string phone,
            decimal commissionRate,
            decimal salesAmount = 0m)
            : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
        {
            CommissionRate = commissionRate;
            SalesAmount    = salesAmount;
        }

        // INHERITANCE: overrides base class virtual method
        public override string GetEmployeeType()
        {
            return "Commission";
        }

        // INHERITANCE: overrides base class virtual method
        public override string GetPayInformation()
        {
            return $"{CommissionRate}% commission";
        }

        // INTERFACE IMPLEMENTATION (IPayable):
        // Pay = sales amount × (commission rate / 100)
        // Hours worked are not a factor for commission employees.
        // POLYMORPHISM: PayrollProcessor calls this through an IPayable reference
        public decimal CalculatePay(decimal hoursWorked)
        {
            // Commission earnings are based solely on sales, not hours
            return SalesAmount * (CommissionRate / 100m);
        }

        // INTERFACE IMPLEMENTATION (IPayable):
        // Returns a readable description of the pay structure
        public string GetPaySummary()
        {
            return $"{CommissionRate}% of ${SalesAmount:N0} sales";
        }
    }
}
