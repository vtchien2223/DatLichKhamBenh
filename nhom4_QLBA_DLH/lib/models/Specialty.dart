class Specialty {
  final int id;
  final String specialtyName;

  Specialty({required this.id, required this.specialtyName});

  factory Specialty.fromJson(Map<String, dynamic> json) {
    return Specialty(
      id: json['id'],
      specialtyName: json['specialtyName'] ?? 'Không rõ', // Thêm fallback
    );
  }
}
