using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;
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

    protected override void SaveReport(string formattedData)
    {
        string filePath = "Reports/UserReport.json";
        File.WriteAllText(filePath, formattedData);
        Console.WriteLine($"📄 JSON Report saved to {filePath}");
    }
}
