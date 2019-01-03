using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TrainingTask.Common.Exceptions
{
    public class ForeignKeyViolationException : Exception
    {
        public ForeignKeyViolationException()
        {
        }

        public ForeignKeyViolationException(string message) : base(message)
        {
        }

        public ForeignKeyViolationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ForeignKeyViolationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
