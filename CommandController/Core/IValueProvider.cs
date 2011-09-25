using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    /// <summary>
    /// A provider that extracts argument values over a given interface (e.g. command line, form UI, etc).
    /// </summary>
    internal interface IValueProvider
    {
        // TODO: Introduce ArgumentTypeNotSupported exception for when an argument type is not supported by a provider.

        /// <summary>
        /// Gets the value for a <see cref="StringArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        string GetValue(StringArgument argument);

        /// <summary>
        /// Gets the value for an <see cref="IntegerArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        int? GetValue(IntegerArgument argument);

        /// <summary>
        /// Gets the value for a <see cref="FlagArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        bool GetValue(FlagArgument argument);

        /// <summary>
        /// Gets the value for <paramref name="argument"/> and returns it as an <see cref="Object"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value for <paramref name="argument"/> as an <see cref="Object"/>.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        object GetValueAsObject(IArgument argument);
    }
}
