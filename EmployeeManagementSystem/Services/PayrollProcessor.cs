/*
 * Name: Sherika Fayson
 * Date: May 24, 2026
 * Purpose: Provides payroll processing for all IPayable employees.
 *          PayrollProcessor demonstrates two key OO concepts:
 *
 *          1. COMPOSITION — PayrollProcessor is composed of (contains) a list of
 *             IPayable objects. It does not inherit from Employee or any other
 *             class; it uses employee objects as components to do its work.
 *
 *          2. POLYMORPHISM — ProcessPayroll() and GetTotalPayroll() iterate over
 *             a List<IPayable>, calling CalculatePay() and GetPaySummary() on each
 *             element through the IPayable interface reference. The C# runtime
 *             dispatches to the correct implementation (HourlyEmployee,
 *             SalariedEmployee, or CommissionEmployee) at run time, without this
 *             class needing to know which concrete type it's working with.
 */

using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    /// <summary>
    /// Processes payroll for a collection of IPayable employees.
    /// Demonstrates COMPOSITION (holds IPayable list) and POLYMORPHISM
    /// (calls interface methods that resolve to different implementations at runtime).
    /// </summary>
    public class PayrollProcessor
    {
        // COMPOSITION: PayrollProcessor is composed of a list of IPayable objects.
        // It does not know whether each element is Hourly, Salaried, or Commission
        // — it only knows each one honors the IPayable contract.
        private readonly List<IPayable> _employees;

        /// <summary>
        /// Initializes the processor with a collection of payable employees.
        /// </summary>
        /// <param name="employees">Any objects that implement IPayable</param>
        public PayrollProcessor(List<IPayable> employees)
        {
            _employees = employees;
        }

        /// <summary>
        /// Processes payroll for all employees and returns a result per employee.
        ///
        /// POLYMORPHISM DEMONSTRATED HERE:
        /// The foreach loop calls payable.CalculatePay() and payable.GetPaySummary()
        /// through the IPayable interface reference. At runtime, the CLR dispatches
        /// to whichever concrete implementation matches the actual object type:
        ///   - HourlyEmployee.CalculatePay()     → hoursWorked × hourlyRate
        ///   - SalariedEmployee.CalculatePay()   → annualSalary ÷ 26
        ///   - CommissionEmployee.CalculatePay() → salesAmount × commissionRate
        /// The processor does not contain a single if/switch on employee type.
        /// </summary>
        /// <param name="hoursWorked">Hours worked this pay period (default 80 = 2 weeks)</param>
        /// <returns>A list of PayrollResult objects, one per employee</returns>
        public List<PayrollResult> ProcessPayroll(decimal hoursWorked = 80m)
        {
            var results = new List<PayrollResult>();

            // POLYMORPHISM: 'payable' is typed as IPayable, not any concrete class.
            // Each method call on 'payable' resolves to the correct implementation
            // based on the runtime type of the object stored in the list.
            foreach (IPayable payable in _employees)
            {
                // POLYMORPHISM: CalculatePay dispatches to the correct subclass implementation
                decimal amount = payable.CalculatePay(hoursWorked);

                // POLYMORPHISM: GetPaySummary dispatches to the correct subclass implementation
                string summary = payable.GetPaySummary();

                // Retrieve display info — Employee is the base class; all three types inherit from it
                if (payable is Employee emp)
                {
                    results.Add(new PayrollResult
                    {
                        EmployeeId   = emp.EmployeeId,
                        EmployeeName = $"{emp.FirstName} {emp.LastName}",
                        EmployeeType = emp.GetEmployeeType(),
                        PaySummary   = summary,
                        PayAmount    = amount
                    });
                }
            }

            return results;
        }

        /// <summary>
        /// Sums total payroll across all employees for the period.
        ///
        /// POLYMORPHISM: Same CalculatePay() call — each type handles it differently —
        /// but the result is summed uniformly. This is the power of programming to
        /// an interface: the caller doesn't care about the type, only the contract.
        /// </summary>
        /// <param name="hoursWorked">Hours worked this pay period</param>
        /// <returns>Total gross payroll for all employees</returns>
        public decimal GetTotalPayroll(decimal hoursWorked = 80m)
        {
            decimal total = 0m;

            // POLYMORPHISM: Each type's CalculatePay() is called through IPayable reference
            foreach (IPayable payable in _employees)
                total += payable.CalculatePay(hoursWorked);

            return total;
        }
    }
}
