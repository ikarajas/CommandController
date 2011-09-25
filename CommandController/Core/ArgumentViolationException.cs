using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Thrown to indicate that a restriction around an <see cref="IArgument"/> has been violated.
    /// </summary>
    internal abstract class ArgumentViolationException : Exception
    {
        /// <summary>
        /// Gets the violated argument.
        /// </summary>
        /// <value>The violated argument.</value>
        public IArgument ViolatedArgument
        {
            get { return _violatedArgument; }
        }
        private IArgument _violatedArgument;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentViolationException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        public ArgumentViolationException(IArgument violatedArgument)
            : base()
        {
            if (violatedArgument == null)
            {
                throw new ArgumentNullException("violatedArgument");
            }
            _violatedArgument = violatedArgument;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentViolationException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        public ArgumentViolationException(IArgument violatedArgument, string message)
            : base(message)
        {
            if (violatedArgument == null)
            {
                throw new ArgumentNullException("violatedArgument");
            }
            _violatedArgument = violatedArgument;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArgumentViolationException"/> class.
        /// </summary>
        /// <param name="violatedArgument">The violated argument.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ArgumentViolationException(IArgument violatedArgument, string message, Exception innerException)
            : base(message, innerException)
        {
            if (violatedArgument == null)
            {
                throw new ArgumentNullException("violatedArgument");
            }
            _violatedArgument = violatedArgument;
        }
    }
}
