using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dream_Bright.Models.Main;
using System.Text;
using Newtonsoft.Json;


namespace MyProject.Models
{
    public class JSONReport : ReportTemplate<User>
    {
        public JSONReport(StudyAbroadDbContext dbContext) : base(dbContext) { }

        protected override List<User> FetchData()
        {
            return _dbContext.Users.ToList();
        }

        protected override string FormatData(List<User> data)
        {
            return JsonConvert.SerializeObject(data, Formatting.Indented);
        }

        protected override string SaveReport(string formattedData)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/reports");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "UserReport.json");
            File.WriteAllText(filePath, formattedData);

            return filePath;
        }
    }
}
