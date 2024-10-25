namespace Dream_Bridge.Models.Main
{
    public class StudyAbroadCategoryViewModel
    {
        public string NamecategoryStab { get; set; } = null!;
        public int IdUser { get; set; } // Nếu bạn cần lưu thông tin người dùng tạo danh mục
        public List<StudyAbroadCatalog> StudyAbroadCatalogs { get; set; } = new List<StudyAbroadCatalog>();

    }
}

