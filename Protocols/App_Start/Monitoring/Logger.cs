using log4net;
using log4net.Config;

namespace Protocols.Monitoring
{
    public static class Logger
    {
        public static ILog Log => LogManager.GetLogger("ProtocolsLogger");
        public static void InitLogger() => XmlConfigurator.Configure();
    }
}