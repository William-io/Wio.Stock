namespace Stock.Contract.BoundedContext.ValueObjects;

public readonly record struct SpecificationSet
        (int AdvanceAmount, //100
    int MaximumStock, //10
    int MinimumStock, //10
    int StockOut, //5
    int StockEntry, //2
    bool IsHospitalProduct, //true
    bool IsPerishable, //false
    int DamagedProduct, //3
    decimal Price); //130.00
