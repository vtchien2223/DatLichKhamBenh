class Notification {
  final int id;
  final int appointmentId;
  final String message;
  final String? urlImage;
  final DateTime appointmentDate;

  Notification({
    required this.id,
    required this.appointmentId,
    required this.message,
    this.urlImage,
    required this.appointmentDate,
  });

  // Chuyển từ JSON sang NotificationModel
  factory Notification.fromJson(Map<String, dynamic> json) {
    return Notification(
      id: json['Id'],
      appointmentId: json['AppointmentId'],
      message: json['Message'],
      urlImage: json['UrlImage'],
      appointmentDate: DateTime.parse(json['AppointmentDate']),
    );
  }
}
