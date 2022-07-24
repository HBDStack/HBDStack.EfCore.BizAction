﻿// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace BizLogic.Orders;

public class PlaceOrderInDto
{
    public bool AcceptTAndCs { get; set; }

    public string UserId { get; set; }

    public List<OrderLineItem> CheckoutLineItems { get; set; }
}