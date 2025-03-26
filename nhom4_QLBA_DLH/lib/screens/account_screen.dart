import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'login_screen.dart';

class AccountScreen extends StatefulWidget {
  @override
  _AccountScreenState createState() => _AccountScreenState();
}

class _AccountScreenState extends State<AccountScreen> {
  bool isLoggedIn = false;
  String userName = 'Khách';
  String userEmail = '';

  @override
  void initState() {
    super.initState();
    _loadUserInfo(); // Kiểm tra và lấy thông tin người dùng
  }

  /// Hàm kiểm tra trạng thái đăng nhập và lấy thông tin người dùng
  Future<void> _loadUserInfo() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      final name = prefs.getString('userName') ?? "Khách";
      final email = prefs.getString('userEmail') ?? "";
      final token = prefs.getString('jwt_token') ?? "";

      setState(() {
        userName = name;
        userEmail = email;
      });

      debugPrint('Tên người dùng: $userName');
      debugPrint('Email: $userEmail');
      debugPrint('Token: $token');
    } catch (e) {
      debugPrint("Lỗi khi tải thông tin người dùng: $e");
    }
  }



  /// Hàm đăng xuất
  Future<void> _logout() async {
    try {
      final prefs = await SharedPreferences.getInstance();
      await prefs.clear(); // Xóa tất cả dữ liệu lưu trữ

      setState(() {
        isLoggedIn = false;
        userName = "Khách";
        userEmail = "";
      });

      // Chuyển về màn hình đăng nhập
      Navigator.pushReplacement(
        context,
        MaterialPageRoute(builder: (context) => LoginScreen()),
      );
    } catch (e) {
      debugPrint("Lỗi khi đăng xuất: $e");
    }
  }

  void _showLogoutConfirmationDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text("Xác nhận đăng xuất"),
          content: const Text("Bạn có chắc chắn muốn đăng xuất không?"),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Đóng hộp thoại
              },
              child: const Text("Hủy"),
            ),
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Đóng hộp thoại
                _logout(); // Gọi hàm _logout nếu người dùng chọn "Đăng xuất"
              },
              child: const Text("Đăng xuất"),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.grey[100],
      body: SingleChildScrollView(
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            // Header Section
            _buildHeader(),
            const SizedBox(height: 16),

            // List of Options
            _buildOptionsList(),

            const SizedBox(height: 16),

            // Footer Section
            _buildFooter(),
          ],
        ),
      ),

      // Thanh điều hướng
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: 4, // Tab "Tài khoản"
        onTap: (index) {
          // Xử lý chuyển tab
        },
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.home),
            label: 'Trang chủ',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.folder),
            label: 'Hồ sơ',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.receipt),
            label: 'Phiếu khám',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.notifications),
            label: 'Thông báo',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person),
            label: 'Tài khoản',
          ),
        ],
      ),
    );
  }

  /// Xây dựng phần Header
  Widget _buildHeader() {
    return Container(
      padding: const EdgeInsets.symmetric(vertical: 20),
      decoration: const BoxDecoration(
        gradient: LinearGradient(
          colors: [Colors.blue, Colors.lightBlueAccent],
          begin: Alignment.topCenter,
          end: Alignment.bottomCenter,
        ),
      ),
      child: Column(
        children: [
          const CircleAvatar(
            radius: 40,
            backgroundColor: Colors.white,
            child: Icon(Icons.person, size: 40, color: Colors.blue),
          ),
          const SizedBox(height: 8),
          Text(
            userName,
            style: const TextStyle(
              fontSize: 18,
              fontWeight: FontWeight.bold,
              color: Colors.white,
            ),
          ),
          const SizedBox(height: 8),
          Text(
            userEmail,
            style: const TextStyle(
              fontSize: 16,
              color: Colors.white70,
            ),
          ),
        ],
      ),
    );
  }

  /// Xây dựng danh sách tùy chọn
  Widget _buildOptionsList() {
    return Column(
      children: [
        _buildListTile(
          icon: Icons.security,
          title: "Quy định sử dụng",
          onTap: () {},
        ),
        _buildListTile(
          icon: Icons.privacy_tip,
          title: "Chính sách bảo mật",
          onTap: () {},
        ),
        _buildListTile(
          icon: Icons.assignment,
          title: "Điều khoản dịch vụ",
          onTap: () {},
        ),
        _buildListTile(
          icon: Icons.phone,
          title: "Tổng đài CSKH 19002115",
          onTap: () {},
        ),
        _buildListTile(
          icon: Icons.thumb_up,
          title: "Đánh giá ứng dụng",
          onTap: () {},
        ),
        _buildListTile(
          icon: Icons.share,
          title: "Chia sẻ ứng dụng",
          onTap: () {},
        ),
          _buildListTile(
            icon: Icons.logout,
            title: "Đăng xuất",
            onTap: _showLogoutConfirmationDialog,
          ),
      ],
    );
  }

  /// Xây dựng phần Footer
  Widget _buildFooter() {
    return Container(
      padding: const EdgeInsets.all(16),
      color: Colors.white,
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.start,
        children: const [
          Text(
            "Địa chỉ: Cơ sở 221B Hoàng Văn Thụ, Phường 8, Quận Phú Nhuận, TP. Hồ Chí Minh",
            style: TextStyle(fontSize: 14, color: Colors.black87),
          ),
          SizedBox(height: 8),
          Text(
            "Email: bvdaihoccoso3@umc.edu.vn\n(028) 38 420 070\nhttps://www.bvdaihoccoso3.com.vn/",
            style: TextStyle(fontSize: 14, color: Colors.blue),
          ),
          SizedBox(height: 8),
          Text(
            "Phiên bản 1.0.1",
            style: TextStyle(fontSize: 14, color: Colors.grey),
          ),
        ],
      ),
    );
  }

  /// Xây dựng danh sách các mục
  Widget _buildListTile({
    required IconData icon,
    required String title,
    required VoidCallback onTap,
  }) {
    return ListTile(
      leading: Icon(icon, color: Colors.blue),
      title: Text(
        title,
        style: const TextStyle(fontSize: 16),
      ),
      trailing: const Icon(Icons.arrow_forward_ios, size: 16),
      onTap: onTap,
    );
  }
}
