using Stock.PublisherSystem.SharedKernel.ValueObjects;

namespace Stock.Contract.BoundedContext.ValueObjects;

public class Supplier
{
    public static Supplier UnsignedSupplier(string firstName, string lastName,
                                        string email, string phone)
    {
        return new Supplier(firstName, lastName, email, phone, false, Guid.Empty);
    }

    public static Supplier SignedSupplier(string firstName, string lastName,
                                      string email, string phone, Guid signedSupplierId)
    {
        return new Supplier(firstName, lastName, email, phone, true, signedSupplierId);
    }

    private Supplier(string firstName, string lastName, string email,
                   string phone, bool signed, Guid signedId)
    {
        Name = new Person(firstName, lastName);
        if (signed)
        { SignedSupplierId = signedId; }
        Signed = signed;
        Phone = phone;
        Email = email;
    }

    public Person Name { get; init; }
    public string Email { get; init; }
    public string Phone { get; init; }
    public bool Signed { get; init; }
    public Guid SignedSupplierId { get; init; }

    public string FullName => Name.FullName;

    public Supplier FixName(string first, string last)
    {
        return new Supplier(first, last, Email, Phone, Signed, SignedSupplierId);
    }

    public Supplier AddPhone(string newphone)
    {
        return new Supplier(Name.FirstName, Name.LastName, Email, newphone, Signed, SignedSupplierId);
    }

    public override bool Equals(object? obj)
    {
        return obj is Supplier author &&
               Signed == author.Signed &&
               SignedSupplierId.Equals(author.SignedSupplierId) &&
               Email == author.Email &&
               Phone == author.Phone &&
               EqualityComparer<Person>.Default.Equals(Name, author.Name);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Signed, SignedSupplierId, Email, Phone, Name);
    }
}
