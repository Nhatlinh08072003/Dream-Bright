using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Dream_Bridge.Models;
using Dream_Bridge.Models.Main;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Dream_Bright.Models.Main;

namespace MyProject.Models
{
    public abstract class ReportTemplate<T>
    {
        protected readonly StudyAbroadDbContext _dbContext;

        public ReportTemplate(StudyAbroadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public string GenerateReport()
        {
            var data = FetchData();
            string formattedData = FormatData(data);
            return SaveReport(formattedData);
        }

        protected abstract List<T> FetchData();
        protected abstract string FormatData(List<T> data);
        protected abstract string SaveReport(string formattedData);
    }
}
