/*
 * Name: Sherika Fayson
 * Date: May 15, 2026
 * Purpose: Represents an employee address and demonstrates composition.
 */

namespace EmployeeManagementSystem.Models
{
  public class Address
  {
    public string Street { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string ZipCode { get; set; }

    public Address(string street, string city, string state, string zipCode)
    {
      Street = street;
      City = city;
      State = state;
      ZipCode = zipCode;
    }

    public string GetFullAddress()
    {
      return $"{Street}, {City}, {State} {ZipCode}";
    }
  }
}
