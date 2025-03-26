import 'dart:convert';

import 'package:jwt_decoder/jwt_decoder.dart';
import 'package:nhom4_qlba_dlh/config/config_url.dart';
import 'package:http/http.dart' as http;
import 'package:shared_preferences/shared_preferences.dart';

class AuthService {
  // đường dẫn tới API login
  String get apiUrl => "${Config_URL.baseUrl}login";

  Future<Map<String, dynamic>> login(String username, String password) async {
    try {
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: {"Content-Type": "application/json"},
        body: jsonEncode({
          "username": username,
          "password": password,
        }),
      );

      if (response.statusCode == 200) {
        final data = jsonDecode(response.body);
        bool status = data['status'];
        if (!status) {
          return {"success": false, "message": data['message']};
        }

        String token = data['token'];
        String name = data['name'];
        String email = data['email'];
        String role = data['role']; // Nhận vai trò từ phản hồi API

        // Lưu thông tin vào SharedPreferences
        SharedPreferences prefs = await SharedPreferences.getInstance();
        prefs.setString('jwt_token', token);
        prefs.setString('userName', name);
        prefs.setString('userEmail', email);
        prefs.setString('userRole', role); // Lưu vai trò

        return {
          "success": true,
          "token": token,
          "role": role,
          "name": name,
          "email": email,
        };
      } else {
        return {"success": false, "message": "Login failed: ${response.statusCode}"};
      }
    } catch (e) {
      return {"success": false, "message": "Network error: $e"};
    }
  }


  Future<Map<String, dynamic>> register({
    required String username,
    required String email,
    required String password,
    required String initials,
    required String role,
  }) async {
    try {
      final response = await http.post(
        Uri.parse("${Config_URL.baseUrl}register"), // Đảm bảo đây là URL chính xác
        headers: {"Content-Type": "application/json"},
        body: jsonEncode({
          "username": username,
          "email": email,
          "password": password,
          "initials": initials,
          "role": role,
        }),
      );

      // Kiểm tra phản hồi từ API
      print("Response status: ${response.statusCode}");
      print("Response body: ${response.body}");

      if (response.statusCode == 200 || response.statusCode == 201) {
        final data = jsonDecode(response.body);
        return {
          "success": true,
          "message": data['message'] ?? 'Đăng ký thành công!',
        };
      } else {
        final data = jsonDecode(response.body);
        return {
          "success": false,
          "message": data['message'] ?? 'Đăng ký thất bại',
        };
      }
    } catch (e) {
      return {
        "success": false,
        "message": "Lỗi kết nối mạng: $e",
      };
    }
  }


}
