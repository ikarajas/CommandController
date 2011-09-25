using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommandController.Core;

namespace CommandLineDemo.Operations
{
    class Operation1 : Operation
    {
        public override string OperationId
        {
            get { return "op1"; }
        }

        public override string UsageDescription
        {
            get { return "Test operation 1"; }
        }

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

        [OperationArgument]
        IntegerArgument argument_int1 = new IntegerArgument("int1", "Integer 1", "A mandatory integer argument", true, false);

        [OperationArgument]
        IntegerArgument argument_int2 = new IntegerArgument("int2", "Integer 2", "An optional integer argument", false, false);

        [OperationArgument]
        StringArgument argument_string1 = new StringArgument("str1", "String 1", "An optional string argument", false, false);

        [OperationArgument]
        FlagArgument argument_flag1 = new FlagArgument("flag1", "Flag 1", "An exclusive flag argument.", true);

        #endregion

        public override int Run()
        {
            if (argument_flag1.Value)
            {
                Console.WriteLine("The exclusive argument: Flag 1 was set.");
            }
            else
            {
                Console.WriteLine("The exclusive argument: Flag 1 was not set.");

                Console.WriteLine("Integer argument 1: {0}", argument_int1.Value);
                Console.WriteLine("Integer argument 2: {0}", argument_int2.Value);
                Console.WriteLine("String argument 1: {0}", argument_int2.Value);
            }

            return 0;
        }
    }
}
