using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Interface for operation arguments.
    /// </summary>
    public interface IArgument
    {
        /// <summary>
        /// The argument ID. Used, for example, as a command line argument.
        /// </summary>
        /// <value>The argument ID.</value>
        string Id { get; }

        /// <summary>
        /// Gets a user-friendly argument name.
        /// </summary>
        /// <value>The user-friendly argument name.</value>
        string FriendlyName { get; }

        /// <summary>
        /// A description of the argument.
        /// </summary>
        /// <value>The description string.</value>
        string Description { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IArgument"/> is required.
        /// </summary>
        /// <remarks>
        /// This can never be <c>true</c> while <see cref="Exclusive"/> is also <c>true</c>.
        /// Additionally, the presence of a required argument will not be enforced if an
        /// <see cref="Exclusive"/> argument has been set to <c>true</c>.
        /// </remarks>
        /// <value><c>true</c> if the argument is required; otherwise, <c>false</c>.</value>
        bool Mandatory { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="IArgument"/> can be used in conjunction
        /// with other arguments. If it is <c>true</c>, the <see cref="OperationController"/> will
        /// flag the error and prevent the given operation from running.
        /// </summary>
        /// <remarks>
        /// This can never be <c>true</c> while <see cref="Mandatory"/> is also <c>true</c>.
        /// </remarks>
        /// <value><c>true</c> if the argument is exclusive; otherwise, <c>false</c>.</value>
        bool Exclusive { get; }

    }
}
