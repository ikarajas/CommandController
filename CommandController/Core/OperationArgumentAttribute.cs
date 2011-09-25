using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommandController.Core
{
    [AttributeUsage(AttributeTargets.Field)]
    public class OperationArgumentAttribute : Attribute
    {
    }
}
