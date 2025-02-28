using System;
using os_project_partB.IPC;
using os_project_partB.Utils;

class Program
{
    static void Main(string[] args)
    {
        // Ensures the argument is there to check process type
        if (args.Length == 0)
        {
            Console.WriteLine("Usage: dotnet run -- ParentProcess | ChildProcess");
            return;
        }

        // Checks which process needs to execute from argument
        if (args[0] == "ParentProcess")
        {
            ParentProcess.Start(); // To run parent logic
        }
        else if (args[0] == "ChildProcess")
        {
            ChildProcess.Start(); // To run child logic
        }
        else if(args[0] == "Benchmark")
        {
            PerformanceTester.BenchmarkDataTransfer(); // To run performance test
        }
        else
        {
            Console.WriteLine("Invalid argument. Use 'ParentProcess' or 'ChildProcess'."); // Fail safe message
        }
    }
}
