using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

using CommandController.Core;

namespace CommandController.FrontEnds.CommandLine
{
    /// <summary>
    /// Controller for the command line front-end.
    /// </summary>
    public class CommandLineController : IFrontEndController
    {
        #region IFrontEndController Members

        public int Start(object data)
        {
            if (!(data is string[]))
            {
                throw new ArgumentException("The data parameter should be a string array containing command line arguments.");
            }
            string[] args = (string[])data;


            string[] subArgs = null;
            if (args.Length == 0)
            {
                Console.WriteLine(@"
Usage: 
  {0} /operation_id: Runs the operation with the ID: ""operation_id"".

Getting help:
  {0}: Prints this message.

  {0} /?: Prints a list of available operations.

  {0} /help operation_id: Prints detailed help for the operation identified by
      ""operation_id"".

  {0} /fullhelp: Prints detailed help for all available operations.
", ExeName
                    );
                return 0;
            }
            else
            {
                List<string> subArgsList = new List<string>();

                for (int i = 1; i < args.Length; i++)
                {
                    subArgsList.Add(args[i]);
                }
                subArgs = subArgsList.ToArray();
            }

            try
            {
                string opKey = args[0];
                if (opKey.StartsWith("/", StringComparison.OrdinalIgnoreCase))
                {
                    opKey = opKey.Substring(1).ToLowerInvariant();
                    switch (opKey)
                    {
                        case "?":
                            Console.Write(GetUsage(false, null));
                            return 0;
                        case "help":
                            if (args.Length > 1)
                            {
                                string opArg = args[1];
                                Console.Write(GetUsage(true, opArg));
                            }
                            else
                            {
                                Console.Write("Please specify an operation to get help for.");
                            }
                            return 0;
                        case "fullhelp":
                            Console.Write(GetUsage(true, null));
                            return 0;
                        default:
                            return OperationController.InvokeOperationById(opKey, new CommandLineValueProvider(subArgs));
                    }
                }

                Console.Write(GetUsage(false, null));
                return 1;
            }
            catch (InvalidOperationArgumentException ioae)
            {
                Console.WriteLine("Invalid \"{0}\" value specified.", ioae.ViolatedArgument.Id);
                return 1;
            }
            catch (ArgumentExclusivityException aee)
            {
                Console.WriteLine("The \"{0}\" argument cannot be specified with any other arguments.", aee.ViolatedArgument.Id);
                return 1;
            }
            catch (ArgumentRequirementException are)
            {
                Console.WriteLine("The \"{0}\" argument must be specified.", are.ViolatedArgument.Id);
                return 1;
            }
            catch (ArgumentNotDefinedException ande)
            {
                Console.WriteLine("\"{0}\" has not been defined as an argument for the current operation. " +
                    "Ensure that a reference to it is retured by the operation's Arguments property.", ande.ViolatedArgument.Id);
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR");
                StaticUtils.PrintExceptionDetails(ex);
                return 1;
            }
        }

        #endregion

        internal static string ExeName = Assembly.GetEntryAssembly().GetName().Name;

        private static string GetUsage(bool full, string operationId)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine();
            if (!String.IsNullOrEmpty(operationId) && !OperationController.HasOperation(operationId))
            {
                sb.AppendFormat("Operation: {0} does not exist.", operationId);
                sb.AppendLine();
            }
            else
            {
                IEnumerable<Operation> availableOperations = OperationController.Operations
                    .Where(op =>
                        (String.IsNullOrEmpty(operationId) && op.Visible) ||
                        (!String.IsNullOrEmpty(operationId) && op.OperationId == operationId));

                if (availableOperations.Count() > 0)
                {
                    foreach (Operation op in availableOperations)
                    {
                        sb.AppendFormat("Operation: {0}", op.OperationId);
                        sb.AppendLine();
                        sb.AppendFormat("Usage: {0} /{1}", ExeName, op.OperationId);

                        List<string> argumentUsageStrings = new List<string>();
                        foreach (IArgument argument in op.Arguments)
                        {
                            argumentUsageStrings.Add(GetArgumentUsageString(argument));
                        }

                        sb.AppendFormat(" " + String.Join(" ", argumentUsageStrings.ToArray()));
                        sb.AppendLine();
                        sb.AppendLine();
                        if (full)
                        {
                            sb.Append(op.UsageDescription);
                            sb.AppendLine();

                            sb.Append("Arguments:");
                            sb.AppendLine();

                            foreach (IArgument argument in op.Arguments)
                            {
                                sb.AppendFormat("  {0}: {1}", GetArgumentUsageString(argument), argument.Description);
                                sb.AppendLine();
                            }
                        }
                        sb.AppendFormat("---");
                        sb.AppendLine();
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.Append("No visible operations found.");
                    sb.AppendLine();
                }
            }
            return sb.ToString();
        }

        private static string GetArgumentUsageString(IArgument argument)
        {
            string retval;
            if (argument is FlagArgument)
            {
                retval = argument.Id;
            }
            else if (argument is IntegerArgument)
            {
                retval = argument.Id + "=x";
            }
            else if (argument is StringArgument)
            {
                retval = argument.Id + "=<" + argument.FriendlyName + ">";
            }
            else
            {
                return String.Empty;
            }

            if (!argument.Mandatory)
            {
                retval = "[" + retval + "]";
            }
            return retval;
        }
    }
}
