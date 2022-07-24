﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

namespace HBDStack.EfCore.BizActions.Abstraction;

/// <summary>
///     This is an async Action that returns a Task containing a status with a result TOut
/// </summary>
/// <typeparam name="TOut">Output from the business logic</typeparam>
public interface IGenericActionOutOnlyAsync<TOut> : IBizActionStatus
{
    /// <summary>
    ///     Async method containing business logic that will be called
    /// </summary>
    /// <returns>Task containing result, or default value if fails</returns>
    Task<TOut> BizActionAsync(CancellationToken cancellationToken = default);
}