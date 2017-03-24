using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Linq;
using Core.Extensions;

namespace Core.Helpers
{
    public class TimeMeasurer : IDisposable
    {
        private readonly ConcurrentDictionary<string, string> operationsTimes;
        private readonly Stopwatch stopwatch;

        public TimeMeasurer()
        {
            operationsTimes = new ConcurrentDictionary<string, string>();
            stopwatch = Stopwatch.StartNew();
        }

        public OperationTimeMeasurer StartOperation(string operation)
            => new OperationTimeMeasurer(operationsTimes, operation);

        public string Results
            => operationsTimes
                    .OrderBy(g => g.Key)
                    .Select(g => $"{g.Key}: {g.Value}ms")
                    .JoinStrings("\n") 
                    +
                    $"Общее время: {stopwatch.ElapsedMilliseconds}ms";

        public void Dispose()
        {
            stopwatch.Stop();
        }
    }
}