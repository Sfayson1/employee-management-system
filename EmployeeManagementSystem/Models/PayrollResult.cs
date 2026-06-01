/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Data model for a single employee's payroll calculation result.
 *          Produced by PayrollProcessor and passed to the view layer for display.
 *
 * CONSTRUCTORS (Phase 3):
 *   Two constructors demonstrate different construction styles:
 *
 *   1. Static constructor — runs exactly once, the first time this class is referenced,
 *      before any instance is created. Used here to initialize the TaxYear field,
 *      which is the same for all PayrollResult objects in a given session.
 *      Static constructors take no parameters and have no access modifier.
 *
 *   2. Parameterized constructor — creates a fully populated result in one step.
 *      PayrollProcessor now uses this constructor (rather than an object initializer)
 *      so the result object is complete and valid from the moment it is created.
 *
 * ACCESS SPECIFIERS (Phase 3):
 *   - TaxYear: public get, private set — a class-level (static) value readable externally
 *     but set only by the static constructor.
 *   - All instance properties: public get, private set — PayrollResult is an immutable
 *     record once constructed. All values are supplied via the constructor; nothing
 *     outside the class should overwrite a calculated pay result.
 */

namespace EmployeeManagementSystem.Models
{
    public class PayrollResult
    {
        // ACCESS SPECIFIER — private set (static):
        // TaxYear is a class-level constant for the session; only the static constructor sets it.
        public static int TaxYear { get; private set; }

        // ACCESS SPECIFIER — private set (instance):
        // All result fields are populated at construction time and should not change afterward.
        public int     EmployeeId   { get; private set; }
        public string  EmployeeName { get; private set; }
        public string  EmployeeType { get; private set; }
        public string  PaySummary   { get; private set; }
        public decimal PayAmount    { get; private set; }

        // CONSTRUCTOR — static:
        // Runs once when the PayrollResult class is first loaded. No parameters,
        // no access modifier — these are requirements of the C# static constructor syntax.
        // Sets TaxYear to the current calendar year so all PayrollResult instances share it.
        static PayrollResult()
        {
            TaxYear = DateTime.Now.Year;
        }

        // CONSTRUCTOR — parameterized (instance):
        // Creates a fully populated, immutable result in a single call.
        // PayrollProcessor calls this directly; the private setters ensure
        // no code outside the constructor can alter the calculated values.
        public PayrollResult(
            int     employeeId,
            string  employeeName,
            string  employeeType,
            string  paySummary,
            decimal payAmount)
        {
            EmployeeId   = employeeId;
            EmployeeName = employeeName;
            EmployeeType = employeeType;
            PaySummary   = paySummary;
            PayAmount    = payAmount;
        }
    }
}
