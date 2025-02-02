﻿using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;

namespace Gary.Models
{
    public class Customer : ITableEntity
    {
        [Key]
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? Password { get; set; }

        // ITableEntity implementation
        public string? PartitionKey { get; set; }
        public string? RowKey { get; set; }
        public ETag ETag { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
    }
}

