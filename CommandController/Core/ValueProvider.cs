using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    internal abstract class ValueProvider : IValueProvider
    {
        #region IValueProvider Members

        /// <summary>
        /// Gets the value for a <see cref="StringArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        public abstract string GetValue(StringArgument argument);

        /// <summary>
        /// Gets the value for an <see cref="IntegerArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        public abstract int? GetValue(IntegerArgument argument);

        /// <summary>
        /// Gets the value for a <see cref="FlagArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        public abstract bool GetValue(FlagArgument argument);

        /// <summary>
        /// Gets the value for <paramref name="argument"/> and returns it as an <see cref="Object"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value for <paramref name="argument"/> as an <see cref="Object"/>.</returns>
        /// <exception cref="InvalidOperationArgumentException">
        /// May be thrown if the value for the argument cannot be converted to the appropriate type.
        /// </exception>
        public object GetValueAsObject(IArgument argument)
        {
            object value;
            // TODO: Move this logic into a value provider superclass.
            if (argument is FlagArgument)
            {
                value = GetValue((FlagArgument)argument);
            }
            else if (argument is IntegerArgument)
            {
                // Note: this can throw an InvalidOperationArgumentException.
                value = GetValue((IntegerArgument)argument);
            }
            else if (argument is StringArgument)
            {
                value = GetValue((StringArgument)argument);
            }
            else
            {
                throw new Exception("This should not happen.");
            }
            return value;
        }

        #endregion

    }
}
