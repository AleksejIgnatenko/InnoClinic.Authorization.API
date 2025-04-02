using Serilog;
using Serilog.Core;

namespace InnoClinic.Authorization.API.Extensions
{
    public static class LoggerExtensions
    {
        public static Logger CreateSerilog(this LoggerConfiguration loggerConfiguration)
        {
            Logger logger = loggerConfiguration
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            return logger;
        }
    }
}
