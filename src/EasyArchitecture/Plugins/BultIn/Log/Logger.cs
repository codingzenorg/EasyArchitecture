using System;
using System.IO;
using EasyArchitecture.Plugins.Contracts.Log;

namespace EasyArchitecture.Plugins.BultIn.Log
{
    internal class Logger : ILogger
    {
        private readonly FileInfo _arquivo;

        public Logger(FileInfo arquivo)
        {
            _arquivo = arquivo;
        }

        private const string DefaultPath = "Log";
        private const string DefaultExtension = ".log";

        public void Log(LogLevel logLevel, Guid identifier, object message, Exception exception)
        {
            using (var writer = _arquivo.AppendText())
            {
                writer.WriteLine(
                    string.Format("{0} [{1}] [{2}] {3} {4}{5}",
                        DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss,fff"),
                        System.Threading.Thread.CurrentThread.Name,
                        identifier,
                        logLevel.ToString().ToUpperInvariant(),
                        message,
                        exception)
                );
            }
        }
    }
}