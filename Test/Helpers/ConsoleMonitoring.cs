using System;
using System.Diagnostics;

namespace Test.Helpers
{
    public class ConsoleMonitoring : IDisposable
    {
        private readonly Stopwatch stopwatch;
        private readonly string operationTitle;

        public ConsoleMonitoring(string operationTitle)
        {
            this.operationTitle = operationTitle;
            stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            stopwatch.Stop();
            Console.WriteLine($"{operationTitle} - {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}