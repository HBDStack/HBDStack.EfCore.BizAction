﻿// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using AutoMapper;
using HBDStack.EfCore.BizActions.Abstraction;
using TestBizLayer.BizDTOs;

namespace Tests.DTOs;

[AutoMap(typeof(NestedBizDataOut))]
public class AutoMapNestedOutDto : IBizOutAutoMap
{
    public string Output { get; set; }

    public ServiceLayerNestedOutChildDto ChildData { get; set; }
}