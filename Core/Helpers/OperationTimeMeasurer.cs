using System;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Core.Helpers
{
    public class OperationTimeMeasurer : IDisposable
    {
        private readonly ConcurrentDictionary<string, string> operationsTimes;
        private readonly Stopwatch stopwatch;
        private readonly string operation;

        public OperationTimeMeasurer(ConcurrentDictionary<string, string> operationsTimes, string operation)
        {
            this.operationsTimes = operationsTimes;
            this.operation = operation;

            stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            operationsTimes[operation] = $"{stopwatch.ElapsedMilliseconds}ms";
        }
    }
}