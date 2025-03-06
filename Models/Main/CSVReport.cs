using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dream_Bright.Models.Main;
using System.Text;

namespace MyProject.Models
{
    public class CSVReport : ReportTemplate<User>
    {
        public CSVReport(StudyAbroadDbContext dbContext) : base(dbContext) { }

        protected override List<User> FetchData()
        {
            return _dbContext.Users.ToList();
        }

        protected override string FormatData(List<User> data)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("ID,Name,Email");

            foreach (var user in data)
            {
                sb.AppendLine($"{user.IdUser},{user.FullName},{user.Email}");
            }

            return sb.ToString();
        }

        protected override string SaveReport(string formattedData)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/reports");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "UserReport.csv");
            File.WriteAllText(filePath, formattedData);

            return filePath;
        }
    }
}
