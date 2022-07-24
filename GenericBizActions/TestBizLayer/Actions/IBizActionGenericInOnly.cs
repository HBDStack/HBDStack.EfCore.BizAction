﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System.Collections.ObjectModel;
using HBD.EfCore.BizAction;

namespace TestBizLayer.Actions;

public interface IBizActionGenericInOnly : IGenericActionInOnly<Collection<int>>
{
}