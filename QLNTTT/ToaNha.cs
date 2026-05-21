using BUS;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QLNTTT
{
    public partial class ToaNha : Form
    {
        public ToaNha()
        {
            InitializeComponent();
        }
        ToaNha_BUS toaNha_BUS = new ToaNha_BUS();
        void LoadData()
        {
            try
            {
                // 1. Lấy danh sách từ lớp BUS
                var danhSach = toaNha_BUS.GetAll();

                // 2. Kiểm tra dữ liệu trước khi đổ vào lưới
                if (danhSach != null)
                {
                    dataGridViewtoanha.DataSource = danhSach;

                    // 3. Đổi tên cột hiển thị (Khớp với tên thuộc tính trong image_775a0e.png)
                    // Lưu ý: Tên cột trong ngoặc ["..."] phải viết hoa thường chính xác như trong Model
                    dataGridViewtoanha.Columns["MaToaNha"].HeaderText = "Mã Tòa Nhà";
                    dataGridViewtoanha.Columns["TenToaNha"].HeaderText = "Tên Tòa Nhà";
                    dataGridViewtoanha.Columns["DiaChi"].HeaderText = "Địa Chỉ";
                    dataGridViewtoanha.Columns["SoTang"].HeaderText = "Số Tầng";
                    dataGridViewtoanha.Columns["SoPhong"].HeaderText = "Số Phòng";
                    dataGridViewtoanha.Columns["TrangThai"].HeaderText = "Trạng Thái";

                    // 3. Cấu hình giao diện cho DataGridView
                    dataGridViewtoanha.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewtoanha.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewtoanha.ReadOnly = true;

                    // Ẩn các cột quan hệ (Navigation Properties) nếu dùng Entity Framework
                    // Thường EF sẽ tạo thêm các cột như "Phongs", "HopDongs" ở cuối lưới
                    foreach (DataGridViewColumn col in dataGridViewtoanha.Columns)
                    {
                        if (col.ValueType.Name.Contains("ICollection") || col.ValueType.Name.Contains("Entity"))
                        {
                            col.Visible = false;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Không thể tải dữ liệu tòa nhà. Vui lòng kiểm tra kết nối!", "Thông báo");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải dữ liệu: " + ex.Message);
            }

        }
        private void StyleForm()
        {
            this.BackColor = Color.FromArgb(245, 247, 250);
            dataGridViewtoanha.BorderStyle = BorderStyle.None;

            dataGridViewtoanha.BackgroundColor = Color.White;

            dataGridViewtoanha.EnableHeadersVisualStyles = false;

            dataGridViewtoanha.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(52, 152, 219);

            dataGridViewtoanha.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dataGridViewtoanha.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dataGridViewtoanha.ColumnHeadersHeight = 40;

            dataGridViewtoanha.RowTemplate.Height = 35;

            dataGridViewtoanha.DefaultCellStyle.Font =
                new Font("Segoe UI", 10);

            dataGridViewtoanha.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(245, 245, 245);

            dataGridViewtoanha.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridViewtoanha.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(52, 152, 219);

            dataGridViewtoanha.DefaultCellStyle.SelectionForeColor =
                Color.White;
        }
        private void StyleButton(Button btn, Color color)
        {
            btn.BackColor = color;
            btn.ForeColor = Color.White;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.Font = new Font(
                "Segoe UI",
                10,
                FontStyle.Bold);

            btn.Cursor = Cursors.Hand;
            btn.BackColor = color;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
        }
     
        private void AddHover(Button btn)
        {
            Color oldColor = btn.BackColor;

            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ControlPaint.Light(oldColor);
            };

            btn.MouseLeave += (s, e) =>
            {
                btn.BackColor = oldColor;
            };
        }
        void ClearData()
        {
            textBoxmatoannha.Clear();
            textBoxtentoanha.Clear();
            textBoxdiachi.Clear();

            numSoTang.Value = 0;
            numSoPhong.Value = 0;

            cbbTrangThai.SelectedIndex = 0;

            textBoxmatoannha.Enabled = true; // cho nhập lại mã
        }
        void LoadcbTrangThai()
        {
            cbbTrangThai.Items.Add("Đang hoạt động");
            cbbTrangThai.Items.Add("Đang sửa chữa");
            cbbTrangThai.Items.Add("Đã ngừng");
            cbbTrangThai.SelectedIndex = 0; // Chọn mục đầu tiên làm mặc định
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            // 🔹 0. Cho phép nhập mã khi thêm mới
            textBoxmatoannha.Enabled = true;

            // 1. Kiểm tra các trường văn bản bắt buộc
            if (string.IsNullOrWhiteSpace(textBoxmatoannha.Text) ||
                string.IsNullOrWhiteSpace(textBoxtentoanha.Text) ||
                string.IsNullOrWhiteSpace(textBoxdiachi.Text))
            {
                MessageBox.Show("Vui lòng điền đầy đủ Mã, Tên và Địa chỉ tòa nhà!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 2. Khởi tạo đối tượng từ Model Entity Framework
                DTO.ToaNha_DTO tn = new DTO.ToaNha_DTO
                {
                    // Lấy dữ liệu từ TextBox
                    MaToaNha= textBoxmatoannha.Text.Trim(),
                    TenToaNha = textBoxtentoanha.Text.Trim(),
                    DiaChi = textBoxdiachi.Text.Trim(),

                    // Lấy dữ liệu từ NumericUpDown (Dùng thuộc tính .Value)
                    // Lưu ý: Ép kiểu sang (int) vì .Value trả về kiểu decimal
                    SoTang = (int)numSoTang.Value,
                    SoPhong = (int)numSoPhong.Value,

                    // Lấy giá trị từ ComboBox (Trạng thái)
                    // .Text sẽ lấy chữ đang hiển thị trong CBB
                    TrangThai = cbbTrangThai.Text
                };

                // 3. Gọi lớp BUS để xử lý và nhận thông báo kết quả
                string ketQua = toaNha_BUS.Insert_ToaNha(tn);

                if (ketQua == "success")
                {
                    MessageBox.Show("Thêm tòa nhà thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Làm mới danh sách hiển thị
                    LoadData();
                    ClearData();

                }
                else
                {
                    // Hiển thị thông báo lỗi cụ thể trả về từ DAL/BUS
                    MessageBox.Show(ketQua, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi hệ thống: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Lấy dữ liệu từ form
                ToaNha_DTO dto = new ToaNha_DTO()
                {
                    MaToaNha = textBoxmatoannha.Text,
                    TenToaNha = textBoxtentoanha.Text,
                    DiaChi = textBoxdiachi.Text,
                    SoTang = (int)numSoTang.Value, // ✅ đúng
                    SoPhong = (int)numSoPhong.Value,
                    TrangThai = cbbTrangThai.Text
                };

                // 2. Gọi BUS
                ToaNha_BUS bus = new ToaNha_BUS();
                string result = bus.Update_ToaNha(dto);

                // 3. Thông báo
                if (result == "success")
                {
                    MessageBox.Show("Cập nhật thành công!");
                    LoadData(); // reload DataGridView
                }
                else
                {
                    MessageBox.Show(result);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Dữ liệu không hợp lệ!");
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đã chọn chưa
            if (string.IsNullOrEmpty(textBoxmatoannha.Text))
            {
                MessageBox.Show("Vui lòng chọn tòa nhà cần xóa!");
                return;
            }

            // 2. Hỏi xác nhận
            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa tòa nhà này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (rs == DialogResult.No)
                return;

            try
            {
                // 3. Gọi BUS
                string kq = toaNha_BUS.Delete_ToaNha(textBoxmatoannha.Text);

                if (kq == "success")
                {
                    MessageBox.Show("Xóa thành công!");

                    LoadData();   // reload grid
                    ClearData();  // reset form
                }
                else
                {
                    MessageBox.Show(kq); // lỗi từ DAL (FK, không tồn tại...)
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private GraphicsPath GetRoundRect(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void RoundGroupBox(GroupBox gb)
        {
            gb.Paint += (s, e) =>
            {
                Rectangle rect = gb.ClientRectangle;
                rect.Inflate(-1, -1);

                using (GraphicsPath path = GetRoundRect(rect, 20))
                {
                    gb.Region = new Region(path);

                    e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                    using (Pen p = new Pen(Color.FromArgb(220, 220, 220), 1))
                    {
                        e.Graphics.DrawPath(p, path);
                    }
                }
            };
        }
        private void StyleThongKe()
        {
            // Card Tổng phòng
            groupBox3.BackColor = Color.FromArgb(219, 234, 254);
            groupBox3.ForeColor = Color.FromArgb(37, 99, 235);

            lblTongPhong.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTongPhong.ForeColor = Color.FromArgb(37, 99, 235);
            lblTongPhong.Dock = DockStyle.Fill;
            lblTongPhong.TextAlign = ContentAlignment.MiddleCenter;

            // Card Đang thuê
            groupBox4.BackColor = Color.FromArgb(220, 252, 231);
            groupBox4.ForeColor = Color.FromArgb(22, 163, 74);

            lblDangThue.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblDangThue.ForeColor = Color.FromArgb(22, 163, 74);
            lblDangThue.Dock = DockStyle.Fill;
            lblDangThue.TextAlign = ContentAlignment.MiddleCenter;

            // Card Phòng trống
            groupBox5.BackColor = Color.FromArgb(254, 243, 199);
            groupBox5.ForeColor = Color.FromArgb(217, 119, 6);

            lblPhongTrong.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblPhongTrong.ForeColor = Color.FromArgb(217, 119, 6);
            lblPhongTrong.Dock = DockStyle.Fill;
            lblPhongTrong.TextAlign = ContentAlignment.MiddleCenter;
        }
        private void LoadThongKe()
        {
            if (string.IsNullOrEmpty(textBoxmatoannha.Text))
                return;

            using (var db = new QLNT_DoVanTieuEntities())
            {
                string maToaNha = textBoxmatoannha.Text;

                int tongPhong = db.Phongs
                    .Count(x => x.MaToaNha == maToaNha);

                int phongDangThue = db.Phongs
                    .Count(x => x.MaToaNha == maToaNha
                             && x.TrangThai == "Đang thuê");

                int phongTrong = db.Phongs
                    .Count(x => x.MaToaNha == maToaNha
                             && x.TrangThai == "Trống");

                lblTongPhong.Text = tongPhong.ToString();
                lblDangThue.Text = phongDangThue.ToString();
                lblPhongTrong.Text = phongTrong.ToString();
            }
        }
        private void ToaNha_Load(object sender, EventArgs e)
        {
            StyleForm();
            StyleThongKe();
            RoundGroupBox(groupBox3);
            RoundGroupBox(groupBox4);
            RoundGroupBox(groupBox5);
            AddHover(btnsua);
            AddHover(btnxoa);
                AddHover(btnthoat);
            LoadData();
            LoadcbTrangThai();  
            ClearData();
            if (dataGridViewtoanha.Rows.Count > 0)
            {
                textBoxmatoannha.Text =
                    dataGridViewtoanha.Rows[0]
                    .Cells["MaToaNha"].Value.ToString();

                LoadThongKe();
            }
        }

        private void dataGridViewtoanha_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 1. Kiểm tra click hợp lệ (tránh click header)
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = dataGridViewtoanha.Rows[e.RowIndex];

                // 2. Đổ dữ liệu lên form
                textBoxmatoannha.Text = row.Cells["MaToaNha"].Value?.ToString();
                textBoxtentoanha.Text = row.Cells["TenToaNha"].Value?.ToString();
                textBoxdiachi.Text = row.Cells["DiaChi"].Value?.ToString();

                // NumericUpDown → dùng Value (không dùng Text)
                if (row.Cells["SoTang"].Value != null)
                    numSoTang.Value = Convert.ToDecimal(row.Cells["SoTang"].Value);

                if (row.Cells["SoPhong"].Value != null)
                    numSoPhong.Value = Convert.ToDecimal(row.Cells["SoPhong"].Value);

                // ComboBox
                if (row.Cells["TrangThai"].Value != null)
                    cbbTrangThai.Text = row.Cells["TrangThai"].Value.ToString();
                // 🔥 QUAN TRỌNG: khóa mã
                textBoxmatoannha.Enabled = false;
                LoadThongKe(); // Cập nhật thống kê khi chọn tòa nhà
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng: " + ex.Message);
            }
        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            ClearData();
             LoadData();
        }
    }
}
