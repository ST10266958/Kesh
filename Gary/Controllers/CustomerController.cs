using Gary.Models;

using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gary.Controllers
{
    public class CustomerController : Controller
    {
        private readonly TableStorageService _tableStorageService;

        public CustomerController(TableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        [HttpGet]
        public IActionResult AddCustomer()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.PartitionKey = "CustomersPartition";
                customer.RowKey = Guid.NewGuid().ToString();
                await _tableStorageService.AddCustomerAsync(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteCustomer(string partitionKey, string rowKey)
        {
            await _tableStorageService.DeleteCustomerAsync(partitionKey, rowKey);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _tableStorageService.GetAllCustomerAsync();
            return View(customers);
        }
    }
}

