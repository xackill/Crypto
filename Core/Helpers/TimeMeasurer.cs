using System.Collections.Concurrent;
using System.Linq;
using Core.Extensions;

namespace Core.Helpers
{
    public class TimeMeasurer
    {
        private readonly ConcurrentDictionary<string, string> operationsTimes;

        public TimeMeasurer()
        {
            operationsTimes = new ConcurrentDictionary<string, string>();
        }

        public OperationTimeMeasurer StartOperation(string operation)
            => new OperationTimeMeasurer(operationsTimes, operation);

        public string Results
            => operationsTimes.Select(g => $"{g.Key} -- {g.Value}").JoinStrings("\n");
    }
}