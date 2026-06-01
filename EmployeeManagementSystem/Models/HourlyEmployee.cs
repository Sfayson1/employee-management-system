/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Concrete implementation of the abstract Employee class for hourly-wage workers.
 *          Also implements IPayable so it can participate in payroll processing.
 *
 * ABSTRACTION (Phase 3):
 *   HourlyEmployee extends the abstract Employee base class, which means it MUST
 *   provide concrete implementations for GetEmployeeType(), GetPayInformation(), and
 *   the new GetDetailedSummary(). The compiler enforces this — if any abstract method
 *   were missing, the project would not compile. This is the core benefit of using an
 *   abstract base class: the design contract is enforced, not merely documented.
 *
 * CONSTRUCTORS (Phase 3 — two constructors now provided):
 *   1. Full parameterized constructor — for complete employee data at the time of hire.
 *      Chains to the protected Employee base constructor via : base(...).
 *   2. Simplified constructor — for quick setup when only a name and pay rate are
 *      available (e.g., a new hire whose department/contact info is still being processed).
 *      Uses constructor chaining via : this(...) to call Constructor 1, supplying safe
 *      defaults for the remaining fields. Chaining keeps initialization logic in one place.
 *
 * ACCESS SPECIFIERS (Phase 3 — reviewed and tightened):
 *   - HourlyRate: private set — once negotiated at hire, the rate is controlled through
 *     an HR process, not via arbitrary external property assignment. The getter remains
 *     public so pay calculations and display remain transparent.
 *   - All IPayable methods: public — required by the interface contract.
 *   - All abstract override methods: public — required by the base class contract.
 */

using EmployeeManagementSystem.Interfaces;

namespace EmployeeManagementSystem.Models
{
    // ABSTRACTION: must implement all abstract methods declared in Employee.
    // INTERFACE:   must implement CalculatePay and GetPaySummary from IPayable.
    public class HourlyEmployee : Employee, IPayable
    {
        // ACCESS SPECIFIER — private set:
        // HourlyRate is established in the constructor. External code reads it
        // (public get) but cannot change it (private set) — preventing ad-hoc
        // rate changes that bypass proper HR authorization.
        public decimal HourlyRate { get; private set; }

        // CONSTRUCTOR 1 — Full parameterized (chains to abstract base class):
        // Used when the complete employee record is available at the time of creation.
        // The : base(...) call invokes Employee's protected constructor.
        public HourlyEmployee(
            int     employeeId,
            string  firstName,
            string  lastName,
            Address employeeAddress,
            string  department,
            string  jobTitle,
            string  email,
            string  phone,
            decimal hourlyRate)
            : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
        {
            HourlyRate = hourlyRate;
        }

        // CONSTRUCTOR 2 — Simplified (uses constructor chaining via :this):
        // For quick creation when only name and pay rate are known at intake.
        // Calls Constructor 1 via :this(...), supplying "Pending" defaults for
        // department/title and a default Address. Constructor chaining ensures
        // initialization logic lives in exactly one place (Constructor 1).
        public HourlyEmployee(int employeeId, string firstName, string lastName, decimal hourlyRate)
            : this(employeeId, firstName, lastName, new Address(),
                   "Pending", "Pending", string.Empty, string.Empty, hourlyRate)
        {
        }

        // ABSTRACTION — implements Employee abstract method.
        // Returns the concrete type name; callers need not know the runtime type.
        public override string GetEmployeeType() => "Hourly";

        // ABSTRACTION — implements Employee abstract method.
        public override string GetPayInformation() => $"${HourlyRate:F2}/hour";

        // ABSTRACTION — implements new Phase 3 abstract method.
        // Produces a full self-description without requiring callers to cast to HourlyEmployee.
        // ReportGenerator calls this through an Employee reference — polymorphism at work.
        public override string GetDetailedSummary()
        {
            return $"[Hourly] {FirstName} {LastName} | {JobTitle} — {Department}" +
                   $" | Rate: ${HourlyRate:F2}/hr | {EmployeeAddress.GetFullAddress()}";
        }

        // IPayable — INTERFACE IMPLEMENTATION:
        // Pay = hours worked × hourly rate.
        public decimal CalculatePay(decimal hoursWorked) => hoursWorked * HourlyRate;

        // IPayable — INTERFACE IMPLEMENTATION:
        // Human-readable description of the pay structure.
        public string GetPaySummary() => $"${HourlyRate:F2}/hr × hours worked";
    }
}
