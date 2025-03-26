import 'package:flutter/material.dart';
import 'package:nhom4_qlba_dlh/screens/admin_screen.dart';
import 'package:nhom4_qlba_dlh/screens/register_screen.dart';
import 'package:nhom4_qlba_dlh/utils/auth.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'package:jwt_decoder/jwt_decoder.dart'; // Để giải mã token
import 'main_screen.dart';

class LoginScreen extends StatefulWidget {
  const LoginScreen({super.key});

  @override
  _LoginScreenState createState() => _LoginScreenState();
}

class _LoginScreenState extends State<LoginScreen> {
  final TextEditingController _usernameController = TextEditingController();
  final TextEditingController _passwordController = TextEditingController();
  bool _isLoading = false;
  bool _obscurePassword = true;

  @override
  void initState() {
    super.initState();
    _checkToken(); // Kiểm tra token khi mở màn hình
  }

  // Kiểm tra token trong SharedPreferences
  Future<void> _checkToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    String? token = prefs.getString('jwt_token');

    if (token != null && !JwtDecoder.isExpired(token)) {
      // Nếu token tồn tại và còn hạn, giải mã token
      Map<String, dynamic> decodedToken = JwtDecoder.decode(token);
      String role = decodedToken['role'] ?? 'User';

      if (role == 'Admin') {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => MainScreen()), //const AdminScreen()),
        );
      } else {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => MainScreen()),
        );
      }
    }
  }

  Future<void> _handleLogin() async {
    if (_usernameController.text.isEmpty || _passwordController.text.isEmpty) {
      ScaffoldMessenger.of(context).showSnackBar(
        const SnackBar(
          content: Text('Vui lòng nhập đầy đủ thông tin'),
          backgroundColor: Colors.red,
        ),
      );
      return;
    }

    setState(() => _isLoading = true);

    // Gọi Auth.login để xử lý đăng nhập
    Map<String, dynamic> result = await Auth.login(
      _usernameController.text,
      _passwordController.text,
    );

    setState(() => _isLoading = false);

    if (result['success'] == true) {
      // Lấy thông tin người dùng từ kết quả trả về của API
      String token = result['token'];
      String userName = result['name'] ?? 'Không có tên'; // Đảm bảo lấy đúng giá trị
      String userEmail = result['email'] ?? 'Không có email';
      String role = result['role'] ?? 'User'; // Vai trò mặc định là User nếu không có

      // Lưu token và các thông tin khác vào SharedPreferences
      SharedPreferences prefs = await SharedPreferences.getInstance();
      await prefs.setString('jwt_token', token);
      await prefs.setString('userName', userName);
      await prefs.setString('userEmail', userEmail);
      await prefs.setString('userRole', role);

      // Kiểm tra log để đảm bảo thông tin đã được lưu
      debugPrint('Token: $token');
      debugPrint('Tên: $userName');
      debugPrint('Email: $userEmail');
      debugPrint('Vai trò: $role');

      // Điều hướng theo vai trò
      if (role == 'Admin') {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => MainScreen()),//const AdminScreen()),
        );
      } else {
        Navigator.pushReplacement(
          context,
          MaterialPageRoute(builder: (context) => MainScreen()),
        );
      }
    } else {
      // Hiển thị thông báo lỗi nếu đăng nhập thất bại
      String errorMessage = result['message'] ?? 'Tên đăng nhập hoặc mật khẩu không đúng';
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(
          content: Text(errorMessage),
          backgroundColor: Colors.red,
        ),
      );
    }
  }



  @override
  Widget build(BuildContext context) {
    return Scaffold(
      backgroundColor: Colors.white,
      body: SafeArea(
        child: SingleChildScrollView(
          child: Padding(
            padding: const EdgeInsets.symmetric(horizontal: 24.0),
            child: Column(
              children: [
                const SizedBox(height: 60),
                Text(
                  'NHOM4_QLBA',
                  textAlign: TextAlign.center,
                  style: TextStyle(
                    fontSize: 40,
                    fontWeight: FontWeight.bold,
                    color: Colors.blue[700],
                  ),
                ),
                const SizedBox(height: 40),
                TextField(
                  controller: _usernameController,
                  keyboardType: TextInputType.emailAddress,
                  decoration: InputDecoration(
                    hintText: 'Số điện thoại hoặc email',
                    filled: true,
                    fillColor: Colors.grey[100],
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide.none,
                    ),
                    contentPadding: const EdgeInsets.symmetric(
                      horizontal: 16,
                      vertical: 16,
                    ),
                  ),
                ),
                const SizedBox(height: 12),
                TextField(
                  controller: _passwordController,
                  obscureText: _obscurePassword,
                  decoration: InputDecoration(
                    hintText: 'Mật khẩu',
                    filled: true,
                    fillColor: Colors.grey[100],
                    border: OutlineInputBorder(
                      borderRadius: BorderRadius.circular(12),
                      borderSide: BorderSide.none,
                    ),
                    contentPadding: const EdgeInsets.symmetric(
                      horizontal: 16,
                      vertical: 16,
                    ),
                    suffixIcon: IconButton(
                      icon: Icon(
                        _obscurePassword ? Icons.visibility_off : Icons.visibility,
                        color: Colors.grey,
                      ),
                      onPressed: () {
                        setState(() => _obscurePassword = !_obscurePassword);
                      },
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                SizedBox(
                  height: 50,
                  child: ElevatedButton(
                    onPressed: _isLoading ? null : _handleLogin,
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.blue[700],
                      shape: RoundedRectangleBorder(
                        borderRadius: BorderRadius.circular(12),
                      ),
                      elevation: 0,
                    ),
                    child: _isLoading
                        ? const SizedBox(
                      width: 20,
                      height: 20,
                      child: CircularProgressIndicator(
                        strokeWidth: 2,
                        valueColor: AlwaysStoppedAnimation<Color>(Colors.white),
                      ),
                    )
                        : const Text(
                      'Đăng nhập',
                      style: TextStyle(
                        fontSize: 16,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
                const SizedBox(height: 20),
                // Thêm điều hướng đến màn hình đăng ký
                Row(
                  mainAxisAlignment: MainAxisAlignment.center,
                  children: [
                    const Text("Chưa có tài khoản? "),
                    TextButton(
                      onPressed: () {
                        Navigator.push(
                          context,
                          MaterialPageRoute(builder: (context) => const RegistrationScreen()),
                        );
                      },
                      child: const Text(
                        'Đăng ký ngay',
                        style: TextStyle(color: Colors.blue),
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),
        ),
      ),
    );
  }
}
