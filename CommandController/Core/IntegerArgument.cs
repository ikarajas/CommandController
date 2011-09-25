using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// An integer argument.
    /// </summary>
    public class IntegerArgument : Argument<int>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IntegerArgument"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="friendlyName">The user-friendly argument name.</param>
        /// <param name="description">The description.</param>
        /// <param name="required">Indicates whether this <see cref="IArgument"/> is required.
        /// Cannot be <c>true</c> if <paramref name="exclusive"/> is <c>true</c>.</param>
        /// <param name="exclusive">Indicates whether this <see cref="IArgument"/> can be used in conjunction with other arguments.
        /// Cannot be <c>true</c> if <paramref name="required"/> is <c>true</c>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="required"/> and <paramref name="exclusive"/> are both <c>true</c>.
        /// </exception>
        public IntegerArgument(string id, string friendlyName, string description, bool required, bool exclusive)
            : base(id, friendlyName, description, required, exclusive)
        {
        }

        public int GetValue(int defaultOnUnspecified)
        {
            return (Value == 0) ? defaultOnUnspecified : Value;
        }
    }
}
