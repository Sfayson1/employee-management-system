/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026
 * Purpose: Abstract base class for all report types in the Employee Management System.
 *
 * ABSTRACTION (Phase 3 — second abstract class beyond Employee):
 *   ReportGenerator demonstrates that abstraction is useful wherever there is a
 *   family of related behaviors with a shared structure but type-specific content.
 *   It makes sense here because:
 *
 *   - A "generic report" has no meaning on its own — every useful report has a
 *     specific subject (employee directory, payroll summary, department roster, etc.).
 *   - The mechanics of producing a header and footer are identical for all report
 *     types, so that shared logic lives once in the base class.
 *   - GenerateReport() is abstract: each concrete subclass knows its own data and
 *     format; the base class cannot and should not prescribe that content.
 *
 *   This follows the Template Method pattern: the base class defines the skeleton
 *   (header + abstract body + footer); concrete subclasses fill in the body.
 *
 *   Inheriting classes benefit because they receive the shared header/footer helpers
 *   for free and only need to implement the content-specific GenerateReport() method.
 *   The application as a whole benefits because any new report type automatically
 *   conforms to the same structural contract — adding a DepartmentRosterReport,
 *   for example, requires only extending ReportGenerator and implementing one method.
 *
 * CONSTRUCTORS (Phase 3):
 *   A single protected parameterized constructor captures the report title and
 *   timestamps the report at construction time. Protected access means only subclasses
 *   can call it via : base(...) — reinforcing that ReportGenerator is a base type only.
 *
 * ACCESS SPECIFIERS:
 *   - ReportTitle:      protected — subclasses embed the title in report content.
 *   - _generatedOn:     private   — a timestamp is an internal detail; no subclass
 *                                   needs to override or expose it.
 *   - GetReportHeader / GetReportFooter: protected — shared helpers for subclasses only;
 *                                        external callers have no reason to call them directly.
 *   - GenerateReport:   public    — the single method external code calls on any report.
 */

namespace EmployeeManagementSystem.Models
{
    // ABSTRACTION: abstract class cannot be instantiated.
    // Forces all concrete report types to implement GenerateReport().
    public abstract class ReportGenerator
    {
        // ACCESS SPECIFIER — protected (readable by subclasses, hidden from external code):
        // Subclasses need the title to embed in their content, so it must be at least protected.
        // private set ensures only this constructor can assign it.
        protected string ReportTitle { get; private set; }

        // ACCESS SPECIFIER — private field:
        // The generation timestamp is an internal detail used only by GetReportHeader().
        // No subclass needs direct access to this value.
        private readonly DateTime _generatedOn;

        // CONSTRUCTOR — protected parameterized:
        // Only subclasses invoke this via : base(reportTitle).
        // abstract + protected together make clear: this class exists only to be extended.
        protected ReportGenerator(string reportTitle)
        {
            ReportTitle  = reportTitle;
            _generatedOn = DateTime.Now;
        }

        // ABSTRACTION — abstract method:
        // Each report type knows its own data and format; the base class cannot define it.
        // All concrete subclasses must supply a GenerateReport() implementation.
        public abstract string GenerateReport();

        // ACCESS SPECIFIER — protected:
        // Header/footer helpers are part of the template available to subclasses,
        // but hidden from callers who interact only through the public GenerateReport() method.
        protected string GetReportHeader()
        {
            return $"╔══════════════════════════════════════════════════╗\n" +
                   $"  {ReportTitle}\n" +
                   $"  Generated: {_generatedOn:MMMM d, yyyy h:mm tt}\n" +
                   $"╚══════════════════════════════════════════════════╝";
        }

        protected string GetReportFooter()
        {
            return "─────────────────────────────────────────────────────\n" +
                   "  End of Report";
        }
    }
}
