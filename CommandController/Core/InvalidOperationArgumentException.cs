using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Thrown to indicate that the value specified for an <see cref="IArgument"/> value is invalid.
    /// </summary>
    internal class InvalidOperationArgumentException : ArgumentViolationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationArgumentException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        public InvalidOperationArgumentException(IArgument violatedArgument)
            : base(violatedArgument)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationArgumentException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        public InvalidOperationArgumentException(IArgument violatedArgument, string message)
            : base(violatedArgument, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidOperationArgumentException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidOperationArgumentException(IArgument violatedArgument, string message, Exception innerException)
            : base(violatedArgument, message, innerException)
        {
        }
    }
}
