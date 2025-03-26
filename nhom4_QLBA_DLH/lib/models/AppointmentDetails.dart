import 'package:nhom4_qlba_dlh/models/Doctor.dart';
import 'package:nhom4_qlba_dlh/models/Service.dart';

import 'package:nhom4_qlba_dlh/models/Doctor.dart';
import 'package:nhom4_qlba_dlh/models/Service.dart';

class AppointmentDetails {
  final int id;
  final String userName;
  final int doctorId;
  final int serviceId;
  final int appointmentId;
  final DateTime? appointmentDate;
  final DateTime createdAt;
  final String? appointmentDateStart;  // Thời gian bắt đầu (String)
  final String? appointmentDateEnd;    // Thời gian kết thúc (String)
  final Doctor? doctor;
  final Service? service;

  AppointmentDetails({
    required this.id,
    required this.userName,
    required this.doctorId,
    required this.serviceId,
    required this.appointmentId,
    this.appointmentDate,
    required this.createdAt,
    this.appointmentDateStart,
    this.appointmentDateEnd,
    this.doctor,
    this.service,
  });

  factory AppointmentDetails.fromJson(Map<String, dynamic> json) {
    return AppointmentDetails(
      id: json['id'],
      userName: json['userName'],
      doctorId: json['doctorId'],
      serviceId: json['serviceId'],
      appointmentId: json['appointmentId'],
      appointmentDate: json['appointmentDate'] != null
          ? DateTime.parse(json['appointmentDate'])
          : null,
      createdAt: DateTime.parse(json['createdAt']),
      appointmentDateStart: json['appointmentDateStart'],
      appointmentDateEnd: json['appointmentDateEnd'],
      doctor: json['doctor'] != null ? Doctor.fromJson(json['doctor']) : null,
      service: json['service'] != null ? Service.fromJson(json['service']) : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'userName': userName,
      'doctorId': doctorId,
      'serviceId': serviceId,
      'appointmentId': appointmentId,
      'appointmentDate': appointmentDate?.toIso8601String(),
      'createdAt': createdAt.toIso8601String(),
      'appointmentDateStart': appointmentDateStart,
      'appointmentDateEnd': appointmentDateEnd,
      'doctor': doctor?.toJson(),
      'service': service?.toJson(),
    };
  }
}
