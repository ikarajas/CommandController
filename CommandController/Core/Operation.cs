using System;
using System.Collections.Generic;
using System.Text;

using CommandController.FrontEnds;
using System.Diagnostics;
using System.Reflection;

namespace CommandController.Core
{
    /// <summary>
    /// A delegate that executes before a given <see cref="Operation"/> is run.
    /// </summary>
    public delegate void OperationSetupHandler();

    /// <summary>
    /// A delegate that executes after a given <see cref="Operation"/> has finished running.
    /// </summary>
    public delegate void OperationTearDownHandler();

	public abstract class Operation
	{
        /// <summary>
        /// Gets a string used to identify the operation. This is used to execute an operation when running on the command line.
        /// </summary>
        /// <value>The operation ID.</value>
        public string OperationId
        {
            get
            {
                string value = OperationDefinition.OperationId;

                if (string.IsNullOrEmpty(value))
                {
                    throw new InvalidOperationException(string.Format("Operation {0} has a null or empty operation ID.", GetType().Name));
                }

                return value;
            }
        }

        /// <summary>
        /// Gets the operation's arguments.
        /// </summary>
        /// <remarks>
        /// The arguments are obtained by interrogating the operation's type for all instance fields that
        /// have the <see cref="OperationArgumentAttribute"/> attribute.
        /// </remarks>
        /// <value>The arguments.</value>
        public IArgument[] Arguments
        {
            get
            {
                if (_arguments == null)
                {
                    List<IArgument> argsList = new List<IArgument>();
                    foreach (FieldInfo fi in GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
                    {
                        foreach (object attr in fi.GetCustomAttributes(true))
                        {
                            if (attr is OperationArgumentAttribute)
                            {
                                OperationArgumentAttribute argumentAttribute = (OperationArgumentAttribute)attr;
                                object fieldValue = fi.GetValue(this);
                                Type fieldType = fi.FieldType;

                                IArgument argumentObj;
                                if (fieldType == typeof(string))
                                {
                                    argumentObj = new StringArgument(
                                        argumentAttribute.Id,
                                        argumentAttribute.FriendlyName,
                                        argumentAttribute.Description,
                                        argumentAttribute.Mandatory,
                                        argumentAttribute.Exclusive);
                                }
                                else if (fieldType == typeof(int))
                                {
                                    argumentObj = new IntegerArgument(
                                        argumentAttribute.Id,
                                        argumentAttribute.FriendlyName,
                                        argumentAttribute.Description,
                                        argumentAttribute.Mandatory,
                                        argumentAttribute.Exclusive);
                                }
                                else if (fieldType == typeof(bool))
                                {
                                    argumentObj = new FlagArgument(
                                        argumentAttribute.Id,
                                        argumentAttribute.FriendlyName,
                                        argumentAttribute.Description,
                                        argumentAttribute.Exclusive);
                                }
                                else
                                {
                                    throw new InvalidOperationException(
                                        String.Format("Field: {0} cannot be used as an argument as it is neither an " +
                                        "integer, string, or boolean field.", fi.Name));
                                }

                                argsList.Add((IArgument)argumentObj);
                            }
                        }
                    }
                    _arguments = argsList.ToArray();
                }
                return _arguments;
            }
        }
        private IArgument[] _arguments;

        /// <summary>
        /// Gets a string describing the usage of the operation.
        /// </summary>
        /// <value>The description string.</value>
        public string UsageDescription
        {
            get
            {
                return OperationDefinition.UsageDescription;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Operation"/> is visible.
        /// </summary>
        /// <remarks>
        /// If this is not overridden, it will always return <c>false</c>.
        /// </remarks>
        /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
        public virtual bool Visible
        {
            get
            {
                return OperationDefinition.Visible;
            }
        }

        private OperationDefinitionAttribute OperationDefinition
        {
            get
            {
                if (_operationDefinition == null)
                {
                    foreach (object attr in GetType().GetCustomAttributes(false))
                    {
                        if (_operationDefinition != null && (attr is OperationDefinitionAttribute))
                        {
                            throw new InvalidOperationException("An operation should only have one OperationDefinitionAttribute. " +
                                "This has more than one.");
                        }

                        if (attr is OperationDefinitionAttribute)
                        {
                            _operationDefinition = (OperationDefinitionAttribute)attr;
                        }
                    }
                }
                return _operationDefinition;
            }
        }
        private OperationDefinitionAttribute _operationDefinition;

        /// <summary>
        /// Invokes any handlers that have been assigned to the <see cref="Setup"/> event.
        /// </summary>
        internal void DoSetup()
        {
            if (Setup != null)
            {
                Setup();
            }
        }

        /// <summary>
        /// Invokes any handlers that have been assigned to the <see cref="TearDown"/> event.
        /// </summary>
        internal void DoTearDown()
        {
            if (TearDown != null)
            {
                TearDown();
            }
        }

        /// <summary>
        /// Runs the operation.
        /// </summary>
        /// <returns>An integer indicating the result of the operation. Return <c>0</c> to indicate success.</returns>
		public abstract int Run();

        /// <summary>
        /// An event that executes before the operation is run.
        /// </summary>
        public event OperationSetupHandler Setup;

        /// <summary>
        /// An event that executes after the operation has finished running.
        /// </summary>
        public event OperationTearDownHandler TearDown;

    }
}
