
import 'package:flutter/material.dart';

class Appointment {
  int id;
  Duration? appointmentDateStart; // Chỉ giờ bắt đầu
  Duration? appointmentDateEnd;   // Chỉ giờ kết thúc
  bool isFull;

  Appointment({
    required this.id,
    this.appointmentDateStart,
    this.appointmentDateEnd,
    this.isFull = false,
  });

  // Factory để tạo từ JSON
  factory Appointment.fromJson(Map<String, dynamic> json) {
    return Appointment(
      id: json['id'],
      appointmentDateStart: json['appointmentDateStart'] != null
          ? _parseDuration(json['appointmentDateStart'])
          : null,
      appointmentDateEnd: json['appointmentDateEnd'] != null
          ? _parseDuration(json['appointmentDateEnd'])
          : null,
      isFull: json['isFull'] ?? false,
    );

  }

  // Chuyển đổi sang JSON
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'appointmentDateStart': appointmentDateStart != null
          ? _formatDuration(appointmentDateStart!)
          : null,
      'appointmentDateEnd': appointmentDateEnd != null
          ? _formatDuration(appointmentDateEnd!)
          : null,
    };
  }

  // Hàm chuyển từ chuỗi sang Duration
  static Duration _parseDuration(String time) {
    final parts = time.split(':');
    // Kiểm tra định dạng và chuyển thành Duration (giờ, phút, giây)
    if (parts.length == 3) {
      return Duration(
        hours: int.parse(parts[0]),
        minutes: int.parse(parts[1]),
        seconds: int.parse(parts[2]),
      );
    } else {
      throw FormatException("Invalid time format");
    }
  }

  // Hàm định dạng Duration thành chuỗi (HH:mm:ss)
  static String _formatDuration(Duration duration) {
    // Đảm bảo chuỗi có định dạng HH:mm:ss
    return duration.toString().split('.').first.padLeft(8, "0"); // Đảm bảo hiển thị giờ, phút, giây
  }
}

void main() {
  runApp(MaterialApp(home: BookAppointmentScreen()));
}

class BookAppointmentScreen extends StatelessWidget {
  final List<Appointment> appointments = [
    Appointment(id: 1, appointmentDateStart: Duration(hours: 15, minutes: 0, seconds: 0)),
    Appointment(id: 2, appointmentDateStart: Duration(hours: 9, minutes: 30, seconds: 0)),
  ];

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Đặt lịch khám"),
      ),
      body: ListView.builder(
        itemCount: appointments.length,
        itemBuilder: (context, index) {
          final appointment = appointments[index];
          return ListTile(
            title: Text("Appointment ID: ${appointment.id}"),
            subtitle: Text("Giờ bắt đầu: ${Appointment._formatDuration(appointment.appointmentDateStart!)}"),
          );
        },
      ),
    );
  }
}
