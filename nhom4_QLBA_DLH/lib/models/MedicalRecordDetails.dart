class MedicalRecordDetails {
  final int id;
  final int patientId;
  final int appointmentDetailId;
  final String? treatment;
  final DateTime? createdAt;

  MedicalRecordDetails({
    required this.id,
    required this.patientId,
    required this.appointmentDetailId,
    this.treatment,
    this.createdAt,
  });

  factory MedicalRecordDetails.fromJson(Map<String, dynamic> json) {
    return MedicalRecordDetails(
      id: json['id'],
      patientId: json['patientId'],
      appointmentDetailId: json['appointmentDetailId'],
      treatment: json['treatment'],
      createdAt: json['createdAt'] != null ? DateTime.parse(json['createdAt']) : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'patientId': patientId,
      'appointmentDetailId': appointmentDetailId,
      'treatment': treatment,
      'createdAt': createdAt?.toIso8601String(),
    };
  }
}