using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using os_project_partB.Models;
using os_project_partB.Utils;

namespace os_project_partB.IPC
{
    public class ParentProcess
    {
        public static void Start()
        {
            // This creates a pipe of type anonymous so that there is a connection from parent --> child
            using (AnonymousPipeServerStream pipeServer = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable))
            {
                // This sets up the child process
                Process child = new Process();
                child.StartInfo.FileName = "dotnet"; // Dotnet command used to run child
                child.StartInfo.Arguments = "run -- ChildProcess";
                child.StartInfo.UseShellExecute = false; // for IPC functionality
                child.StartInfo.RedirectStandardInput = false; // pipe already in use
                child.StartInfo.Environment["PIPE_HANDLE"] = pipeServer.GetClientHandleAsString(); // This sends the pipe handle to child

                child.Start();
                pipeServer.DisposeLocalCopyOfClientHandle(); // Clears local copy of pipe handle

                using (StreamWriter writer = new StreamWriter(pipeServer)) // Using writer to send data through the pipe
                {
                    writer.AutoFlush = true; // immediate send of data

                    // Sample bank transaction variable type JSON
                    var transaction = new Transaction("123456", 500.75, "Deposit");
                    string jsonData = JsonSerializer.Serialize(transaction);
                    Console.WriteLine($"Parent: Sending JSON: {jsonData}");
                    writer.WriteLine(jsonData); // Sends data to child()
                }

                child.WaitForExit(); // Checks to see if child complete
            }
        }
    }
}
