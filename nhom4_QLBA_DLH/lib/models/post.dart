class Post {
  final int? id;
  final String title;
  final String content;
  final String imageUrl;
  final String author;
  final DateTime createdAt;

  Post({
    this.id,
    required this.title,
    required this.content,
    required this.imageUrl,
    required this.author,
    required this.createdAt,
  });

  factory Post.fromJson(Map<String, dynamic> json) {
    return Post(
      id: json['id'],
      title: json['title'],
      content: json['content'],
      imageUrl: json['imageUrl'],
      author: json['author'],
      createdAt: DateTime.parse(json['createdAt']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'title': title,
      'content': content,
      'imageUrl': imageUrl,
      'author': author,
      'createdAt': createdAt.toIso8601String(),
    };
  }
}
