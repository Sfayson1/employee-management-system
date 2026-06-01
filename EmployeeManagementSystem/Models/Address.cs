/*
 * Name:    Sherika Fayson
 * Date:    May 31, 2026 (Phase 3 — updated from Phase 2)
 * Purpose: Represents a mailing address; composed inside every Employee object.
 *
 * CONSTRUCTORS (Phase 3 — three constructors now provided):
 *   Address demonstrates multiple constructor types to support different scenarios:
 *
 *   1. Default constructor — creates a blank/placeholder address.
 *      Useful when address details are not yet known at object creation time
 *      (e.g., when using the simplified employee constructors added in Phase 3).
 *
 *   2. Parameterized constructor — standard full-detail construction from known values.
 *      Used for all pre-existing employees where the full address is available.
 *
 *   3. Copy constructor — creates an independent copy of an existing Address.
 *      Prevents aliasing bugs where two Employee objects share the same instance;
 *      if one employee "moves," their Address can be replaced without affecting another.
 *
 * ACCESS SPECIFIERS (Phase 3 — tightened from Phase 2):
 *   All four properties use private set. Address is treated as a value object:
 *   once constructed, its individual fields should not be changed externally.
 *   If an employee moves, the caller assigns a new Address object to
 *   Employee.EmployeeAddress — the existing Address is not mutated in place.
 *   This encapsulation prevents accidental partial updates (e.g., changing only
 *   the city without updating the zip code).
 */

namespace EmployeeManagementSystem.Models
{
    public class Address
    {
        // ACCESS SPECIFIER — private set:
        // Address fields are immutable after construction. External code reads them
        // (public get) but cannot modify individual fields (private set).
        // To update an address, a new Address object must be created and assigned.
        public string Street  { get; private set; }
        public string City    { get; private set; }
        public string State   { get; private set; }
        public string ZipCode { get; private set; }

        // CONSTRUCTOR 1 — Default (no parameters):
        // Creates an empty/unknown address placeholder.
        // Needed when Address is required structurally but details are not yet available —
        // for example, the simplified employee constructors (added in Phase 3) use this
        // so a valid Employee object can be created before the full address is on file.
        public Address()
        {
            Street  = string.Empty;
            City    = string.Empty;
            State   = string.Empty;
            ZipCode = string.Empty;
        }

        // CONSTRUCTOR 2 — Parameterized (full detail):
        // Standard construction from known field values. Used for all employees
        // in the main employee directory where complete address data is available.
        public Address(string street, string city, string state, string zipCode)
        {
            Street  = street;
            City    = city;
            State   = state;
            ZipCode = zipCode;
        }

        // CONSTRUCTOR 3 — Copy constructor:
        // Creates a new, independent Address with the same values as the source.
        // Prevents aliasing: if two employees share an Address reference and one
        // "moves," copying the source ensures the other employee's address is unaffected.
        public Address(Address source)
        {
            Street  = source.Street;
            City    = source.City;
            State   = source.State;
            ZipCode = source.ZipCode;
        }

        // Returns a formatted single-line address string for display purposes.
        public string GetFullAddress()
        {
            if (string.IsNullOrWhiteSpace(Street))
                return "Address on file";
            return $"{Street}, {City}, {State} {ZipCode}";
        }
    }
}
