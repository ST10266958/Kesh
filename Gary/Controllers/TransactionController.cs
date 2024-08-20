using Gary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gary.Controllers
{
    public class TransactionController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public TransactionController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        [HttpGet]
        public IActionResult AddTransaction()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTransaction(Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                transaction.PartitionKey = "TransactionsPartition";
                transaction.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddTransactionAsync(transaction);
                return RedirectToAction("Index");
            }
            return View(transaction);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTransaction(string partitionKey, string rowKey)
        {
            object value = await _tableStorageService.DeleteTransactionAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var transactions = await _tableStorageService.GetAllTransactionAsync();
            return View(transactions);
        }
    }
}
