﻿using Xunit;

namespace IFVM.Core.Tests
{
    public class SanityTests
    {
        [Fact]
        public void boolean_checks()
        {
            Assert.True(true, "If this fails, all hope is lost.");
            Assert.False(false, "If this fails, all hope is lost.");
        }
    }
}
