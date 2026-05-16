/*
 * Name: Sherika Fayson
 * Date: May 15, 2026
 * Purpose: Base employee class used to demonstrate inheritance and composition.
 */

namespace EmployeeManagementSystem.Models
{
  // Base class demonstrating inheritance
  public class Employee
  {
    public int EmployeeId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Composition: Employee has an Address
    public Address EmployeeAddress { get; set; }

    public string Department { get; set; }
    public string JobTitle { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }

    public Employee(int employeeId, string firstName, string lastName, Address employeeAddress, string department, string jobTitle, string email, string phone)
    {
      EmployeeId = employeeId;
      FirstName = firstName;
      LastName = lastName;
      EmployeeAddress = employeeAddress;
      Department = department;
      JobTitle = jobTitle;
      Email = email;
      Phone = phone;
    }

    public virtual string GetEmployeeType()
    {
      return "General Employee";
    }

    public virtual string GetPayInformation()
    {
      return "Pay information not available";
    }
  }
}
