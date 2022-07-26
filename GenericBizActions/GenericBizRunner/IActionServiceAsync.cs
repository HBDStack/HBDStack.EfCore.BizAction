﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.BizAction;

/// <summary>
///     This defines the interface for the ActionServiceAsync that uses the default EF Core DbContext
/// </summary>
/// <typeparam name="TBizInstance">The instance of the business logic to run</typeparam>
public interface IActionServiceAsync<TBizInstance> : IActionServiceAsync<DbContext, TBizInstance>
    where TBizInstance : class
{
}

/// <summary>
///     This is the primary interface to the async actions
/// </summary>
/// <typeparam name="TContext"></typeparam>
/// <typeparam name="TBizInstance"></typeparam>
public interface IActionServiceAsync<TContext, TBizInstance> where TContext : DbContext where TBizInstance : class
{
    #region Properties

    /// <summary>
    ///     This contains the Status after the BizAction is run
    /// </summary>
    IBizActionStatus Status { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// This will return a new class for input.
    /// If the type is based on a GenericActionsDto it will run SetupSecondaryData on it before handing it back
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="runBeforeSetup">An optional action to set something in the new DTO before SetupSecondaryData is called</param>
    /// <returns></returns>
    //Task<TDto> GetDtoAsync<TDto>(Action<TDto> runBeforeSetup = null) where TDto : class, new();

    /// <summary>
    /// This should be called if a model error is found in the input data.
    /// If the provided data is a GenericActions dto it will call SetupSecondaryData
    /// to reset any data in the dto ready for reshowing the dto to the user.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    //Task<TDto> ResetDtoAsync<TDto>(TDto dto, CancellationToken cancellationToken = default) where TDto : class;

    /// <summary>
    ///     This will run a business action that takes and input and produces a result
    /// </summary>
    /// <typeparam name="TOut">
    ///     The type of the result to return. Should either be the Business logic output type or class which
    ///     inherits fromm GenericActionFromBizDto
    /// </typeparam>
    /// <param name="inputData">
    ///     The input data. It should be Should either be the Business logic input type or class which
    ///     inherits form GenericActionToBizDto
    /// </param>
    /// <returns>The result, or default(TOut) if there is an error</returns>
    Task<TOut> RunBizActionAsync<TOut>(object inputData, CancellationToken cancellationToken = default);

    /// <summary>
    ///     This will run a business action that does not take an input but does produces a result
    /// </summary>
    /// <typeparam name="TOut">
    ///     The type of the result to return. Should either be the Business logic output type or class which
    ///     inherits fromm GenericActionFromBizDto
    /// </typeparam>
    /// <returns>The result, or default(TOut) if there is an error</returns>
    Task<TOut> RunBizActionAsync<TOut>(CancellationToken cancellationToken = default);

    /// <summary>
    ///     This runs a business action which takes an input and returns just a status message
    /// </summary>
    /// <param name="inputData">
    ///     The input data. It should be Should either be the Business logic input type or class which
    ///     inherits form GenericActionToBizDto
    /// </param>
    /// <returns>status message with no result part</returns>
    Task RunBizActionAsync(object inputData, CancellationToken cancellationToken = default);

    #endregion Methods
}