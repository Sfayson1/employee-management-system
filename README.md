# Project Name: Fayson Employee Management System

## Project Description
The Fayson Employee Management System is a web application built with ASP.NET Core Razor Pages
that allows users to manage employee records across three pay types: Hourly, Salaried, and
Commission-based. The application demonstrates core object-oriented programming concepts in C#
including interfaces, polymorphism, inheritance, abstraction, constructors, and access specifiers,
and stores all employee data in a SQLite database with full CRUD functionality.

## Project Tasks
- **Task 1: Set up the development environment**
  - Install .NET SDK and configure the ASP.NET Core Razor Pages project
  - Configure Git and GitHub repository

- **Task 2: Phase 1 — Object-Oriented Design**
  - Design the Employee class hierarchy (Hourly, Salaried, Commission)
  - Implement the Address composition class and basic employee directory

- **Task 3: Phase 2 — Interfaces and Polymorphism**
  - Implement the IPayable interface for payroll processing
  - Build the PayrollProcessor service using polymorphic method dispatch

- **Task 4: Phase 3 — Abstraction, Constructors, and Access Specifiers**
  - Refactor Employee to an abstract base class with abstract methods
  - Add multiple constructors (full parameterized, simplified, copy, static)
  - Tighten access specifiers (private set, protected set, private readonly)
  - Build the abstract ReportGenerator hierarchy (EmployeeDirectoryReport, PayrollSummaryReport)

- **Task 5: Phase 4 — Database Interactions**
  - Integrate SQLite via Microsoft.Data.Sqlite for persistent data storage
  - Implement full CRUD operations (Create, Read, Update, Delete)
  - Build Razor Pages for Employee list, Create, Edit, Details, and Delete
  - Seed the database with realistic employee records on first launch

- **Task 6: Test the application**
  - Manually verify all CRUD operations through the browser UI
  - Confirm payroll calculations and report generation are correct

- **Task 7: Document the project**
  - Add header and inline documentation to every source file
  - Create this README file

## Project Skills Learned
- Object-oriented programming in C# (interfaces, inheritance, polymorphism, abstraction)
- Constructors: parameterized, simplified, copy, static, and constructor chaining
- Access specifiers: public, private, protected, private readonly
- ASP.NET Core Razor Pages (page models, tag helpers, model binding)
- SQLite database integration with ADO.NET (Microsoft.Data.Sqlite)
- CRUD operations with parameterized SQL queries
- Dependency injection in ASP.NET Core
- TempData for cross-request messaging
- Version control with Git and GitHub

## Language Used
- **C#**: Primary programming language for all application logic
- **ASP.NET Core Razor Pages**: Web framework for the UI and page models
- **HTML/CSS**: For page layout and styling
- **SQL (SQLite)**: For persistent employee data storage

## Development Process Used
- **Phased / Iterative Development**: Each weekly phase builds directly on the previous,
  adding one new set of OOP concepts while preserving all prior functionality.

## Notes
- Ensure the .NET 10 SDK is installed before running the application.
- Dependencies are restored automatically; or run manually:
  ```
  dotnet restore
  ```
- Use the following command to run the server locally:
  ```
  dotnet run
  ```
- The SQLite database file (`employees.db`) is created automatically in the project
  directory on first launch and seeded with six sample employees.
- No environment variables are required to run the application locally.

## Link to Project
[Fayson Employee Management System Repository](https://github.com/sfayson71/Fayson_EmployeeManagementSystem)

## License
This project is licensed under the GNU License - see the
[LICENSE](LICENSE) file for details.
