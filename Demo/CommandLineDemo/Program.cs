using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using CommandController.FrontEnds.CommandLine;

namespace CommandLineDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineController clc = new CommandLineController();
            
            clc.Start(args);
        }
    }
}
