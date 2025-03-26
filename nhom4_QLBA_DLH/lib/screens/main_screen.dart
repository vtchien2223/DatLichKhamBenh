import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:http/http.dart';
import 'package:nhom4_qlba_dlh/models/AppointmentDetails.dart';
import 'package:nhom4_qlba_dlh/models/post.dart';
import 'package:nhom4_qlba_dlh/screens/apponitment_screen.dart';
import 'package:nhom4_qlba_dlh/screens/patient_screen.dart';
import 'package:shared_preferences/shared_preferences.dart';
import 'bookappointment_screen.dart';
import 'package:nhom4_qlba_dlh/screens/account_screen.dart';
import 'package:nhom4_qlba_dlh/screens/medicalform_screen.dart';
import 'package:nhom4_qlba_dlh/screens/notification_screen.dart';
import 'package:nhom4_qlba_dlh/screens/profile_screen.dart';

class MainScreen extends StatefulWidget {
  @override
  _MainScreenState createState() => _MainScreenState();
}

class _MainScreenState extends State<MainScreen> {
  int _selectedIndex = 0;

  final List<Widget> _pages = [
    MainScreenContent(),
    PatientScreen(),
    AppointmentDetailsScreen(),
    NotificationScreen(),
    AccountScreen(),
  ];

  void _onItemTapped(int index) {
    setState(() {
      _selectedIndex = index;
    });
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: _pages[_selectedIndex],
      bottomNavigationBar: BottomNavigationBar(
        currentIndex: _selectedIndex,
        onTap: _onItemTapped,
        type: BottomNavigationBarType.fixed,
        items: const [
          BottomNavigationBarItem(
            icon: Icon(Icons.home_outlined),
            activeIcon: Icon(Icons.home),
            label: 'Trang chủ',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.folder_open_outlined),
            activeIcon: Icon(Icons.folder),
            label: 'Hồ sơ',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.receipt_long_outlined),
            activeIcon: Icon(Icons.receipt_long),
            label: 'Lịch hẹn',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.notifications_none),
            activeIcon: Icon(Icons.notifications),
            label: 'Thông báo',
          ),
          BottomNavigationBarItem(
            icon: Icon(Icons.person_outline),
            activeIcon: Icon(Icons.person),
            label: 'Tài khoản',
          ),
        ],
        selectedItemColor: Colors.blue,
        unselectedItemColor: Colors.grey,
        selectedFontSize: 12,
        unselectedFontSize: 12,
        showSelectedLabels: true,
        showUnselectedLabels: true,
      ),
    );
  }
}

class MainScreenContent extends StatefulWidget {
  @override
  _MainScreenContentState createState() => _MainScreenContentState();
}

class _MainScreenContentState extends State<MainScreenContent> {
  final String apiUrl = 'https://10.0.2.2:7290/api/PostApi';
  List<Post> posts = [];
  bool isLoading = false;


  @override
  void initState() {
    super.initState();
    fetchPosts();
    checkAndNotifyUpcomingAppointments();
  }

  Future<String?> _getToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('jwt_token');
  }

  Future<void> fetchPosts() async {
    setState(() {
      isLoading = true;
    });

    try {
      String? token = await _getToken();
      if (token == null) {
        throw Exception('Không tìm thấy token');
      }

      final response = await http.get(
        Uri.parse(apiUrl),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
      );

      if (response.statusCode == 200) {
        final List<dynamic> data = json.decode(response.body);
        setState(() {
          posts = data.map((e) => Post.fromJson(e)).toList();
          isLoading = false;
        });
      } else if (response.statusCode == 403) {
        // Hiển thị hộp thoại khi không có quyền truy cập
        _showAccessDeniedDialog();
      } else {
        throw Exception('Lỗi không xác định');
      }
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi: $error')),
      );
      setState(() {
        isLoading = false;
      });
    }
  }

// Hàm hiển thị Dialog thông báo lỗi
  void _showAccessDeniedDialog() {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('Lỗi quyền truy cập'),
          content: const Text('BẠN KHÔNG CÓ QUYỀN TRUY CẬP'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Đóng Dialog
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }


  void _showSuccessDialog(String message) {
    showDialog(
      context: context,
      builder: (BuildContext context) {
        return AlertDialog(
          title: const Text('THÀNH CÔNG'),
          content: Text(message),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.of(context).pop(); // Đóng Dialog
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  Future<void> deletePost(int id) async {
    try {
      String? token = await _getToken();
      final response = await http.delete(
        Uri.parse('$apiUrl/$id'),
        headers: {
          'Authorization': 'Bearer $token',
        },
      );

      if (response.statusCode == 204) {
        setState(() {
          posts.removeWhere((post) => post.id == id);
        });
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Xóa bài viết thành công')),
        );
      } else {
        //throw Exception('BẠN KHÔNG CÓ QUYỀN TRUY CẬP');
        _showAccessDeniedDialog();
      }
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi: $error')),
      );
    }
  }


  void showPostDialog({Post? post}) {
    final titleController = TextEditingController(text: post?.title ?? '');
    final contentController = TextEditingController(text: post?.content ?? '');
    final imageUrlController = TextEditingController(text: post?.imageUrl ?? '');
    final authorController = TextEditingController(text: post?.author ?? '');

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(post == null ? 'Thêm bài viết' : 'Chỉnh sửa bài viết'),
          content: SingleChildScrollView(
            child: Column(
              children: [
                TextField(
                  controller: titleController,
                  decoration: const InputDecoration(labelText: 'Tiêu đề'),
                ),
                TextField(
                  controller: contentController,
                  decoration: const InputDecoration(labelText: 'Nội dung'),
                  maxLines: 3,
                ),
                TextField(
                  controller: imageUrlController,
                  decoration: const InputDecoration(labelText: 'URL Hình ảnh'),
                ),
                TextField(
                  controller: authorController,
                  decoration: const InputDecoration(labelText: 'Tác giả'),
                ),
              ],
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('Hủy'),
            ),
            ElevatedButton(
              onPressed: () {
                final newPost = Post(
                  id: post?.id,
                  title: titleController.text,
                  content: contentController.text,
                  imageUrl: imageUrlController.text,
                  author: authorController.text,
                  createdAt: DateTime.now(),
                );

                if (post == null) {
                  createPost(newPost);
                } else {
                  updatePost(newPost);
                }

                Navigator.pop(context);
              },
              child: const Text('Lưu'),
            ),
          ],
        );
      },
    );
  }

  Future<void> createPost(Post newPost) async {
    try {
      String? token = await _getToken();
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
        body: json.encode({
          'title': newPost.title,
          'content': newPost.content,
          'imageUrl': newPost.imageUrl,
          'author': newPost.author,
          'createdAt': newPost.createdAt.toIso8601String(),
        }),
      );

      print('Response status: ${response.statusCode}');
      print('Response body: ${response.body}');
      if (response.statusCode == 201) {
        fetchPosts(); // Cập nhật danh sách bài viết sau khi thêm thành công
      } else {
        _showAccessDeniedDialog();
        _showSuccessDialog('Bài viết đã được thêm thành công!');
      }
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi: $error')),
      );
    }
  }


  Future<void> updatePost(Post updatedPost) async {
    try {
      String? token = await _getToken();
      final response = await http.put(
        Uri.parse('$apiUrl/${updatedPost.id}'),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
        body: json.encode(updatedPost.toJson()),
      );

      if (response.statusCode == 204) {
        fetchPosts();
        _showSuccessDialog('Bài viết đã được cập nhật thành công!');
      } else {
        _showAccessDeniedDialog();
      }
    } catch (error) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi: $error')),
      );
    }
  }

  Future<String?> getCurrentUserName() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('userName');
  }

  Future<List<AppointmentDetails>> fetchUpcomingAppointmentsByUser(String userName) async {
    try {
      final response = await http.get(
        Uri.parse('https://10.0.2.2:7290/api/AppointmentDetails/UpcomingAppointments?userName=$userName'),
        headers: {
          'Authorization': 'Bearer YOUR_TOKEN',
          'Content-Type': 'application/json',
        },
      );

      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        return data.map((json) => AppointmentDetails.fromJson(json)).toList();
      } else {
        throw Exception('Failed to fetch upcoming appointments');
      }
    } catch (e) {
      throw Exception('Error fetching data: $e');
    }
  }

  Future<void> checkAndNotifyUpcomingAppointments() async {
    try {
      SharedPreferences prefs = await SharedPreferences.getInstance();

      // Kiểm tra trạng thái đã hiển thị thông báo
      bool isNotificationShown = prefs.getBool('notification_shown') ?? false;

      // Nếu đã thông báo, thoát ra ngay
      if (isNotificationShown) return;

      // Lấy username hiện tại
      String? currentUserName = await getCurrentUserName();
      if (currentUserName == null) {
        print('No userName found in SharedPreferences');
        return;
      }

      // Lấy danh sách các cuộc hẹn sắp tới
      final appointments = await fetchUpcomingAppointmentsByUser(currentUserName);
      if (appointments.isNotEmpty) {
        // Hiển thị thông báo
        _showUpcomingAppointmentsDialog(appointments);

        // Đánh dấu trạng thái đã thông báo
        await prefs.setBool('notification_shown', true);
      }
    } catch (e) {
      print('Error: $e');
    }
  }

  Future<void> checkAndShowUpcomingAppointmentsDialog(List<AppointmentDetails> appointments) async {
    SharedPreferences prefs = await SharedPreferences.getInstance();

    // Kiểm tra trạng thái thông báo
    bool isNotificationShown = prefs.getBool('notification_shown') ?? false;

    if (!isNotificationShown) {
      // Hiển thị thông báo nếu chưa được thông báo lần nào
      _showUpcomingAppointmentsDialog(appointments);

      // Lưu trạng thái đã hiển thị thông báo
      await prefs.setBool('notification_shown', true);
    }
  }


  void _showUpcomingAppointmentsDialog(List<AppointmentDetails> appointments) {
    final uniqueAppointments = appointments.toSet();
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text(
            'Nhắc nhở cuộc hẹn',
            style: TextStyle(
              fontWeight: FontWeight.bold,
              fontSize: 20,
              color: Colors.blueAccent,
            ),
          ),
          content: SingleChildScrollView(
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: uniqueAppointments.map((appointment) {
                return Padding(
                  padding: const EdgeInsets.only(bottom: 16.0),
                  child: Card(
                    elevation: 6,
                    shape: RoundedRectangleBorder(
                      borderRadius: BorderRadius.circular(12),
                    ),
                    color: Colors.white,
                    child: Padding(
                      padding: const EdgeInsets.all(12.0),
                      child: Column(
                        crossAxisAlignment: CrossAxisAlignment.start,
                        children: [
                          Row(
                            children: [
                              Icon(Icons.person, color: Colors.blue, size: 24),
                              SizedBox(width: 8),
                              Text(
                                'Bác sĩ: ${appointment.doctor?.fullName ?? "Không rõ"}',
                                style: TextStyle(
                                  fontWeight: FontWeight.bold,
                                  fontSize: 16,
                                  color: Colors.black87,
                                ),
                              ),
                            ],
                          ),
                          SizedBox(height: 10),
                          Row(
                            children: [
                              Icon(Icons.healing, color: Colors.green, size: 24),
                              SizedBox(width: 8),
                              Text(
                                'Dịch vụ: ${appointment.service?.serviceName ?? "Không rõ"}',
                                style: TextStyle(
                                  fontWeight: FontWeight.bold,
                                  fontSize: 16,
                                  color: Colors.black87,
                                ),
                              ),
                            ],
                          ),
                          SizedBox(height: 10),
                          Row(
                            children: [
                              Icon(Icons.calendar_today, color: Colors.orange, size: 24),
                              SizedBox(width: 8),
                              Text(
                                'Ngày: ${appointment.appointmentDate?.toLocal().toString().split(' ')[0] ?? "Không rõ"}',
                                style: TextStyle(
                                  fontWeight: FontWeight.bold,
                                  fontSize: 16,
                                  color: Colors.black87,
                                ),
                              ),
                            ],
                          ),
                        ],
                      ),
                    ),
                  ),
                );
              }).toList(),
            ),
          ),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text(
                'OK',
                style: TextStyle(fontWeight: FontWeight.bold),
              ),
            ),
          ],
        );
      },
    );
  }

  Future<void> resetNotificationState() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    await prefs.setBool('notification_shown', false);
  }

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(
      child: Column(
        crossAxisAlignment: CrossAxisAlignment.stretch,
        children: [
          // Header
          Container(
            padding: const EdgeInsets.symmetric(vertical: 20, horizontal: 16),
            decoration: const BoxDecoration(
              gradient: LinearGradient(
                colors: [Colors.blue, Colors.lightBlueAccent],
                begin: Alignment.topCenter,
                end: Alignment.bottomCenter,
              ),
              borderRadius: BorderRadius.only(
                bottomLeft: Radius.circular(0),
                bottomRight: Radius.circular(0),
              ),
            ),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: const [
                Text(
                  'BONEKA AMBALABU',
                  style: TextStyle(
                    fontSize: 24,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
                SizedBox(height: 8),
                Row(
                  children: [
                    Icon(
                      Icons.star, // Thay thế bằng icon bạn muốn
                      color: Colors.white70,
                      size: 16,
                    ),
                    SizedBox(width: 8), // Khoảng cách giữa icon và text
                    Text(
                      'Hello you, Welcome',
                      style: TextStyle(
                        fontSize: 16,
                        color: Colors.white70,
                      ),
                    ),
                  ],
                ),
              ],
            ),
          ),

          // Đặt lịch khám bệnh
          Container(
            margin: const EdgeInsets.symmetric(horizontal: 16, vertical: 8),
            child: ElevatedButton.icon(
              onPressed: () {
                Navigator.push(
                  context,
                  MaterialPageRoute(builder: (context) => BookAppointmentScreen()),
                );
              },
              style: ElevatedButton.styleFrom(
                padding: const EdgeInsets.symmetric(vertical: 16),
                shape: RoundedRectangleBorder(
                  borderRadius: BorderRadius.circular(12),
                ),
                backgroundColor: Colors.blue,
              ),
              icon: const Icon(Icons.calendar_today, color: Colors.white),
              label: const Text(
                'Đặt lịch khám bệnh',
                style: TextStyle(fontSize: 16, color: Colors.white),
              ),
            ),
          ),

          // Danh sách bài viết
          Padding(
            padding: const EdgeInsets.all(16),
            child: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              children: [
                // Tiêu đề và nút thêm bài viết
                Row(
                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                  children: [
                    const Text(
                      'Tin mới nhất',
                      style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
                    ),
                    ElevatedButton.icon(
                      onPressed: () {
                        showPostDialog();
                      },
                      icon: const Icon(Icons.add),
                      label: const Text('Thêm bài viết'),
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.green,
                      ),
                    ),
                  ],
                ),

                const SizedBox(height: 8),

                // Danh sách bài viết
                isLoading
                    ? const Center(child: CircularProgressIndicator())
                    : posts.isEmpty
                    ? const Center(child: Text('Không có bài viết nào'))
                    : ListView.builder(
                  shrinkWrap: true,
                  physics: const NeverScrollableScrollPhysics(),
                  itemCount: posts.length,
                  itemBuilder: (context, index) {
                    final post = posts[index];
                    return Dismissible(
                      key: Key(post.id.toString()),
                      direction: DismissDirection.endToStart,
                      confirmDismiss: (direction) async {
                        return await showDialog<bool>(
                          context: context,
                          builder: (context) => AlertDialog(
                            title: const Text('Xác nhận'),
                            content: const Text('Bạn có chắc muốn xóa bài viết này?'),
                            actions: [
                              TextButton(
                                onPressed: () => Navigator.of(context).pop(false),
                                child: const Text('Hủy'),
                              ),
                              TextButton(
                                onPressed: () => Navigator.of(context).pop(true),
                                child: const Text('Xóa'),
                              ),
                            ],
                          ),
                        );
                      },
                      onDismissed: (direction) {
                        deletePost(post.id!);
                      },
                      background: Container(
                        color: Colors.red,
                        alignment: Alignment.centerRight,
                        padding: const EdgeInsets.symmetric(horizontal: 20),
                        child: const Icon(Icons.delete, color: Colors.white),
                      ),
                      child: GestureDetector(
                        onLongPress: () => showPostDialog(post: post),
                        child: Card(
                          margin: const EdgeInsets.symmetric(vertical: 8),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(12),
                          ),
                          elevation: 4,
                          child: Padding(
                            padding: const EdgeInsets.all(12),
                            child: Column(
                              crossAxisAlignment: CrossAxisAlignment.start,
                              children: [
                                // Hình ảnh bài viết
                                if (post.imageUrl.isNotEmpty)
                                  ClipRRect(
                                    borderRadius: BorderRadius.circular(8),
                                    child: AspectRatio(
                                      aspectRatio: 16 / 9,
                                      child: Image.network(
                                        post.imageUrl,
                                        fit: BoxFit.cover,
                                        errorBuilder: (context, error, stackTrace) {
                                          return Container(
                                            color: Colors.grey[200],
                                            child: const Center(
                                              child: Icon(
                                                Icons.broken_image,
                                                size: 50,
                                                color: Colors.grey,
                                              ),
                                            ),
                                          );
                                        },
                                      ),
                                    ),
                                  ),
                                const SizedBox(height: 8),
                                // Tiêu đề bài viết
                                Text(
                                  post.title,
                                  style: const TextStyle(
                                    fontSize: 18,
                                    fontWeight: FontWeight.bold,
                                  ),
                                  maxLines: 2,
                                  overflow: TextOverflow.ellipsis,
                                ),
                                const SizedBox(height: 4),
                                // Nội dung bài viết
                                Text(
                                  post.content,
                                  style: const TextStyle(
                                    fontSize: 14,
                                    color: Colors.black87,
                                  ),
                                  maxLines: 3,
                                  overflow: TextOverflow.ellipsis,
                                ),
                                const SizedBox(height: 8),
                                // Thông tin thêm
                                Row(
                                  mainAxisAlignment: MainAxisAlignment.spaceBetween,
                                  children: [
                                    Text(
                                      'Tác giả: ${post.author}',
                                      style: const TextStyle(
                                        fontSize: 12,
                                        color: Colors.grey,
                                      ),
                                    ),
                                    Text(
                                      'Ngày: ${post.createdAt.toLocal().toString().split(' ')[0]}',
                                      style: const TextStyle(
                                        fontSize: 12,
                                        color: Colors.grey,
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
                  },
                )

              ],
            ),
          ),
        ],
      ),
    );
  }
}
