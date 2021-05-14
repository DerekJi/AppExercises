using System;
using Xunit;
using Xunit.Abstractions;

namespace AppEx.Tests
{
    public abstract class TestFixtureBed : IDisposable, IClassFixture<DependencyInjectionsFixture>
    {
        protected IServiceProvider localServiceProvider { get; set; }
        protected ITestOutputHelper localOutput { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="di"></param>
        public TestFixtureBed(DependencyInjectionsFixture di, ITestOutputHelper testOutputHelper)
        {
            localServiceProvider = di.ServiceProvider;
            localOutput = testOutputHelper;
        }

        /// <summary>
        /// Release resources
        /// </summary>
        public virtual void Dispose()
        {
            localServiceProvider = null;
        }
    }
}
