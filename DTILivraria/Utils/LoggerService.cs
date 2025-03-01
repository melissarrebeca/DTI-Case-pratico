using System;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using System.IO;

namespace DTILivraria.Utils
{
    public static class LoggerService
    {
        private static Logger _logger;
        
        public static void InitializeLogger()
        {
            string logDirectory = "Logs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }
            
            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File(
                    Path.Combine(logDirectory, "log_.txt"),
                    rollingInterval: RollingInterval.Day,
                    restrictedToMinimumLevel: LogEventLevel.Debug,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
                )
                .Enrich.WithThreadId()
                .Enrich.WithEnvironmentName()
                .CreateLogger();
                
            // Log de inicialização
            _logger.Information("Sistema de Logs inicializado com sucesso");
        }
        
        public static void Debug(string message)
        {
            _logger?.Debug(message);
        }
        
        public static void Information(string message)
        {
            _logger?.Information(message);
        }
        
        public static void Warning(string message)
        {
            _logger?.Warning(message);
        }
        
        public static void Error(string message)
        {
            _logger?.Error(message);
        }
        
        public static void Error(Exception ex, string message)
        {
            _logger?.Error(ex, message);
        }
        
        public static void Critical(string message)
        {
            _logger?.Fatal(message);
        }
        
        public static void Critical(Exception ex, string message)
        {
            _logger?.Fatal(ex, message);
        }
    }
}