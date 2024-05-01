using Stock.Contract.BoundedContext.Enum;
using Stock.Contract.BoundedContext.Services;
using Stock.Contract.BoundedContext.ValueObjects;
using Stock.PublisherSystem.SharedKernel;

namespace Stock.Contract.BoundedContext.ContractAggregate;

public class StockMoviment : BaseEntity<Guid>
{
    public string MovimentNumber => _movimentNumber;
    public DateOnly DateInitiated => _initiated;

    public Guid CurrentVersionId { get; private set; }
    public Guid FinalVersionId { get; private set; }

    public bool Completed { get; private set; }
    public DateOnly CompletedDate { get; private set; }

    public DateOnly Fullfilled { get; private set; }
    public IEnumerable<BatchTracking> Versions => _versions.ToList();

    private string _movimentNumber;
    private DateOnly _initiated;
    private readonly List<BatchTracking> _versions = new List<BatchTracking>();

    public StockMoviment(DateOnly initDate, List<Supplier> suppliers, string workingTitle)
    {
        _initiated = initDate;
        Id = Guid.NewGuid();
        var baseattribs =
            VersionAttributeFactory.Create(Id, workingTitle, suppliers,
                                           ModReason.NewContract, "New Contract");
        BatchTracking version = BatchTracking.CreateNew(baseattribs);
        _movimentNumber = GenerateContractNumber(version);
        AddVersion(version);
    }

    private void AddVersion(BatchTracking version)
    {
        _versions.Add(version);
        CurrentVersionId = version.Id;
    }

    public void CreateRevisionUsingSameSpecs
       (ModReason modReason, string modDescription, string title, List<Supplier> suppliers,
        DateOnly? customDeadline)
    {
        CreateRevision(modReason, modDescription, title, suppliers, customDeadline,
                       CurrentVersion().Specs, true);
    }

    public void CreateRevisionUsingNewSpecs
        (ModReason modReason, string modDescription, string title, List<Supplier> suppliers,
         DateOnly? customDeadline, SpecificationSet specs)
    {
        CreateRevision(modReason, modDescription, title, suppliers, customDeadline, specs, false);
    }

    private void CreateRevision
        (ModReason modReason, string modDescription, string title, List<Supplier> suppliers,
         DateOnly? customDeadline, SpecificationSet specs, bool sameSpecs)
    {
        var baseattribs =
            VersionAttributeFactory.Create(Id, title, suppliers, modReason, modDescription);
        BatchTracking revision;
        if (customDeadline == null)
        {
            revision = BatchTracking.CreateRevision(baseattribs, specs, !sameSpecs);
        }
        else
        {
            revision = BatchTracking.CreateRevisionWithCustomDeadline(
                baseattribs, specs, !sameSpecs, (DateOnly)customDeadline);
        }

        AddVersion(revision);
    }

    public BatchTracking GetVersion(Guid versionId)
    {
        return Versions.SingleOrDefault(v => v.Id == versionId);
    }

    public BatchTracking CurrentVersion()
    {
        return Versions.Single(v => v.Id == CurrentVersionId);
    }

    public void FinalVersionSignedByAllParties()
    {
        Completed = true;
        CompletedDate = DateOnly.FromDateTime(DateTime.Now);
        FinalVersionId = CurrentVersionId;
    }

    public void CurrentVersionAcceptedVerbally()
    {
        CurrentVersion().VersionAccepted();
    }

    public void AddSupplier(Supplier Supplier, BatchTracking version)
    {
        version.AddSupplier(Supplier);
    }

    private string GenerateContractNumber(BatchTracking version)
    {
        var date = DateInitiated.ToShortDateString();
        var SupplierInits =
            new string(version.Suppliers.SelectMany(a => a.Name.ComplexInitials).ToArray());
        return $"{date}_{SupplierInits}";
    }

}
