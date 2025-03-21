using Microsoft.AspNetCore.Mvc;
using Dream_Bridge.Models.Bridge;
using System.Collections.Generic;

namespace Dream_Bridge.Controllers
{
    public class BridgeController : Controller
    {
        public IActionResult Index(string method = "Online", string level = "Basic")
        {
            ConsultingMethod consulting;

            // Tùy chọn hình thức tư vấn và cấp độ dịch vụ dựa trên tham số
            if (method == "Online")
            {
                consulting = level == "Basic" ? new OnlineConsulting(new BasicService()) :
                                                new OnlineConsulting(new PremiumService());
            }
            else
            {
                consulting = level == "Basic" ? new DirectConsulting(new BasicService()) :
                                                new DirectConsulting(new PremiumService());
            }

            // Gửi kết quả tư vấn vào View
            var result = consulting.Consult();
            return View((object)result);
        }
    }
}
