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

        public void Debug(string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} Debug: {message}");
            }
        }

        public void Error(string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} Error: {message}");
            }
        }

        public void Info(string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} Info: {message}");
            }
        }

        public void Fatal(string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} Fatal: {message}");
            }
        }

        public void Trace(string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} Trace: {message}");
            }
        }

        public void Warn(string message)
        {
            lock (lockObj)
            {
                _file.Write($"{DateTime.Now} Warn: {message}");
            }
        }

        public void Dispose()
        {
            lock (lockObj)
            {
                _file.Flush();
                _file.Close();
            }
        }
    }
}
