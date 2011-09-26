using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace CommandController.Core
{
    /// <summary>
    /// A simple argument.
    /// </summary>
    public abstract class Argument<T> : IArgument, ITypedArgument<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Argument"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="friendlyName">The user-friendly argument name.</param>
        /// <param name="description">The description.</param>
        /// <param name="required">
        /// Indicates whether this <see cref="IArgument"/> is required.
        /// Cannot be <c>true</c> if <paramref name="exclusive"/> is <c>true</c>.
        /// </param>
        /// <param name="exclusive">
        /// Indicates whether this <see cref="IArgument"/> can be used in conjunction with other arguments.
        /// Cannot be <c>true</c> if <paramref name="required"/> is <c>true</c>.
        /// </param>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="required"/> and <paramref name="exclusive"/> are both <c>true</c>.
        /// </exception>
        public Argument(string id, string friendlyName, string description, bool required, bool exclusive)
        {
            if (required && exclusive)
            {
                throw new ArgumentException("An argument cannot be both required and exclusive.");
            }

            _id = id;
            _friendlyName = friendlyName;
            _description = description;
            _required = required;
            _exclusive = exclusive;
        }

        #region IArgument Members

        /// <summary>
        /// The argument ID. Used, for example, as a command line argument.
        /// </summary>
        /// <value>The argument ID.</value>
        public string Id
        {
            get { return _id; }
        }
        private string _id;

        /// <summary>
        /// Gets a user-friendly argument name.
        /// </summary>
        /// <value>The user-friendly argument name.</value>
        public string FriendlyName
        {
            get { return _friendlyName; }
        }
        private string _friendlyName;

        /// <summary>
        /// A description of the argument.
        /// </summary>
        /// <value>The description string.</value>
        public string Description
        {
            get { return _description; }
        }
        private string _description;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IArgument"/> is required.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the argument is required; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This can never be <c>true</c> while <see cref="Exclusive"/> is also <c>true</c>.
        /// Additionally, the presence of a required argument will not be enforced if an
        /// <see cref="Exclusive"/> argument has been set to <c>true</c>.
        /// </remarks>
        public bool Mandatory
        {
            get { return _required; }
        }
        private bool _required;

        /// <summary>
        /// Gets a value indicating whether this <see cref="IArgument"/> can be used in conjunction
        /// with other arguments. If it is <c>true</c>, the <see cref="OperationController"/> will
        /// flag the error and prevent the given operation from running.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the argument is exclusive; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// This can never be <c>true</c> while <see cref="Mandatory"/> is also <c>true</c>.
        /// </remarks>
        public bool Exclusive
        {
            get { return _exclusive; }
        }
        private bool _exclusive;

        #endregion

        #region ITypedArgument<T> Members

        /// <summary>
        /// Gets the type of the value associated with the argument.
        /// </summary>
        /// <value>The type.</value>
        public Type Type
        {
            get { return typeof(T); }
        }

        /// <summary>
        /// Gets the supplied value for the argument. If <typeparamref name="T"/> is a value type, the return value
        /// for an unspecified argument will be whatever value is returned by <c>default(T)</c>. For example, in the
        /// case of an unspecified integer argument, the returned value will be 0.
        /// </summary>
        /// <value>The value.</value>
        public T Value
        {
            get
            {
                T value;
                if (OperationController.GetCachedArgumentValue<T>(this, out value))
                {
                    return value;
                }
                else
                {
                    if (typeof(T) == typeof(bool))
                    {
                        Debug.Assert(false, "A flag argument's value should always be evaluated to true or false.");
                    }
                    return default(T);
                }
            }
        }

        #endregion
    }
}
