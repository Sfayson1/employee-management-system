/*
 * Name: Sherika Fayson
 * Date: May 15, 2026
 * Purpose: Represents a salaried employee and demonstrates inheritance.
 */

namespace EmployeeManagementSystem.Models
{
  // Derived class demonstrating inheritance
  public class SalariedEmployee : Employee
  {
    public decimal AnnualSalary { get; set; }

    public SalariedEmployee(int employeeId, string firstName, string lastName, Address employeeAddress, string department, string jobTitle, string email, string phone, decimal annualSalary)
        : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
    {
      AnnualSalary = annualSalary;
    }

    public override string GetEmployeeType()
    {
      return "Salaried";
    }

    public override string GetPayInformation()
    {
      return $"${AnnualSalary}/year";
    }
  }
}
