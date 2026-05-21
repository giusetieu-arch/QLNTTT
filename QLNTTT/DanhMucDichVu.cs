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
    public partial class DanhMucDichVu : Form
    {
        public DanhMucDichVu_DTO DichVuDuocChon;
        public DanhMucDichVu()
        {
            InitializeComponent();
        }
        DanhMucDichVu_BUS bus = new DanhMucDichVu_BUS();
        void loaddata()
        {
            try
            {
                // Giả sử DataGridView của bạn tên là dgvDichVu
                dgvDichVu.DataSource = bus.LayTatCa();
                // Nếu thấy thằng này thì ẩn nó đi ngay
                if (dgvDichVu.Columns.Contains("HopDong_DichVu"))
                {
                    dgvDichVu.Columns["HopDong_DichVu"].Visible = false;
                }
                // Tùy chỉnh tiêu đề cột (nếu muốn)
                dgvDichVu.Columns["MaDichVu"].HeaderText = "Mã DV";
                dgvDichVu.Columns["TenDichVu"].HeaderText = "Tên Dịch Vụ";
                dgvDichVu.Columns["DonGia"].HeaderText = "Đơn Giá";
                dgvDichVu.Columns["DonGia"].DefaultCellStyle.Format = "N0";
                dgvDichVu.Columns["DonViTinh"].HeaderText = "ĐVT";
                dgvDichVu.Columns["HinhThucTinh"].HeaderText = "Hình Thức Tính";
                dgvDichVu.Columns["GhiChu"].HeaderText = "Ghi Chú";
                dgvDichVu.Columns["TrangThai"].HeaderText = "Trạng Thái";
                dgvDichVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvDichVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvDichVu.ReadOnly = true;
              
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }
        void loadcbDonViTinh()
        {
            cbDonViTinh.Items.Clear();

            cbDonViTinh.Items.Add("Người");
            cbDonViTinh.Items.Add("Phòng");
            cbDonViTinh.Items.Add("kWh");
            cbDonViTinh.Items.Add("m3");
            cbDonViTinh.Items.Add("Xe");
            cbDonViTinh.Items.Add("Tháng");

            cbDonViTinh.SelectedIndex = 0;
        }
        void loadcbtrangthai()
        {
            cbTrangThai.Items.Clear();
            cbTrangThai.Items.Add("Đang kinh doanh");
            cbTrangThai.Items.Add("Ngừng cung cấp");
            cbTrangThai.SelectedIndex = 0;
        }
        void clear()
        {
            txtMaDV.Clear();
            txtTenDV.Clear();

            // NumericUpDown
            numDonGia.Value = 0;

            cbDonViTinh.SelectedIndex = 0;

          if (cbHinhThucTinh.Items.Count > 0)
    cbHinhThucTinh.SelectedIndex = 0;

if (cbTrangThai.Items.Count > 0)
    cbTrangThai.SelectedIndex = 0;

            txtMaDV.Enabled = true;

            txtMaDV.Focus();
        }
        void loadcbhinhthutinh()
        {
            cbHinhThucTinh.Items.Clear();

            cbHinhThucTinh.Items.Add("DAU_NGUOI");
            cbHinhThucTinh.Items.Add("PHONG");
            cbHinhThucTinh.Items.Add("TIEU_THU");

            cbHinhThucTinh.SelectedIndex = 0;
        }
        private void DanhMucDichVu_Load(object sender, EventArgs e)
        {
            loaddata();
            loadcbtrangthai();
            loadcbhinhthutinh();
            loadcbDonViTinh();
            clear();
           LoadThongKeDichVu();
            StyleThongKe();
                RoundGroupBox(groupBox3);
                RoundGroupBox(groupBox4);
                RoundGroupBox(groupBox5);
                RoundGroupBox(groupBox6);
                RoundGroupBox(groupBox7);
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text) ||
       string.IsNullOrWhiteSpace(txtTenDV.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            DTO.DanhMucDichVu_DTO dto = new DTO.DanhMucDichVu_DTO()
            {
                MaDichVu = txtMaDV.Text.Trim(),
                TenDichVu = txtTenDV.Text.Trim(),

                // NumericUpDown
                DonGia = numDonGia.Value,

                DonViTinh = cbDonViTinh.Text,
                HinhThucTinh = cbHinhThucTinh.Text,
                GhiChu=txtghichu.Text,
                TrangThai = cbTrangThai.Text
            };

            string kq = bus.Them(dto);

            if (kq == "success")
            {
                MessageBox.Show("Thêm thành công!");
                loaddata();
                clear();
            }
            else
            {
                MessageBox.Show(kq);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ!");
                return;
            }

            DTO.DanhMucDichVu_DTO dto = new DTO.DanhMucDichVu_DTO()
            {
                MaDichVu = txtMaDV.Text.Trim(),
                TenDichVu = txtTenDV.Text.Trim(),
                DonGia = numDonGia.Value,
                DonViTinh = cbDonViTinh.Text,
                HinhThucTinh = cbHinhThucTinh.Text,
                GhiChu = txtghichu.Text,
                TrangThai = cbTrangThai.Text
            };

            string kq = bus.Sua(dto);

            if (kq == "success")
            {
                MessageBox.Show("Cập nhật thành công!");
                loaddata();
                clear();
            }
            else
            {
                MessageBox.Show(kq);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaDV.Text))
            {
                MessageBox.Show("Vui lòng chọn dịch vụ cần xóa!");
                return;
            }

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa dịch vụ này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (rs == DialogResult.No)
                return;

            string kq = bus.Xoa(txtMaDV.Text);

            if (kq == "success")
            {
                MessageBox.Show("Xóa thành công!");
                loaddata();
                clear();
            }
            else
            {
                MessageBox.Show(kq);
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvDichVu_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvDichVu.Rows[e.RowIndex];

            txtMaDV.Text = row.Cells["MaDichVu"].Value.ToString();
            txtTenDV.Text = row.Cells["TenDichVu"].Value.ToString();

            // NumericUpDown
            numDonGia.Value = Convert.ToDecimal(row.Cells["DonGia"].Value);

            cbDonViTinh.Text = row.Cells["DonViTinh"].Value.ToString();

            cbHinhThucTinh.Text = row.Cells["HinhThucTinh"].Value.ToString();
            txtghichu.Text = row.Cells["GhiChu"].Value.ToString();
            cbTrangThai.Text = row.Cells["TrangThai"].Value.ToString();

            // khóa mã khi sửa
            txtMaDV.Enabled = false;
        }

        private void dgvDichVu_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dgvDichVu.Rows[e.RowIndex];

            DichVuDuocChon = new DanhMucDichVu_DTO()
            {
                MaDichVu = row.Cells["MaDichVu"].Value?.ToString(),
                TenDichVu = row.Cells["TenDichVu"].Value?.ToString(),
                DonGia = Convert.ToDecimal(row.Cells["DonGia"].Value ?? 0),

                HinhThucTinh =
          row.Cells["HinhThucTinh"].Value?.ToString() ?? ""
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void StyleThongKe()
        {
            // Tổng dịch vụ - Xanh dương
            groupBox3.BackColor = Color.FromArgb(219, 234, 254);
            groupBox3.ForeColor = Color.FromArgb(37, 99, 235);

            lblTongDV.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTongDV.ForeColor = Color.FromArgb(37, 99, 235);
            lblTongDV.Dock = DockStyle.Fill;
            lblTongDV.TextAlign = ContentAlignment.MiddleCenter;

            // Đang kinh doanh - Xanh lá
            groupBox4.BackColor = Color.FromArgb(220, 252, 231);
            groupBox4.ForeColor = Color.FromArgb(22, 163, 74);

            lblDangKD.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblDangKD.ForeColor = Color.FromArgb(22, 163, 74);
            lblDangKD.Dock = DockStyle.Fill;
            lblDangKD.TextAlign = ContentAlignment.MiddleCenter;

            // Ngừng cung cấp - Đỏ
            groupBox5.BackColor = Color.FromArgb(254, 226, 226);
            groupBox5.ForeColor = Color.FromArgb(220, 38, 38);

            lblNgungCC.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblNgungCC.ForeColor = Color.FromArgb(220, 38, 38);
            lblNgungCC.Dock = DockStyle.Fill;
            lblNgungCC.TextAlign = ContentAlignment.MiddleCenter;

            // Đầu người - Tím
            groupBox6.BackColor = Color.FromArgb(237, 233, 254);
            groupBox6.ForeColor = Color.FromArgb(124, 58, 237);

            lblDauNguoi.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblDauNguoi.ForeColor = Color.FromArgb(124, 58, 237);
            lblDauNguoi.Dock = DockStyle.Fill;
            lblDauNguoi.TextAlign = ContentAlignment.MiddleCenter;

            // Tiêu thụ - Xanh ngọc
            groupBox7.BackColor = Color.FromArgb(204, 251, 241);
            groupBox7.ForeColor = Color.FromArgb(13, 148, 136);

            lblTheoPhong.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTheoPhong.ForeColor = Color.FromArgb(13, 148, 136);
            lblTheoPhong.Dock = DockStyle.Fill;
            lblTheoPhong.TextAlign = ContentAlignment.MiddleCenter;
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
        private void LoadThongKeDichVu()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                lblTongDV.Text =
                    db.DanhMucDichVus.Count().ToString();

                lblDangKD.Text =
                    db.DanhMucDichVus.Count(x =>
                        x.TrangThai == "Đang kinh doanh")
                    .ToString();

                lblNgungCC.Text =
                    db.DanhMucDichVus.Count(x =>
                        x.TrangThai == "Ngừng cung cấp")
                    .ToString();

                lblDauNguoi.Text =
                    db.DanhMucDichVus.Count(x =>
                        x.HinhThucTinh == "DAU_NGUOI")
                    .ToString();

                lblTheoPhong.Text =
                    db.DanhMucDichVus.Count(x =>
                        x.HinhThucTinh == "PHONG")
                    .ToString();

               
            }
        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            clear();
            loaddata();
            LoadThongKeDichVu();
        }
    }
}
