using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;

namespace os_project_partB.Utils
{
    public class PerformanceTester
    {
        public static void BenchmarkDataTransfer()
        {
            // Makes pipe type anonymous for parent -> child
            using (AnonymousPipeServerStream pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
            {
                Process child = new Process();
                child.StartInfo.FileName = "dotnet";
                child.StartInfo.Arguments = "run -- ChildProcess";
                child.StartInfo.UseShellExecute = false;
                child.StartInfo.RedirectStandardInput = false;
                child.StartInfo.Environment["PIPE_HANDLE"] = pipeServer.GetClientHandleAsString();

                child.Start();
                pipeServer.DisposeLocalCopyOfClientHandle();

                using (StreamWriter writer = new StreamWriter(pipeServer))
                {
                    writer.AutoFlush = true;
                    var stopwatch = Stopwatch.StartNew();

                    // Sends 100,000 messages to the child process for performance measure
                    for (int i = 0; i < 100000; i++)
                    {
                        writer.WriteLine($"Message {i}");
                    }

                    stopwatch.Stop(); // Stop method for timer
                    Console.WriteLine($"Parent: Sent 100,000 messages in {stopwatch.ElapsedMilliseconds}ms");
                }

                child.WaitForExit();
            }
        }
    }
}
