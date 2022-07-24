using System.Linq;
using HBDStack.EfCore.BizAction;
using Xunit;

namespace Tests.UnitTests.TestActionsAsync;

public class TestAsyncMethodParameters
{
    #region Methods

    [Fact]
    public void AllAsyncMethodShouldSupportCancellationToken()
    {
        var methods = typeof(BizActionStatus).Assembly.GetExportedTypes()
            .SelectMany(t => t.GetMethods()).Where(m => m.ReturnType.Name.StartsWith("Task"));

        foreach (var method in methods)
        {
            if (method.Name.StartsWith("GetDtoAsync")) continue;
            if (method.GetParameters().Any(p => p.ParameterType.Name.StartsWith("CancellationToken"))) continue;
            Assert.True(false,
                $"Method {method.Name} of {method.DeclaringType.Name} should support CancellationToken");
        }
    }

    #endregion Methods
}