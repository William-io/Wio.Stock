using Stock.PublisherSystem.SharedKernel;

namespace Stock.Maintenance.Domain;

public class Product : BaseEntity<int>
{
    public int SupplierId { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly EntryDate { get; set; } //Tem que ser validade
    public List<DateOnly> BatchDates { get; set; } = new();
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public decimal ListPrice { get; set; }
    public Category PrimaryCategory { get; set; }
    public List<Category> Categories { get; set; } = new();

    public Product(int supplierId, string title, Category primaryCategory)
    {
        SupplierId = supplierId;
        Title = title;
        PrimaryCategory = primaryCategory;
        Categories = new List<Category> { primaryCategory };
    }

    public override string ToString()
    {
        return Title;
    }

}