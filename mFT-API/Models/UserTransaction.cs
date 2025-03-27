using System;
namespace mFT_API.Models
{
	public class UserTransaction
	{
        public required string TransactionName { get; set; }
        public required string Amount { get; set; }
        public required string TransactionType { get; set; }
        //public required string Category { get; set; }
        //public required string RecurrenceFrequency { get; set; }
        //public required DateOnly DueDate { get; set; }
        //public required DateOnly PaidDate { get; set; }
        //public required int NumberOfOccurrences { get; set; }
        //public required int DayOfMonth { get; set; }
        //public required int SemiMonthlySecondDay { get; set; }
        //public required string Notes { get; set; }
        //public required int UserID { get; set; }
    }
}

