using Serilog;
using Serilog.Core;

namespace InnoClinic.Authorization.API.Extensions
{
    /// <summary>
    /// Provides extension methods for configuring and creating Serilog logger instances.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Creates a Serilog logger with the specified configuration.
        /// </summary>
        /// <param name="loggerConfiguration">The logger configuration to use for creating the logger.</param>
        /// <returns>A configured <see cref="Logger"/> instance.</returns>
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