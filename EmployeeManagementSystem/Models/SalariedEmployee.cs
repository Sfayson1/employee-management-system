/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Concrete implementation of the abstract Employee class for salaried workers.
 *          Also implements IPayable so it can participate in payroll processing.
 *
 * ABSTRACTION (Phase 3):
 *   SalariedEmployee extends the abstract Employee base class and must provide
 *   concrete implementations of all three abstract methods: GetEmployeeType(),
 *   GetPayInformation(), and GetDetailedSummary(). Without these, the class would
 *   not compile. This guarantee — enforced by the abstract keyword — is exactly why
 *   abstraction is valuable: it removes the possibility of an incomplete subclass
 *   silently failing at runtime.
 *
 * CONSTRUCTORS (Phase 3 — two constructors now provided):
 *   1. Full parameterized constructor — for complete employee data at the time of hire.
 *      Chains to the protected Employee base constructor via : base(...).
 *   2. Simplified constructor — for quick creation when only a name and annual salary
 *      are available. Chains to Constructor 1 via :this(...) with safe defaults,
 *      keeping initialization logic in a single location.
 *
 * ACCESS SPECIFIERS (Phase 3):
 *   - AnnualSalary: private set — salary changes require an HR-managed process;
 *     arbitrary external assignment is prevented. The getter is public for display
 *     and pay-calculation transparency.
 */

using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    public class SalariedEmployee : Employee, IPayable
    {
        // ACCESS SPECIFIER — private set:
        // Annual salary is set at construction (or via a controlled HR method).
        // External code may read it (public get) but cannot reassign it directly.
        public decimal AnnualSalary { get; private set; }

        // CONSTRUCTOR 1 — Full parameterized:
        // Used for complete employee records with all known details.
        // Chains to the protected Employee base constructor via : base(...).
        public SalariedEmployee(
            int     employeeId,
            string  firstName,
            string  lastName,
            Address employeeAddress,
            string  department,
            string  jobTitle,
            string  email,
            string  phone,
            decimal annualSalary)
            : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
        {
            AnnualSalary = annualSalary;
        }

        // CONSTRUCTOR 2 — Simplified (constructor chaining via :this):
        // For quick creation when only a name and salary are immediately known.
        // Chains to Constructor 1, supplying "Pending" defaults; this means the
        // initialization code in Constructor 1 executes once, without duplication.
        public SalariedEmployee(int employeeId, string firstName, string lastName, decimal annualSalary)
            : this(employeeId, firstName, lastName, new Address(),
                   "Pending", "Pending", string.Empty, string.Empty, annualSalary)
        {
        }

        // ABSTRACTION — implements Employee abstract method.
        public override string GetEmployeeType() => "Salaried";

        // ABSTRACTION — implements Employee abstract method.
        public override string GetPayInformation() => $"${AnnualSalary:N0}/year";

        // ABSTRACTION — implements new Phase 3 abstract method.
        // Provides a full self-description without requiring a cast to SalariedEmployee.
        public override string GetDetailedSummary()
        {
            return $"[Salaried] {FirstName} {LastName} | {JobTitle} — {Department}" +
                   $" | Salary: ${AnnualSalary:N0}/yr | {EmployeeAddress.GetFullAddress()}";
        }

        // IPayable — INTERFACE IMPLEMENTATION:
        // Salaried employees are paid regardless of hours; annual ÷ 26 bi-weekly periods.
        public decimal CalculatePay(decimal hoursWorked) => AnnualSalary / 26m;

        // IPayable — INTERFACE IMPLEMENTATION:
        public string GetPaySummary() => $"${AnnualSalary:N0}/yr ÷ 26 periods";
    }
}
