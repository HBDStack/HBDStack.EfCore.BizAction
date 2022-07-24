﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizActions.Abstraction;

namespace TestBizLayer.ActionsAsync;

public interface IBizActionCheckSqlErrorHandlerWriteDbAsync : IGenericActionInOnlyWriteDbAsync<string>
{
}