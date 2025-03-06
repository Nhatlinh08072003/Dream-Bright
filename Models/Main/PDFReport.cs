using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dream_Bright.Models.Main;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;
using System.IO;


namespace MyProject.Models
{
    public class PDFReport : ReportTemplate<User>
    {
        public PDFReport(StudyAbroadDbContext dbContext) : base(dbContext) { }

        protected override List<User> FetchData()
        {
            return _dbContext.Users.ToList();
        }

        protected override string FormatData(List<User> data)
        {
            return string.Empty;
        }

        protected override string SaveReport(string formattedData)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/reports");
            Directory.CreateDirectory(folderPath);

            string filePath = Path.Combine(folderPath, "UserReport.pdf");

            // Tạo tài liệu PDF
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            XGraphics gfx = XGraphics.FromPdfPage(page);
            XFont font = new XFont("Arial", 12, XFontStyle.Regular);

            gfx.DrawString("User Report", font, XBrushes.Black, new XPoint(40, 50));

            int yPos = 80;
            foreach (var user in FetchData())
            {
                string userInfo = $"ID: {user.IdUser}, Name: {user.FullName}, Email: {user.Email}";
                gfx.DrawString(userInfo, font, XBrushes.Black, new XPoint(40, yPos));
                yPos += 20;
            }

            document.Save(filePath);
            return filePath;
        }
    }
}
