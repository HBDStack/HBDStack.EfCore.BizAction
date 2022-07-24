// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBD.EfCore.BizAction.Internal;
using HBD.EfCore.BizAction.PublicButHidden;
using HBDStack.EfCore.BizActions.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace HBD.EfCore.BizAction;

/// <summary>
///     This defines the ActionServiceAsync using the default DbContext
/// </summary>
/// <typeparam name="TBizInstance">The instance of the business logic you are linking to</typeparam>
public class ActionServiceAsync<TBizInstance> : ActionServiceAsync<DbContext, TBizInstance>,
    IActionServiceAsync<TBizInstance>
    where TBizInstance : class, IBizActionStatus
{
    #region Constructors

    /// <inheritdoc />
    public ActionServiceAsync(DbContext context, TBizInstance bizInstance,
        IWrappedBizRunnerConfigAndMappings wrappedConfig, IEnumerable<IModelValidator> validators = null)
        : base(context, bizInstance, wrappedConfig, validators)
    {
    }

    #endregion Constructors
}

/// <summary>
///     This defines the ActionServiceAsync where you supply the type of the DbContext you want used with the business
///     logic
/// </summary>
/// <typeparam name="TContext">The EF Core DbContext to be used wit this business logic</typeparam>
/// <typeparam name="TBizInstance">The instance of the business logic you are linking to</typeparam>
public class ActionServiceAsync<TContext, TBizInstance> : IActionServiceAsync<TContext, TBizInstance>
    where TContext : DbContext
    where TBizInstance : class, IBizActionStatus
{
    #region Constructors

    /// <inheritdoc />
    public ActionServiceAsync(TContext context, TBizInstance bizInstance,
        IWrappedBizRunnerConfigAndMappings wrappedConfig, IEnumerable<IModelValidator> validators = null)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _bizInstance = bizInstance ?? throw new ArgumentNullException(nameof(bizInstance));
        _wrappedConfig = wrappedConfig ?? throw new ArgumentNullException(nameof(wrappedConfig));
        _turnOffCaching = _wrappedConfig.Config.TurnOffCaching;

        if (validators != null)
            _modelValidator = validators.FirstOrDefault();
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    ///     This contains the Status after it has been run
    /// </summary>
    public IBizActionStatus Status => _bizInstance;

    #endregion Properties

    #region Fields

    private readonly TBizInstance _bizInstance;
    private readonly TContext _context;
    private readonly IModelValidator _modelValidator;
    private readonly bool _turnOffCaching;
    private readonly IWrappedBizRunnerConfigAndMappings _wrappedConfig;

    #endregion Fields

    #region Methods

    /// <summary>
    /// This will return a new class for input.
    /// If the type is based on a GenericActionsDto it will run SetupSecondaryData on it before handing it back
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="runBeforeSetup">An optional action to set something in the new DTO before SetupSecondaryData is called</param>
    /// <returns></returns>
    //public async Task<TDto> GetDtoAsync<TDto>(Action<TDto> runBeforeSetup = null) where TDto : class, new()
    //{
    //    if (!typeof(TDto).IsClass)
    //        throw new InvalidOperationException("You should only call this on a primitive type. Its only useful for Dtos.");

    //    var decoder = new BizDecoder(typeof(TBizInstance), RequestedInOut.InOrInOut | RequestedInOut.Async, _turnOffCaching);
    //    var toBizCopier = DtoAccessGenerator.BuildCopier(typeof(TDto), decoder.BizInfo.GetBizInType(), true, true, _turnOffCaching);
    //    return await toBizCopier.CreateDataWithPossibleSetupAsync(_context, Status, runBeforeSetup).ConfigureAwait(false);
    //}

    /// <summary>
    /// This should be called if a model error is found in the input data.
    /// If the provided data is a GenericActions dto, or it has ISetupsecondaryData, it will call SetupSecondaryData
    /// to reset any data in the dto ready for reshowing the dto to the user.
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <returns></returns>
    //public async Task<TDto> ResetDtoAsync<TDto>(TDto dto, CancellationToken cancellationToken = default) where TDto : class
    //{
    //    if (!typeof(TDto).IsClass)
    //        throw new InvalidOperationException("You should only call this on a primitive type. Its only useful for Dtos.");

    //    var decoder = new BizDecoder(typeof(TBizInstance), RequestedInOut.InOrInOut | RequestedInOut.Async, _turnOffCaching);
    //    var toBizCopier = DtoAccessGenerator.BuildCopier(typeof(TDto), decoder.BizInfo.GetBizInType(), true, true, _turnOffCaching);
    //    await toBizCopier.SetupSecondaryDataIfRequiredAsync(_context, Status, dto).ConfigureAwait(false);
    //    return dto;
    //}

    /// <summary>
    ///     This will run a business action that takes an input and produces a result
    /// </summary>
    /// <typeparam name="TOut">
    ///     The type of the result to return. Should either be the Business logic output type or class which
    ///     inherits fromm GenericActionFromBizDto
    /// </typeparam>
    /// <param name="inputData">
    ///     The input data. It should be Should either be the Business logic input type or class which
    ///     inherits form GenericActionToBizDto
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>The returned data after the run, or default value is thewre was an error</returns>
    public async Task<TOut> RunBizActionAsync<TOut>(dynamic inputData,
        CancellationToken cancellationToken = default)
    {
        await ValidateAsync(inputData, cancellationToken);
        if (Status.HasErrors) return default;

        var decoder = new BizDecoder(typeof(TBizInstance), RequestedInOut.InOut | RequestedInOut.Async,
            _turnOffCaching);
        return await ((Task<TOut>) decoder.BizInfo.GetServiceInstance(_wrappedConfig)
                .RunBizActionDbAndInstanceAsync<TOut>(_context, _bizInstance, inputData, cancellationToken))
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     This will run a business action that does not take an input but does produces a result
    /// </summary>
    /// <typeparam name="TOut">
    ///     The type of the result to return. Should either be the Business logic output type or class which
    ///     inherits fromm GenericActionFromBizDto
    /// </typeparam>
    /// <returns>The returned data after the run, or default value if there was an error</returns>
    public async Task<TOut> RunBizActionAsync<TOut>(CancellationToken cancellationToken = default)
    {
        var decoder = new BizDecoder(typeof(TBizInstance), RequestedInOut.Out | RequestedInOut.Async,
            _turnOffCaching);
        return await ((Task<TOut>) decoder.BizInfo.GetServiceInstance(_wrappedConfig)
                .RunBizActionDbAndInstanceAsync<TOut>(_context, _bizInstance, cancellationToken))
            .ConfigureAwait(false);
    }

    /// <summary>
    ///     This runs a business action which takes an input and returns just a status message
    /// </summary>
    /// <param name="inputData">
    ///     The input data. It should be Should either be the Business logic input type or class which
    ///     inherits form GenericActionToBizDto
    /// </param>
    /// <param name="cancellationToken"></param>
    /// <returns>status message with no result part</returns>
    public async Task RunBizActionAsync(dynamic inputData, CancellationToken cancellationToken = default)
    {
        await ValidateAsync(inputData, cancellationToken);
        if (Status.HasErrors) return;

        var decoder = new BizDecoder(typeof(TBizInstance), RequestedInOut.In | RequestedInOut.Async,
            _turnOffCaching);

        await ((Task) decoder.BizInfo.GetServiceInstance(_wrappedConfig)
                .RunBizActionDbAndInstanceAsync(_context, _bizInstance, inputData, cancellationToken))
            .ConfigureAwait(false);
    }

    protected virtual async Task ValidateAsync(dynamic inputData, CancellationToken cancellationToken= default)
    {
        if (_modelValidator == null) return;

        var result = await _modelValidator.ValidateAsync(inputData, cancellationToken);
        if (result != null && result.HasErrors)
            Status.CombineStatuses(result);
    }

    #endregion Methods
}