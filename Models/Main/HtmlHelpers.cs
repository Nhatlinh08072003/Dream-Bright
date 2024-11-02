using Microsoft.AspNetCore.Html;

namespace Dream_Bridge.Helpers // Thay 'YourNamespace' bằng namespace của bạn
{
    public static class HtmlHelpers
    {

        public static IHtmlContent FormatNewsContent(string content)
        {
            // Thay thế các thẻ h1, p với các lớp CSS mong muốn
            content = content.Replace("<h1>", "<h1 class='mb-4 mt-10  text-2xl font-bold'>");
            content = content.Replace("<p>", "<p class='mt-2 text-gray-700'>");
            content = content.Replace("<ul>", "<ul class='list-inside list-disc text-sm '>");
            // Thay thế thêm cho các thẻ khác nếu cần
            return new HtmlString(content);
        }
    }
}
