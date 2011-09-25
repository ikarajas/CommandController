using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Thrown to indicate that a required <see cref="IArgument"/> has not been set.
    /// </summary>
    internal class ArgumentRequirementException : ArgumentViolationException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentRequirementException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        public ArgumentRequirementException(IArgument violatedArgument)
            : base(violatedArgument)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentRequirementException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        public ArgumentRequirementException(IArgument violatedArgument, string message)
            : base(violatedArgument, message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentRequirementException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ArgumentRequirementException(IArgument violatedArgument, string message, Exception innerException)
            : base(violatedArgument, message, innerException)
        {
        }
    }
}
