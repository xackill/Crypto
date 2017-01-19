using System;
using System.Diagnostics;

namespace Test
{
    public class ConsoleMonitoring : IDisposable
    {
        public readonly Stopwatch Stopwatch;
        public readonly string OperationTitle;

        public ConsoleMonitoring(string operationTitle)
        {
            OperationTitle = operationTitle;
            Stopwatch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            Stopwatch.Stop();
            Console.WriteLine($"{OperationTitle} - {Stopwatch.ElapsedMilliseconds}ms");
        }
    }
}