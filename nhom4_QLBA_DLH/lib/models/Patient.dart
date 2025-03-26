class Patient {
  final int id;
  final String? fullName;
  final String? phoneNumber;
  final String? email;
  final String? urlAvatar;
  final String? userName;
  final DateTime? createdAt;

  Patient({
    required this.id,
    this.fullName,
    this.phoneNumber,
    this.email,
    this.urlAvatar,
    this.userName,
    this.createdAt,
  });

  factory Patient.fromJson(Map<String, dynamic> json) {
    return Patient(
      id: json['id'],
      fullName: json['fullName'],
      phoneNumber: json['phoneNumber'],
      email: json['email'],
      urlAvatar: json['urlAvatar'],
      userName: json['userName'],
      createdAt: json['createdAt'] != null
          ? DateTime.parse(json['createdAt'])
          : null,
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'fullName': fullName,
      'phoneNumber': phoneNumber,
      'email': email,
      'urlAvatar': urlAvatar,
      'userName': userName,
      'createdAt': createdAt?.toIso8601String(),
    };
  }
}
