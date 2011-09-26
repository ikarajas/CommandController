using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandController.Core;

namespace CommandLineDemo.Operations
{
    [OperationDefinition("op1", "Test operation 1", Visible=true)]
    class Operation1 : Operation
    {
        // A constructor is entirely optional, but is useful if you want to assign
        // setup and teardown event handlers.
        public Operation1()
        {
            Setup += new OperationSetupHandler(() =>
            {
                Console.WriteLine("Setting up...");
            });

            TearDown += new OperationTearDownHandler(() =>
            {
                Console.WriteLine("Tearing down...");
            });
        }

        #region Arguments

        [OperationArgument("int1", "Integer 1", "A mandatory integer argument", Mandatory = true)]
        int argument_int1 = 0;

        [OperationArgument("int2", "Integer 2", "An optional integer argument")]
        int argument_int2 = 0;

        [OperationArgument("str1", "String 1", "An optional string argument")]
        string argument_string1 = string.Empty;

        [OperationArgument("flag1", "Flag 1", "An exclusive flag argument.", Exclusive = true)]
        bool argument_flag1 = false;

        #endregion

        public override int Run()
        {
            if (argument_flag1)
            {
                Console.WriteLine("The exclusive argument: Flag 1 was set.");
            }
            else
            {
                Console.WriteLine("The exclusive argument: Flag 1 was not set.");

                Console.WriteLine("Integer argument 1: {0}", argument_int1);
                Console.WriteLine("Integer argument 2: {0}", argument_int2);
                Console.WriteLine("String argument 1: {0}", argument_string1);
            }

            return 0;
        }
    }
}
