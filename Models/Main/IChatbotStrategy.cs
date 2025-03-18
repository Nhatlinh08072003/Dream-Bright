public interface IChatbotStrategy
{
    string GetAdvice(string userInput);
}

public class ChatbotContext
{
    private IChatbotStrategy strategy;

    public string GetResponse(string userInput)
    {
        // Chọn chiến lược dựa trên nội dung câu hỏi
        if (userInput.Contains("Mỹ") || userInput.Contains("mỹ") || userInput.Contains("canada") || userInput.Contains("Canada") || userInput.Contains("úc") || userInput.Contains("Úc") || userInput.Contains("Singapore"))
        {
            strategy = new CountryStrategy();
        }
        else if (userInput.Contains("IELTS") || userInput.Contains("ielts") || userInput.Contains("GPA") || userInput.Contains("gpa") || userInput.Contains("SAT") || userInput.Contains("sat"))
        {
            strategy = new AcademicStrategy();
        }
        else if (userInput.Contains("học phí") || userInput.Contains("Học phí") || userInput.Contains("cao") || userInput.Contains("thấp") || userInput.Contains("trung bình"))
        {
            strategy = new TuitionStrategy();
        }
        else if (userInput.Contains("xin chào") || userInput.Contains("chào"))
        {
            strategy = new CountryStrategy();
        }
        else if (userInput.Contains("cảm ơn") || userInput.Contains("Cảm ơn") || userInput.Contains("Cảm Ơn") || userInput.Contains("cảm Ơn"))
        {
            strategy = new CountryStrategy();
        }
        else
        {
            return "Xin lỗi, tôi chưa hiểu câu hỏi của bạn. Bạn có thể hỏi về du học, học lực hoặc học phí.";
        }

        return strategy.GetAdvice(userInput);
    }
}

