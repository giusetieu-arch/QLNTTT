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
    public partial class CuDan : Form
    {
        public CuDan_DTO CuDanDuocChon = null;
        public CuDan()
        {
            InitializeComponent();
        }
        CuDan_BUS cuDan_BUS = new CuDan_BUS();
        void loaddata()
        {
            dataGridViewCuDan.DataSource = cuDan_BUS.LayCuDanHopLe();
            try
            {
                var ds = cuDan_BUS.GetAll();

                if (ds != null)
                {
                    dataGridViewCuDan.DataSource = null;
                    dataGridViewCuDan.DataSource = ds;

                    dataGridViewCuDan.Columns["MaCuDan"].HeaderText = "Mã cư dân";
                    dataGridViewCuDan.Columns["TenCuDan"].HeaderText = "Tên cư dân";
                    dataGridViewCuDan.Columns["NgaySinh"].HeaderText = "Ngày sinh";
                    dataGridViewCuDan.Columns["GioiTinh"].HeaderText = "Giới tính";
                    dataGridViewCuDan.Columns["Email"].HeaderText = "Email";
                    dataGridViewCuDan.Columns["CCCD"].HeaderText = "CCCD";
                    dataGridViewCuDan.Columns["SDT"].HeaderText = "SĐT";
                    dataGridViewCuDan.Columns["QueQuan"].HeaderText = "Quê quán";
                    dataGridViewCuDan.Columns["NgayTao"].HeaderText = "Ngày tạo";
                    dataGridViewCuDan.Columns["TrangThai"].HeaderText = "Trạng thái";

                    // Format ngày tháng
                    dataGridViewCuDan.Columns["NgaySinh"].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dataGridViewCuDan.Columns["NgayTao"].DefaultCellStyle.Format = "dd/MM/yyyy";

                    dataGridViewCuDan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewCuDan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewCuDan.ReadOnly = true;

                    // Ẩn navigation properties nếu có (Entity Framework)
                    foreach (DataGridViewColumn col in dataGridViewCuDan.Columns)
                    {
                        if (col.ValueType != null)
                        {
                            if (col.ValueType.Name.Contains("Entity") ||
                                col.ValueType.Name.Contains("ICollection") ||
                                col.ValueType.Name.Contains("CuDan") && col.Name != "MaCuDan")
                            {
                                col.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu cư dân: " + ex.Message, "Lỗi",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        void loadcbtrangthai()
        {
            comboBoxTrangThai.Items.Clear();
            comboBoxTrangThai.Items.Add("Đang cư trú");
            comboBoxTrangThai.Items.Add("Đã chuyển đi");
            comboBoxTrangThai.Items.Add("Tạm vắng");
            comboBoxTrangThai.SelectedIndex = 0;
        }
        void loadcbgioitinh()
        {
            comboBoxGioiTinh.Items.Clear();
            comboBoxGioiTinh.Items.Add("Nam");
            comboBoxGioiTinh.Items.Add("Nữ");
            comboBoxGioiTinh.SelectedIndex = 0;
        }
        void clear()
        {
            txtMaCuDan.Clear();
            txtTenCuDan.Clear();
            dtpNgaySinh.Value = DateTime.Now;
            comboBoxGioiTinh.SelectedIndex = 0;
            txtCCCD.Clear();
            txtSDT.Clear();
            txtQueQuan.Clear();
            dtpNgayTao.Value = DateTime.Now;
            comboBoxTrangThai.SelectedIndex = 0;
            txtMaCuDan.Focus();
            txtEmail.Clear();

            // Reset trạng thái nút
            btnthem.Enabled = true;
            btnsua.Enabled = false;
            btnxoa.Enabled = false;
           // isEditMode = false;

        }
        private void CuDan_Load(object sender, EventArgs e)
        {
            dtpNgaySinh.Format = DateTimePickerFormat.Custom;
            dtpNgaySinh.CustomFormat = "dd/MM/yyyy";
            
           
            //dtpNgaySinh.MaxDate = DateTime.Now.AddYears(-18);
            loaddata();
            loadcbgioitinh();
            loadcbtrangthai();
            clear();
            LoadThongKeCuDan();
            StyleThongKe();
            RoundGroupBox(groupBox3);
            RoundGroupBox(groupBox4);
            RoundGroupBox(groupBox5);
            RoundGroupBox(groupBox6);
            RoundGroupBox(groupBox7);
            RoundGroupBox(groupBox8);
        }


        private void btnthem_Click(object sender, EventArgs e)
        {
            if (!txtEmail.Text.Contains("@gmail.com"))
            {
                MessageBox.Show("Email phải có @gmail.com");
                return;
            }
            // kiểm tra dữ liệu
            if (string.IsNullOrWhiteSpace(txtMaCuDan.Text) ||
                string.IsNullOrWhiteSpace(txtTenCuDan.Text) ||
                string.IsNullOrWhiteSpace(txtCCCD.Text) ||
                string.IsNullOrWhiteSpace(txtSDT.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                CuDan_DTO cd = new CuDan_DTO()
                {
                    MaCuDan = txtMaCuDan.Text.Trim(),
                    TenCuDan = txtTenCuDan.Text.Trim(),
                    NgaySinh = dtpNgaySinh.Value,
                    GioiTinh = comboBoxGioiTinh.Text,
                    Email = txtEmail.Text.Trim(),
                    CCCD = txtCCCD.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    QueQuan = txtQueQuan.Text.Trim(),
                    NgayTao = dtpNgayTao.Value,
                    TrangThai = comboBoxTrangThai.Text
                };

                string kq = cuDan_BUS.Insert(cd);

                if (kq == "success")
                {
                    MessageBox.Show("Thêm cư dân thành công!");

                    loaddata();
                    clear();
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
            clear();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaCuDan.Text))
            {
                MessageBox.Show("Vui lòng chọn cư dân cần sửa!");
                return;
            }

            try
            {
                CuDan_DTO cd = new CuDan_DTO()
                {
                    MaCuDan = txtMaCuDan.Text.Trim(),
                    TenCuDan = txtTenCuDan.Text.Trim(),
                    NgaySinh = dtpNgaySinh.Value,
                    GioiTinh = comboBoxGioiTinh.Text,
                    Email = txtEmail.Text.Trim(),
                    CCCD = txtCCCD.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    QueQuan = txtQueQuan.Text.Trim(),
                    NgayTao = dtpNgayTao.Value,
                    TrangThai = comboBoxTrangThai.Text
                };

                string kq = cuDan_BUS.Update(cd);

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            clear();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaCuDan.Text))
            {
                MessageBox.Show("Vui lòng chọn cư dân cần xóa!");
                return;
            }

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa cư dân này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (rs == DialogResult.No)
                return;

            try
            {
                string kq = cuDan_BUS.Delete(txtMaCuDan.Text);

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
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridViewCuDan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow row = dataGridViewCuDan.Rows[e.RowIndex];

                txtMaCuDan.Text = row.Cells["MaCuDan"].Value?.ToString();
                txtTenCuDan.Text = row.Cells["TenCuDan"].Value?.ToString();

                if (row.Cells["NgaySinh"].Value != null)
                    dtpNgaySinh.Value = Convert.ToDateTime(row.Cells["NgaySinh"].Value);

                comboBoxGioiTinh.Text = row.Cells["GioiTinh"].Value?.ToString();
                txtEmail.Text =
    row.Cells["Email"].Value?.ToString();

                txtCCCD.Text = row.Cells["CCCD"].Value?.ToString();
                txtSDT.Text = row.Cells["SDT"].Value?.ToString();
                txtQueQuan.Text = row.Cells["QueQuan"].Value?.ToString();

                if (row.Cells["NgayTao"].Value != null)
                    dtpNgayTao.Value = Convert.ToDateTime(row.Cells["NgayTao"].Value);

                comboBoxTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();

                // khóa mã khi sửa
               // txtMaCuDan.Enabled = false;

                // bật tắt nút
               // btnthem.Enabled = false;
                //btnsua.Enabled = true;
                //btnxoa.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chọn dòng: " + ex.Message);
            }
        }

        private void dataGridViewCuDan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            DataGridViewRow row = dataGridViewCuDan.Rows[e.RowIndex];

            CuDanDuocChon = new CuDan_DTO()
            {
                MaCuDan = row.Cells["MaCuDan"].Value.ToString(),
                TenCuDan = row.Cells["TenCuDan"].Value.ToString()
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
        private void LoadThongKeCuDan()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                // Tổng cư dân
                lblTongCuDan.Text =
                    db.CuDans.Count().ToString();

                // Đang cư trú
                lblDangCuTru.Text =
                    db.CuDans.Count(x => x.TrangThai == "Đang cư trú")
                    .ToString();

                // Đã chuyển đi
                lblDaChuyenDi.Text =
                    db.CuDans.Count(x => x.TrangThai == "Đã chuyển đi")
                    .ToString();

                // Tạm vắng
                lblTamVang.Text =
                    db.CuDans.Count(x => x.TrangThai == "Tạm vắng")
                    .ToString();

                // Cư dân mới trong tháng
                lblMoiTrongThang.Text =
                    db.CuDans.Count(x =>
                        x.NgayTao.Value.Month == DateTime.Now.Month &&
                        x.NgayTao.Value.Year == DateTime.Now.Year)
                    .ToString();
                lblNam.Text = db.CuDans
     .Count(x => x.GioiTinh.Trim().ToLower() == "nam")
     .ToString();

                lblNu.Text = db.CuDans
                    .Count(x => x.GioiTinh.Trim().ToLower() == "nữ" ||
                                x.GioiTinh.Trim().ToLower() == "nu")
                    .ToString();
            }
        }
        private void StyleThongKe()
        {
            // Tổng phòng - Xanh dương
            groupBox3.BackColor = Color.FromArgb(219, 234, 254);
            groupBox3.ForeColor = Color.FromArgb(37, 99, 235);

            lblTongCuDan.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTongCuDan.ForeColor = Color.FromArgb(37, 99, 235);
            lblTongCuDan.Dock = DockStyle.Fill;
            lblTongCuDan.TextAlign = ContentAlignment.MiddleCenter;

            // Đang thuê - Xanh lá
            groupBox4.BackColor = Color.FromArgb(220, 252, 231);
            groupBox4.ForeColor = Color.FromArgb(22, 163, 74);

            lblDangCuTru.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblDangCuTru.ForeColor = Color.FromArgb(22, 163, 74);
            lblDangCuTru.Dock = DockStyle.Fill;
            lblDangCuTru.TextAlign = ContentAlignment.MiddleCenter;

            // Phòng trống - Vàng
            groupBox5.BackColor = Color.FromArgb(254, 243, 199);
            groupBox5.ForeColor = Color.FromArgb(217, 119, 6);

            lblDaChuyenDi.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblDaChuyenDi.ForeColor = Color.FromArgb(217, 119, 6);
            lblDaChuyenDi.Dock = DockStyle.Fill;
            lblDaChuyenDi.TextAlign = ContentAlignment.MiddleCenter;

            // Card 4 - Đỏ
            groupBox6.BackColor = Color.FromArgb(254, 226, 226);
            groupBox6.ForeColor = Color.FromArgb(220, 38, 38);

            lblTamVang.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTamVang.ForeColor = Color.FromArgb(220, 38, 38);
            lblTamVang.Dock = DockStyle.Fill;
            lblTamVang.TextAlign = ContentAlignment.MiddleCenter;

            // Card 5 - Tím
            groupBox7.BackColor = Color.FromArgb(237, 233, 254);
            groupBox7.ForeColor = Color.FromArgb(124, 58, 237);

            lblNam.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblNam.ForeColor = Color.FromArgb(124, 58, 237);
            
            lblNam.TextAlign = ContentAlignment.MiddleCenter;
            lblNu.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblNu.ForeColor = Color.FromArgb(124, 58, 237);
            
            lblNu.TextAlign = ContentAlignment.MiddleCenter;


            // Card 6 - Xanh ngọc
            groupBox8.BackColor = Color.FromArgb(204, 251, 241);
            groupBox8.ForeColor = Color.FromArgb(13, 148, 136);

            lblMoiTrongThang.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblMoiTrongThang.ForeColor = Color.FromArgb(13, 148, 136);
            lblMoiTrongThang.Dock = DockStyle.Fill;
            lblMoiTrongThang.TextAlign = ContentAlignment.MiddleCenter;
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

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            clear();
             loaddata();
             LoadThongKeCuDan();
        }
    }
}
