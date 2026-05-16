/*
 * Name: Sherika Fayson
 * Date: May 15, 2026
 * Purpose: Represents a commission employee and demonstrates inheritance.
 */

namespace EmployeeManagementSystem.Models
{
  // Derived class demonstrating inheritance
  public class CommissionEmployee : Employee
  {
    public decimal CommissionRate { get; set; }

    public CommissionEmployee(
        int employeeId,
        string firstName,
        string lastName,
        Address employeeAddress,
        string department,
        string jobTitle,
        string email,
        string phone,
        decimal commissionRate)
        : base(employeeId, firstName, lastName, employeeAddress, department, jobTitle, email, phone)
    {
      CommissionRate = commissionRate;
    }

    public override string GetEmployeeType()
    {
      return "Commission";
    }

    public override string GetPayInformation()
    {
      return $"{CommissionRate}% commission";
    }
  }
}
