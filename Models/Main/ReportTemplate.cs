using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dream_Bridge.Models.Main;
public abstract class ReportTemplate<T>
{
    protected readonly StudyAbroadDbContext _dbContext;

    public ReportTemplate(StudyAbroadDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // Template Method - Định nghĩa quy trình xuất báo cáo
    public void GenerateReport()
    {
        var data = FetchData(); // Bước 1: Lấy dữ liệu từ database
        string formattedData = FormatData(data); // Bước 2: Xử lý dữ liệu
        SaveReport(formattedData); // Bước 3: Xuất báo cáo
    }

    // Lấy dữ liệu từ Database (tùy theo từng loại báo cáo)
    protected abstract List<T> FetchData();

    // Xử lý dữ liệu (tùy theo định dạng báo cáo: PDF, CSV, JSON)
    protected abstract string FormatData(List<T> data);

    // Xuất báo cáo (ghi ra file hoặc gửi qua email)
    protected abstract void SaveReport(string formattedData);
}