using System;
using System.Diagnostics;
using System.Threading.Tasks;

using Xunit;

namespace TwilightSparkle.Forum.UnitTests
{
    public abstract class BddAsyncTest : IAsyncLifetime
    {
        private readonly string _traceId;
        private readonly Stopwatch _sw;


        public BddAsyncTest()
        {
            _traceId = $"{Guid.NewGuid()} - {GetType()}";
            _sw = new Stopwatch();
        }


        public Task InitializeAsync()
        {
            _sw.Start();
            Trace.WriteLine($"@_ Initialise {_traceId} ");

            return Setup();
        }

        public Task DisposeAsync()
        {
            _sw.Stop();
            Trace.WriteLine($"@_ Dispose {_sw.Elapsed.TotalSeconds}s - {_traceId} ");

            return Task.CompletedTask;
        }


        protected abstract Task Setup();
    }
}
