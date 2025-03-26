import 'package:flutter/material.dart';

class FindProfileScreen extends StatefulWidget {
  @override
  _FindProfileScreenState createState() => _FindProfileScreenState();
}

class _FindProfileScreenState extends State<FindProfileScreen> {
  int _selectedTab = 0; // Tab 0: Nhập số hồ sơ, Tab 1: Quên mã số bệnh nhân
  String gender = "Nam"; // Giới tính mặc định

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Thêm hồ sơ đặt khám"),
        centerTitle: true,
        backgroundColor: Colors.blue,
        elevation: 0,
      ),
      body: Column(
        children: [
          // Tab Bar
          Row(
            children: [
              Expanded(
                child: GestureDetector(
                  onTap: () {
                    setState(() {
                      _selectedTab = 0;
                    });
                  },
                  child: Container(
                    padding: const EdgeInsets.symmetric(vertical: 16),
                    decoration: BoxDecoration(
                      color: _selectedTab == 0 ? Colors.blue : Colors.white,
                      border: Border.all(color: Colors.blue),
                    ),
                    child: Text(
                      "Nhập số hồ sơ",
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        color: _selectedTab == 0 ? Colors.white : Colors.blue,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
              ),
              Expanded(
                child: GestureDetector(
                  onTap: () {
                    setState(() {
                      _selectedTab = 1;
                    });
                  },
                  child: Container(
                    padding: const EdgeInsets.symmetric(vertical: 16),
                    decoration: BoxDecoration(
                      color: _selectedTab == 1 ? Colors.blue : Colors.white,
                      border: Border.all(color: Colors.blue),
                    ),
                    child: Text(
                      "Quên mã số bệnh nhân",
                      textAlign: TextAlign.center,
                      style: TextStyle(
                        color: _selectedTab == 1 ? Colors.white : Colors.blue,
                        fontWeight: FontWeight.bold,
                      ),
                    ),
                  ),
                ),
              ),
            ],
          ),
          Expanded(
            child: SingleChildScrollView(
              padding: const EdgeInsets.all(16),
              child: _selectedTab == 0
                  ? _buildEnterProfileNumber()
                  : _buildForgetProfileInfo(),
            ),
          ),
        ],
      ),
    );
  }

  // Tab "Nhập số hồ sơ"
  Widget _buildEnterProfileNumber() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Container(
          padding: const EdgeInsets.all(12),
          decoration: BoxDecoration(
            color: Colors.yellow.shade100,
            borderRadius: BorderRadius.circular(8),
          ),
          child: const Text(
            "Số hồ sơ được in trên toa thuốc, phiếu chỉ định, hoặc phiếu trả kết quả cận lâm sàng",
            style: TextStyle(fontSize: 14, color: Colors.orange),
          ),
        ),
        const SizedBox(height: 16),
        const Text(
          "Vui lòng nhập số hồ sơ để tiếp tục",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 8),
        Row(
          children: [
            Expanded(
              child: TextField(
                decoration: InputDecoration(
                  hintText: "Ví dụ: N17-XXXXXX",
                  border: OutlineInputBorder(
                    borderRadius: BorderRadius.circular(8),
                  ),
                  contentPadding: const EdgeInsets.symmetric(
                      vertical: 12, horizontal: 16),
                ),
              ),
            ),
            const SizedBox(width: 8),
            ElevatedButton(
              onPressed: () {
                // Xử lý tìm kiếm
              },
              style: ElevatedButton.styleFrom(
                backgroundColor: Colors.blue,
                padding:
                const EdgeInsets.symmetric(horizontal: 16, vertical: 16),
              ),
              child: const Text("Tìm kiếm"),
            ),
          ],
        ),
        const SizedBox(height: 16),
        const Text(
          "Kết quả:",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 8),
        const Text(
          "Bạn chưa có kết quả tìm hồ sơ nào",
          style: TextStyle(fontSize: 14, color: Colors.grey),
        ),
      ],
    );
  }

  // Tab "Quên mã số bệnh nhân"
  Widget _buildForgetProfileInfo() {
    return Column(
      crossAxisAlignment: CrossAxisAlignment.start,
      children: [
        Container(
          padding: const EdgeInsets.all(12),
          decoration: BoxDecoration(
            color: Colors.yellow.shade100,
            borderRadius: BorderRadius.circular(8),
          ),
          child: const Text(
            "Vui lòng nhập chính xác những thông tin bên dưới, chúng tôi sẽ giúp bạn tìm số hồ sơ",
            style: TextStyle(fontSize: 14, color: Colors.orange),
          ),
        ),
        const SizedBox(height: 16),
        _buildTextField("Họ và chữ lót (có dấu)*"),
        const SizedBox(height: 16),
        _buildTextField("Tên người bệnh (có dấu)*"),
        const SizedBox(height: 16),
        _buildTextField("Năm sinh*"),
        const SizedBox(height: 16),
        _buildGenderSelector(),
        const SizedBox(height: 16),
        _buildTextField("Tỉnh / TP*"),
        const SizedBox(height: 16),
        const Text(
          "Kết quả:",
          style: TextStyle(fontSize: 16, fontWeight: FontWeight.bold),
        ),
        const SizedBox(height: 8),
        const Text(
          "Bạn chưa có kết quả tìm hồ sơ nào",
          style: TextStyle(fontSize: 14, color: Colors.grey),
        ),
        const SizedBox(height: 24),
        SizedBox(
          width: double.infinity,
          child: ElevatedButton(
            onPressed: () {
              // Xử lý tiếp tục
            },
            style: ElevatedButton.styleFrom(
              backgroundColor: Colors.blue,
              padding: const EdgeInsets.symmetric(vertical: 16),
              shape: RoundedRectangleBorder(
                borderRadius: BorderRadius.circular(8),
              ),
            ),
            child: const Text(
              "TIẾP TỤC",
              style: TextStyle(
                fontSize: 16,
                fontWeight: FontWeight.bold,
                color: Colors.white,
              ),
            ),
          ),
        ),
      ],
    );
  }

  Widget _buildTextField(String label) {
    return TextField(
      decoration: InputDecoration(
        labelText: label,
        border: OutlineInputBorder(
          borderRadius: BorderRadius.circular(8),
        ),
        contentPadding:
        const EdgeInsets.symmetric(vertical: 12, horizontal: 16),
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
}
