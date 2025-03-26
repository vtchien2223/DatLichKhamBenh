import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:nhom4_qlba_dlh/models/MedicalRecordDetails.dart';
class MedicalFormScreen extends StatefulWidget {
  @override
  _MedicalFormScreenState createState() => _MedicalFormScreenState();
}

class _MedicalFormScreenState extends State<MedicalFormScreen> {
  List<MedicalRecordDetails> _medicalRecords = [];
  bool isLoading = false;
  final String apiUrl = 'https://10.0.2.2:7290/api/MedicalRecord'; // API URL của bạn

  @override
  void initState() {
    super.initState();
    fetchMedicalRecords();
  }

  // Fetch danh sách hồ sơ bệnh án từ API
  Future<void> fetchMedicalRecords() async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.get(Uri.parse(apiUrl));
      if (response.statusCode == 200) {
        final List data = json.decode(response.body);
        setState(() {
          _medicalRecords = data.map((json) => MedicalRecordDetails.fromJson(json)).toList();
        });
      } else {
        _showErrorSnackbar('Failed to load medical records. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error fetching medical records: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Thêm hồ sơ bệnh án
  Future<void> addMedicalRecord(MedicalRecordDetails record) async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(record.toJson()),
      );
      if (response.statusCode == 201) {
        fetchMedicalRecords(); // Tải lại danh sách sau khi thêm
        _showSuccessSnackbar('Medical record added successfully.');
      } else {
        _showErrorSnackbar('Failed to add medical record. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error adding medical record: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Sửa hồ sơ bệnh án
  Future<void> updateMedicalRecord(MedicalRecordDetails record) async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.put(
        Uri.parse('$apiUrl/${record.id}'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(record.toJson()),
      );
      if (response.statusCode == 200 || response.statusCode == 204) {
        fetchMedicalRecords();
        _showSuccessSnackbar('Medical record updated successfully.');
      } else {
        _showErrorSnackbar('Failed to update medical record. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error updating medical record: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Xóa hồ sơ bệnh án
  Future<void> deleteMedicalRecord(int id) async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.delete(Uri.parse('$apiUrl/$id'));
      if (response.statusCode == 204) {
        setState(() {
          _medicalRecords.removeWhere((record) => record.id == id);
        });
        _showSuccessSnackbar('Medical record deleted successfully.');
      } else {
        _showErrorSnackbar('Failed to delete medical record. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error deleting medical record: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Hiển thị Snackbar lỗi
  void _showErrorSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      content: Text(message),
      backgroundColor: Colors.red,
    ));
  }

  // Hiển thị Snackbar thành công
  void _showSuccessSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      content: Text(message),
      backgroundColor: Colors.green,
    ));
  }

  // Hiển thị form thêm/sửa hồ sơ bệnh án
  Future<void> showMedicalRecordForm({MedicalRecordDetails? record}) async {
    final treatmentController = TextEditingController(text: record?.treatment ?? '');
    final patientIdController = TextEditingController(text: record?.patientId.toString() ?? '');
    final appointmentDetailIdController = TextEditingController(text: record?.appointmentDetailId.toString() ?? '');

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(record == null ? 'Add Medical Record' : 'Edit Medical Record'),
          content: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              TextField(
                controller: patientIdController,
                decoration: InputDecoration(labelText: 'Patient ID'),
                keyboardType: TextInputType.number,
              ),
              TextField(
                controller: appointmentDetailIdController,
                decoration: InputDecoration(labelText: 'Appointment Detail ID'),
                keyboardType: TextInputType.number,
              ),
              TextField(
                controller: treatmentController,
                decoration: InputDecoration(labelText: 'Treatment'),
              ),
            ],
          ),
          actions: [
            TextButton(
              onPressed: () async {
                final newRecord = MedicalRecordDetails(
                  id: record?.id ?? 0,
                  patientId: int.parse(patientIdController.text),
                  appointmentDetailId: int.parse(appointmentDetailIdController.text),
                  treatment: treatmentController.text,
                  createdAt: record?.createdAt ?? DateTime.now(),
                );

                if (record == null) {
                  await addMedicalRecord(newRecord);
                } else {
                  await updateMedicalRecord(newRecord);
                }

                Navigator.of(context).pop();
              },
              child: Text('Save'),
            ),
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel'),
            ),
          ],
        );
      },
    );
  }

  // Xác nhận xóa hồ sơ bệnh án
  void showDeleteConfirmation(int id) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text('Confirm Delete'),
          content: Text('Are you sure you want to delete this medical record?'),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel'),
            ),
            TextButton(
              onPressed: () async {
                Navigator.of(context).pop();
                await deleteMedicalRecord(id);
              },
              child: Text('Delete'),
            ),
          ],
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: Text('Medical Records Management'),
        actions: [
          IconButton(
            icon: Icon(Icons.add),
            onPressed: () => showMedicalRecordForm(), // Mở form thêm hồ sơ bệnh án
          ),
        ],
      ),
      body: isLoading
          ? Center(child: CircularProgressIndicator())
          : ListView.builder(
        itemCount: _medicalRecords.length,
        itemBuilder: (context, index) {
          final record = _medicalRecords[index];
          return GestureDetector(
            onLongPress: () => showMedicalRecordForm(record: record), // Mở form chỉnh sửa hồ sơ bệnh án
            child: Dismissible(
              key: Key(record.id.toString()),
              direction: DismissDirection.endToStart,
              background: Container(
                color: Colors.red,
                alignment: Alignment.centerRight,
                padding: EdgeInsets.symmetric(horizontal: 20),
                child: Icon(Icons.delete, color: Colors.white),
              ),
              confirmDismiss: (direction) async {
                showDeleteConfirmation(record.id);
                return false;
              },
              child: ListTile(
                title: Text('Patient ID: ${record.patientId}'),
                subtitle: Text('Treatment: ${record.treatment ?? 'No treatment provided'}'),
              ),
            ),
          );
        },
      ),
    );
  }
}