﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBD.EfCore.BizAction;
using TestBizLayer.BizDTOs;

namespace TestBizLayer.Actions.Concrete;

public class BizActionOutOnly : BizActionStatus, IBizActionOutOnly
{
    public BizDataOut BizAction()
    {
        Message = "All Ok";
        return new BizDataOut("Result");
    }
}