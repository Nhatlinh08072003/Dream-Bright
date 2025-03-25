using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Dream_Bright.Models;
using Dream_Bright.Services.Adapters;
using Dream_Bright.Services.PaymentGateways;
using Dream_Bright.Services.Logging;

namespace DreamBright.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdapterController : Controller
    {
        private readonly ITransactionLogger _logger;
        private readonly List<PaymentTransaction> _transactions;

        public AdapterController(ITransactionLogger logger)
        {
            _logger = logger;
            _transactions = new List<PaymentTransaction>
            {
                new PaymentTransaction {
                    Id = 1,
                    Amount = 100,
                    Currency = "USD",
                    Gateway = "PayPal",
                    Status = "Completed",
                    Date = DateTime.Now.AddDays(-2),
                    Description = "Thanh toán khóa học tiếng Anh"
                },
                new PaymentTransaction {
                    Id = 2,
                    Amount = 200000,
                    Currency = "VND",
                    Gateway = "VNPay",
                    Status = "Completed",
                    Date = DateTime.Now.AddDays(-1),
                    Description = "Thanh toán phí tư vấn du học"
                }
            };
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(_transactions);
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var transaction = _transactions.Find(t => t.Id == id);
            if (transaction == null)
            {
                return NotFound();
            }
            return View(transaction);
        }

        [HttpPost("process")]
        public IActionResult ProcessPayment([FromBody] PaymentRequest request)
        {
            try
            {
                IPaymentGateway gateway = request.Gateway switch
                {
                    "PayPal" => new PayPalAdapter(new PayPalService(_logger)),
                    "VNPay" => new VNPayAdapter(new VNPayService(_logger)),
                    _ => throw new ArgumentException("Invalid payment gateway")
                };

                gateway.ProcessPayment(request.Amount);

                var newTransaction = new PaymentTransaction
                {
                    Id = _transactions.Count + 1,
                    Amount = request.Amount,
                    Currency = request.Gateway == "PayPal" ? "USD" : "VND",
                    Gateway = request.Gateway,
                    Status = "Completed",
                    Date = DateTime.Now,
                    Description = request.Description
                };

                _transactions.Add(newTransaction);

                return Ok(new
                {
                    success = true,
                    transactionId = newTransaction.Id,
                    message = $"Payment processed via {request.Gateway}"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
    }
}