/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Processes payroll for all IPayable employees.
 *          Demonstrates COMPOSITION and POLYMORPHISM.
 *
 * PHASE 3 CHANGE — PayrollResult constructor:
 *   Now uses the parameterized PayrollResult constructor (added in Phase 3)
 *   rather than an object initializer. This aligns with the private-set access
 *   specifiers on PayrollResult properties — construction is the only place
 *   the values are set.
 *
 * ACCESS SPECIFIERS:
 *   - _employees: private readonly — the processor's data source is set at
 *     construction and must not be replaced or mutated externally.
 */

using EmployeeManagementSystem.Interfaces;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Services
{
    /// <summary>
    /// Processes payroll for a collection of IPayable employees.
    /// COMPOSITION: holds a private List&lt;IPayable&gt;.
    /// POLYMORPHISM: calls CalculatePay() through IPayable references;
    /// the CLR dispatches to the correct concrete implementation at runtime.
    /// </summary>
    public class PayrollProcessor
    {
        // ACCESS SPECIFIER — private readonly:
        // The employee list is set at construction. readonly prevents reassignment;
        // private prevents external code from accessing or replacing the list.
        // External code interacts only through the public ProcessPayroll / GetTotalPayroll methods.
        private readonly List<IPayable> _employees;

        // CONSTRUCTOR — parameterized:
        // Accepts any collection of IPayable objects; the processor does not
        // care whether they are Hourly, Salaried, or Commission — only that
        // they honor the IPayable contract.
        public PayrollProcessor(List<IPayable> employees)
        {
            _employees = employees;
        }

        /// <summary>
        /// Processes payroll for all employees.
        /// POLYMORPHISM: CalculatePay() and GetPaySummary() are called through
        /// IPayable references — the CLR dispatches to each concrete type's
        /// implementation without a single if/switch on employee type.
        /// </summary>
        public List<PayrollResult> ProcessPayroll(decimal hoursWorked = 80m)
        {
            var results = new List<PayrollResult>();

            foreach (IPayable payable in _employees)
            {
                // POLYMORPHISM: each of these calls resolves to a different
                // implementation depending on the runtime type stored in the list.
                decimal amount  = payable.CalculatePay(hoursWorked);
                string  summary = payable.GetPaySummary();

                if (payable is Employee emp)
                {
                    // PHASE 3: using the parameterized PayrollResult constructor
                    // instead of an object initializer, consistent with private-set access.
                    results.Add(new PayrollResult(
                        emp.EmployeeId,
                        $"{emp.FirstName} {emp.LastName}",
                        emp.GetEmployeeType(),
                        summary,
                        amount));
                }
            }

            return results;
        }

        /// <summary>
        /// Sums total payroll across all employees for the period.
        /// POLYMORPHISM: same CalculatePay() call — each type handles it differently.
        /// </summary>
        public decimal GetTotalPayroll(decimal hoursWorked = 80m)
        {
            decimal total = 0m;
            foreach (IPayable payable in _employees)
                total += payable.CalculatePay(hoursWorked);
            return total;
        }
    }
}
