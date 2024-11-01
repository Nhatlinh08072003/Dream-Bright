namespace Dream_Bridge.ViewModels
{
    public class NewsViewModel
    {
        public string TitleNews { get; set; }
        public string NewsDescription { get; set; }
        public string NewsContent { get; set; }
        public IFormFile? ImageFile { get; set; } // For file upload
        public string? NewsImage { get; set; } // To hold the image path for display
        public DateTime CreatedAt { get; set; } // Include CreatedAt property if needed
        public List<NewsViewModel> NewsList { get; set; } = new List<NewsViewModel>(); // Initialize to avoid null reference

    }
}
