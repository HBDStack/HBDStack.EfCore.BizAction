using HBD.EfCore.BizAction.Configuration;
using Xunit;
using Xunit.Extensions.AssertExtensions;

namespace Tests.UnitTests.Setup;

public class TestGenericConfig
{
    #region Methods

    [Fact]
    public void ShouldNotValidateByDefault()
    {
        new GenericBizRunnerConfig().DoNotValidateSaveChanges
            .ShouldBeTrue();
    }

    #endregion Methods
}