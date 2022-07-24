using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using HBDStack.EfCore.BizAction;
using HBDStack.EfCore.BizActions.Abstraction;

namespace Tests.Biz;

// public interface ITestBiz : IGenericAction<TestModel, TestModel>
// {
// }

public interface ITestBizAsync : IGenericActionAsync<TestModel, TestModel>
{
}

// public interface ITestBizInOnly : IGenericActionInOnly<TestModel>
// {
// }

public interface ITestBizInOnlyAsync : IGenericActionInOnlyAsync<TestModel>
{
}

// public class TestBiz : BizActionStatus, ITestBiz
// {
//     #region Methods
//
//     public TestModel BizAction(TestModel inputData)
//     {
//         return inputData;
//     }
//
//     #endregion Methods
// }

public class TestBizAsync : BizActionStatus, ITestBizAsync
{
    #region Methods

    public Task<TestModel> BizActionAsync(TestModel inputData, CancellationToken cancellationToken= default)
    {
        return Task.FromResult(inputData);
    }

    #endregion Methods
}

// public class TestBizInOnly : BizActionStatus, ITestBizInOnly
// {
//     #region Methods
//
//     public void BizAction(TestModel inputData)
//     {
//     }
//
//     #endregion Methods
// }

public class TestBizInOnlyAsync : BizActionStatus, ITestBizInOnlyAsync
{
    #region Methods

    public Task BizActionAsync(TestModel inputData, CancellationToken cancellationToken= default)
    {
        return Task.CompletedTask;
    }

    #endregion Methods
}

public class TestModel
{
    #region Properties

    [Required] public string Name { get; set; }

    #endregion Properties
}

internal class TestModelValidator : AbstractValidator<TestModel>
{
    #region Constructors

    public TestModelValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }

    #endregion Constructors
}