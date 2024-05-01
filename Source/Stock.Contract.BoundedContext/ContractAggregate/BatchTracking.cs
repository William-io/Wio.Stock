using Stock.Contract.BoundedContext.Enum;
using Stock.Contract.BoundedContext.Services;
using Stock.Contract.BoundedContext.ValueObjects;
using Stock.PublisherSystem.SharedKernel;
using Stock.PublisherSystem.SharedKernel.ValueObjects;
using System.Text.Json;
using static Stock.Contract.BoundedContext.Services.VersionAttributeFactory;

namespace Stock.Contract.BoundedContext.ContractAggregate;

public class BatchTracking : BaseEntity<Guid>
{
    private bool _hasRevisedSpecSet;
    public SpecificationSet Specs { get; init; }
    public Guid ContractId { get; init; }
    public string WorkingTitle { get; init; }
    public DateTime DateCreated { get; init; }
    public DateTime DateSentToSuppliers { get; private set; }
    public DateOnly AcceptanceDeadline { get; init; }
    public string ModificationDetails { get; init; }
    public ModReason ModificationReason { get; init; }
    public bool Accepted { get; private set; }

    private readonly List<Supplier> _suppliers = new List<Supplier>();
    public IEnumerable<Supplier> Suppliers => _suppliers.ToList();

    private BatchTracking(SpecificationSet specs, BaseAttributes attribs,
                            DateOnly? deadline, bool revisedSpecs)
    {
        Id = Guid.NewGuid();
        Specs = specs;
        _hasRevisedSpecSet = revisedSpecs;
        DateCreated = DateTime.Today;
        DateSentToSuppliers = DateCreated.AddDays(1);
        if (deadline is null)
        { AcceptanceDeadline = DateOnly.FromDateTime(DateCreated.AddDays(10)); }
        else
        { AcceptanceDeadline = (DateOnly)deadline; }
        ModificationReason = attribs.ModReason;
        ModificationDetails = attribs.ModDescription;
        WorkingTitle = attribs.WorkingTitle;
        _suppliers.AddRange(attribs.Suppliers);

    }

    public static BatchTracking CreateNew(BaseAttributes attribs)
    {
        return new BatchTracking(GetDefaultSpecs(), attribs, null, false);
    }

    public static BatchTracking CreateRevision
        (BaseAttributes attribs, SpecificationSet specs, bool hasRevisedSpecs)
    {
        return new BatchTracking(specs, attribs, null, hasRevisedSpecs); ;
    }

    public static BatchTracking CreateRevisionWithCustomDeadline
        (BaseAttributes attribs, SpecificationSet specs, bool hasRevisedSpecs, DateOnly deadline)
    {
        return new BatchTracking(specs, attribs, deadline, hasRevisedSpecs);
    }

    public void SentToSuppliers(DateTime datesent)
    {
        DateSentToSuppliers = datesent;
    }
    public void AddSupplier(Supplier supplier)
    {
        _suppliers.Add(supplier);
    }

    public void VersionAccepted()
    {
        Accepted = true;
    }
    public static SpecificationSet GetDefaultSpecs()
    {
        //read from json file
        string fileName = "DefaultSpecificationSet.json";
        string jsonString = File.ReadAllText(fileName);
        return JsonSerializer.Deserialize<SpecificationSet>(jsonString)!;
    }
}