using DataLayer.EfClasses;
using HBDStack.EfCore.BizActions.Abstraction;

namespace BizLogic.Orders;

public interface IPlaceOrderAction : IGenericActionWriteDbAsync<PlaceOrderInDto, Order>
{
}