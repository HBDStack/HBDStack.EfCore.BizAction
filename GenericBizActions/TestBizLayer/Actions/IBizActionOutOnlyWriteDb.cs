﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBD.EfCore.BizAction;
using TestBizLayer.BizDTOs;

namespace TestBizLayer.Actions;

public interface IBizActionOutOnlyWriteDb : IGenericActionOutOnlyWriteDb<BizDataOut>
{
}