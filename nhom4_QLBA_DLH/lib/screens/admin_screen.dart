import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'login_screen.dart';

class AdminScreen extends StatelessWidget {
  const AdminScreen({super.key});

  Future<void> _logout(BuildContext context) async {
    // Xóa token khỏi SharedPreferences
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.remove('jwt_token');

    // Điều hướng về màn hình đăng nhập và xóa toàn bộ stack
    Navigator.pushAndRemoveUntil(
      context,
      MaterialPageRoute(builder: (context) => const LoginScreen()),
          (route) => false,
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Admin Dashboard"),
        actions: [
          IconButton(
            icon: const Icon(Icons.logout),
            onPressed: () {
              // Hiển thị hộp thoại xác nhận trước khi logout
              showDialog(
                context: context,
                builder: (context) => AlertDialog(
                  title: const Text("Confirm Logout"),
                  content: const Text("Are you sure you want to logout?"),
                  actions: [
                    TextButton(
                      onPressed: () => Navigator.pop(context), // Đóng hộp thoại
                      child: const Text("Cancel"),
                    ),
                    TextButton(
                      onPressed: () {
                        Navigator.pop(context); // Đóng hộp thoại
                        _logout(context); // Gọi hàm logout
                      },
                      child: const Text("Logout"),
                    ),
                  ],
                ),
              );
            },
          ),
        ],
      ),
      body: Center(
        child: Text(
          'Welcome, Admin!',
          style: Theme.of(context).textTheme.headlineMedium,
        ),
      ),
    );
  }
}
