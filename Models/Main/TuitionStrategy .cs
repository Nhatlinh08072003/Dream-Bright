public class TuitionStrategy : IChatbotStrategy
{
    public string GetAdvice(string userInput)  // Viết hoa chữ "G" để đúng chuẩn C#
    {
        if (userInput.Contains("thấp")) return "Bạn có thể chọn các trường ở Đức hoặc Pháp với học phí thấp.";
        if (userInput.Contains("cao")) return "Bạn có thể xem xét các trường Ivy League tại Mỹ.";
        if (userInput.Contains("trung bình")) return "Bạn có thể tìm hiểu các trường công lập tại Canada.";
        return "Bạn muốn tư vấn theo mức học phí nào? Cao, trung bình, thấp?";
    }
}
