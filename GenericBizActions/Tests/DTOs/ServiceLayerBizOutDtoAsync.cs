// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using AutoMapper;
using TestBizLayer.BizDTOs;

namespace Tests.DTOs;

[AutoMap(typeof(BizDataOut))]
public class ServiceLayerBizOutDtoAsync //: GenericActionFromBizDtoAsync<BizDataOut, ServiceLayerBizOutDtoAsync>
{
    public string Output { get; set; }

    //public bool SetupSecondaryOutputDataCalled { get; private set; }
    //public bool CopyFromBizDataCalled { get; private set; }

    //protected internal override Task SetupSecondaryOutputDataAsync(DbContext db)
    //{
    //    SetupSecondaryOutputDataCalled = true;
    //    return Task.CompletedTask;
    //}

    //protected internal override async Task<ServiceLayerBizOutDtoAsync> CopyFromBizDataAsync(DbContext db, IMapper mapper, BizDataOut source)
    //{
    //    var data = await base.CopyFromBizDataAsync(db, mapper, source);
    //    data.CopyFromBizDataCalled = true;
    //    return data;
    //}
}