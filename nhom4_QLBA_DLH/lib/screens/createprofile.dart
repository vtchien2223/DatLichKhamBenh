import 'package:flutter/material.dart';

class CreateProfileScreen extends StatefulWidget {
  @override
  _CreateProfileScreenState createState() => _CreateProfileScreenState();
}

class _CreateProfileScreenState extends State<CreateProfileScreen> {
  String gender = "Nam"; // Giới tính mặc định
  String relation = "Khác"; // Quan hệ mặc định

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Tạo mới hồ sơ"),
        centerTitle: true,
        backgroundColor: Colors.blue,
        elevation: 0,
      ),
      body: SingleChildScrollView(
        padding: const EdgeInsets.all(16.0),
        child: Column(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: [
            // Thông báo
            Container(
              padding: const EdgeInsets.all(12.0),
              decoration: BoxDecoration(
                color: Colors.blue.shade50,
                borderRadius: BorderRadius.circular(8),
              ),
              child: const Text(
                "Vui lòng cung cấp thông tin chính xác để được phục vụ tốt nhất. Trong trường hợp cung cấp sai thông tin bệnh nhân & điện thoại, việc xác nhận cuộc hẹn sẽ không hiệu lực trước khi đặt khám.",
                style: TextStyle(fontSize: 14, color: Colors.blue),
              ),
            ),
            const SizedBox(height: 16),

            // Thông tin bệnh nhân
            const Text(
              "Thông tin bệnh nhân:",
              style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
            ),
            const SizedBox(height: 16),

            _buildTextField("Họ và tên (có dấu)*"),
            const SizedBox(height: 16),

            _buildTextField("Ngày sinh*", hint: "Nhập 01/01/xxxx"),
            const SizedBox(height: 16),

            _buildGenderSelector(),
            const SizedBox(height: 16),

            _buildTextField("Email (dùng để nhận phiếu khám bệnh)"),
            const SizedBox(height: 16),

            _buildTextField("Quốc gia*", hint: "Việt Nam"),
            const SizedBox(height: 16),

            _buildTextField("Tỉnh / TP*"),
            const SizedBox(height: 16),

            _buildTextField("Quận / Huyện*"),
            const SizedBox(height: 16),

            _buildTextField("Phường / Xã*"),
            const SizedBox(height: 16),

            _buildTextField("Địa chỉ*", hint: "Nhập Ấp, thôn, tổ, khóm, tên đường,..."),
            const SizedBox(height: 16),

            _buildRelationSelector(),
            const SizedBox(height: 24),

            // Nút "Tạo mới hồ sơ"
            SizedBox(
              width: double.infinity,
              child: ElevatedButton(
                onPressed: () {
                  // Xử lý khi bấm tạo hồ sơ
                },
                style: ElevatedButton.styleFrom(
                  backgroundColor: Colors.blue,
                  padding: const EdgeInsets.symmetric(vertical: 16),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                ),
                child: const Text(
                  "TẠO MỚI HỒ SƠ",
                  style: TextStyle(
                    fontSize: 16,
                    fontWeight: FontWeight.bold,
                    color: Colors.white,
                  ),
                ),
              ),
            ),
          ],
        ),
      ),
    );
  }

  Widget _buildTextField(String label, {String? hint}) {
    return TextField(
      decoration: InputDecoration(
        labelText: label,
        hintText: hint,
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(8),
        ),
        filled: true,
        fillColor: Colors.white,
        contentPadding: const EdgeInsets.symmetric(vertical: 12, horizontal: 16),
      ),
    );
  }

  Widget _buildGenderSelector() {
    return Row(
      children: [
        Expanded(
          child: RadioListTile(
            title: const Text("Nam"),
            value: "Nam",
            groupValue: gender,
            onChanged: (value) {
              setState(() {
                gender = value!;
              });
            },
          ),
        ),
        Expanded(
          child: RadioListTile(
            title: const Text("Nữ"),
            value: "Nữ",
            groupValue: gender,
            onChanged: (value) {
              setState(() {
                gender = value!;
              });
            },
          ),
        ),
      ],
    );
  }

  Widget _buildRelationSelector() {
    const relations = ["Vợ / Chồng", "Tôi", "Mẹ", "Bố", "Con", "Khác"];
    return Wrap(
      spacing: 8,
      children: relations
          .map((relation) => ChoiceChip(
        label: Text(relation),
        selected: this.relation == relation,
        onSelected: (isSelected) {
          setState(() {
            this.relation = relation;
          });
        },
        selectedColor: Colors.blue,
        labelStyle: TextStyle(
          color: this.relation == relation ? Colors.white : Colors.black,
        ),
      ))
          .toList(),
    );
  }
}
