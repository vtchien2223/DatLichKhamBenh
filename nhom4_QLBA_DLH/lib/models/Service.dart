
class Service {
  final int id;
  final String serviceName;
  final String description;
  final double price;

  Service({
    required this.id,
    required this.serviceName,
    required this.description,
    required this.price,
  });

  factory Service.fromJson(Map<String, dynamic> json) {
    return Service(
      id: json['id'],
      serviceName: json['serviceName'],
      description: json['description'],
      price: json['price'],
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'serviceName': serviceName,
      'description': description,
      'price': price,
    };
  }
}

/*class Service {
  final int id;
  final String serviceName;
  final String description;
  final double price;

  Service({
    required this.id,
    required this.serviceName,
    required this.description,
    required this.price,
  });

  // Phương thức chuyển từ JSON sang đối tượng Service
  factory Service.fromJson(Map<String, dynamic> json) {
    return Service(
      id: json['id'] ?? 0,
      serviceName: json['serviceName'] ?? 'Không rõ',
      description: json['description'] ?? '',
      price: (json['price'] is int) ? (json['price'] as int).toDouble() : (json['price'] ?? 0.0),
    );
  }

  // Phương thức chuyển từ đối tượng Service sang JSON
  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'serviceName': serviceName,
      'description': description,
      'price': price,
    };
  }
}*/
