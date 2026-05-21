using BUS;
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
    public partial class LoaiPhong : Form
    {
        public LoaiPhong()
        {
            InitializeComponent();
        }
        LoaiPhong_BUS loaiPhong_BUS = new LoaiPhong_BUS();
     
        void LoadData()
        {
            try
            {
                var ds = loaiPhong_BUS.GetAll();

                if (ds != null)
                {
                    dataGridViewLoaiPhong.DataSource = ds;

                    dataGridViewLoaiPhong.Columns["MaLoaiPhong"].HeaderText = "Mã";
                    dataGridViewLoaiPhong.Columns["TenLoaiPhong"].HeaderText = "Tên loại phòng";
                    dataGridViewLoaiPhong.Columns["SoNguoiToiDa"].HeaderText = "Số người tối đa";
                    dataGridViewLoaiPhong.Columns["GiaThueMacDinh"].HeaderText = "Giá thuê mặc định";
                    dataGridViewLoaiPhong.Columns["DonGiaDien"].HeaderText = "Đơn giá điện";
                    dataGridViewLoaiPhong.Columns["DonGiaNuoc"].HeaderText = "Đơn giá nước";


                    dataGridViewLoaiPhong.Columns["GiaThueMacDinh"].DefaultCellStyle.Format = "N0";
                    dataGridViewLoaiPhong.Columns["DonGiaDien"].DefaultCellStyle.Format = "N0";
                    dataGridViewLoaiPhong.Columns["DonGiaNuoc"].DefaultCellStyle.Format = "N0";
                    dataGridViewLoaiPhong.Columns["MoTa"].HeaderText = "Mô tả";
                    dataGridViewLoaiPhong.Columns["TrangThai"].HeaderText = "Trạng thái";

                    dataGridViewLoaiPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewLoaiPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewLoaiPhong.ReadOnly = true;
                }
                else
                {
                    MessageBox.Show("Không tải được dữ liệu!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }
        void LoadcbTrangThai()
        {
            cbbTrangThai.Items.Clear();

            cbbTrangThai.Items.Add("Đang sử dụng");
            cbbTrangThai.Items.Add("Ngừng sử dụng");

            cbbTrangThai.SelectedIndex = 0;
        }
        void clearinput()
        {
            txtMaLoaiPhong.Clear();
            txtTenLoaiPhong.Clear();
            txtMoTa.Clear();

            // 🔥 NumericUpDown
            numSoNguoi.Value = 1;
            numGiaThue.Value = 0;
            numGiaDien.Value = 0;
            numGiaNuoc.Value = 0;

            // 🔥 ComboBox
            cbbTrangThai.SelectedIndex = 0;

            txtMaLoaiPhong.Enabled = true;
        }
        private void StyleForm()
        {
            // 🌈 nền form mềm
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Font = new Font("Segoe UI", 10);

            // 🧾 DataGridView style
            dataGridViewLoaiPhong.BorderStyle = BorderStyle.None;
            dataGridViewLoaiPhong.BackgroundColor = Color.White;

            dataGridViewLoaiPhong.EnableHeadersVisualStyles = false;

            dataGridViewLoaiPhong.ColumnHeadersDefaultCellStyle.BackColor =
                Color.FromArgb(74, 144, 226);

            dataGridViewLoaiPhong.ColumnHeadersDefaultCellStyle.ForeColor =
                Color.White;

            dataGridViewLoaiPhong.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dataGridViewLoaiPhong.RowTemplate.Height = 35;

            dataGridViewLoaiPhong.AlternatingRowsDefaultCellStyle.BackColor =
                Color.FromArgb(245, 245, 245);

            dataGridViewLoaiPhong.DefaultCellStyle.SelectionBackColor =
                Color.FromArgb(52, 152, 219);

            dataGridViewLoaiPhong.DefaultCellStyle.SelectionForeColor =
                Color.White;

            dataGridViewLoaiPhong.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dataGridViewLoaiPhong.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }
        private void StyleButton(Button btn, Color color)
        {
            btn.BackColor = color;
            btn.ForeColor = Color.White;

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;

            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
        }
        private void btnthem_Click(object sender, EventArgs e)
        {
            // 🔹 1. Validate cơ bản
            if (string.IsNullOrWhiteSpace(txtMaLoaiPhong.Text) ||
                string.IsNullOrWhiteSpace(txtTenLoaiPhong.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Mã và Tên loại phòng!");
                return;
            }

            try
            {
                // 🔹 2. Lấy dữ liệu từ form
                LoaiPhong_DTO dto = new LoaiPhong_DTO()
                {
                    MaLoaiPhong = txtMaLoaiPhong.Text.Trim(),
                    TenLoaiPhong = txtTenLoaiPhong.Text.Trim(),
                    SoNguoiToiDa = (int)numSoNguoi.Value,
                    GiaThueMacDinh = (decimal)numGiaThue.Value,
                    DonGiaDien = (decimal)numGiaDien.Value,
                    DonGiaNuoc = (decimal)numGiaNuoc.Value,
                    MoTa = txtMoTa.Text,
                    TrangThai = cbbTrangThai.Text
                };

                // 🔥 3. Validate logic thêm (BUS sẽ check tiếp)
                if (dto.SoNguoiToiDa <= 0)
                {
                    MessageBox.Show("Số người phải > 0!");
                    return;
                }

                if (dto.GiaThueMacDinh < 0)
                {
                    MessageBox.Show("Giá thuê không hợp lệ!");
                    return;
                }

                // 🔹 4. Gọi BUS
                string kq = loaiPhong_BUS.Insert_LoaiPhong(dto);

                // 🔹 5. Kết quả
                if (kq == "success")
                {
                    MessageBox.Show("Thêm thành công!");

                    LoadData();    // reload grid
                    clearinput();  // reset form
                }
                else
                {
                    MessageBox.Show(kq); // lỗi từ DAL/BUS
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi hệ thống: " + ex.Message);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            // 🔹 1. Kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(txtMaLoaiPhong.Text))
            {
                MessageBox.Show("Vui lòng chọn loại phòng cần sửa!");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTenLoaiPhong.Text))
            {
                MessageBox.Show("Tên loại phòng không được để trống!");
                return;
            }

            try
            {
                // 🔹 2. Lấy dữ liệu từ form
                LoaiPhong_DTO dto = new LoaiPhong_DTO()
                {
                    MaLoaiPhong = txtMaLoaiPhong.Text,
                    TenLoaiPhong = txtTenLoaiPhong.Text,
                    SoNguoiToiDa = (int)numSoNguoi.Value,
                    GiaThueMacDinh = (decimal)numGiaThue.Value,
                    DonGiaDien = (decimal)numGiaDien.Value,
                    DonGiaNuoc = (decimal)numGiaNuoc.Value,
                    MoTa = txtMoTa.Text,
                    TrangThai = cbbTrangThai.Text
                };

                // 🔥 3. Validate thêm
                if (dto.SoNguoiToiDa <= 0)
                {
                    MessageBox.Show("Số người phải > 0!");
                    return;
                }

                // 🔹 4. Gọi BUS
                string kq = loaiPhong_BUS.Update_LoaiPhong(dto);

                // 🔹 5. Kết quả
                if (kq == "success")
                {
                    MessageBox.Show("Cập nhật thành công!");

                    LoadData();
                    clearinput();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            // 🔹 1. Kiểm tra đã chọn chưa
            if (string.IsNullOrWhiteSpace(txtMaLoaiPhong.Text))
            {
                MessageBox.Show("Vui lòng chọn loại phòng cần xóa!");
                return;
            }

            // 🔹 2. Xác nhận
            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa loại phòng này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (rs == DialogResult.No)
                return;

            try
            {
                // 🔹 3. Gọi BUS
                string kq = loaiPhong_BUS.Delete_LoaiPhong(txtMaLoaiPhong.Text);

                if (kq == "success")
                {
                    MessageBox.Show("Xóa thành công!");

                    LoadData();
                    clearinput();
                }
                else
                {
                    MessageBox.Show(kq); // lỗi FK hoặc không tồn tại
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // 🔹 1. Tránh click vào header
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = dataGridViewLoaiPhong.Rows[e.RowIndex];

                // 🔹 2. Đổ dữ liệu lên form
                txtMaLoaiPhong.Text = row.Cells["MaLoaiPhong"].Value?.ToString();
                txtTenLoaiPhong.Text = row.Cells["TenLoaiPhong"].Value?.ToString();
                txtMoTa.Text = row.Cells["MoTa"].Value?.ToString();

                // 🔥 NumericUpDown
                if (row.Cells["SoNguoiToiDa"].Value != null)
                    numSoNguoi.Value = Convert.ToDecimal(row.Cells["SoNguoiToiDa"].Value);

                if (row.Cells["GiaThueMacDinh"].Value != null)
                    numGiaThue.Value = Convert.ToDecimal(row.Cells["GiaThueMacDinh"].Value);

                if (row.Cells["DonGiaDien"].Value != null)
                    numGiaDien.Value = Convert.ToDecimal(row.Cells["DonGiaDien"].Value);

                if (row.Cells["DonGiaNuoc"].Value != null)
                    numGiaNuoc.Value = Convert.ToDecimal(row.Cells["DonGiaNuoc"].Value);

                // 🔥 ComboBox
                if (row.Cells["TrangThai"].Value != null)
                    cbbTrangThai.Text = row.Cells["TrangThai"].Value.ToString();

                // 🔥 QUAN TRỌNG: khóa mã (không cho sửa)
                txtMaLoaiPhong.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi chọn dòng: " + ex.Message);
            }
        }

        private void LoaiPhong_Load(object sender, EventArgs e)
        {
            LoadData();
            LoadcbTrangThai();
            clearinput();
           LoadThongKe();
            StyleForm();
            StyleThongKe();
            RoundGroupBox(groupBox3);
            RoundGroupBox(groupBox1);

        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            clearinput();
            LoadData();
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

            lblTongLoaiPhong.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTongLoaiPhong.ForeColor = Color.FromArgb(37, 99, 235);
            lblTongLoaiPhong.Dock = DockStyle.Fill;
            lblTongLoaiPhong.TextAlign = ContentAlignment.MiddleCenter;

          
        }
        private void LoadThongKe()
        {
            lblTongLoaiPhong.Text =
                loaiPhong_BUS.TongLoaiPhong().ToString();
        }
        private void btntim_Click(object sender, EventArgs e)
        {
            int soNguoi = (int)numericUpDown1.Value;

            dataGridViewLoaiPhong.DataSource =
                loaiPhong_BUS.TimTheoSoNguoi(soNguoi);
        }
    }
}
