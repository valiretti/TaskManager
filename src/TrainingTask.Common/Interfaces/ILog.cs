using System;
using System.Collections.Generic;
using System.Text;

namespace TrainingTask.Common.Interfaces
{
    public interface ILog : IDisposable
    {
        /// <summary>
        /// Log a message object with the Debug level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Debug(string message);

        /// <summary>
        /// Log a message object with the Error level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Error(string message);

        /// <summary>
        /// Log a message object with the Info level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Info(string message);

        /// <summary>
        /// Log a message object with the Fatal level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Fatal(string message);

        /// <summary>
        /// Log a message object with the Trace level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Trace(string message);

        /// <summary>
        /// Log a message object with the Warn level.
        /// </summary>
        /// <param name="message">The message object to log.</param>
        void Warn(string message);
    }
}
