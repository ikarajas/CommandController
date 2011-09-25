using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Diagnostics.CodeAnalysis;

namespace CommandController
{
    /// <summary>
    /// A class containing static helper methods.
    /// </summary>
    public static class StaticUtils
    {
        /// <summary>
        /// Attempts to parse <paramref name="valueString"/> for an integer value, printing <paramref name="errorMessage"/>
        /// to the console if the string could not be evaluated successfully.
        /// </summary>
        /// <param name="valueString">The string to parse.</param>
        /// <param name="errorMessage">The message to print on error.</param>
        /// <param name="value">If successful, the integer value is written to this output parameter.</param>
        /// <returns><c>true</c> if <paramref name="valueString"/> could be parsed successfully, otherwise <c>false</c>.</returns>
        public static bool TryParseValue(string valueString, string errorMessage, out int value)
        {
            try
            {
                value = int.Parse(valueString, CultureInfo.InvariantCulture);
                return true;
            }
            catch (FormatException)
            {
                if (!String.IsNullOrEmpty(errorMessage))
                {
                    Console.WriteLine(errorMessage);
                }
                value = -1;
                return false;
            }
        }

        /// <summary>
        /// An action designed to be repeated 0 or more times using the <see cref="Repeat"/> method.
        /// </summary>
        /// <param name="iterationNum">The iteration number.</param>
        public delegate void RepeatableAction(int iterationNum);

        /// <summary>
        /// Repeats <paramref name="action"/> a number of times. Functionally similar to a <c>for</c> loop.
        /// The number of times the action is repeated is determined by subtracting <paramref name="iterationStartIndex"/>
        /// from <paramref name="iterations"/>.
        /// </summary>
        /// <param name="iterationStartIndex">Start index of the iteration.</param>
        /// <param name="iterations">The number iterations.</param>
        /// <param name="action">The action to repeat.</param>
        public static void Repeat(int iterationStartIndex, int iterationEndIndex, RepeatableAction action)
        {
            for (int i = iterationStartIndex; i < iterationEndIndex; i++)
            {
                action(i);
            }
        }

        /// <summary>
        /// An action designed to be retried, if necessary, using the <see cref="RetryOnFail"/> method.
        /// </summary>
        [SuppressMessage("Microsoft.Naming", "CA1704", Justification = "\"Retryable\" is intentional.")]
        public delegate void RetryableAction();

        /// <summary>
        /// Calls <paramref name="action"/> and retries a given number of times if it throws an exception.
        /// If an exception is thrown, <paramref name="actionDescriptionOnError"/> is printed to the console,
        /// along with details on the exception.
        /// </summary>
        /// <param name="attemptsAllowed">The number of attempts allowed.</param>
        /// <param name="actionDescriptionOnError">A message to print to the console if the action throws an exception.</param>
        /// <param name="action">The action.</param>
        /// <returns><c>true</c>, if the action could be run successfully (at least once), or <c>false</c> if not.</returns>
        [SuppressMessage("Microsoft.Design", "CA1031", Justification = "We want to catch everything that action can throw here.")]
        public static bool RetryOnFail(int attemptsAllowed, string actionDescriptionOnError, RetryableAction action)
        {
            bool success = false;
            int attempts = 0;
            while (!success && (attempts < attemptsAllowed))
            {
                try
                {
                    action();
                    success = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(actionDescriptionOnError);
                    PrintExceptionDetails(ex);
                }
                attempts++;
            }
            return success;
        }

        private static Dictionary<int, string> _paddingMap;
        private static object _paddingMapLockObj = new object();

        /// <summary>
        /// Prepends zeroes to <paramref name="num"/> and returns it as a string.
        /// Enough zeroes will be prepended such that the value returned will have
        /// <paramref name="length"/> digits.
        /// </summary>
        /// <param name="num">The number, as an integer.</param>
        /// <param name="length">The number of digits required.</param>
        /// <returns>The padded number string.</returns>
        public static string PadWithZeros(int num, int length)
        {
            if (_paddingMap == null)
            {
                lock (_paddingMapLockObj)
                {
                    if (_paddingMap == null)
                    {
                        Dictionary<int, string> temp = new Dictionary<int, string>();
                        temp.Add(1, "0");
                        temp.Add(2, "00");
                        temp.Add(3, "000");
                        temp.Add(4, "0000");
                        temp.Add(5, "00000");
                        temp.Add(6, "000000");
                        temp.Add(7, "0000000");
                        temp.Add(8, "00000000");
                        temp.Add(9, "000000000");
                        temp.Add(10, "0000000000");
                        _paddingMap = temp;
                    }
                }
            }

            string numStr = num.ToString(CultureInfo.InvariantCulture);
            int requiredPaddingLength = length - numStr.Length;
            if (requiredPaddingLength < 0)
            {
                return numStr;
            }

            if (_paddingMap.ContainsKey(requiredPaddingLength))
            {
                return _paddingMap[requiredPaddingLength] + numStr;
            }
            else
            {
                throw new ArgumentException("That's way too much padding.");
            }
        }

        /// <summary>
        /// Prints details on <paramref name="ex"/> to the console, including full inner exception details.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public static void PrintExceptionDetails(Exception ex)
        {
            Console.WriteLine("{0}: {1}", ex.GetType().FullName, ex.Message);
            Console.WriteLine(ex.StackTrace);
            Exception currentException = ex;
            Exception innerException = null;

            if (currentException.InnerException == null)
            {
                Console.WriteLine("No inner exception.");
            }

            while ((innerException = currentException.InnerException) != null)
            {
                currentException = innerException;
                Console.WriteLine();
                Console.WriteLine("---{0}Inner exception:{0}{1}: {2}{0}{3}",
                    Environment.NewLine,
                    currentException.GetType().FullName,
                    currentException.Message,
                    currentException.StackTrace);
            }
        }

        /// <summary>
        /// Extension method for <see cref="System.Type"/>. Determines if an instance of <see cref="Type"/>
        /// denotes a class that inherits from <paramref name="inheritTest"/>.
        /// </summary>
        /// <param name="t">The instance of <see cref="Type"/> to inspect.</param>
        /// <param name="inheritTest">The type to search for in the inheritance hierarchy.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="t"/> is a type that inherits from
        /// <paramref name="inheritTest"/>, otherwise <c>false</c>.
        /// </returns>
        public static bool InheritsFrom(this Type t, Type inheritTest)
        {
            if (t.IsInterface)
                return false;

            if (t.BaseType == typeof(object))
            {
                return false;
            }
            else
            {
                if (t.BaseType == inheritTest)
                {
                    return true;
                }
                else
                {
                    return t.BaseType.InheritsFrom(inheritTest);
                }
            }
        }
    }
}
