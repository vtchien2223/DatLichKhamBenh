import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:nhom4_qlba_dlh/models/Appointment.dart';
import 'package:nhom4_qlba_dlh/models/Doctor.dart';
import 'package:cached_network_image/cached_network_image.dart';
import 'package:nhom4_qlba_dlh/models/Specialty.dart';
import 'package:nhom4_qlba_dlh/models/service.dart';
import 'package:shared_preferences/shared_preferences.dart';

class BookAppointmentScreen extends StatefulWidget {
  @override
  _BookAppointmentScreenState createState() => _BookAppointmentScreenState();
}

class _BookAppointmentScreenState extends State<BookAppointmentScreen> {
  final String baseSpecialtyUrl = 'https://10.0.2.2:7290/api/Specialty';
  final String baseDoctorUrl = 'https://10.0.2.2:7290/api/Doctor';
  final String baseServiceUrl = 'https://10.0.2.2:7290/api/Service';
  final String baseAppointmentUrl = 'https://10.0.2.2:7290/api/Appointment';
  final String baseAddAppointmentUrl = 'https://10.0.2.2:7290/api/AppointmentDetails';


  int currentStep = 0;
  Specialty? selectedSpeciality;
  int? selectedDoctorId;
  List<Map<String, dynamic>> selectedServiceIds = [];
  DateTime selectedDate = DateTime.now();
  Appointment? selectedAppointment;

  List<Specialty> specialities = [];
  List<Service> services = [];
  List<Doctor> doctors = [];
  List<Doctor> filteredDoctors = [];
  List<Appointment> appointments = [];

  String selectedTimeSlot = ''; // Biến để lưu giờ được chọn

  @override
  void initState() {
    super.initState();
    fetchSpecialities();
    fetchDoctors();
    fetchServices();
    fetchAppointments();
  }

  Future<String> getCurrentUserName() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('userName') ?? 'Unknown';
  }

  Future<String?> _getToken() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('jwt_token');
  }

  void _showSuccessDialog(String message) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Thành công'),
          content: Text(message),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context), // Đóng dialog
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }


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

  Future<void> fetchAppointments() async {
    try {
      final response = await http.get(
        Uri.parse(baseAppointmentUrl),
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        setState(() {
          appointments = data.map((item) => Appointment.fromJson(item)).toList();
        });
      } else {
        throw Exception('Failed to fetch appointments');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching appointments: $e')),
      );
    }
  }

  Future<void> fetchSpecialities() async {
    try {
      final response = await http.get(
        Uri.parse(baseSpecialtyUrl),
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        setState(() {
          specialities = data.map((item) => Specialty.fromJson(item)).toList();
        });
      } else {
        throw Exception('Failed to fetch specialties');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching specialties: $e')),
      );
    }
  }

  Future<void> fetchDoctors() async {
    try {
      final response = await http.get(
        Uri.parse(baseDoctorUrl),
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        setState(() {
          doctors = data.map((item) => Doctor.fromJson(item)).toList();
          filteredDoctors = doctors;
        });
      } else {
        throw Exception('Failed to fetch doctors');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching doctors: $e')),
      );
    }
  }

  Future<void> createDoctor(Doctor newDoctor) async {
    try {
      final response = await http.post(
        Uri.parse(baseDoctorUrl),
        headers: {'Content-Type': 'application/json'},
        body: json.encode({
          'fullName': newDoctor.fullName,
          'phoneNumber': newDoctor.phoneNumber,
          'email': newDoctor.email,
          'urlAvatar': newDoctor.urlAvatar,
          'specialtyId': newDoctor.specialtyId,
        }),
      );

      if (response.statusCode == 201) {
        await fetchDoctors(); // Cập nhật danh sách
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Thêm bác sĩ thành công!')),
        );
      } else {
        throw Exception('Thêm bác sĩ thất bại!');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error: $e')),
      );
    }
  }

  Future<void> updateDoctor(Doctor updatedDoctor) async {
    try {
      final response = await http.put(
        Uri.parse('$baseDoctorUrl/${updatedDoctor.id}'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode({
          'fullName': updatedDoctor.fullName,
          'phoneNumber': updatedDoctor.phoneNumber,
          'email': updatedDoctor.email,
          'urlAvatar': updatedDoctor.urlAvatar,
          'specialtyId': updatedDoctor.specialtyId,
        }),
      );

      print('Response status: ${response.statusCode}');
      print('Response body: ${response.body}');

      if (response.statusCode == 200 || response.statusCode == 204) {
        await fetchDoctors(); // Cập nhật danh sách
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Cập nhật bác sĩ thành công!')),
        );
      } else {
        final errorMsg = json.decode(response.body)['message'] ?? 'Lỗi không xác định';
        throw Exception(errorMsg);
      }
    } catch (e) {
      print('Error: $e');
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi cập nhật bác sĩ: $e')),
      );
    }
  }


  // Xóa bác sĩ
  Future<void> deleteDoctor(int doctorId) async {
    try {
      final response = await http.delete(
        Uri.parse('$baseDoctorUrl/$doctorId'),
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 204) {
        await fetchDoctors(); // Cập nhật danh sách
        ScaffoldMessenger.of(context).showSnackBar(
          const SnackBar(content: Text('Xóa bác sĩ thành công!')),
        );
      } else {
        throw Exception('Xóa bác sĩ thất bại!');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error: $e')),
      );
    }
  }


  Future<void> fetchServices() async {
    try {
      final response = await http.get(
        Uri.parse(baseServiceUrl),
        headers: {'Content-Type': 'application/json'},
      );

      if (response.statusCode == 200) {
        List<dynamic> data = json.decode(response.body);
        setState(() {
          services = data.map((item) => Service.fromJson(item)).toList();
        });
      } else {
        throw Exception('Failed to fetch services');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error fetching services: $e')),
      );
    }
  }

  void filterDoctorsBySpecialty(int? specialtyId) {
    setState(() {
      if (specialtyId == null) {
        filteredDoctors = doctors;
      } else {
        filteredDoctors =
            doctors.where((doctor) => doctor.specialtyId == specialtyId).toList();
      }
    });
  }

  void _showSpecialityDialog() {
    showModalBottomSheet(
      context: context,
      builder: (context) {
        return Column(
          crossAxisAlignment: CrossAxisAlignment.stretch,
          children: [
            Padding(
              padding: const EdgeInsets.all(16.0),
              child: const Text(
                "Chọn chuyên khoa",
                style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
              ),
            ),
            Expanded(
              child: ListView.builder(
                itemCount: specialities.length,
                itemBuilder: (context, index) {
                  final speciality = specialities[index];
                  return ListTile(
                    title: Text(speciality.specialtyName),
                    onTap: () {
                      setState(() {
                        selectedSpeciality = speciality;
                        filterDoctorsBySpecialty(speciality.id);
                      });
                      Navigator.pop(context);
                    },
                  );
                },
              ),
            ),
          ],
        );
      },
    );
  }

  Widget _buildProgressBar() {
    return Row(
      children: [
        _buildStepIcon(Icons.person, 0),
        _buildStepDivider(0),
        _buildStepIcon(Icons.medical_services, 1),
        _buildStepDivider(1),
        _buildStepIcon(Icons.calendar_today, 2),
      ],
    );
  }

  Widget _buildStepIcon(IconData icon, int step) {
    return Icon(
      icon,
      color: currentStep >= step ? Colors.blue : Colors.grey,
      size: 30,
    );
  }

  Widget _buildStepDivider(int step) {
    return Expanded(
      child: Divider(
        color: currentStep > step ? Colors.blue : Colors.grey,
        thickness: 2,
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Đặt lịch khám"),
        centerTitle: true,
        backgroundColor: Colors.blue,
      ),
      body: Column(
        children: [
          if (currentStep == 0)
            Padding(
              padding: const EdgeInsets.all(16.0),
              child: ElevatedButton(
                onPressed: _showSpecialityDialog,
                child: const Text("Chọn chuyên khoa"),
              ),
            ),
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: _buildProgressBar(),
          ),
          Expanded(
            child: _buildStepContent(),
          ),
          Padding(
            padding: const EdgeInsets.all(16.0),
            child: Row(
              mainAxisAlignment: MainAxisAlignment.spaceBetween,
              children: [
                if (currentStep > 0)
                  ElevatedButton(
                    onPressed: () {
                      setState(() {
                        currentStep--;
                      });
                    },
                    style: ElevatedButton.styleFrom(
                      backgroundColor: Colors.grey,
                    ),
                    child: const Text("Quay lại"),
                  ),
                ElevatedButton(
                  onPressed: () {
                    if (currentStep < 2) {
                      setState(() {
                        currentStep++;
                      });
                    } else {
                      _showConfirmationDialog();
                    }
                  },
                  style: ElevatedButton.styleFrom(
                    backgroundColor: Colors.blue,
                  ),
                  child: Text(currentStep < 2 ? "Tiếp tục" : "Hoàn thành"),
                ),
              ],
            ),
          ),

        ],
      ),
    );
  }

  Widget _buildStepContent() {
    switch (currentStep) {
      case 0:
        return _buildDoctorSelection();
      case 1:
        return _buildServiceSelection();
      case 2:
        return _buildDateAndTimeSelection();
      default:
        return Container();
    }
  }

  Widget _buildDoctorSelection() {
    return Column(
      children: [
        ElevatedButton(
          onPressed: () => _showDoctorManagementDialog(),
          child: const Text('Thêm Bác Sĩ'),
        ),
        Expanded(
          child: filteredDoctors.isEmpty
              ? const Center(child: Text("Không có bác sĩ nào"))
              : ListView.builder(
            itemCount: filteredDoctors.length,
            itemBuilder: (context, index) {
              final doctor = filteredDoctors[index];
              return Dismissible(
                key: Key(doctor.id.toString()),
                direction: DismissDirection.endToStart,
                background: Container(
                  color: Colors.red,
                  alignment: Alignment.centerRight,
                  padding: const EdgeInsets.symmetric(horizontal: 20),
                  child: const Icon(Icons.delete, color: Colors.white),
                ),
                onDismissed: (direction) {
                  deleteDoctor(doctor.id!);
                },
                child: Card(
                  margin: const EdgeInsets.symmetric(
                      horizontal: 16.0, vertical: 8.0),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8.0),
                  ),
                  child: ListTile(
                    leading: ClipRRect(
                      borderRadius: BorderRadius.circular(30.0),
                      child: CachedNetworkImage(
                        imageUrl: doctor.urlAvatar.isNotEmpty
                            ? doctor.urlAvatar
                            : '',
                        width: 60,
                        height: 60,
                        fit: BoxFit.cover,
                        placeholder: (context, url) =>
                        const CircularProgressIndicator(),
                        errorWidget: (context, url, error) => Image.asset(
                          'assets/User1.png',
                          width: 60,
                          height: 60,
                          fit: BoxFit.cover,
                        ),
                      ),
                    ),
                    title: Text(
                      "${doctor.fullName}",
                      style: const TextStyle(fontWeight: FontWeight.bold),
                    ),
                    subtitle: Column(
                      crossAxisAlignment: CrossAxisAlignment.start,
                      children: [
                        Text(
                          "SĐT: ${doctor.phoneNumber}",
                          style: const TextStyle(
                              fontSize: 12, color: Colors.red),
                        ),
                        Text(
                          "Email: ${doctor.email}",
                          style: const TextStyle(
                              fontSize: 10, color: Colors.blue),
                        ),
                      ],
                    ),
                    trailing: ElevatedButton(
                      onPressed: () {
                        setState(() {
                          selectedDoctorId = doctor.id;
                          currentStep++;
                        });
                      },
                      style: ElevatedButton.styleFrom(
                        backgroundColor: Colors.blue,
                        shape: RoundedRectangleBorder(
                          borderRadius: BorderRadius.circular(8.0),
                        ),
                      ),
                      child: const Text("Chọn"),
                    ),
                    onTap: () => _showDoctorManagementDialog(doctor: doctor),
                  ),
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  void _showDoctorManagementDialog({Doctor? doctor}) {
    final TextEditingController nameController =
    TextEditingController(text: doctor?.fullName ?? '');
    final TextEditingController phoneController =
    TextEditingController(text: doctor?.phoneNumber ?? '');
    final TextEditingController emailController =
    TextEditingController(text: doctor?.email ?? '');
    final TextEditingController avatarController =
    TextEditingController(text: doctor?.urlAvatar ?? '');
    int? selectedSpecialty = doctor?.specialtyId;

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(doctor == null ? 'Thêm Bác Sĩ' : 'Sửa Bác Sĩ'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: nameController,
                  decoration: const InputDecoration(labelText: 'Tên Bác Sĩ'),
                ),
                TextField(
                  controller: phoneController,
                  decoration: const InputDecoration(labelText: 'Số Điện Thoại'),
                ),
                TextField(
                  controller: emailController,
                  decoration: const InputDecoration(labelText: 'Email'),
                ),
                TextField(
                  controller: avatarController,
                  decoration:
                  const InputDecoration(labelText: 'Link Ảnh Đại Diện'),
                ),
                DropdownButton<int>(
                  value: selectedSpecialty,
                  hint: const Text('Chọn Chuyên Khoa'),
                  onChanged: (value) {
                    setState(() {
                      selectedSpecialty = value;
                    });
                  },
                  items: specialities.map((specialty) {
                    return DropdownMenuItem<int>(
                      value: specialty.id,
                      child: Text(specialty.specialtyName),
                    );
                  }).toList(),
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
                final newDoctor = Doctor(
                  id: doctor?.id ?? 0,
                  fullName: nameController.text,
                  phoneNumber: phoneController.text,
                  email: emailController.text,
                  urlAvatar: avatarController.text,
                  specialtyId: selectedSpecialty ?? 0,
                );
                if (doctor == null) {
                  createDoctor(newDoctor);
                } else {
                  updateDoctor(newDoctor);
                }
                Navigator.pop(context);
              },
              child: Text(doctor == null ? 'Thêm' : 'Cập Nhật'),
            ),
          ],
        );
      },
    );
  }

  Widget _buildDateAndTimeSelection() {
    return Column(
      children: [
        const Text(
          "Chọn ngày và giờ khám",
          style: TextStyle(fontSize: 18, fontWeight: FontWeight.bold),
        ),
        SizedBox(
          height: 300,
          child: CalendarDatePicker(
            initialDate: selectedDate,
            firstDate: DateTime.now(),
            lastDate: DateTime.now().add(const Duration(days: 30)),
            onDateChanged: (date) {
              setState(() {
                selectedDate = date;
              });
            },
          ),
        ),
        Expanded(
          child: ListView.builder(
            itemCount: appointments.length,
            itemBuilder: (context, index) {
              final appointment = appointments[index];
              final timeSlot =
                  "${_formatTime(appointment.appointmentDateStart)} - ${_formatTime(appointment.appointmentDateEnd)}";
              return ListTile(
                title: Text(timeSlot),
                selected: selectedAppointment == appointment,
                onTap: () {
                  setState(() {
                    selectedAppointment = appointment;
                  });
                },
              );
            },
          ),
        ),
      ],
    );
  }

  //Service
  Widget _buildServiceSelection() {
    return Column(
      children: [
        ElevatedButton(
          onPressed: () => _showServiceDialog(), // Nút thêm dịch vụ
          child: const Text('Thêm Dịch Vụ'),
        ),
        const SizedBox(height: 10),
        Expanded(
          child: ListView.builder(
            itemCount: services.length,
            itemBuilder: (context, index) {
              final service = services[index];
              return Dismissible(
                key: Key(service.id.toString()),
                direction: DismissDirection.endToStart,
                background: Container(
                  color: Colors.red,
                  alignment: Alignment.centerRight,
                  padding: const EdgeInsets.symmetric(horizontal: 20),
                  child: const Icon(Icons.delete, color: Colors.white),
                ),
                onDismissed: (direction) {
                  _confirmDeleteService(service.id); // Hộp thoại xác nhận xóa
                },
                child: ListTile(
                  title: Text(service.serviceName),
                  subtitle: Text("Giá: ${service.price.toStringAsFixed(2)} VND"),
                  trailing: Checkbox(
                    value: selectedServiceIds.any((s) => s['id'] == service.id),
                    onChanged: (isSelected) {
                      setState(() {
                        if (isSelected == true) {
                          if (selectedServiceIds.length < 2) {
                            selectedServiceIds.add({
                              'id': service.id,
                              'name': service.serviceName,
                            });
                          } else {
                            // Hiển thị Dialog khi chọn quá 2 dịch vụ
                            _showMaxServicesDialog();
                          }
                        } else {
                          selectedServiceIds
                              .removeWhere((s) => s['id'] == service.id);
                        }
                      });
                    },
                  ),
                  onLongPress: () => _showServiceDialog(service: service), // Nhấn giữ để sửa
                ),
              );
            },
          ),
        ),
      ],
    );
  }

  Future<int> checkAppointmentAvailability(DateTime appointmentDate, int doctorId) async {
    try {
      final response = await http.post(
        Uri.parse('$baseAddAppointmentUrl/Check'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode({
          'appointmentDate': appointmentDate.toIso8601String(),
          'doctorId': doctorId,
        }),
      );

      if (response.statusCode == 200) {
        final data = json.decode(response.body);
        return data['count']; // Trả về số lượng người đã đặt trong giờ này
      } else {
        throw Exception('Failed to check appointment availability');
      }
    } catch (e) {
      throw Exception('Error checking availability: $e');
    }
  }


  void _showMaxServicesDialog() {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Thông báo'),
          content: const Text('Bạn chỉ có thể chọn tối đa 2 dịch vụ 1 lúc ! Chân thành cảm ơn'),
          actions: [
            TextButton(
              onPressed: () {
                Navigator.pop(context); // Đóng dialog
              },
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }


  void _showServiceDialog({Service? service}) {
    final TextEditingController nameController =
    TextEditingController(text: service?.serviceName ?? '');
    final TextEditingController priceController =
    TextEditingController(text: service?.price.toString() ?? '');
    final TextEditingController descriptionController =
    TextEditingController(text: service?.description ?? ''); // Thêm mô tả

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(service == null ? 'Thêm Dịch Vụ' : 'Sửa Dịch Vụ'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: nameController,
                  decoration: const InputDecoration(labelText: 'Tên Dịch Vụ'),
                ),
                TextField(
                  controller: priceController,
                  decoration: const InputDecoration(labelText: 'Giá Dịch Vụ'),
                  keyboardType: TextInputType.number,
                ),
                TextField(
                  controller: descriptionController,
                  decoration: const InputDecoration(labelText: 'Mô Tả'),
                  maxLines: 3, // Cho phép nhập mô tả dài hơn
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
                final double? price = double.tryParse(priceController.text);
                if (price == null || price < 0) {
                  ScaffoldMessenger.of(context).showSnackBar(
                    const SnackBar(content: Text('Vui lòng nhập giá hợp lệ!')),
                  );
                  return;
                }

                final newService = Service(
                  id: service?.id ?? 0,
                  serviceName: nameController.text,
                  description: descriptionController.text,
                  price: price,
                );

                if (service == null) {
                  _createService(newService);
                } else {
                  _updateService(newService);
                }
                Navigator.pop(context);
              },
              child: Text(service == null ? 'Thêm' : 'Cập Nhật'),
            ),
          ],
        );
      },
    );
  }



  Future<void> _createService(Service newService) async {
    try {
      String? token = await _getToken();
      final response = await http.post(
        Uri.parse(baseServiceUrl),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
        body: json.encode(newService.toJson()),
      );

      if (response.statusCode == 201) {
        await fetchServices();
        _showSuccessDialog('Thêm dịch vụ thành công');
      } else {
        _showAccessDeniedDialog();
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi thêm dịch vụ: $e')),
      );
    }
  }

  Future<void> _updateService(Service updatedService) async {
    try {
      String? token = await _getToken();
      final response = await http.put(
        Uri.parse('$baseServiceUrl/${updatedService.id}'),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
        body: json.encode(updatedService.toJson()),
      );

      if (response.statusCode == 204) {
        await fetchServices();
        _showSuccessDialog('Cặp nhật dịch vụ thành công');
      } else {
        _showAccessDeniedDialog();
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi cập nhật dịch vụ: $e')),
      );
    }
  }


  Future<void> _deleteService(int serviceId) async {
    try {
      String? token = await _getToken();
      final response = await http.delete(
        Uri.parse('$baseServiceUrl/$serviceId'),
        headers: {
          'Authorization': 'Bearer $token',
          'Content-Type': 'application/json',
        },
      );

      if (response.statusCode == 204) {
        await fetchServices();
        _showSuccessDialog('Xóa dịch vụ thành công');
      } else {
        _showAccessDeniedDialog();
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Lỗi xóa dịch vụ: $e')),
      );
    }
  }

  void _confirmDeleteService(int serviceId) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('Xác nhận xóa'),
          content: const Text('Bạn có chắc chắn muốn xóa dịch vụ này?'),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context),
              child: const Text('Hủy'),
            ),
            ElevatedButton(
              onPressed: () {
                _deleteService(serviceId);
                Navigator.pop(context);
              },
              child: const Text('Xóa'),
            ),
          ],
        );
      },
    );
  }

  //=========================================================================//


  String _formatTime(Duration? time) {
    if (time == null) {
      return "00:00:00";
    }
    int hours = time.inHours;
    int minutes = time.inMinutes % 60;
    int seconds = time.inSeconds % 60;
    return "${hours.toString().padLeft(2, '0')}:${minutes.toString().padLeft(2, '0')}:${seconds.toString().padLeft(2, '0')}";
  }

    Future<void> _showConfirmationDialog() async {
      final doctor = doctors.firstWhere((d) => d.id == selectedDoctorId);

      // Lấy UserName từ SharedPreferences
      final String userName = await getCurrentUserName();

      final startHour = selectedAppointment?.appointmentDateStart?.inHours ?? 0;
      final startMinute = (selectedAppointment?.appointmentDateStart?.inMinutes ?? 0) % 60;
      final startSecond = (selectedAppointment?.appointmentDateStart?.inSeconds ?? 0) % 60;

      final endHour = selectedAppointment?.appointmentDateEnd?.inHours ?? 0;
      final endMinute = (selectedAppointment?.appointmentDateEnd?.inMinutes ?? 0) % 60;
      final endSecond = (selectedAppointment?.appointmentDateEnd?.inSeconds ?? 0) % 60;

      showDialog(
        context: context,
        builder: (context) {
          return AlertDialog(
            title: const Text("Xác nhận thông tin"),
            content: Column(
              crossAxisAlignment: CrossAxisAlignment.start,
              mainAxisSize: MainAxisSize.min,
              children: [
                Text("UserName: $userName"), // Hiển thị UserName
                Text("Bác sĩ: ${doctor.fullName}"),
                Text("SĐT: ${doctor.phoneNumber}"),
                Text("Email: ${doctor.email}"),
                Text("Dịch vụ: ${selectedServiceIds.map((s) => s['name']).join(", ")}"),
                Text("Ngày khám: ${selectedDate.toLocal().toString().split(' ')[0]}"),
                Text(
                  "Giờ khám: ${startHour.toString().padLeft(2, '0')}:${startMinute.toString().padLeft(2, '0')}:${startSecond.toString().padLeft(2, '0')} - ${endHour.toString().padLeft(2, '0')}:${endMinute.toString().padLeft(2, '0')}:${endSecond.toString().padLeft(2, '0')}",
                ),
              ],
            ),
            actions: [
              TextButton(
                onPressed: () => Navigator.pop(context),
                child: const Text("Hủy"),
              ),
              ElevatedButton(
                onPressed: () {
                  Navigator.pop(context);
                  saveAppointment();
                },
                child: const Text("Xác nhận"),
              ),
            ],
          );
        },
      );
    }


  void _showErrorDialog(String message) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: const Text('THÔNG BÁO'),
          content: Text(message),
          actions: [
            TextButton(
              onPressed: () => Navigator.pop(context), // Đóng dialog
              child: const Text('OK'),
            ),
          ],
        );
      },
    );
  }

  // Future<void> saveAppointment() async {
  //   if (selectedAppointment == null || selectedDoctorId == null || selectedServiceIds.isEmpty) {
  //     _showErrorDialog('Vui lòng chọn đầy đủ thông tin THÔNG TIN KHÔNG ĐƯỢC BỎ TRỐNG');
  //     return;
  //   }
  //
  //   try {
  //     // Thay thế bằng tên người dùng hiện tại
  //     final String userName = await getCurrentUserName();
  //
  //     for (var service in selectedServiceIds) {
  //       final body = {
  //         'doctorId': selectedDoctorId, // ID của bác sĩ
  //         'serviceId': service['id'], // ID dịch vụ đã chọn
  //         'appointmentId': selectedAppointment!.id, // ID của cuộc hẹn đã chọn
  //         'appointmentDate': selectedDate.toIso8601String(), // Ngày giờ cuộc hẹn, format ISO 8601
  //         'createdAt': DateTime.now().toIso8601String(), // Ngày tạo cuộc hẹn
  //         'userName': userName, // Thêm trường UserName
  //       };
  //
  //       final response = await http.post(
  //         Uri.parse(baseAddAppointmentUrl),
  //         headers: {'Content-Type': 'application/json'},
  //         body: json.encode(body),
  //       );
  //
  //       if (response.statusCode == 201) {
  //         print('Appointment saved for service: ${service['id']}');
  //       } else {
  //         throw Exception('Failed to save appointment for service: ${service['id']}');
  //       }
  //     }
  //     _showSuccessDialog('Đặt lịch thành công');
  //   } catch (e) {
  //     ScaffoldMessenger.of(context).showSnackBar(
  //       SnackBar(content: Text('Error saving appointment: $e')),
  //     );
  //   }
  // }
  Future<void> saveAppointment() async {
    if (selectedAppointment == null || selectedDoctorId == null || selectedServiceIds.isEmpty) {
      _showErrorDialog('Vui lòng chọn đầy đủ thông tin. THÔNG TIN KHÔNG ĐƯỢC BỎ TRỐNG!');
      return;
    }

    try {
      // Lấy tên người dùng hiện tại từ SharedPreferences
      final String userName = await getCurrentUserName();

      // Tạo danh sách các request POST
      List<Future<http.Response>> requests = [];

      for (var service in selectedServiceIds) {
        final body = {
          'doctorId': selectedDoctorId, // ID của bác sĩ
          'serviceId': service['id'], // ID dịch vụ đã chọn
          'appointmentId': selectedAppointment!.id, // ID của cuộc hẹn đã chọn
          'appointmentDate': selectedDate.toIso8601String(), // Ngày giờ cuộc hẹn (ISO 8601)
          'createdAt': DateTime.now().toIso8601String(), // Ngày tạo cuộc hẹn
          'userName': userName, // Thêm trường UserName
        };

        // Thêm request vào danh sách
        requests.add(
          http.post(
            Uri.parse(baseAddAppointmentUrl),
            headers: {'Content-Type': 'application/json'},
            body: json.encode(body),
          ),
        );
      }

      // Gửi tất cả các request cùng lúc
      final responses = await Future.wait(requests);

      // Kiểm tra kết quả của tất cả các request
      bool allSuccess = true;
      for (var response in responses) {
        if (response.statusCode != 201) {
          allSuccess = false;
          print('Failed to save appointment: ${response.body}');
        }
      }

      if (allSuccess) {
        _showSuccessDialog('Đặt lịch thành công!');
      } else {
        _showErrorDialog('Một số dịch vụ không thể lưu lịch hẹn.');
      }
    } catch (e) {
      ScaffoldMessenger.of(context).showSnackBar(
        SnackBar(content: Text('Error saving appointment: $e')),
      );
    }
  }

}
