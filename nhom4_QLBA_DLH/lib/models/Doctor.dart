class Doctor {
  final int id;
  final String fullName;
  final int specialtyId;
  final String phoneNumber;
  final String email;
  final String urlAvatar;

  Doctor({
    required this.id,
    required this.fullName,
    required this.specialtyId,
    required this.phoneNumber,
    required this.email,
    required this.urlAvatar,
  });

  factory Doctor.fromJson(Map<String, dynamic> json) {
    return Doctor(
      id: json['id'],
      fullName: json['fullName'] ?? 'Không rõ',
      specialtyId: json['specialtyId'],
      phoneNumber: json['phoneNumber'] ?? 'Không rõ',
      email: json['email'] ?? 'Không rõ',
      urlAvatar: json['urlAvatar'] ?? '',
    );
  }

  // Phương thức chuyển từ đối tượng Doctor sang JSON
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'fullName': fullName,
      'specialtyId': specialtyId,
      'phoneNumber': phoneNumber,
      'email': email,
      'urlAvatar': urlAvatar,
    };
  }
}
