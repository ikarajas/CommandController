using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandController.Core;
using System.Reflection;

namespace CommandController.Core
{
    internal static class OperationController
    {
        internal static Operation[] Operations
        {
            get
            {
                if (_operations == null)
                {
                    List<Operation> operationsList = new List<Operation>();
                    foreach (Type type in Assembly.GetEntryAssembly().GetTypes())
                    {
                        if (type.InheritsFrom(typeof(Operation)) && !type.IsAbstract)
                        {
                            ConstructorInfo constructor = null;
                            try
                            {
                                constructor = type.GetConstructor(new Type[0]);
                            }
                            catch (ArgumentException ae)
                            {
                                throw new InvalidOperationException("All Operations must have a default constructor.", ae);
                            }
                            operationsList.Add((Operation)constructor.Invoke(new object[0]));
                        }
                    }

                    _operations = operationsList.ToArray();
                }
                return _operations;
            }

        }

        private static Operation[] _operations;

        internal static Dictionary<string, Operation> OperationsDict
        {
            get
            {
                if (_operationsDict == null)
                {
                    _operationsDict = new Dictionary<string, Operation>();
                    foreach (Operation op in Operations)
                    {
                        _operationsDict.Add(op.OperationId, op);
                    }
                }
                return _operationsDict;
            }
        }

        private static Dictionary<string, Operation> _operationsDict;

        internal static bool HasOperation(string operationId)
        {
            if (String.IsNullOrEmpty(operationId))
                throw new ArgumentException("operationId cannot be null or empty.");
            return OperationsDict.ContainsKey(operationId);
        }

        internal static Operation GetOperation(string operationId)
        {
            if (String.IsNullOrEmpty(operationId))
                throw new ArgumentException("operationId cannot be null or empty.");
            if (!HasOperation(operationId))
                throw new ArgumentException("operationId does not refer to an existing operation.");
            return OperationsDict[operationId];
        }

        /// <summary>
        /// Invokes the operation with the ID: <paramref name="operationId"/>.
        /// </summary>
        /// <param name="operationId">The operation id.</param>
        /// <param name="valueProvider">The value provider for the current front-end.</param>
        /// <returns>The operation's return code.</returns>
        /// <exception cref="ArgumentExclusivityException">
        /// Thrown when an exclusive argument is used in conjunction with other arguments.
        /// </exception>
        /// <exception cref="ArgumentRequirementException">
        /// Thrown when a required argument has not been specified.
        /// </exception>
        /// <exception cref="InvalidOperationArgumentException">
        /// Thrown when the specified value for an argument is invalid.
        /// </exception>
        public static int InvokeOperationById(string operationId, IValueProvider valueProvider)
        {
            if (OperationController.OperationsDict.ContainsKey(operationId))
            {
                ClearArgumentCache();

                Operation op = OperationsDict[operationId];
                foreach (IArgument argument in op.Arguments)
                {
                    SetCachedArgumentValue(argument, valueProvider.GetValueAsObject(argument));
                }

                // Now that they have been extracted, validate the arguments against the exclusivity and requirement restrictions.
                ValidateArgumentsByExclusivity(op.Arguments);
                ValidateArgumentsByRequirement(op.Arguments);

                // Now set the argument field values on the Operation instance.
                SetOperationArgumentFieldValues(op);
                
                op.DoSetup();

                try
                {
                    return op.Run();
                }
                finally
                {
                    op.DoTearDown();
                }
            }
            Console.WriteLine("Operation with ID: {0} not found." , operationId);
            return 1;
        }

        private static void ValidateArgumentsByExclusivity(IArgument[] arguments)
        {
            IArgument[] setExclusiveArgs = (
                from arg in arguments
                where (arg.Exclusive && ArgumentIsSet(arg))
                select arg)
                .ToArray<IArgument>();

            IArgument[] setNonExclusiveArgs = (
                from arg in arguments
                where (!arg.Exclusive && ArgumentIsSet(arg))
                select arg)
                .ToArray<IArgument>();

            if (setExclusiveArgs.Length > 0)
            {
                if (setExclusiveArgs.Length == 1)
                {
                    if (setNonExclusiveArgs.Length > 0)
                    {
                        throw new ArgumentExclusivityException(setExclusiveArgs[0]);
                    }
                }
                else
                {
                    throw new ArgumentExclusivityException(setExclusiveArgs[0]);
                }
            }
        }

        private static void ValidateArgumentsByRequirement(IArgument[] arguments)
        {
            bool exclusiveArgumentIsSet = (
                from arg in arguments
                where (arg.Exclusive && ArgumentIsSet(arg))
                select arg
                ).Count() > 0;

            if (!exclusiveArgumentIsSet)
            {
                IArgument[] requiredArgsNotSet = (
                    from arg in arguments
                    where (arg.Mandatory && !ArgumentIsSet(arg))
                    select arg)
                    .ToArray<IArgument>();

                if (requiredArgsNotSet.Length > 0)
                {
                    throw new ArgumentRequirementException(requiredArgsNotSet[0]);
                }
            }
        }

        private static void SetOperationArgumentFieldValues(Operation operation)
        {
            foreach (FieldInfo fi in operation.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                foreach (object attr in fi.GetCustomAttributes(true))
                {
                    if (attr is OperationArgumentAttribute)
                    {
                        OperationArgumentAttribute argumentAttribute = (OperationArgumentAttribute)attr;
                        fi.SetValue(operation, _argumentValueCache[argumentAttribute.Id]);
                    }
                }
            }
        }

        private static void ClearArgumentCache()
        {
            _argumentValueCache.Clear();
        }

        private static void SetCachedArgumentValue(IArgument argument, object value)
        {
            _argumentValueCache[argument.Id] = value;
        }

        private static bool ArgumentIsSet(IArgument argument)
        {
            if (argument is FlagArgument)
            {
                // A FlagArgument will always appear in the cache. If it hasn't been set, its value will resolve to false.
                return (bool)_argumentValueCache[argument.Id];
            }
            else
            {
                return _argumentValueCache[argument.Id] != null;
            }
        }

        /// <summary>
        /// Gets the cached argument value for <paramref name="argument"/>, if it is present,
        /// and assigns it <paramref name="value"/>.
        /// Will return <c>true</c> if the value is present, or <c>false</c> if it isn't.
        /// </summary>
        /// <typeparam name="T">Expected type of <paramref name="value"/>.</typeparam>
        /// <param name="argument">The argument.</param>
        /// <param name="value">The value to output.</param>
        /// <returns><c>true</c> if the value is present, or <c>false</c> if it isn't.</returns>
        /// <exception cref="InvalidCastException">
        /// Thrown if the value output to <paramref name="value"/> is not an instance of <typeparamref name="T"/>
        /// </exception>.
        internal static bool GetCachedArgumentValue<T>(IArgument argument, out T value)
        {
            value = default(T);
            object cachedValueAsObject;
            try
            {
                cachedValueAsObject = _argumentValueCache[argument.Id];
            }
            catch (KeyNotFoundException knfe)
            {
                throw new ArgumentNotDefinedException(argument, "", knfe);
            }

            if (cachedValueAsObject == null)
            {
                return false;
            }
            else
            {
                if (!(cachedValueAsObject is T))
                {
                    throw new InvalidCastException(String.Format("Argument value for: {0} is not an instance of {1}.", argument.Id, typeof(T).FullName));
                }
                value = (T)cachedValueAsObject;
                return true;
            }
        }

        private static Dictionary<string, object> _argumentValueCache = new Dictionary<string,object>();
    }
}
