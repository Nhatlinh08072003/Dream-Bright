using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.AspNetCore.Authorization;
// using Microsoft.EntityFrameworkCore;
using Dream_Bridge.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Dream_Bridge.Hubs;
using Dream_Bridge.Model.Main;
using Microsoft.AspNetCore.Authentication;
using DreamBright.Models;

namespace Dream_Bridge.Controllers
{
    [Route("Notification")]
    public class NotificationController : Controller
    {
        private readonly StudyAbroadDbContext _studyAbroadDbContext;

        public NotificationController(StudyAbroadDbContext studyAbroadDbContext)
        {
            _studyAbroadDbContext = studyAbroadDbContext;
        }

        [HttpGet("my-notifications")]
        public async Task<IActionResult> GetMyNotifications()
        {
            try
            {
                var notifications = await _studyAbroadDbContext.Notifications
                    .OrderByDescending(n => n.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                return Json(new { success = true, notifications });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Internal Server Error", error = ex.Message });
            }
        }


        [HttpPost("SendNotification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid notification data" });
                }

                var notification = new Dream_Bridge.Models.Main.Notification
                {
                    UserId = model.UserId,
                    Title = model.Title,
                    Message = model.Message,
                    Type = model.Type, // Ensure this is mapped correctly
                    CreatedAt = DateTime.Now,
                    IsRead = false
                };

                _studyAbroadDbContext.Notifications.Add(notification);
                await _studyAbroadDbContext.SaveChangesAsync();

                return Json(new { success = true, message = "Notification saved successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Internal Server Error", error = ex.Message });
            }
        }
        public IActionResult TestNotification()
        {
            return View();
        }
    }
}