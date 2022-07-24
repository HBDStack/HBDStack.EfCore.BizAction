using HBDStack.EfCore.BizActions.Abstraction;

namespace BizLogic.Orders;

public interface IChangeDeliverAction : IGenericActionInOnlyWriteDbAsync<BizChangeDeliverDto>
{
}