﻿using Microsoft.Extensions.Logging;
using System.Reflection;

namespace WebStore.Loggin
{
    public static class Log4NetLoggerFactoryExtensions
    {
        private static string CheckFilePath(string FilePath)
        {
            if (FilePath is not { Length: > 0 })
                throw new ArgumentException("Не указан путь к файлу", nameof(FilePath));

            if (Path.IsPathRooted(FilePath))
                return FilePath;

            var assembly = Assembly.GetEntryAssembly();
            var dir = Path.GetDirectoryName(assembly.Location);
            return Path.Combine(dir!, FilePath);
        }
        public static ILoggingBuilder AddLog4Net(this ILoggingBuilder builder, string ConfigurationFile = "log4net.config")
        {
            builder.AddProvider(new Log4NetLoggerProvider(ConfigurationFile));
            return builder;
        }
    }
}
