using Stock.Contract.BoundedContext.ContractAggregate;
using Stock.Contract.BoundedContext.Enum;
using Stock.Contract.BoundedContext.ValueObjects;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace StockTests;

[TestClass]
public class StockMovimentTests
{
    List<Supplier> _unsignedSuppliers;
    StockMoviment _moviment;

    public StockMovimentTests()
    {
       _unsignedSuppliers = new List<Supplier> { Supplier.UnsignedSupplier("first", "last", "email", "phone") };
       _moviment = new StockMoviment(DateOnly.FromDateTime(DateTime.Now), _unsignedSuppliers, "producttitle");
    }

    [TestMethod]
    public void NewStockMovimentHasId()
    {
        Assert.AreNotEqual(Guid.Empty, _moviment.Id);
    }

    [TestMethod]
    public void NewStockHasExpectedStockMovimentNumber()
    {
        Assert.AreEqual($"{DateTime.Today.ToShortDateString()}_firlas", _moviment.MovimentNumber);
    }

    [TestMethod]
    public void VersionRevisionResultsinChangeInCurrentVersionId()
    {
        var stockMoviment = new StockMoviment(DateOnly.FromDateTime(DateTime.Now), _unsignedSuppliers, "producttitle");
        var firstVersionId = stockMoviment.CurrentVersion().Id;
        stockMoviment.CreateRevisionUsingSameSpecs(ModReason.Other, "abc", "xyz", _unsignedSuppliers, null);
        Assert.AreNotEqual(firstVersionId, stockMoviment.CurrentVersionId);
    }

    [TestMethod]
    public void VersionRevisionResultsinNonEmptyVersionId()
    {
        var firstVersionId = _moviment.CurrentVersion().Id;
        _moviment.CreateRevisionUsingSameSpecs(ModReason.Other, "abc", "xyz", _unsignedSuppliers, null);
        Assert.AreNotEqual(Guid.Empty, _moviment.CurrentVersion().Id);
    }

    [TestMethod]
    public void AddingContractRevisionIncreasestheNumberOfVersions()
    {
        _moviment.CreateRevisionUsingSameSpecs
            (ModReason.ChangeProduct, "abc", "title", _unsignedSuppliers, null);
        Assert.AreEqual(2, _moviment.Versions.Count());
    }

    [TestMethod]
    public void ContractRevisionResultsInCorrectCurrentVersion()
    {
        _moviment.CreateRevisionUsingSameSpecs(ModReason.ChangeProduct, "abc", "title", _unsignedSuppliers, null);
        var ccv = _moviment.CurrentVersion();
        CollectionAssert.AreEqual
           (new string[] { ModReason.ChangeProduct.ToString(), "abc", "title", "fl" },
            new string[] { ccv.ModificationReason.ToString(), ccv.ModificationDetails,
                           ccv.WorkingTitle, ccv.Suppliers.FirstOrDefault().Name.SingleInitials });
    }

    [TestMethod]
    public void ContractRevisionWithSameSpecsSetsHasRevisedSpecsCorrectValue()
    {
        _moviment.CreateRevisionUsingSameSpecs(ModReason.ChangeProduct, "abc", "title",
                                               _unsignedSuppliers, null);
        var ccv = _moviment.CurrentVersion();
        var theField = typeof(BatchTracking)
            .GetField("_hasRevisedSpecSet",
                       BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
        var hasRevisedSpecs = (bool)theField.GetValue(ccv);

        Assert.IsFalse(hasRevisedSpecs);
    }

    [TestMethod]
    public void DerivedContractIdIsProtected()
    {
        var prop = typeof(Contract).GetProperty("Id");
        Assert.IsTrue(prop.SetMethod.IsFamily);
    }

}
