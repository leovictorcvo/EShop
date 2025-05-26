namespace Ordering.Domain.ValuesObjects;
public record Address
{
    public string FirstName { get; } = default!;
    public string LastName { get; } = default!;
    public string EmailAddresss { get; } = default!;
    public string AddressLine { get; } = default!;
    public string Country { get; } = default!;
    public string State { get; } = default!;
    public string ZipCode { get; } = default!;

    protected Address() { }
    private Address(string firstName, string lastName, string emailAddresss, string addressLine, string country, string state, string zipCode)
    {
        FirstName = firstName;
        LastName = lastName;
        EmailAddresss = emailAddresss;
        AddressLine = addressLine;
        Country = country;
        State = state;
        ZipCode = zipCode;
    }
    public static Address Of(string firstName, string lastName, string emailAddresss, string addressLine, string country, string state, string zipCode)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
        ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
        ArgumentException.ThrowIfNullOrWhiteSpace(emailAddresss);
        ArgumentException.ThrowIfNullOrWhiteSpace(addressLine);
        ArgumentException.ThrowIfNullOrWhiteSpace(country);
        ArgumentException.ThrowIfNullOrWhiteSpace(state);
        ArgumentException.ThrowIfNullOrWhiteSpace(zipCode);

        return new Address(firstName, lastName, emailAddresss, addressLine, country, state, zipCode);
    }
}