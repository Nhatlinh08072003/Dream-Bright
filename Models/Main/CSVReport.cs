using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;
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

    protected override void SaveReport(string formattedData)
    {
        string filePath = "Reports/UserReport.csv";
        File.WriteAllText(filePath, formattedData);
        Console.WriteLine($"📄 CSV Report saved to {filePath}");
    }
}
