using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace DreamBright.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdapterController : Controller
    {
        [HttpGet("index")]
        public IActionResult Index()
        {
            var payments = new List<string>
        {
            "Thanh toán 100$ qua PayPal.",
            "Thanh toán 200,000 VND qua VNPay.",
            "Thanh toán 150$ qua PayPal.",
            "Thanh toán 300,000 VND qua VNPay."
        };

            return View(payments);
        }

        [HttpGet("details/{id}")]
        public IActionResult Details(int id)
        {
            var paymentDetails = id switch
            {
                1 => "Chi tiết giao dịch: Thanh toán 100$ qua PayPal.",
                2 => "Chi tiết giao dịch: Thanh toán 200,000 VND qua VNPay.",
                3 => "Chi tiết giao dịch: Thanh toán 150$ qua PayPal.",
                4 => "Chi tiết giao dịch: Thanh toán 300,000 VND qua VNPay.",
                _ => "Không tìm thấy giao dịch."
            };

            return Content(paymentDetails); // Trả về thông tin chi tiết đơn giản
        }
    }

}
