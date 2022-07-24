// Copyright (c) 2019 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using AutoMapper;
using HBD.EfCore.BizAction.PublicButHidden;
using HBDStack.EfCore.BizActions.Abstraction;

namespace HBD.EfCore.BizAction.Configuration;

/// <summary>
///     This class is used to set up the IWrappedBizRunnerConfigAndMappings when you aren't using dependency injection.
///     Useful for unit tests, Azure Functions etc.
/// </summary>
public class NonDiBizSetup
{
    //private readonly IMapper mapper;
    private readonly IGenericBizRunnerConfig _config;
    private readonly IMapper _mapper;

    /// <summary>
    ///     We only create the IWrappedConfigAndMapper when someone needs it.
    ///     This allows you to add multiple DTOs and the AutoMapper mapping is only worked out once they are all in.
    /// </summary>
    private IWrappedBizRunnerConfigAndMappings _configAndMapper;


    /// <summary>
    ///     This creates the NonDiBizSetup - useful if you don't have any DTOs to map
    /// </summary>
    /// <param name="publicConfig"></param>
    public NonDiBizSetup(IMapper mapper, IGenericBizRunnerConfig publicConfig = null)
    {
        _mapper = mapper;
        _config = publicConfig ?? new GenericBizRunnerConfig();
        //_bizInProfile = new BizRunnerProfile();
        //_bizOutProfile = new BizRunnerProfile();
    }
    //private readonly BizRunnerProfile _bizInProfile;
    //private readonly BizRunnerProfile _bizOutProfile;

    /// <summary>
    ///     This holds the global configuration and the AutoMapper data
    /// </summary>
    public IWrappedBizRunnerConfigAndMappings WrappedConfig =>
        _configAndMapper ??= new WrappedBizRunnerConfigAndMappings(_mapper, _config);

    /// <summary>
    ///     This creates the NonDiBizSetup and adds one GenericBizRunner DTO
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    /// <param name="publicConfig"></param>
    /// <returns></returns>
    public static NonDiBizSetup SetupDtoMapping<TDto>(IMapper mapper, IGenericBizRunnerConfig publicConfig = null)
    {
        var nonDiConf = new NonDiBizSetup(mapper, publicConfig);
        //SetupDtoMapping(typeof(TDto), nonDiConf);
        return nonDiConf;
    }

    /// <summary>
    /// This adds a GenericBizRunner DTO to the NonDiBizSetup instance
    /// </summary>
    /// <typeparam name="TDto"></typeparam>
    //public void AddDtoMapping<TDto>()
    //{
    //    SetupDtoMapping(typeof(TDto), this);
    //}

    //---------------------------------------------------
    //private 

    //private static void SetupDtoMapping(Type dtoType, NonDiBizSetup nonDiConf)
    //{
    //    var bizIn = dtoType.GetInterface(nameof(IGenericActionToBizDto)) != null;
    //    if (!bizIn && dtoType.GetInterface(nameof(IGenericActionFromBizDto)) == null)
    //        throw new InvalidOperationException($"The class {dtoType.Name} doesn't inherit from one of the Biz Runner Dto classes.");

    //    SetupDtoMappings.SetupGenericActionMappingForDto(dtoType, bizIn ? nonDiConf._bizInProfile : nonDiConf._bizOutProfile, bizIn);
    //}
}