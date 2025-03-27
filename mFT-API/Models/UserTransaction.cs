using System;
namespace mFT_API.Models
{
	public class UserTransaction
	{
        public int? TransactionID { get; set; }
        public required string TransactionName { get; set; }
        public required decimal TransactionAmount { get; set; }
        public required int TransactionType { get; set; }
        public required int TransactionCategory { get; set; }
        public int RecurrenceFrequency { get; set; }
        public required DateTime DueDate { get; set; }
        public DateTime? PaidDate { get; set; }
        public int NumberOfOccurrences { get; set; }
        public int DayOfMonth { get; set; }
        public int SemiMonthlySecondDay { get; set; }
        public string? Notes { get; set; }
        public required int UserID { get; set; }
        public string? CreditCardVendor { get; set; }
        public int? CreditCardLastFourDigits { get; set; }
        public int? CreditCardType { get; set; }
        public int? ParentRecurringTransactionId { get; set; }
    }
}

