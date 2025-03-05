public class CountryStrategy : IChatbotStrategy
{
    public string GetAdvice(string userInput)  // Viết hoa chữ G để đúng chuẩn C#
    {
        if (userInput.Contains("Mỹ")) return "Du học Mỹ cần visa F1 và điểm IELTS 6.5+.";
        if (userInput.Contains("mỹ")) return "Du học Mỹ cần visa F1 và điểm IELTS 6.5+.";
        if (userInput.Contains("Canada")) return "Du học Canada có chương trình CES không cần chứng minh tài chính.";
        if (userInput.Contains("canada")) return "Du học Canada có chương trình CES không cần chứng minh tài chính.";
        if (userInput.Contains("Úc")) return "Du học Úc có nhiều chương trình học bổng hấp dẫn.";
        if (userInput.Contains("úc")) return "Du học Úc có nhiều chương trình học bổng hấp dẫn.";
        if (userInput.Contains("xin chào")) return "Xin chào! Tôi có thể giúp gì cho bạn?";
        if (userInput.Contains("Xin chào")) return "Xin chào! Tôi có thể giúp gì cho bạn?";
        if (userInput.Contains("chào")) return "Xin chào! Tôi có thể giúp gì cho bạn?";
        if (userInput.Contains("Cảm Ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        if (userInput.Contains("cảm ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        if (userInput.Contains("Cảm ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        if (userInput.Contains("Cảm Ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        return "Bạn muốn tư vấn về quốc gia nào? Mỹ, Canada, Úc?";
    }
}