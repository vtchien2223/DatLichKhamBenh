import 'package:flutter/material.dart';

class NotificationScreen extends StatelessWidget {
  final List<Map<String, String>> notifications = [
    {
      'title': '⚡ Lịch nghỉ Quốc khánh 2024 Bệnh viện Đại học Y Dược TPHCM',
      'content': '🇻🇳 Đại Lễ Quốc khánh 02.09.2024 sắp đến! Chúng tôi xin thông báo lịch nghỉ của Bệnh viện ĐH Y Dược...',
      'time': '08:15 28/08/2024'
    },
    {
      'title': '⚡ Lịch nghỉ Quốc khánh 2024 Bệnh viện Đại học Y Dược TPHCM',
      'content': '🇻🇳 Đại Lễ Quốc khánh 02.09.2024 sắp đến! Chúng tôi xin thông báo lịch nghỉ của Bệnh viện ĐH Y Dược...',
      'time': '20:50 23/08/2024'
    },
    {
      'title': '🔥 Bệnh sởi bùng phát, Sở Y tế TPHCM đề xuất công bố dịch',
      'content': '⚡ Từ 23/05 đến nay, các bệnh viện đã ghi nhận hơn 500 ca sởi phát ban nghi nhiễm và 346 ca d...',
      'time': '19:40 16/08/2024'
    },
    {
      'title': 'Tiêm chủng an toàn, đặt lịch dễ dàng tại 6 bệnh viện uy tín',
      'content': 'Bảo vệ sức khỏe bằng cách lựa chọn cơ sở y tế tiêm chủng uy tín, Medpro cung cấp danh sách 6 bệ...',
      'time': '18:58 13/07/2024'
    },
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey[100],
      appBar: AppBar(
        title: const Text(
          'Danh sách thông báo',
          style: TextStyle(color: Colors.white),
        ),
        backgroundColor: Colors.blue,
        centerTitle: true,
        elevation: 0,
      ),
      body: ListView.builder(
        padding: const EdgeInsets.all(16),
        itemCount: notifications.length,
        itemBuilder: (context, index) {
          return Card(
            margin: const EdgeInsets.only(bottom: 16),
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(12),
            ),
            elevation: 3,
            child: Padding(
              padding: const EdgeInsets.all(12),
              child: Row(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Container(
                    width: 40,
                    height: 40,
                    decoration: BoxDecoration(
                      color: Colors.blue,
                      borderRadius: BorderRadius.circular(8),
                    ),
                    child: const Icon(Icons.email, color: Colors.white),
                  ),
                  const SizedBox(width: 12),
                  Expanded(
                    child: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          notifications[index]['title']!,
                          style: const TextStyle(
                            fontSize: 16,
                            fontWeight: FontWeight.bold,
                          ),
                        ),
                        const SizedBox(height: 8),
                        Text(
                          notifications[index]['content']!,
                          style: const TextStyle(
                            fontSize: 14,
                            color: Colors.black87,
                          ),
                          maxLines: 2,
                          overflow: TextOverflow.ellipsis,
                        ),
                        const SizedBox(height: 8),
                        Text(
                          notifications[index]['time']!,
                          style: const TextStyle(
                            fontSize: 12,
                            color: Colors.grey,
                          ),
                        ),
                      ],
                    ),
                  ),
                ],
              ),
            ),
          );
        },
      ),
    );
  }
}
