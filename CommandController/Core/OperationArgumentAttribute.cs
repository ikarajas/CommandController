using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OperationArgumentAttribute : Attribute
    {
        public readonly string Id;

        public readonly string FriendlyName;

        public readonly string Description;

        public bool Mandatory
        {
            get
            {
                return _mandatory;
            }
            set
            {
                _mandatory = value;
            }
        }

        private bool _mandatory;

        public bool Exclusive
        {
            get
            {
                return _exclusive;
            }
            set
            {
                _exclusive = value;
            }
        }

        private bool _exclusive;

        /// <summary>
        /// Initializes a new instance of <see cref="OperationArgumentAttribute"/>.
        /// </summary>
        public OperationArgumentAttribute(string id, string friendlyName, string description)
        {
            Id = id;
            FriendlyName = friendlyName;
            Description = description;
        }
    }
}
