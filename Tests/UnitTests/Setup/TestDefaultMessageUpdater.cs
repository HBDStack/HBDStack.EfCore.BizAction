﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using HBDStack.EfCore.BizAction;
using HBDStack.EfCore.BizAction.Configuration;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.Setup;

public class TestDefaultMessageUpdater
{
    [Fact]
    public void TestDefaultWriteMessage()
    {
        //SETUP
        var status = new TestBizActionStatus();

        //ATTEMPT
        DefaultMessageUpdater.UpdateSuccessMessageOnGoodWrite(status, new GenericBizRunnerConfig());

        //VERIFY
        status.HasErrors.ShouldBeFalse();
        status.Message.ShouldEqual("Successfully saved.");
    }

    [Fact]
    public void TestAddWriteMessage()
    {
        //SETUP
        var status = new TestBizActionStatus();
        status.Message = "My action";

        //ATTEMPT
        DefaultMessageUpdater.UpdateSuccessMessageOnGoodWrite(status, new GenericBizRunnerConfig());

        //VERIFY
        status.HasErrors.ShouldBeFalse();
        status.Message.ShouldEqual("My action saved.");
    }

    [Fact]
    public void TestAddWriteMessageNoAddedSaved()
    {
        //SETUP
        var status = new TestBizActionStatus();
        status.Message = "My action.";

        //ATTEMPT
        DefaultMessageUpdater.UpdateSuccessMessageOnGoodWrite(status, new GenericBizRunnerConfig());

        //VERIFY
        status.HasErrors.ShouldBeFalse();
        status.Message.ShouldEqual("My action.");
    }

    private class TestBizActionStatus : BizActionStatus
    {
    }
}