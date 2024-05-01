using Stock.PublisherSystem.SharedKernel;

namespace Stock.Maintenance.Domain;

public class Supplier : BaseEntity<int>
{
    public string FullName { get; set; } = null!;
    public string PreferredName { get; set; } = string.Empty;
    public string Pronouns { get; set; } = string.Empty;
    public List<string> CNPJ { get; set; } = new();
    public string? CompanyName { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public IList<Product> Products { get; set; } = new List<Product>();

    public Supplier(
        string fullName, 
        string preferredName, 
        string salutation, 
        string emailAddress)
    {
        FullName = fullName;
        PreferredName = preferredName;
        Pronouns = salutation;
        EmailAddress = emailAddress;
    }

    public override string ToString()
    {
        return FullName.ToString();
    }

}
