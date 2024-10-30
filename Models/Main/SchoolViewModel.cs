using Dream_Bridge.Models.Main;

public class SchoolViewModel
{
    public int IdSchool { get; set; }
    public IFormFile? ImageFile { get; set; } // For the uploaded image
    public string? TitleSchool { get; set; } // School name
    public string? SchoolDescription { get; set; } // Description
    public string? Nation { get; set; } // Country
    public string? StateCity { get; set; } // City or State
    public decimal AverageTuition { get; set; } // Tuition fees
    public string? Level { get; set; } // School level
    public int IdcategoryStab { get; set; } // Category ID
    public List<School> Schools { get; set; } = new List<School>();

}
