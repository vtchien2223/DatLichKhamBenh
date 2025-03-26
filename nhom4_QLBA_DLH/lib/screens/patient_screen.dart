import 'dart:convert';
import 'package:flutter/material.dart';
import 'package:http/http.dart' as http;
import 'package:nhom4_qlba_dlh/models/Patient.dart';
import 'package:shared_preferences/shared_preferences.dart';

class PatientScreen extends StatefulWidget {
  @override
  _PatientScreenState createState() => _PatientScreenState();
}

class _PatientScreenState extends State<PatientScreen> {
  List<Patient> patients = [];
  final String apiUrl = 'https://10.0.2.2:7290/api/Patient';
  bool isLoading = false;

  @override
  void initState() {
    super.initState();
    fetchPatients();
  }

  // Fetch danh sách bệnh nhân từ API
  Future<void> fetchPatients() async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.get(Uri.parse(apiUrl));
      if (response.statusCode == 200) {
        final List data = json.decode(response.body);
        setState(() {
          patients = data.map((json) => Patient.fromJson(json)).toList();
        });
      } else {
        _showErrorSnackbar('Failed to load patients. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error fetching patients: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Thêm bệnh nhân mới
  Future<void> addPatient(Patient patient) async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.post(
        Uri.parse(apiUrl),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(patient.toJson()),
      );

      if (response.statusCode == 201) {
        fetchPatients();
        _showSuccessSnackbar('Patient added successfully.');
      } else {
        _showErrorSnackbar('Failed to add patient. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error adding patient: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Sửa bệnh nhân
  Future<void> updatePatient(Patient patient) async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.put(
        Uri.parse('$apiUrl/${patient.id}'),
        headers: {'Content-Type': 'application/json'},
        body: json.encode(patient.toJson()),
      );
      if (response.statusCode == 200 || response.statusCode == 204) {
        fetchPatients();
        _showSuccessSnackbar('Patient updated successfully.');
      } else {
        _showErrorSnackbar('Failed to update patient. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error updating patient: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  // Xóa bệnh nhân
  Future<void> deletePatient(int id) async {
    setState(() {
      isLoading = true;
    });
    try {
      final response = await http.delete(Uri.parse('$apiUrl/$id'));
      if (response.statusCode == 204) {
        setState(() {
          patients.removeWhere((patient) => patient.id == id);
        });
        _showSuccessSnackbar('Patient deleted successfully.');
      } else {
        _showErrorSnackbar('Failed to delete patient. Status code: ${response.statusCode}');
      }
    } catch (e) {
      _showErrorSnackbar("Error deleting patient: $e");
    } finally {
      setState(() {
        isLoading = false;
      });
    }
  }

  Future<String> getCurrentUserName() async {
    SharedPreferences prefs = await SharedPreferences.getInstance();
    return prefs.getString('userName') ?? 'Unknown';
  }

  // Hiển thị form thêm/sửa bệnh nhân
  Future<void> showPatientForm({Patient? patient}) async {
    final fullNameController = TextEditingController(text: patient?.fullName ?? '');
    final phoneNumberController = TextEditingController(text: patient?.phoneNumber ?? '');
    final emailController = TextEditingController(text: patient?.email ?? '');
    final urlAvatarController = TextEditingController(text: patient?.urlAvatar ?? '');
    final String userName = await getCurrentUserName();

    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text(patient == null ? 'Add Patient' : 'Edit Patient'),
          content: SingleChildScrollView(
            child: Column(
              mainAxisSize: MainAxisSize.min,
              children: [
                TextField(
                  controller: fullNameController,
                  decoration: InputDecoration(labelText: 'Full Name'),
                ),
                TextField(
                  controller: phoneNumberController,
                  decoration: InputDecoration(labelText: 'Phone Number'),
                ),
                TextField(
                  controller: emailController,
                  decoration: InputDecoration(labelText: 'Email'),
                ),
                TextField(
                  controller: urlAvatarController,
                  decoration: InputDecoration(labelText: 'URL Avatar'),
                ),
              ],
            ),
          ),
          actions: [
            TextButton(
              onPressed: () async {
                if (fullNameController.text.isEmpty) {
                  _showErrorSnackbar('Full Name cannot be empty.');
                  return;
                }

                final newPatient = Patient(
                  id: patient?.id ?? 0,
                  fullName: fullNameController.text,
                  phoneNumber: phoneNumberController.text,
                  email: emailController.text,
                  userName: patient?.userName ?? userName,
                  urlAvatar: urlAvatarController.text.isEmpty ? null : urlAvatarController.text,
                  createdAt: patient?.createdAt ?? DateTime.now(),
                );

                if (patient == null) {
                  await addPatient(newPatient);
                } else {
                  await updatePatient(newPatient);
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

  void _showErrorSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      content: Text(message),
      backgroundColor: Colors.red,
    ));
  }

  void _showSuccessSnackbar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(SnackBar(
      content: Text(message),
      backgroundColor: Colors.green,
    ));
  }

  void showDeleteConfirmation(int id) {
    showDialog(
      context: context,
      builder: (context) {
        return AlertDialog(
          title: Text('Confirm Delete'),
          content: Text('Are you sure you want to delete this patient?'),
          actions: [
            TextButton(
              onPressed: () => Navigator.of(context).pop(),
              child: Text('Cancel'),
            ),
            TextButton(
              onPressed: () async {
                Navigator.of(context).pop();
                await deletePatient(id);
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
        title: Text('Patient Management'),
        actions: [
          IconButton(
            icon: Icon(Icons.add),
            onPressed: () => showPatientForm(),
          ),
        ],
      ),
      body: isLoading
          ? Center(child: CircularProgressIndicator())
          : ListView.builder(
        itemCount: patients.length,
        itemBuilder: (context, index) {
          final patient = patients[index];
          return GestureDetector(
            onLongPress: () => showPatientForm(patient: patient),
            child: Dismissible(
              key: Key(patient.id.toString()),
              direction: DismissDirection.endToStart,
              background: Container(
                color: Colors.red,
                alignment: Alignment.centerRight,
                padding: EdgeInsets.symmetric(horizontal: 20),
                child: Icon(Icons.delete, color: Colors.white),
              ),
              confirmDismiss: (direction) async {
                showDeleteConfirmation(patient.id);
                return false;
              },
              child: ListTile(
                leading: CircleAvatar(
                  backgroundImage: patient.urlAvatar != null && patient.urlAvatar!.isNotEmpty
                      ? NetworkImage(patient.urlAvatar!)
                      : null,
                  child: patient.urlAvatar == null || patient.urlAvatar!.isEmpty
                      ? Icon(Icons.person)
                      : null,
                ),
                title: Text(patient.fullName ?? 'No Name'),
                subtitle: Text(patient.email ?? 'No Email'),
              ),
            ),
          );
        },
      ),
    );
  }
}
