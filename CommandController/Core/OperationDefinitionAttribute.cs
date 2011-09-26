using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    [AttributeUsage(AttributeTargets.Class)]
    public class OperationDefinitionAttribute : Attribute
    {
        /// <summary>
        /// The string used to identify the operation.
        /// </summary>
        public readonly string OperationId;

        /// <summary>
        /// The string describing the usage of the operation.
        /// </summary>
        /// <value>The description string.</value>
        public readonly string UsageDescription;

        /// <summary>
        /// Gets or sets whether or not the <see cref="Operation"/> is visible. Defaults to <c>true</c>.
        /// </summary>
        public bool Visible
        {
            get
            {
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }

        private bool _visible = true;

        /// <summary>
        /// Creates a new instance of <see cref="OperationDefinitionAttribute"/>.
        /// </summary>
        /// <param name="operationId">
        /// The string used to identify the operation.
        /// </param>
        /// <param name="usageDescription">
        /// A string describing the usage of the operation.
        /// </param>
        public OperationDefinitionAttribute(string operationId, string usageDescription)
        {
            OperationId = operationId;
            UsageDescription = usageDescription;
        }
    }
}
