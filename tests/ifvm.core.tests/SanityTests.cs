using Xunit;

namespace IFVM.Core.Tests
{
    public class SanityTests
    {
        [Fact]
        public void true_is_true()
        {
            Assert.True(true, "If this fails, all hope is lost.");
        }
    }
}
