# Dream-Bright
README Dự Án
Yêu cầu chung khi làm dự án
1. Yêu cầu về môi trường phát triển
Ngôn ngữ lập trình: .NET (sử dụng ASP.NET Core 8 MVC).
Quản lý package: Sử dụng NuGet cho các package liên quan đến .NET và npm hoặc yarn cho các package frontend.
CSS Framework: Tailwind CSS được sử dụng cho việc thiết kế giao diện.
Database: Hỗ trợ SQLite, SQL Server, hoặc bất kỳ hệ quản trị cơ sở dữ liệu phù hợp với dự án.
Công cụ phát triển: Visual Studio, VS Code, hoặc các IDE hỗ trợ .NET.
2. Yêu cầu cài đặt
Clone dự án

bash
Sao chép mã
git clone <URL repo của bạn>
cd <thư mục dự án>
Cài đặt các package

bash
Sao chép mã
# Cài đặt package .NET
dotnet restore

# Cài đặt package frontend
npm install
Cấu hình môi trường: Thêm file appsettings.json và thiết lập các thông tin như kết nối cơ sở dữ liệu, thông số cần thiết khác.

Chạy ứng dụng

Sao chép mã
dotnet watch run run
Hoặc chạy từ IDE như Visual Studio.

3. Quy tắc làm việc
Quản lý source code: Sử dụng git, mỗi thay đổi cần được đẩy vào một branch riêng và tạo pull request (PR) để review trước khi merge vào branch chính.
Quy ước đặt tên: Tuân thủ theo quy ước đặt tên của dự án, sử dụng tiếng Anh cho các biến, hàm và file.
Code convention: Theo chuẩn coding của .NET, có thể sử dụng các công cụ như StyleCop để kiểm tra.
Migration: Sử dụng các migration để quản lý cơ sở dữ liệu (đặc biệt nếu chưa sử dụng trước đây).
bash
Sao chép mã
dotnet ef migrations add <TênMigration>
dotnet ef database update
Test: Viết unit test hoặc integration test nếu cần thiết để đảm bảo tính ổn định của ứng dụng.
4. Triển khai
Chuẩn bị server: Thiết lập server phù hợp với yêu cầu của ứng dụng.
CI/CD: Nếu có thể, tích hợp quy trình CI/CD để tự động hóa việc build và deploy ứng dụng.
5. Các tài liệu tham khảo
ASP.NET Core Documentation
Tailwind CSS Documentation
Liên hệ
Email liên hệ: <letiennhatlinh08072003@gmail.com>
Nhóm phát triển: <Công nghệ phần mềm nâng cao>
