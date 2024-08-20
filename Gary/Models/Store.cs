using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace Gary.Models
{
    public class Store : ITableEntity
    {
        [Key]
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public double ItemPrice { get; set; }

        // ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}

