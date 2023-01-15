using Microsoft.Extensions.Logging;

namespace MTCG
{
    public static class Logging
    {
        static ILoggerFactory loggerFactory = null;

        public static ILogger Get<T>()
        {
            if (loggerFactory == null)
                loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .SetMinimumLevel(LogLevel.Trace)
                        .ClearProviders()
                        .AddConsole();
                });

            return loggerFactory.CreateLogger(typeof(T));
        }
    }
}
