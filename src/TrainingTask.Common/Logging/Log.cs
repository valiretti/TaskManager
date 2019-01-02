using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TrainingTask.Common.Interfaces;

namespace TrainingTask.Common.Logging
{
    public class Log : ILog
    {
        protected readonly object lockObj = new object();
        private readonly StreamWriter _file;

        public Log(string pathToLogging)
        {
            _file = new StreamWriter(pathToLogging, true);
        }

        public void Debug(string message) => Write("Debug", message);

        public void Error(string message) => Write("Error", message);

        public void Info(string message) => Write("Info", message);

        public void Fatal(string message) => Write("Fatal", message);

        public void Trace(string message) => Write("Trace", message);

        public void Warn(string message) => Write("Warn", message);
       
        public void Dispose()
        {
            lock (lockObj)
            {
                _file.Flush();
                _file.Close();
            }
        }

        private void Write(string level, string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} {level}: {message}");
            }
        }
    }
}
