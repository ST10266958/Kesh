using Azure;
using Azure.Data.Tables;
using Gary.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class TableStorageService
{
    private readonly TableClient _storeTableClient;
    private readonly TableClient _customerTableClient;
    private readonly TableClient _transactionTableClient;

    public TableStorageService(string connectionString)
    {
        _storeTableClient = new TableClient(connectionString, "Stores");
        _customerTableClient = new TableClient(connectionString, "Customers");
        _transactionTableClient = new TableClient(connectionString, "Transactions");
    }

    public async Task<List<Store>> GetAllStoreAsync()
    {
        var stores = new List<Store>();

        await foreach (var store in _storeTableClient.QueryAsync<Store>())
        {
            stores.Add(store);
        }

        return stores;
    }

    public async Task AddStoreAsync(Store store)
    {
        if (string.IsNullOrEmpty(store.PartitionKey) || string.IsNullOrEmpty(store.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _storeTableClient.AddEntityAsync(store);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding entity to Table Storage", ex);
        }
    }

    public async Task DeleteStoreAsync(string partitionKey, string rowKey)
    {
        await _storeTableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<Store?> GetStoreAsync(string partitionKey, string rowKey)
    {
        try
        {
            var response = await _storeTableClient.GetEntityAsync<Store>(partitionKey, rowKey);
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task<List<Customer>> GetAllCustomerAsync()
    {
        var customers = new List<Customer>();

        await foreach (var customer in _customerTableClient.QueryAsync<Customer>())
        {
            customers.Add(customer);
        }

        return customers;
    }

    public async Task AddCustomerAsync(Customer customer)
    {
        if (string.IsNullOrEmpty(customer.PartitionKey) || string.IsNullOrEmpty(customer.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _customerTableClient.AddEntityAsync(customer);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding entity to Table Storage", ex);
        }
    }

    public async Task DeleteCustomerAsync(string partitionKey, string rowKey)
    {
        await _customerTableClient.DeleteEntityAsync(partitionKey, rowKey);
    }

    public async Task<Customer?> GetCustomerAsync(string partitionKey, string rowKey)
    {
        try
        {
            var response = await _customerTableClient.GetEntityAsync<Customer>(partitionKey, rowKey);
            return response.Value;
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            return null;
        }
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        if (string.IsNullOrEmpty(transaction.PartitionKey) || string.IsNullOrEmpty(transaction.RowKey))
        {
            throw new ArgumentException("PartitionKey and RowKey must be set.");
        }

        try
        {
            await _transactionTableClient.AddEntityAsync(transaction);
        }
        catch (RequestFailedException ex)
        {
            throw new InvalidOperationException("Error adding transaction to Table Storage", ex);
        }
    }

    public async Task<List<Transaction>> GetAllTransactionAsync()
    {
        var transactions = new List<Transaction>();

        await foreach (var transaction in _transactionTableClient.QueryAsync<Transaction>())
        {
            transactions.Add(transaction);
        }

        return transactions;
    }
}
