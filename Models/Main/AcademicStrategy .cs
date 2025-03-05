
public class AcademicStrategy : IChatbotStrategy
{
    public string GetAdvice(string userInput)
    {
        if (userInput.Contains("IELTS")) return "Bạn cần IELTS 6.5+ để du học.";
        if (userInput.Contains("GPA")) return "GPA trên 3.0 giúp bạn có nhiều cơ hội học bổng.";
        if (userInput.Contains("SAT")) return "SAT trên 1300 giúp bạn vào các trường top.";
        return "Bạn muốn tư vấn về học lực? GPA, IELTS, SAT?";
    }
}