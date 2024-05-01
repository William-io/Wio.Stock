using Stock.Contract.BoundedContext.Enum;
using Stock.Contract.BoundedContext.ValueObjects;

namespace Stock.Contract.BoundedContext.Services;

public class VersionAttributeFactory
{
    public static BaseAttributes Create(Guid contractId, string workingTitle,
                                        List<Supplier> supplies, ModReason modReason,
                                        string modDescription)
    {
        return new BaseAttributes(
            contractId, workingTitle, supplies, modReason, modDescription);
    }

    public struct BaseAttributes
    {
        internal BaseAttributes(Guid contractId, string workingTitle,
                                List<Supplier> supplies, ModReason modReason,
                                string modDescription)
        {
            ContractId = contractId;
            WorkingTitle = workingTitle;
            Suppliers = supplies;
            ModReason = modReason;
            ModDescription = modDescription;
        }

        public Guid ContractId { get; }
        public string WorkingTitle { get; }
        public List<Supplier> Suppliers { get; }
        public ModReason ModReason { get; }
        public string ModDescription { get; }
    }
}
