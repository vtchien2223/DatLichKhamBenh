import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:nhom4_qlba_dlh/models/AppointmentDetails.dart';

class AppointmentDetailsScreen extends StatefulWidget {
  @override
  _AppointmentDetailsScreenState createState() =>
      _AppointmentDetailsScreenState();
}

class _AppointmentDetailsScreenState extends State<AppointmentDetailsScreen> {
  final String baseUrl = 'https://10.0.2.2:7290/api/AppointmentDetails';

  List<AppointmentDetails> appointmentDetails = [];

  String? userRole; // Biến để lưu role hiện tại của người dùng

  @override
  void initState() {
    super.initState();
    fetchCurrentUserRole(); // Lấy role khi khởi động
  }

  Future<void> fetchCurrentUserRole() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String? token = prefs.getString('jwt_token');

    if (token != null && !JwtDecoder.isExpired(token)) {
      // Giải mã token để lấy thông tin role
      Map<String, dynamic> decodedToken = JwtDecoder.decode(token);
      setState(() {
        userRole = decodedToken['role'] ?? 'User';
      });
      print('Current User Role: $userRole');
    } else {
      print('Token is missing or expired.');
      setState(() {
        userRole = 'User';
      });
    }

    // Sau khi lấy được role, gọi API để lấy dữ liệu
    fetchAppointmentDetails();
  }

  // Hàm lấy token từ SharedPreferences
  Future<String?> _getToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('jwt_token');
  }

  Future<void> fetchAppointmentDetails() async {
    try {
      SharedPreferences prefs = await SharedPreferences.getInstance();
      String? userName = prefs.getString('userName');
      String url;

      // So sánh role để quyết định URL API
      if (userRole != null && userRole!.toLowerCase() == 'admin') {
        // Nếu là Admin, lấy tất cả lịch hẹn
        url = baseUrl;
      } else {
        // Nếu là User, chỉ lấy lịch hẹn của mình
        url = '$baseUrl/GetAppointmentsByUser?userName=$userName';
      }

      String? token = await _getToken();
      if (token == null) {
        throw Exception('Token is missing');
      }

      final response = await http.get(
        Uri.parse(url),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
      );

      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        setState(() {
          appointmentDetails =
              data.map((item) => AppointmentDetails.fromJson(item)).toList();
        });
      } else {
        throw Exception('Failed to fetch appointment details');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching appointment details: $e')),
      );
    }
  }


  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Chi tiết cuộc hẹn'),
      ),
      body: appointmentDetails.isEmpty
          ? const Center(child: CircularProgressIndicator())
          : ListView.builder(
        itemCount: appointmentDetails.length,
        itemBuilder: (context, index) {
          final appointment = appointmentDetails[index];
          return Card(
            shape: RoundedRectangleBorder(
              borderRadius: BorderRadius.circular(15),
            ),
            margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            elevation: 4,
            child: Padding(
              padding: const EdgeInsets.all(16.0),
              child: Column(
                crossAxisAlignment: CrossAxisAlignment.start,
                children: [
                  Row(
                    children: [
                      const Icon(Icons.person, color: Colors.blue),
                      const SizedBox(width: 8),
                      Text(
                        'Bác sĩ: ${appointment.doctor?.fullName ?? "Không rõ"}',
                        style: TextStyle(
                          fontWeight: FontWeight.bold,
                          fontSize: 18,
                          color: Colors.black87,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 8),
                  Row(
                    children: [
                      const Icon(Icons.medical_services,
                          color: Colors.green),
                      const SizedBox(width: 8),
                      Text(
                        'Dịch vụ: ${appointment.service?.serviceName ?? "Không rõ"}',
                        style: TextStyle(
                          fontSize: 16,
                          color: Colors.black54,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 8),
                  Row(
                    children: [
                      const Icon(Icons.calendar_today,
                          color: Colors.orange),
                      const SizedBox(width: 8),
                      Text(
                        'Ngày: ${appointment.appointmentDate?.toLocal().toString().split(' ')[0] ?? "Không rõ"}',
                        style: TextStyle(
                          fontSize: 16,
                          color: Colors.black54,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 8),
                  Row(
                    children: [
                      const Icon(Icons.access_time_filled,
                          color: Colors.purple),
                      const SizedBox(width: 8),
                      Text(
                        'Thời gian bắt đầu: ${appointment.appointmentDateStart ?? "Không rõ"}',
                        style: TextStyle(
                          fontSize: 16,
                          color: Colors.black54,
                        ),
                      ),
                    ],
                  ),
                  const SizedBox(height: 8),
                  Row(
                    children: [
                      const Icon(Icons.access_time_filled,
                          color: Colors.red),
                      const SizedBox(width: 8),
                      Text(
                        'Thời gian kết thúc: ${appointment.appointmentDateEnd ?? "Không rõ"}',
                        style: TextStyle(
                          fontSize: 16,
                          color: Colors.black54,
                        ),
                      ),
                    ],
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
