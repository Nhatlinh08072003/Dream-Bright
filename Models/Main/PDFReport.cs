using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;
public class PDFReport : ReportTemplate<User>
{
    public PDFReport(StudyAbroadDbContext dbContext) : base(dbContext) { }

    protected override List<User> FetchData()
    {
        return _dbContext.Users.ToList(); // Lấy danh sách User từ database
    }

    protected override string FormatData(List<User> data)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("User Report (PDF Format)");
        sb.AppendLine("==================================");

        foreach (var user in data)
        {
            sb.AppendLine($"ID: {user.IdUser}, Name: {user.FullName}, Email: {user.Email}");
        }

        return sb.ToString();
    }

    protected override void SaveReport(string formattedData)
    {
        string filePath = "Reports/UserReport.pdf";
        File.WriteAllText(filePath, formattedData); // Giả lập ghi ra PDF (thực tế có thể dùng thư viện iTextSharp)
        Console.WriteLine($"📄 PDF Report saved to {filePath}");
    }
}
