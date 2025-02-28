using System;
using System.Net.Http.Headers;
using System.Threading;

class BankAccount{
    private static int balanceA = 100; // Shared resource between threads
    private static int balanceB = 200;

    private static object lockA = new object();
    private static object lockB = new object();

    static void Main(){
        // Creating two threads for transactions
        Thread t1 = new Thread(() => Transfer(10, lockA, lockB));
        Thread t2 = new Thread(() => Transfer(20, lockA, lockB));

        t1.Start();
        t2.Start();

        t1.Join();
        t2.Join();

        Console.WriteLine($"Final Balance Account A: {balanceA}, Account B: {balanceB}");
    }

    // Method for Tranfering between account A and account B while threading in operation
    // Prevents circular dependency as one thread will always proceed
    static void Transfer(int amount, object firstLock, object secondLock){
        lock(firstLock){
            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: Locked First Account");
            Thread.Sleep(500);

            lock(secondLock){
                Console.WriteLine($"Thread{Thread.CurrentThread.ManagedThreadId}: Locked Second Account");
                balanceA -= amount;
                balanceB += amount;
                Console.WriteLine($"Transferred {amount}");
            }
        }
    }
}
