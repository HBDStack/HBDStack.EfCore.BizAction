﻿// Copyright (c) 2018 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT licence. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using HBDStack.StatusGeneric;
using Xunit;
using Xunit.Abstractions;
using Xunit.Extensions.AssertExtensions;

namespace Test;

public class TestStatusGeneric
{
    private readonly ITestOutputHelper _testOutputHelper;

    public TestStatusGeneric(ITestOutputHelper testOutputHelper) => _testOutputHelper = testOutputHelper;

    [Fact]
    public void ValidateResult_To_Generic()
    {
        var rs = new ValidationResult("Phone number is wrong", new[] {"PhoneNumber"}).ToGeneric();
        rs.ErrorMessage.ShouldContain("Phone number is wrong");
        rs.MemberNames.ShouldContain("PhoneNumber");

        _testOutputHelper.WriteLine(JsonSerializer.Serialize(rs, new JsonSerializerOptions {WriteIndented = true}));
    }

    [Fact]
    public void GenericValidateResult()
    {
        var rs = new GenericValidationResult("Error_Code","Phone number is wrong", new [] {"PhoneNumber"});
        rs.ErrorCode.ShouldContain("Error_Code");
        rs.ErrorMessage.ShouldContain("Phone number is wrong");
        rs.MemberNames.ShouldContain("PhoneNumber");

        _testOutputHelper.WriteLine(JsonSerializer.Serialize(rs, new JsonSerializerOptions {WriteIndented = true}));
    }

    [Fact]
    public void TestGenericStatusOk()
    {
        //SETUP 

        //ATTEMPT
        var status = new StatusGenericHandler();

        //VERIFY
        status.IsValid.ShouldBeTrue();
        status.Errors.Any().ShouldBeFalse();
        status.Message.ShouldEqual("Success");
    }

    [Fact]
    public void TestGenericStatusSetMessageOk()
    {
        //SETUP 
        var status = new StatusGenericHandler {Message = "New message"};

        //VERIFY
        status.IsValid.ShouldBeTrue();
        status.HasErrors.ShouldBeFalse();
        status.Errors.Any().ShouldBeFalse();
        status.Message.ShouldEqual("New message");
    }

    [Fact]
    public void TestGenericStatusSetMessageViaInterfaceOk()
    {
        //SETUP 
        IStatusGeneric status = new StatusGenericHandler();

        //ATTEMPT
        status.Message = "New message";

        //VERIFY
        status.IsValid.ShouldBeTrue();
        status.HasErrors.ShouldBeFalse();
        status.Errors.Any().ShouldBeFalse();
        status.Message.ShouldEqual("New message");
    }

    [Fact]
    public void TestGenericStatusWithTypeSetMessageViaInterfaceOk()
    {
        //SETUP 
        IStatusGeneric status = new StatusGenericHandler<string>();

        //ATTEMPT
        status.Message = "New message";

        //VERIFY
        status.IsValid.ShouldBeTrue();
        status.HasErrors.ShouldBeFalse();
        status.Errors.Any().ShouldBeFalse();
        status.Message.ShouldEqual("New message");
    }

    [Fact]
    public void TestGenericStatusWithErrorOk()
    {
        //SETUP 
        var status = new StatusGenericHandler();

        //ATTEMPT
        status.AddError("This is an error.");

        //VERIFY
        status.IsValid.ShouldBeFalse();
        status.HasErrors.ShouldBeTrue();
        status.Errors.Single().ToString().ShouldEqual("This is an error.");
        status.Errors.Single().DebugData.ShouldBeNull();
        status.Message.ShouldEqual("Failed with 1 error");
    }

    [Fact]
    public void TestGenericStatusCombineStatusesWithErrorsOk()
    {
        //SETUP 
        var status1 = new StatusGenericHandler();
        var status2 = new StatusGenericHandler();

        //ATTEMPT
        status1.AddError("This is an error.");
        status2.CombineStatuses(status1);

        //VERIFY
        status2.IsValid.ShouldBeFalse();
        status2.Errors.Single().ToString().ShouldEqual("This is an error.");
        status2.Message.ShouldEqual("Failed with 1 error");
    }

    [Fact]
    public void TestGenericStatusCombineStatusesIsValidWithMessageOk()
    {
        //SETUP 
        var status1 = new StatusGenericHandler();
        var status2 = new StatusGenericHandler();

        //ATTEMPT
        status1.Message = "My message";
        status2.CombineStatuses(status1);

        //VERIFY
        status2.IsValid.ShouldBeTrue();
        status2.Message.ShouldEqual("My message");
    }

    [Fact]
    public void TestGenericStatusHeaderAndErrorOk()
    {
        //SETUP 
        var status = new StatusGenericHandler("MyClass");

        //ATTEMPT
        status.AddError("This is an error.");

        //VERIFY
        status.IsValid.ShouldBeFalse();
        status.Errors.Single().ToString().ShouldEqual("MyClass: This is an error.");
    }

    [Fact]
    public void TestGenericStatusHeaderCombineStatusesOk()
    {
        //SETUP 
        var status1 = new StatusGenericHandler("MyClass");
        var status2 = new StatusGenericHandler("MyProp");

        //ATTEMPT
        status2.AddError("This is an error.");
        status1.CombineStatuses(status2);

        //VERIFY
        status1.IsValid.ShouldBeFalse();
        status1.Errors.Single().ToString().ShouldEqual("MyClass>MyProp: This is an error.");
        status1.Message.ShouldEqual("Failed with 1 error");
    }

    [Fact]
    public void TestCaptureException()
    {
        //SETUP
        var status = new StatusGenericHandler();

        //ATTEMPT
        try
        {
            MethodToThrowException();
        }
        catch (Exception ex)
        {
            status.AddError(ex, "This is user-friendly error message");
        }

        //VERIFY
        var lines = status.Errors.Single().DebugData.Split(Environment.NewLine);
        lines.Length.ShouldEqual(6);
        lines[0].ShouldEqual("This is a test");
        lines[1].ShouldStartWith("StackTrace:   at Test.TestStatusGeneric.MethodToThrowException()");
        lines[3].ShouldEqual("Data: data1\t1");
        lines[4].ShouldEqual("Data: data2\t2");
    }

    private void MethodToThrowException()
    {
        var ex = new Exception("This is a test");
        ex.Data.Add("data1", 1);
        ex.Data.Add("data2", "2");
        throw ex;
    }

    //------------------------------------

    [Fact]
    public void TestGenericStatusGenericOk()
    {
        //SETUP 

        //ATTEMPT
        var status = new StatusGenericHandler<string>();

        //VERIFY
        status.IsValid.ShouldBeTrue();
        status.Result.ShouldEqual(null);
    }

    [Fact]
    public void TestGenericStatusGenericSetResultOk()
    {
        //SETUP 

        //ATTEMPT
        var status = new StatusGenericHandler<string>();
        status.SetResult("Hello world");

        //VERIFY
        status.IsValid.ShouldBeTrue();
        status.Result.ShouldEqual("Hello world");
    }

    [Fact]
    public void TestGenericStatusGenericSetResultThenErrorOk()
    {
        //SETUP 

        //ATTEMPT
        var status = new StatusGenericHandler<string>();
        status.SetResult("Hello world");
        var statusCopy = status.AddError("This is an error.");

        //VERIFY
        status.IsValid.ShouldBeFalse();
        status.Result.ShouldEqual(null);
        statusCopy.ShouldEqual(status);
    }

    [Fact]
    public void Error_Format_With_Json()
    {
        //SETUP 
        IStatusGenericHandler status = new StatusGenericHandler(nameof(TestStatusGeneric));

        //ATTEMPT

        //Add error without code
        status.AddError("This is an error.");
        status.AddError("Full name is required", "full_name");
        status.AddError("Minimum source amount is required", "min_source_amount_required", new Dictionary<string, object>
        {
            {"min_source", 100}
        });
        status.AddError("Field is required", "required", new []{"first_name", "last_name"});
        status.AddValidationResult(new GenericValidationResult("error_code_123", "The phone number is required.", new[]{"phone_number"}));
        status.AddValidationResult(new ValidationResult("The phone number is required.", new [] {"phone_number", "mobile_number", "currency_name"}));
        status.AddValidationResult(new GenericValidationResult("error_code_444", "The minimum amount in valid.", new[]{"min_amount"})
        {
            References = new Dictionary<string, object> {["min_amount"] = new {Amount = 13000, Currency = "USD"}}
        });
            
        _testOutputHelper.WriteLine(status.GetAllErrors());
        _testOutputHelper.WriteLine(JsonSerializer.Serialize(status.Errors, new JsonSerializerOptions {WriteIndented = true}));
    }
}