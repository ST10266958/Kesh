using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace Gary.Models
{
    public class Transaction : ITableEntity
    {
        [Key]
        public int TransactionId { get; set; }

        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Introduce validation sample
        [Required(ErrorMessage = "Please select an item.")]
        public int CustomerId { get; set; } // FK to the Customer who made the transaction

        [Required(ErrorMessage = "Please select an item.")]
        public int ItemId { get; set; } // FK to the Store item involved in the transaction

        [Required(ErrorMessage = "Please select the date.")]
        public DateTime TransactionDate { get; set; }

        [Required(ErrorMessage = "Please enter a location.")]
        public string? Location { get; set; }
    }
}

