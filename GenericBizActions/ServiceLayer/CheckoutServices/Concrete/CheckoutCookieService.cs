﻿// Copyright (c) 2017 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using BizLogic.Orders;
using Microsoft.AspNetCore.Http;

namespace ServiceLayer.CheckoutServices.Concrete;

public class CheckoutCookieService
{
    public const string DefaultUserId = "me@GenericBizRunner.com";

    private List<OrderLineItem> _lineItems;

    public CheckoutCookieService(string cookieContent)
    {
        DecodeCookieString(cookieContent);
    }

    public CheckoutCookieService(IRequestCookieCollection cookiesIn)
    {
        var cookieHandler = new CheckoutCookie(cookiesIn);
        DecodeCookieString(cookieHandler.GetValue());
    }

    /// <summary>
    ///     Because we don't get user to log in we assign them a uniquie GUID and store it in the cookie
    /// </summary>
    public string UserId { get; private set; }

    /// <summary>
    ///     This returns the line items in the order they were places
    /// </summary>
    public ImmutableList<OrderLineItem> LineItems => _lineItems.ToImmutableList();

    public void AddLineItem(OrderLineItem newItem)
    {
        _lineItems.Add(newItem);
    }

    public void UpdateLineItem(int itemIndex, OrderLineItem replacement)
    {
        if (itemIndex < 0 || itemIndex > _lineItems.Count)
            throw new InvalidOperationException(
                $"System error. Attempt to remove line item index {itemIndex}. _lineItems.Count = {_lineItems.Count}");

        _lineItems[itemIndex] = replacement;
    }

    public void DeleteLineItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex > _lineItems.Count)
            throw new InvalidOperationException(
                $"System error. Attempt to remove line item index {itemIndex}. _lineItems.Count = {_lineItems.Count}");

        _lineItems.RemoveAt(itemIndex);
    }

    public void ClearAllLineItems()
    {
        _lineItems.Clear();
    }

    public string EncodeForCookie()
    {
        var sb = new StringBuilder();
        sb.Append(UserId);
        foreach (var lineItem in _lineItems) sb.AppendFormat(",{0},{1}", lineItem.BookId, lineItem.NumBooks);
        return sb.ToString();
    }

    //---------------------------------------------------
    //private methods

    private void DecodeCookieString(string cookieContent)
    {
        _lineItems = new List<OrderLineItem>();

        if (cookieContent == null)
        {
            //No cookie exists, so create new user and no orders
            UserId = DefaultUserId;
            return;
        }

        //Has cookie, so decode it
        var parts = cookieContent.Split(',');
        UserId = parts[0];
        for (var i = 0; i < (parts.Length - 1) / 2; i++)
            _lineItems.Add(new OrderLineItem
            {
                BookId = int.Parse(parts[i * 2 + 1]),
                NumBooks = byte.Parse(parts[i * 2 + 2])
            });
    }
}