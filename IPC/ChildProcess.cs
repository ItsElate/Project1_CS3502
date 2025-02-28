using System;
using System.IO;
using System.IO.Pipes;
using System.Text.Json;
using os_project_partB.Models;

namespace os_project_partB.IPC
{
    public class ChildProcess
    {
        public static void Start()
        {
            // Gets pipe handle from enviornment var
            string? pipeHandle = Environment.GetEnvironmentVariable("PIPE_HANDLE");
            if (string.IsNullOrEmpty(pipeHandle)) // Ensures valid pipe handle
            {
                Console.WriteLine("No pipe handle found.");
                return;
            }

            // Connects to pipe from the parent()
            using (AnonymousPipeClientStream pipeClient = new AnonymousPipeClientStream(PipeDirection.In, pipeHandle))
            {
                using (StreamReader reader = new StreamReader(pipeClient)) // Set up reader to contain data
                {
                    string? receivedData = reader.ReadLine(); // Takes JSON data from pipe
                    Console.WriteLine($"Child: Received JSON: {receivedData}");

                    try
                    {
                        // Transfers JSON into a transaction object type
                        var transaction = JsonSerializer.Deserialize<Transaction>(receivedData);
                        Console.WriteLine($"Child: Validated Transaction - Account {transaction.AccountNumber}, Amount {transaction.Amount}, Type {transaction.TransactionType}");
                    }
                    catch (Exception ex)
                    {
                        // Handle JSON parse errors
                        Console.WriteLine($"Child: JSON Parsing Error - {ex.Message}");
                    }
                }
            }
        }
    }
}
