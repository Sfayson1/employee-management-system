/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Abstract base class for all employee types in the Employee Management System.
 *
 * ABSTRACTION (Phase 3 addition):
 *   Employee is now declared abstract because a "generic employee" has no
 *   meaningful existence in this system — every real employee is Hourly,
 *   Salaried, or Commission-based. Making the class abstract:
 *     - Prevents direct instantiation (you cannot hire a "generic employee").
 *     - Forces every concrete subclass to supply its own implementations of
 *       GetEmployeeType(), GetPayInformation(), and GetDetailedSummary().
 *     - Provides shared state (name, address, contact) and a protected
 *       constructor that subclasses chain to via : base(...).
 *   This is a compiler-enforced contract: if a subclass omits any abstract
 *   method, the project will not compile.
 *
 * CONSTRUCTORS (Phase 3):
 *   A single protected parameterized constructor is provided. It is protected
 *   rather than public because only derived classes should call it — the abstract
 *   keyword already blocks direct instantiation, and protected reinforces the
 *   intent that this constructor exists solely for subclass chaining.
 *
 * ACCESS SPECIFIERS (Phase 3 — reviewed and tightened):
 *   - EmployeeId, FirstName, LastName: private set — identity is assigned at hire
 *     and must not change. External code can read (public get) but not modify.
 *   - EmployeeAddress, Department, JobTitle, Email, Phone: protected set —
 *     role and contact data may legitimately change over an employment period;
 *     derived classes (e.g., an HR subclass) may update them, but arbitrary
 *     external code cannot.
 *   - All abstract methods are public — callers need them for display and processing.
 */

namespace EmployeeManagementSystem.Models
{
    // ABSTRACTION: 'abstract' prevents direct instantiation.
    // All three abstract methods below are compiler-enforced contracts —
    // every concrete subclass must implement them or the build fails.
    public abstract class Employee
    {
        // ACCESS SPECIFIER — private set:
        // An employee ID is assigned at creation and never changes.
        // External code can read EmployeeId but cannot modify it after construction.
        public int EmployeeId { get; private set; }

        // ACCESS SPECIFIER — private set:
        // Legal name does not change via application code.
        public string FirstName { get; private set; }
        public string LastName  { get; private set; }

        // ACCESS SPECIFIER — protected set:
        // An employee may move; only this class and its derived classes may update the address.
        // External code (e.g., a Razor page) can read but cannot assign a new Address directly.
        public Address EmployeeAddress { get; protected set; }

        // ACCESS SPECIFIER — protected set:
        // Role and contact info may change over the course of employment.
        // Derived classes (e.g., a future ManagerEmployee) may update these fields,
        // but they are not writable from outside the inheritance hierarchy.
        public string Department { get; protected set; }
        public string JobTitle   { get; protected set; }
        public string Email      { get; protected set; }
        public string Phone      { get; protected set; }

        // CONSTRUCTOR — protected parameterized:
        // Protected access means only derived classes can call this constructor via : base(...).
        // Combined with the abstract keyword on the class, this enforces that Employee
        // is exclusively a base type — it must be subclassed to be used.
        protected Employee(
            int    employeeId,
            string firstName,
            string lastName,
            Address employeeAddress,
            string department,
            string jobTitle,
            string email,
            string phone)
        {
            EmployeeId      = employeeId;
            FirstName       = firstName;
            LastName        = lastName;
            EmployeeAddress = employeeAddress;
            Department      = department;
            JobTitle        = jobTitle;
            Email           = email;
            Phone           = phone;
        }

        // ABSTRACTION — abstract method:
        // Every concrete subtype must identify itself. "General Employee" was a code smell;
        // now the compiler guarantees each subtype supplies a meaningful type name.
        public abstract string GetEmployeeType();

        // ABSTRACTION — abstract method:
        // Pay structures differ fundamentally across employee types — no shared
        // implementation exists, so no default can be defined here.
        public abstract string GetPayInformation();

        // ABSTRACTION — abstract method (new in Phase 3):
        // Forces each employee type to produce a complete, human-readable self-description.
        // Callers (e.g., ReportGenerator) can build reports without casting to a concrete type —
        // this is the key benefit: abstraction hides type details behind a uniform interface.
        public abstract string GetDetailedSummary();
    }
}
