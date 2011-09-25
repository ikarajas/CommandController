using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// A flag argument. A flag can be either set or not set.
    /// </summary>
    public class FlagArgument : Argument<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FlagArgument"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="friendlyName">The user-friendly argument name.</param>
        /// <param name="description">The description.</param>
        /// <param name="exclusive">Indicates whether this <see cref="IArgument"/> can be used in conjunction with other arguments.
        /// Cannot be <c>true</c> if <paramref name="required"/> is <c>true</c>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="required"/> and <paramref name="exclusive"/> are both <c>true</c>.
        /// </exception>
        public FlagArgument(string id, string friendlyName, string description, bool exclusive)
            : base(id, friendlyName, description, false, exclusive)
        {
        }
    }
}
