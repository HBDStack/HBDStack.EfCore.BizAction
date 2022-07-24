﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using BizDbAccess.Orders;
using HBD.EfCore.BizAction;

namespace BizLogic.Orders.Concrete;

public class ChangeDeliveryAction : BizActionStatus, IChangeDeliverAction
{
    private readonly IChangeDeliverDbAccess _dbAccess;

    public ChangeDeliveryAction(IChangeDeliverDbAccess dbAccess)
    {
        _dbAccess = dbAccess;
    }

    public Task BizActionAsync(BizChangeDeliverDto dto, CancellationToken cancellationToken = default)
    {
        var order = _dbAccess.GetOrder(dto.OrderId);
        if (order == null)
            throw new NullReferenceException("Could not find the order. Someone entering illegal ids?");

        var status = order.ChangeDeliveryDate(dto.UserId, dto.NewDeliveryDate);
        CombineStatuses(status);

        Message = $"Your new delivery date is {dto.NewDeliveryDate.ToShortDateString()}.";
        
        return Task.CompletedTask;
    }
}