/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Concrete implementation of the abstract Employee class for commission-based workers.
 *          Also implements IPayable so it can participate in payroll processing.
 *
 * ABSTRACTION (Phase 3):
 *   CommissionEmployee extends the abstract Employee base class. All three abstract
 *   methods — GetEmployeeType(), GetPayInformation(), and GetDetailedSummary() —
 *   must be implemented here or the project will not compile. The abstract contract
 *   ensures this class can never be left in an incomplete state with undefined behavior.
 *
 * CONSTRUCTORS (Phase 3 — two constructors now provided):
 *   1. Full parameterized constructor — for complete employee data, including initial
 *      sales amount if known. Chains to the protected Employee base constructor.
 *   2. Simplified constructor — for quick intake when only a name and commission rate
 *      are available (sales amount starts at zero and is tracked over the pay period).
 *      Chains to Constructor 1 via :this(...) for DRY initialization.
 *
 * ACCESS SPECIFIERS (Phase 3):
 *   - CommissionRate: private set — the commission percentage is negotiated at hire
 *     and changed only through HR processes, not external assignment.
 *   - SalesAmount: private set — sales totals are updated through controlled tracking;
 *     external code should not be able to set this arbitrarily.
 *   Both fields remain publicly readable (get) for pay calculation transparency.
 */

using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    public class CommissionEmployee : Employee, IPayable
    {
        // ACCESS SPECIFIER — private set:
        // Commission rate is established at hire; changes require HR authorization.
        public decimal CommissionRate { get; private set; }

        // ACCESS SPECIFIER — private set:
        // Sales amounts are recorded through internal tracking, not direct assignment.
        public decimal SalesAmount { get; private set; }

        // CONSTRUCTOR 1 — Full parameterized:
        // For complete records. salesAmount defaults to 0m (a new pay period begins with no sales).
        // Chains to the protected Employee base constructor via : base(...).
        public CommissionEmployee(
            int     employeeId,
            string  firstName,
            string  lastName,
            Address employeeAddress,
            string  department,
            string  jobTitle,
            string  email,
            string  phone,
            decimal commissionRate,
            decimal salesAmount = 0m)
            : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
        {
            CommissionRate = commissionRate;
            SalesAmount    = salesAmount;
        }

        // CONSTRUCTOR 2 — Simplified (constructor chaining via :this):
        // For quick creation when only a name and commission rate are immediately known.
        // Sales tracking begins at zero; department/contact details are filled in later.
        // :this chaining prevents duplication of initialization code.
        public CommissionEmployee(int employeeId, string firstName, string lastName, decimal commissionRate)
            : this(employeeId, firstName, lastName, new Address(),
                   "Pending", "Pending", string.Empty, string.Empty, commissionRate, 0m)
        {
        }

        // ABSTRACTION — implements Employee abstract method.
        public override string GetEmployeeType() => "Commission";

        // ABSTRACTION — implements Employee abstract method.
        public override string GetPayInformation() => $"{CommissionRate}% commission";

        // ABSTRACTION — implements new Phase 3 abstract method.
        // Produces a full self-description callable through an Employee reference.
        public override string GetDetailedSummary()
        {
            return $"[Commission] {FirstName} {LastName} | {JobTitle} — {Department}" +
                   $" | Rate: {CommissionRate}% on ${SalesAmount:N0} sales | {EmployeeAddress.GetFullAddress()}";
        }

        // IPayable — INTERFACE IMPLEMENTATION:
        // Pay = sales amount × (commission rate / 100). Hours are not a factor.
        public decimal CalculatePay(decimal hoursWorked) => SalesAmount * (CommissionRate / 100m);

        // IPayable — INTERFACE IMPLEMENTATION:
        public string GetPaySummary() => $"{CommissionRate}% of ${SalesAmount:N0} sales";
    }
}
