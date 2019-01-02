using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TrainingTask.Common.Exceptions
{
    public class UniquenessViolationException : Exception
    {
        public UniquenessViolationException()
        {
        }

        public UniquenessViolationException(string message) : base(message)
        {
        }

        public UniquenessViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UniquenessViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
