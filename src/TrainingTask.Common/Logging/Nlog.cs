using System;
using System.Collections.Generic;
using System.Text;
using NLog;
using TrainingTask.Common.Interfaces;

namespace TrainingTask.Common.Logging
{
    public class Nlog : ILog
    {
        private static readonly Logger Log = LogManager.GetCurrentClassLogger();

        public void Dispose() => LogManager.Shutdown();
       
        public void Debug(string message) => Log.Debug(message);

        public void Error(string message) => Log.Error(message);

        public void Info(string message) => Log.Info(message);

        public void Fatal(string message) => Log.Fatal(message);

        public void Trace(string message) => Log.Trace(message);

        public void Warn(string message) => Log.Warn(message);
    }
}
