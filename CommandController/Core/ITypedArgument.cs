using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// Interface for an argument that defines a value of a specific type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface ITypedArgument<T>
    {
        /// <summary>
        /// Gets the type of the value associated with the argument.
        /// </summary>
        /// <value>The type.</value>
        Type Type { get; }

        /// <summary>
        /// Gets the supplied value for the argument. If <typeparamref name="T"/> is a value type, the return value
        /// for an unspecified argument will be whatever value is returned by <c>default(T)</c>. For example, in the
        /// case of an unspecified integer argument, the returned value will be 0.
        /// </summary>
        /// <value>The value.</value>
        T Value { get; }
    }
}
