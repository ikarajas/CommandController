using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommandController.Core;

namespace CommandController.FrontEnds.CommandLine
{
    internal class CommandLineValueProvider : ValueProvider
    {
        private string[] _subArgs;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineValueProvider"/> class.
        /// </summary>
        /// <param name="subArgs">The operation-specific command line arguments
        /// (e.g. everything after "ExeName /operationid").</param>
        internal CommandLineValueProvider(string[] subArgs)
        {
            _subArgs = subArgs;
        }

        private static bool GetCommandLineArgumentValueAsString(string searchKey, string[] args, out string foundValue)
        {
            searchKey = searchKey.ToLowerInvariant();
            foreach (string arg in args)
            {
                int equalsPos = arg.IndexOf('=');

                if (equalsPos > 0)
                {
                    string key = arg.Substring(0, equalsPos);
                    if (key.ToLowerInvariant() == searchKey)
                    {
                        int startPos = equalsPos + 1;
                        foundValue = arg.Substring(startPos, arg.Length - startPos);
                        return true;
                    }
                }
            }
            foundValue = null;
            return false;
        }

        #region IValueProvider Members

        /// <summary>
        /// Gets the value for a <see cref="StringArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        public override string GetValue(StringArgument argument)
        {
            string value;
            GetCommandLineArgumentValueAsString(argument.Id, _subArgs, out value);
            return value;
        }

        /// <summary>
        /// Gets the value for an <see cref="IntegerArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        public override int? GetValue(IntegerArgument argument)
        {
            string value;
            if (GetCommandLineArgumentValueAsString(argument.Id, _subArgs, out value))
            {
                int retval;
                if (StaticUtils.TryParseValue(value, null, out retval))
                {
                    return retval;
                }
                else
                {
                    throw new InvalidOperationArgumentException(argument);
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the value for a <see cref="FlagArgument"/>.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <returns>The value.</returns>
        public override bool GetValue(FlagArgument argument)
        {
            return (from string arg in _subArgs select arg.ToLowerInvariant()).Contains(argument.Id.ToLowerInvariant());
        }

        #endregion
    }
}
