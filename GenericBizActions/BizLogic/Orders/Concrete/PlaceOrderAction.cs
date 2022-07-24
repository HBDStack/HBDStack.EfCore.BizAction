// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BizDbAccess.Orders;
using DataLayer.EfClasses;
using HBD.EfCore.BizAction;

namespace BizLogic.Orders.Concrete;

public class PlaceOrderAction : BizActionStatus, IPlaceOrderAction
{
    private readonly IPlaceOrderDbAccess _dbAccess;

    public PlaceOrderAction(IPlaceOrderDbAccess dbAccess) => _dbAccess = dbAccess;

    /// <summary>
    ///     This validates the input and if OK creates
    ///     an order and calls the _dbAccess to add to orders
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns>returns an Order. Will be null if there are errors</returns>
    public Task<Order> BizActionAsync(PlaceOrderInDto dto, CancellationToken cancellationToken = default)
    {
        if (!dto.AcceptTAndCs)
        {
            AddError("You must accept the T&Cs to place an order.");
            return Task.FromResult<Order>(null);
        }

        var bookOrders =
            dto.CheckoutLineItems.Select(
                x => _dbAccess.BuildBooksDto(x.BookId, x.NumBooks));
        var orderStatus = Order.CreateOrderFactory(
            dto.UserId, DateTime.Today.AddDays(5),
            bookOrders);
        CombineStatuses(orderStatus);

        if (!HasErrors)
            _dbAccess.Add(orderStatus.Result);

        return Task.FromResult(orderStatus.Result);
    }
}