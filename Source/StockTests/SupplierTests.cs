using Stock.Contract.BoundedContext.ValueObjects;

namespace StockTests;

[TestClass]
public class SupplierTests
{
    [TestMethod]
    public void CanCreateSignedSupplier()
    {
        var signedId = Guid.NewGuid();
        var supplier = Supplier.SignedSupplier("first", "last", "email", "phone", signedId);
        CollectionAssert.AreEqual(new object[] { true, signedId },
                                  new object[] { supplier.Signed, supplier.SignedSupplierId });
    }

    [TestMethod]
    public void CanCreateUnsignedSupplier()
    {
        var supplier = Supplier.UnsignedSupplier("first", "last", "email", "phone");
        Assert.AreEqual(false, supplier.Signed);
    }

    [TestMethod]
    public void CanCreateANewSupplierViaFixSupplierName()
    {
        var supplier = Supplier.UnsignedSupplier("first", "last", "email", "phone");
        var newSupplier = supplier.FixName("newfirst", "newlast");
        CollectionAssert.AreEquivalent(
            new string[] { "newfirst newlast", "email", "phone" },
            new string[] { newSupplier.FullName, newSupplier.Email, newSupplier.Phone });
    }

    [TestMethod]
    public void CanCreateANewSupplierViaAddPhone()
    {
        var supplier = Supplier.UnsignedSupplier("first", "last", "email", string.Empty);
        var newSupplier = supplier.AddPhone("111111");
        CollectionAssert.AreEquivalent(
            new string[] { "first last", "email", "111111" },
            new string[] { newSupplier.FullName, newSupplier.Email, newSupplier.Phone });
    }

    [TestMethod]
    public void UnsignedSupplierHasNoSignedId()
    {
        var supplier = Supplier.UnsignedSupplier("first", "last", "email", "phone");
        Assert.AreEqual(Guid.Empty, supplier.SignedSupplierId);
    }
}
