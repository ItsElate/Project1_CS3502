namespace os_project_partB.Models
{
    public class Transaction
    {
        // Setting up getters and setters for bank data
        public string AccountNumber { get; set; }
        public double Amount { get; set; }
        public string TransactionType { get; set; }
        public DateTime Timestamp { get; set; }

        // Constructor for transaction method
        public Transaction(string accountNumber, double amount, string transactionType)
        {
            AccountNumber = accountNumber;
            Amount = amount;
            TransactionType = transactionType;
            Timestamp = DateTime.UtcNow; // Gets current time stamp
        }
    }
}
