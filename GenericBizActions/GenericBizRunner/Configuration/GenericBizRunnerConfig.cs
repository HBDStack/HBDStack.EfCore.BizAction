// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizActions.Abstraction;
using HBDStack.StatusGeneric;

namespace HBD.EfCore.BizAction.Configuration;

/// <summary>
///     This is allows you to configure certain parts of the of the Generic BizRunner.
///     If you do not provide, or register a class against the IGenericBizRunnerConfig interface, then the default values
///     will be used
/// </summary>
public class GenericBizRunnerConfig : IGenericBizRunnerConfig
{
    #region Properties

    /// <inheritdoc />
    public string AppendToMessageOnGoodWriteToDb { get; set; } = " saved.";

    /// <inheritdoc />
    //public BeforeSaveChangesBizRunner BeforeSaveChanges { get; set; }

    /// <inheritdoc />
    public string DefaultSuccessAndWriteMessage { get; set; } = "Successfully saved.";

    /// <inheritdoc />
    public string DefaultSuccessMessage { get; set; } = "Success.";

    /// <inheritdoc />
    public bool DoNotValidateSaveChanges { get; set; } = true;

    /// <inheritdoc />
    public Func<Exception, IStatusGeneric> SaveChangesExceptionHandler { get; set; } = (exception) => null;

    /// <inheritdoc />
    public bool TurnOffCaching { get; set; }

    /// <inheritdoc />
    public Action<IBizActionStatus, IGenericBizRunnerConfig> UpdateSuccessMessageOnGoodWrite { get; set; } =
        DefaultMessageUpdater.UpdateSuccessMessageOnGoodWrite;

    #endregion Properties

    // default is to return null
}