/*
 * Name: Sherika Fayson
 * Date: May 15, 2026
 * Purpose: Represents an hourly employee and demonstrates inheritance.
 */

namespace EmployeeManagementSystem.Models
{
  // Derived class demonstrating inheritance
  public class HourlyEmployee : Employee
  {
    public decimal HourlyRate { get; set; }

    public HourlyEmployee(int employeeId, string firstName, string lastName, Address employeeAddress, string department, string jobTitle, string email, string phone, decimal hourlyRate)
        : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
    {
      HourlyRate = hourlyRate;
    }

    public override string GetEmployeeType()
    {
      return "Hourly";
    }

    public override string GetPayInformation()
    {
      return $"${HourlyRate}/hour";
    }
  }
}
