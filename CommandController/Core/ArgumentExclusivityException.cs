using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Thrown to indicate that an exclusive <see cref="IArgument"/> has been set in conjunction with other <see cref="IArguments"/>.
    /// </summary>
    internal class ArgumentExclusivityException : ArgumentViolationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentExclusivityException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        public ArgumentExclusivityException(IArgument violatedArgument)
            : base(violatedArgument)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentExclusivityException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        public ArgumentExclusivityException(IArgument violatedArgument, string message)
            : base(violatedArgument, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentExclusivityException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ArgumentExclusivityException(IArgument violatedArgument, string message, Exception innerException)
            : base(violatedArgument, message, innerException)
        {
        }
    }
}
