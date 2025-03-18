public class CountryStrategy : IChatbotStrategy
{
    public string GetAdvice(string userInput)  // Viết hoa chữ G để đúng chuẩn C#
    {
        if (userInput.Contains("Mỹ"))
            return string.Join("\n\n",
             "✅ Visa du học: Bạn cần xin visa F1 để học tại Mỹ. Quá trình xin visa yêu cầu:<br>" +
                  "- Thư mời nhập học từ trường (I-20)<br>" +
                  "- Chứng minh tài chính đủ chi trả học phí và sinh hoạt<br>" +
                  "- Bài phỏng vấn tại Đại sứ quán Mỹ<br><br>" +
                  "✅ Điều kiện tiếng Anh: Hầu hết các trường yêu cầu IELTS tối thiểu 6.5 hoặc TOEFL từ 80 điểm.<br><br>" +
                  "✅ Chi phí du học:<br>" +
                  "- Học phí trung bình: $20,000 - $50,000/năm tùy trường và ngành học<br>" +
                  "- Chi phí sinh hoạt: $10,000 - $20,000/năm<br><br>" +
                  "✅ Học bổng:<br>" +
                  "- Một số trường cung cấp học bổng dựa trên điểm GPA, bài luận, và thành tích ngoại khóa.<br>" +
                  "- Bạn có thể tìm học bổng từ tổ chức chính phủ, trường đại học hoặc các quỹ tài trợ tư nhân.<br><br>" +
                  "✅ Cơ hội sau tốt nghiệp:<br>" +
                  "- Sau khi học xong, bạn có thể xin OPT (Optional Practical Training) để làm việc tại Mỹ trong 12 - 36 tháng.<br>" +
                  "- Nếu được công ty bảo lãnh, bạn có thể xin visa H-1B để tiếp tục làm việc lâu dài.<br><br>" +
                  "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
            );

        if (userInput.Contains("mỹ")) return string.Join("\n\n",
             "✅ Visa du học: Bạn cần xin visa F1 để học tại Mỹ. Quá trình xin visa yêu cầu:<br>" +
                  "- Thư mời nhập học từ trường (I-20)<br>" +
                  "- Chứng minh tài chính đủ chi trả học phí và sinh hoạt<br>" +
                  "- Bài phỏng vấn tại Đại sứ quán Mỹ<br><br>" +
                  "✅ Điều kiện tiếng Anh: Hầu hết các trường yêu cầu IELTS tối thiểu 6.5 hoặc TOEFL từ 80 điểm.<br><br>" +
                  "✅ Chi phí du học:<br>" +
                  "- Học phí trung bình: $20,000 - $50,000/năm tùy trường và ngành học<br>" +
                  "- Chi phí sinh hoạt: $10,000 - $20,000/năm<br><br>" +
                  "✅ Học bổng:<br>" +
                  "- Một số trường cung cấp học bổng dựa trên điểm GPA, bài luận, và thành tích ngoại khóa.<br>" +
                  "- Bạn có thể tìm học bổng từ tổ chức chính phủ, trường đại học hoặc các quỹ tài trợ tư nhân.<br><br>" +
                  "✅ Cơ hội sau tốt nghiệp:<br>" +
                  "- Sau khi học xong, bạn có thể xin OPT (Optional Practical Training) để làm việc tại Mỹ trong 12 - 36 tháng.<br>" +
                  "- Nếu được công ty bảo lãnh, bạn có thể xin visa H-1B để tiếp tục làm việc lâu dài.<br><br>" +
                  "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
            );
        if (userInput.Contains("Canada"))
            return string.Join("\n\n",
                "🍁 Du học Canada là lựa chọn hấp dẫn với chi phí hợp lý, chất lượng giáo dục cao và cơ hội định cư rộng mở.<br>",
                "✅ Visa du học Canada có hai chương trình visa chính:<br>",
                "   - Chương trình SDS Xét duyệt nhanh, yêu cầu IELTS tối thiểu 6.0 và không cần chứng minh tài chính.<br>",
                "   - Chương trình chứng minh tài chính Dành cho sinh viên không đủ điều kiện SDS, cần cung cấp sổ tiết kiệm và chứng minh thu nhập.<br>",
                "✅ Điều kiện tiếng Anh Hầu hết các trường yêu cầu IELTS từ 6.0 - 6.5 hoặc TOEFL iBT từ 80 điểm.<br>",
                "✅ Chi phí du học<br>",
                "   - Học phí trung bình: $15,000 - $35,000 CAD/năm, tùy trường và ngành học.<br>",
                "   - Chi phí sinh hoạt: $10,000 - $15,000 CAD/năm.<br>",
                "✅ Học bổng<br>",
                "   - Các trường đại học và cao đẳng có nhiều chương trình học bổng dựa trên thành tích học tập và ngoại khóa.<br>",
                "   - Một số học bổng nổi bật: Học bổng chính phủ Canada, học bổng của trường, và các tổ chức tài trợ.<br>",
                "✅ Cơ hội sau tốt nghiệp<br>",
                "   - Sau khi tốt nghiệp, sinh viên có thể xin PGWP (Post-Graduation Work Permit) để làm việc tại Canada từ 1 - 3 năm.<br>",
                "   - Sau thời gian làm việc, bạn có thể nộp hồ sơ xin PR (Permanent Residence) theo chương trình Express Entry hoặc các chương trình định cư tỉnh bang.<br>",
                "🌟 Tại sao chọn Canada?<br>",
                "   - Chất lượng giáo dục thuộc top đầu thế giới.<br>",
                "   - Chính sách định cư hấp dẫn cho sinh viên quốc tế.<br>",
                "   - Môi trường sống an toàn, đa văn hóa, và thiên nhiên tuyệt đẹp.<br>",
                "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
            );

        if (userInput.Contains("canada"))
            return string.Join("\n\n",
                "🍁 Du học Canada là lựa chọn hấp dẫn với chi phí hợp lý, chất lượng giáo dục cao và cơ hội định cư rộng mở.<br>",
                "✅ Visa du học Canada có hai chương trình visa chính:<br>",
                "   - Chương trình SDS Xét duyệt nhanh, yêu cầu IELTS tối thiểu 6.0 và không cần chứng minh tài chính.<br>",
                "   - Chương trình chứng minh tài chính Dành cho sinh viên không đủ điều kiện SDS, cần cung cấp sổ tiết kiệm và chứng minh thu nhập.<br>",
                "✅ Điều kiện tiếng Anh Hầu hết các trường yêu cầu IELTS từ 6.0 - 6.5 hoặc TOEFL iBT từ 80 điểm.<br>",
                "✅ Chi phí du học<br>",
                "   - Học phí trung bình: $15,000 - $35,000 CAD/năm, tùy trường và ngành học.<br>",
                "   - Chi phí sinh hoạt: $10,000 - $15,000 CAD/năm.<br>",
                "✅ Học bổng<br>",
                "   - Các trường đại học và cao đẳng có nhiều chương trình học bổng dựa trên thành tích học tập và ngoại khóa.<br>",
                "   - Một số học bổng nổi bật: Học bổng chính phủ Canada, học bổng của trường, và các tổ chức tài trợ.<br>",
                "✅ Cơ hội sau tốt nghiệp<br>",
                "   - Sau khi tốt nghiệp, sinh viên có thể xin PGWP (Post-Graduation Work Permit) để làm việc tại Canada từ 1 - 3 năm.<br>",
                "   - Sau thời gian làm việc, bạn có thể nộp hồ sơ xin PR (Permanent Residence) theo chương trình Express Entry hoặc các chương trình định cư tỉnh bang.<br>",
                "🌟 Tại sao chọn Canada?<br>",
                "   - Chất lượng giáo dục thuộc top đầu thế giới.<br>",
                "   - Chính sách định cư hấp dẫn cho sinh viên quốc tế.<br>",
                "   - Môi trường sống an toàn, đa văn hóa, và thiên nhiên tuyệt đẹp.<br>",
                "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
            );

        if (userInput.Contains("Úc")) return string.Join("<br><br>",
    "🦘 <b>Du học Úc</b> – Điểm đến lý tưởng với nền giáo dục hàng đầu, cơ hội việc làm rộng mở và chính sách định cư hấp dẫn.",

    "✅ <b>Visa du học</b>: Bạn cần xin visa subclass 500 để học tập tại Úc. Điều kiện xin visa gồm:",
    "   - Thư mời nhập học (CoE - Confirmation of Enrollment).",
    "   - Chứng minh tài chính đủ trang trải học phí và sinh hoạt.",
    "   - Yêu cầu tiếng Anh: IELTS tối thiểu 5.5 - 6.5 tùy theo trường và ngành học.",
    "   - Bảo hiểm OSHC (Overseas Student Health Cover) trong suốt thời gian học.",

    "✅ <b>Chi phí du học</b>:",
    "   - Học phí trung bình: AUD $20,000 - $45,000/năm tùy theo bậc học và ngành học.",
    "   - Chi phí sinh hoạt: AUD $18,000 - $25,000/năm.",

    "✅ <b>Học bổng</b>:",
    "   - Học bổng chính phủ Australia Awards dành cho sinh viên xuất sắc.",
    "   - Học bổng Destination Australia hỗ trợ sinh viên học tập tại các vùng ít dân cư.",
    "   - Học bổng từ các trường đại học lên tới 100% học phí.",

    "✅ <b>Cơ hội việc làm</b>:",
    "   - Du học sinh có thể làm thêm tối đa 48 giờ/2 tuần trong thời gian học và không giới hạn giờ làm vào kỳ nghỉ.",
    "   - Các ngành có nhu cầu lao động cao: IT, y tế, kỹ sư, nhà hàng – khách sạn.",

    "✅ <b>Cơ hội sau tốt nghiệp</b>:",
    "   - Sau khi học xong, sinh viên có thể xin visa 485 (Temporary Graduate Visa) để làm việc từ 2 - 4 năm tại Úc.",
    "   - Nếu đủ điều kiện, bạn có thể xin visa thường trú PR theo diện tay nghề hoặc bảo lãnh doanh nghiệp.",

    "🌟 <b>Tại sao chọn Úc?</b>",
    "   - Chất lượng giáo dục được công nhận toàn cầu với các trường thuộc top thế giới như ANU, Melbourne, Sydney, UNSW.",
    "   - Cơ hội ở lại làm việc và định cư cao với chính sách ưu đãi cho sinh viên quốc tế.",
    "   - Môi trường sống an toàn, khí hậu ôn hòa, người dân thân thiện.",

    "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
);

        if (userInput.Contains("úc")) return string.Join("<br><br>",
    "🦘 <b>Du học Úc</b> – Điểm đến lý tưởng với nền giáo dục hàng đầu, cơ hội việc làm rộng mở và chính sách định cư hấp dẫn.",

    "✅ <b>Visa du học</b>: Bạn cần xin visa subclass 500 để học tập tại Úc. Điều kiện xin visa gồm:",
    "   - Thư mời nhập học (CoE - Confirmation of Enrollment).",
    "   - Chứng minh tài chính đủ trang trải học phí và sinh hoạt.",
    "   - Yêu cầu tiếng Anh: IELTS tối thiểu 5.5 - 6.5 tùy theo trường và ngành học.",
    "   - Bảo hiểm OSHC (Overseas Student Health Cover) trong suốt thời gian học.",

    "✅ <b>Chi phí du học</b>:",
    "   - Học phí trung bình: AUD $20,000 - $45,000/năm tùy theo bậc học và ngành học.",
    "   - Chi phí sinh hoạt: AUD $18,000 - $25,000/năm.",

    "✅ <b>Học bổng</b>:",
    "   - Học bổng chính phủ Australia Awards dành cho sinh viên xuất sắc.",
    "   - Học bổng Destination Australia hỗ trợ sinh viên học tập tại các vùng ít dân cư.",
    "   - Học bổng từ các trường đại học lên tới 100% học phí.",

    "✅ <b>Cơ hội việc làm</b>:",
    "   - Du học sinh có thể làm thêm tối đa 48 giờ/2 tuần trong thời gian học và không giới hạn giờ làm vào kỳ nghỉ.",
    "   - Các ngành có nhu cầu lao động cao: IT, y tế, kỹ sư, nhà hàng – khách sạn.",

    "✅ <b>Cơ hội sau tốt nghiệp</b>:",
    "   - Sau khi học xong, sinh viên có thể xin visa 485 (Temporary Graduate Visa) để làm việc từ 2 - 4 năm tại Úc.",
    "   - Nếu đủ điều kiện, bạn có thể xin visa thường trú PR theo diện tay nghề hoặc bảo lãnh doanh nghiệp.",

    "🌟 <b>Tại sao chọn Úc?</b>",
    "   - Chất lượng giáo dục được công nhận toàn cầu với các trường thuộc top thế giới như ANU, Melbourne, Sydney, UNSW.",
    "   - Cơ hội ở lại làm việc và định cư cao với chính sách ưu đãi cho sinh viên quốc tế.",
    "   - Môi trường sống an toàn, khí hậu ôn hòa, người dân thân thiện.",

    "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
);

        if (userInput.Contains("Singapore")) return string.Join("<br><br>",
    "🇸🇬 <b>Du học Singapore</b> – Lựa chọn hàng đầu với nền giáo dục đẳng cấp, khoảng cách địa lý gần Việt Nam, và chính sách thị thực đơn giản.",

    "✅ <b>Visa du học Singapore</b>:",
    "   - Singapore không yêu cầu visa du học (Student Pass) đối với các khóa học dưới 30 ngày.",
    "   - Với chương trình dài hạn, sinh viên cần xin Student Pass từ ICA (Immigration & Checkpoints Authority).",
    "   - Hồ sơ visa đơn giản, không yêu cầu chứng minh tài chính.",

    "✅ <b>Chi phí du học</b>:",
    "   - Học phí trung bình: 10,000 - 50,000 SGD/năm tùy theo bậc học và ngành học.",
    "   - Chi phí sinh hoạt: 8,000 - 15,000 SGD/năm, tùy vào khu vực và mức sống của sinh viên.",

    "✅ <b>Học bổng</b>:",
    "   - Học bổng ASEAN dành cho sinh viên xuất sắc, hỗ trợ 100% học phí và chi phí sinh hoạt.",
    "   - Học bổng từ các trường đại học như NUS, NTU, SMU có giá trị từ 25% - 100% học phí.",
    "   - Các chương trình hỗ trợ tài chính từ chính phủ Singapore và các tổ chức giáo dục.",

    "✅ <b>Cơ hội làm thêm</b>:",
    "   - Du học sinh theo hệ chính quy tại các trường công lập được phép làm thêm tối đa 16 giờ/tuần.",
    "   - Sinh viên học tại các trường tư thục không được phép làm thêm trong thời gian học.",

    "✅ <b>Cơ hội sau tốt nghiệp</b>:",
    "   - Sau khi tốt nghiệp, bạn có thể xin Work Pass hoặc Employment Pass để làm việc tại Singapore.",
    "   - Ngành nghề có nhu cầu nhân lực cao: tài chính, công nghệ thông tin, kỹ thuật, logistics.",
    "   - Chính phủ Singapore có các chương trình hỗ trợ sinh viên quốc tế ở lại làm việc và định cư.",

    "🌟 <b>Tại sao chọn Singapore?</b>",
    "   - Nền giáo dục đẳng cấp với các trường đại học hàng đầu như NUS, NTU, SMU.",
    "   - Khoảng cách địa lý gần Việt Nam, thuận tiện cho việc đi lại.",
    "   - Môi trường sống hiện đại, an toàn và có nhiều cơ hội việc làm hấp dẫn.",

    "🔹 Nếu bạn muốn tìm hiểu rõ hơn thì hãy Chat trực tiếp với nhân viên hoặc đăng ký tư vấn để được trao đổi trực tiếp qua số điện thoại hoặc gmail để được tư vấn kĩ hơn"
);

        if (userInput.Contains("Xin chào")) return "Xin chào! Tôi có thể giúp gì cho bạn?";
        if (userInput.Contains("chào")) return "Xin chào! Tôi có thể giúp gì cho bạn?";
        if (userInput.Contains("Cảm Ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        if (userInput.Contains("cảm ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        if (userInput.Contains("Cảm ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        if (userInput.Contains("Cảm Ơn")) return "Cảm ơn bạn đã dành thời gian với tôi! Nếu bạn muốn hỏi gì về Du Học, Học Phí, Chứng Chỉ thì cứ hỏi nhé!";
        return "Bạn muốn tư vấn về quốc gia nào? Mỹ, Canada, Úc?";
    }
}