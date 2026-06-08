/*
 * Name:    Sherika Fayson
 * Date:    June 7, 2026 (Phase 4)
 * Purpose: SQLite data-access layer for the Employee Management System.
 *          Provides all four CRUD operations (Create, Read, Update, Delete)
 *          on the Employees table stored in a local SQLite database file.
 *
 * ABSTRACTION (carried forward from Phase 3):
 *   GetAllEmployees(), SearchEmployees(), and GetEmployee() all return abstract
 *   Employee references. The concrete subtype (HourlyEmployee, SalariedEmployee,
 *   or CommissionEmployee) is selected at runtime from the EmployeeType column.
 *   Callers interact through the abstract Employee interface and never need to
 *   know which concrete class was constructed — this is abstraction hiding
 *   implementation details behind a stable public surface.
 *
 * CONSTRUCTORS (carried forward from Phase 3):
 *   When reconstructing employees from database rows, this class calls the full
 *   parameterized constructors of each concrete Employee subtype. The Address
 *   value object is rebuilt using its parameterized constructor from the four
 *   address columns stored in the Employees table. Constructor choice matches
 *   the Phase 3 design: simplified constructors exist for quick intake, full
 *   constructors are used when all data is available (as it is from the database).
 *
 * ACCESS SPECIFIERS (carried forward from Phase 3):
 *   _connectionString is private readonly — set once at construction, never
 *   reassigned. All internal helpers (InitializeDatabase, SeedIfEmpty, InsertRow,
 *   ReadEmployees) are private so callers interact only through the four public
 *   CRUD methods. This limits the public surface to exactly what is needed.
 */

using Microsoft.Data.Sqlite;
using EmployeeManagementSystem.Models;

namespace EmployeeManagementSystem.Data
{
    public class EmployeeDatabase
    {
        // ACCESS SPECIFIER — private readonly:
        // The connection string is injected once at construction and immutable afterward.
        // Private access prevents external code from redirecting the database path at runtime.
        private readonly string _connectionString;

        // CONSTRUCTOR — parameterized:
        // Accepts the SQLite connection string, creates the schema if it does not exist,
        // and seeds six realistic employees on first launch. Callers can use the service
        // immediately after the constructor returns — no separate initialization step needed.
        public EmployeeDatabase(string connectionString)
        {
            _connectionString = connectionString;
            InitializeDatabase();
        }

        // Creates the Employees table and seeds initial data.
        // Private: callers interact only through the public CRUD methods.
        private void InitializeDatabase()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            var createCmd = connection.CreateCommand();
            createCmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS Employees (
                    EmployeeId     INTEGER PRIMARY KEY,
                    FirstName      TEXT NOT NULL,
                    LastName       TEXT NOT NULL,
                    Street         TEXT,
                    City           TEXT,
                    State          TEXT,
                    ZipCode        TEXT,
                    Department     TEXT,
                    JobTitle       TEXT,
                    Email          TEXT,
                    Phone          TEXT,
                    EmployeeType   TEXT NOT NULL,
                    HourlyRate     REAL NOT NULL DEFAULT 0,
                    AnnualSalary   REAL NOT NULL DEFAULT 0,
                    CommissionRate REAL NOT NULL DEFAULT 0,
                    SalesAmount    REAL NOT NULL DEFAULT 0
                )";
            createCmd.ExecuteNonQuery();

            SeedIfEmpty(connection);
        }

        // Inserts six realistic employees the first time the database is used.
        // Private: only called once from InitializeDatabase.
        private void SeedIfEmpty(SqliteConnection connection)
        {
            var countCmd = connection.CreateCommand();
            countCmd.CommandText = "SELECT COUNT(*) FROM Employees";
            long existingCount = (long)(countCmd.ExecuteScalar() ?? 0L);
            if (existingCount > 0) return;

            // Six pre-existing employees carried forward from Phase 3 in-memory data.
            InsertRow(connection, 101, "Maya",   "Johnson",
                "123 Pine Street",    "Orlando",     "FL", "32801",
                "Human Resources",        "HR Coordinator",          "m.johnson@company.com", "(407) 555-0101",
                "Hourly",  22.50m, 0m,     0m,  0m);

            InsertRow(connection, 104, "Carlos", "Rivera",
                "77 Harbor Blvd",     "Kissimmee",   "FL", "34741",
                "Facilities",             "Maintenance Technician",  "c.rivera@company.com",  "(407) 555-0104",
                "Hourly",  18.75m, 0m,     0m,  0m);

            InsertRow(connection, 102, "David",  "Carter",
                "850 Lakeview Drive", "Longwood",    "FL", "32750",
                "Information Technology", "Software Engineer",       "d.carter@company.com",  "(321) 555-0202",
                "Salaried", 0m,    68000m, 0m,  0m);

            InsertRow(connection, 105, "Priya",  "Patel",
                "210 Magnolia Lane",  "Winter Park", "FL", "32789",
                "Finance",                "Financial Analyst",       "p.patel@company.com",   "(407) 555-0105",
                "Salaried", 0m,    74500m, 0m,  0m);

            InsertRow(connection, 103, "Sarah",  "Williams",
                "456 Oak Avenue",     "Tampa",       "FL", "33602",
                "Sales",                  "Sales Representative",    "s.williams@company.com","(813) 555-0303",
                "Commission", 0m,  0m,    15m, 42000m);

            InsertRow(connection, 106, "James",  "Thompson",
                "900 Citrus Road",    "Clearwater",  "FL", "33755",
                "Sales",                  "Senior Account Executive","j.thompson@company.com","(727) 555-0106",
                "Commission", 0m,  0m,    18m, 61500m);
        }

        // ── CREATE ────────────────────────────────────────────────────────────
        // Inserts a new employee and returns the auto-assigned EmployeeId.
        public int AddEmployee(
            string firstName, string lastName,
            string street, string city, string state, string zipCode,
            string department, string jobTitle, string email, string phone,
            string employeeType,
            decimal hourlyRate, decimal annualSalary,
            decimal commissionRate, decimal salesAmount)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            // Generate a new ID one above the current maximum.
            var maxIdCmd = connection.CreateCommand();
            maxIdCmd.CommandText = "SELECT COALESCE(MAX(EmployeeId), 100) + 1 FROM Employees";
            int newId = Convert.ToInt32(maxIdCmd.ExecuteScalar());

            InsertRow(connection, newId, firstName, lastName,
                      street, city, state, zipCode,
                      department, jobTitle, email, phone,
                      employeeType, hourlyRate, annualSalary, commissionRate, salesAmount);
            return newId;
        }

        // Shared INSERT helper used by AddEmployee and SeedIfEmpty.
        // Private: implementation detail not needed by callers.
        private void InsertRow(
            SqliteConnection connection,
            int id, string firstName, string lastName,
            string street, string city, string state, string zipCode,
            string department, string jobTitle, string email, string phone,
            string employeeType,
            decimal hourlyRate, decimal annualSalary,
            decimal commissionRate, decimal salesAmount)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                INSERT INTO Employees
                    (EmployeeId, FirstName, LastName,
                     Street, City, State, ZipCode,
                     Department, JobTitle, Email, Phone,
                     EmployeeType, HourlyRate, AnnualSalary, CommissionRate, SalesAmount)
                VALUES
                    ($id, $fn, $ln,
                     $st, $ci, $sta, $zip,
                     $dept, $title, $email, $phone,
                     $type, $hr, $sal, $cr, $sales)";
            cmd.Parameters.AddWithValue("$id",    id);
            cmd.Parameters.AddWithValue("$fn",    firstName);
            cmd.Parameters.AddWithValue("$ln",    lastName);
            cmd.Parameters.AddWithValue("$st",    street);
            cmd.Parameters.AddWithValue("$ci",    city);
            cmd.Parameters.AddWithValue("$sta",   state);
            cmd.Parameters.AddWithValue("$zip",   zipCode);
            cmd.Parameters.AddWithValue("$dept",  department);
            cmd.Parameters.AddWithValue("$title", jobTitle);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$phone", phone);
            cmd.Parameters.AddWithValue("$type",  employeeType);
            cmd.Parameters.AddWithValue("$hr",    (double)hourlyRate);
            cmd.Parameters.AddWithValue("$sal",   (double)annualSalary);
            cmd.Parameters.AddWithValue("$cr",    (double)commissionRate);
            cmd.Parameters.AddWithValue("$sales", (double)salesAmount);
            cmd.ExecuteNonQuery();
        }

        // ── READ ──────────────────────────────────────────────────────────────
        // Returns all employees ordered by last name.
        // ABSTRACTION: returns abstract Employee references — callers do not know the concrete type.
        public List<Employee> GetAllEmployees()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Employees ORDER BY LastName, FirstName";
            return ReadEmployees(cmd);
        }

        // Returns employees whose name, department, or job title contains the search term.
        public List<Employee> SearchEmployees(string searchTerm)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                SELECT * FROM Employees
                WHERE  FirstName  LIKE $term
                    OR LastName   LIKE $term
                    OR Department LIKE $term
                    OR JobTitle   LIKE $term
                ORDER BY LastName, FirstName";
            cmd.Parameters.AddWithValue("$term", $"%{searchTerm}%");
            return ReadEmployees(cmd);
        }

        // Returns a single employee by ID, or null if not found.
        public Employee? GetEmployee(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Employees WHERE EmployeeId = $id";
            cmd.Parameters.AddWithValue("$id", id);
            return ReadEmployees(cmd).FirstOrDefault();
        }

        // Returns the raw stored pay fields for an employee (needed to pre-populate edit forms).
        public (string Type, decimal HourlyRate, decimal AnnualSalary, decimal CommissionRate, decimal SalesAmount)
            GetPayFields(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT EmployeeType, HourlyRate, AnnualSalary, CommissionRate, SalesAmount FROM Employees WHERE EmployeeId = $id";
            cmd.Parameters.AddWithValue("$id", id);
            using var reader = cmd.ExecuteReader();
            if (!reader.Read()) return ("Hourly", 0m, 0m, 0m, 0m);
            return (
                reader.GetString(0),
                Convert.ToDecimal(reader.GetDouble(1)),
                Convert.ToDecimal(reader.GetDouble(2)),
                Convert.ToDecimal(reader.GetDouble(3)),
                Convert.ToDecimal(reader.GetDouble(4))
            );
        }

        // Maps result rows to concrete Employee subtypes.
        // ABSTRACTION: EmployeeType column drives which constructor is called;
        // the returned list contains abstract Employee references.
        // CONSTRUCTOR: full parameterized constructors used — all data is available from the DB.
        // ADDRESS: reconstructed with the parameterized Address constructor.
        private List<Employee> ReadEmployees(SqliteCommand cmd)
        {
            var list = new List<Employee>();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int     id    = reader.GetInt32(reader.GetOrdinal("EmployeeId"));
                string  fn    = reader.GetString(reader.GetOrdinal("FirstName"));
                string  ln    = reader.GetString(reader.GetOrdinal("LastName"));
                string  st    = GetStr(reader, "Street");
                string  ci    = GetStr(reader, "City");
                string  sta   = GetStr(reader, "State");
                string  zip   = GetStr(reader, "ZipCode");
                string  dept  = GetStr(reader, "Department");
                string  title = GetStr(reader, "JobTitle");
                string  email = GetStr(reader, "Email");
                string  phone = GetStr(reader, "Phone");
                string  type  = reader.GetString(reader.GetOrdinal("EmployeeType"));
                decimal hr    = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("HourlyRate")));
                decimal sal   = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("AnnualSalary")));
                decimal cr    = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("CommissionRate")));
                decimal sales = Convert.ToDecimal(reader.GetDouble(reader.GetOrdinal("SalesAmount")));

                // CONSTRUCTOR: Address uses its parameterized constructor to rebuild from stored columns.
                var address = new Address(st, ci, sta, zip);

                // ABSTRACTION: the EmployeeType column determines the concrete class;
                // callers receive an abstract Employee reference.
                Employee emp = type switch
                {
                    "Hourly"     => new HourlyEmployee(id, fn, ln, address, dept, title, email, phone, hr),
                    "Salaried"   => new SalariedEmployee(id, fn, ln, address, dept, title, email, phone, sal),
                    "Commission" => new CommissionEmployee(id, fn, ln, address, dept, title, email, phone, cr, sales),
                    _            => throw new InvalidOperationException($"Unknown employee type: {type}")
                };
                list.Add(emp);
            }
            return list;
        }

        // Null-safe string column helper.
        private static string GetStr(SqliteDataReader r, string col) =>
            r.IsDBNull(r.GetOrdinal(col)) ? string.Empty : r.GetString(r.GetOrdinal(col));

        // ── UPDATE ────────────────────────────────────────────────────────────
        // Overwrites all editable columns for the given EmployeeId.
        public void UpdateEmployee(
            int id,
            string firstName, string lastName,
            string street, string city, string state, string zipCode,
            string department, string jobTitle, string email, string phone,
            string employeeType,
            decimal hourlyRate, decimal annualSalary,
            decimal commissionRate, decimal salesAmount)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"
                UPDATE Employees SET
                    FirstName      = $fn,
                    LastName       = $ln,
                    Street         = $st,
                    City           = $ci,
                    State          = $sta,
                    ZipCode        = $zip,
                    Department     = $dept,
                    JobTitle       = $title,
                    Email          = $email,
                    Phone          = $phone,
                    EmployeeType   = $type,
                    HourlyRate     = $hr,
                    AnnualSalary   = $sal,
                    CommissionRate = $cr,
                    SalesAmount    = $sales
                WHERE EmployeeId = $id";
            cmd.Parameters.AddWithValue("$id",    id);
            cmd.Parameters.AddWithValue("$fn",    firstName);
            cmd.Parameters.AddWithValue("$ln",    lastName);
            cmd.Parameters.AddWithValue("$st",    street);
            cmd.Parameters.AddWithValue("$ci",    city);
            cmd.Parameters.AddWithValue("$sta",   state);
            cmd.Parameters.AddWithValue("$zip",   zipCode);
            cmd.Parameters.AddWithValue("$dept",  department);
            cmd.Parameters.AddWithValue("$title", jobTitle);
            cmd.Parameters.AddWithValue("$email", email);
            cmd.Parameters.AddWithValue("$phone", phone);
            cmd.Parameters.AddWithValue("$type",  employeeType);
            cmd.Parameters.AddWithValue("$hr",    (double)hourlyRate);
            cmd.Parameters.AddWithValue("$sal",   (double)annualSalary);
            cmd.Parameters.AddWithValue("$cr",    (double)commissionRate);
            cmd.Parameters.AddWithValue("$sales", (double)salesAmount);
            cmd.ExecuteNonQuery();
        }

        // ── DELETE ────────────────────────────────────────────────────────────
        // Permanently removes the employee with the given ID from the database.
        public void DeleteEmployee(int id)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            var cmd = connection.CreateCommand();
            cmd.CommandText = "DELETE FROM Employees WHERE EmployeeId = $id";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
