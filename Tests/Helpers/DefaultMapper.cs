using AutoMapper;
using Tests.DTOs;

namespace Tests.Helpers;

public static class DefaultMapper
{
    public static readonly IMapper _mapper = _mapper ??= CreateMapper();

    public static IMapper CreateMapper()
    {
        var config = new MapperConfiguration(cfg => { cfg.AddMaps(typeof(ServiceLayerBizOutDto).Assembly); });

        return config.CreateMapper();
    }
}