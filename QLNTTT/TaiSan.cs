using BUS;
using ClosedXML.Excel;
using ClosedXML.Excel;
using DAL;
using DTO;
using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace QLNTTT
{
    public partial class TaiSan : Form
    {
        public TaiSan()
        {
            InitializeComponent();
        }
        public TaiSan_DTO TaiSanDuocChon
        {
            get;
            set;
        }
        Phong_BUS phong_BUS = new Phong_BUS();
        TaiSan_BUS taiSan_BUS = new TaiSan_BUS();
        void loadcbtrangthai()
        {
            cbbTrangThai.Items.Clear();

            cbbTrangThai.Items.Add("Đang sử dụng");
            cbbTrangThai.Items.Add("Hỏng");
            cbbTrangThai.Items.Add("Đang bảo trì");
            cbbTrangThai.Items.Add("Đã thanh lý");

            cbbTrangThai.SelectedIndex = 0;
        }
        void loadcbphong()
        {
            try
            {
                Phong_BUS phong = new Phong_BUS();

                var ds = phong.GetAll();

                cbbPhong.DataSource = ds;
                cbbPhong.DisplayMember = "TenPhong";
                cbbPhong.ValueMember = "MaPhong";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load phòng: " + ex.Message);
            }
        }
        void loaddata()
        {
            try
            {
                TaiSan_BUS ts = new TaiSan_BUS();

                var ds = ts.GetAll();

                if (ds != null)
                {
                    dataGridViewTaiSan.DataSource = ds;

                    dataGridViewTaiSan.Columns["MaTaiSan"].HeaderText = "Mã tài sản";
                    dataGridViewTaiSan.Columns["TenTaiSan"].HeaderText = "Tên tài sản";
                    dataGridViewTaiSan.Columns["MaPhong"].HeaderText = "Phòng";
                    dataGridViewTaiSan.Columns["GiaTri"].HeaderText = "Giá trị";
                    dataGridViewTaiSan.Columns["Ma_QR_TS"].HeaderText = "Mã QR";
                    dataGridViewTaiSan.Columns["TrangThai"].HeaderText = "Trạng thái";

                    // format tiền
                    dataGridViewTaiSan.Columns["GiaTri"].DefaultCellStyle.Format = "N0";

                    dataGridViewTaiSan.Columns["Phong"].Visible = false;

                    dataGridViewTaiSan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewTaiSan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewTaiSan.ReadOnly = true;

                    // Ẩn navigation properties nếu có
                    foreach (DataGridViewColumn col in dataGridViewTaiSan.Columns)
                    {
                        if (col.ValueType != null)
                        {
                            if (col.ValueType.Name.Contains("Entity") ||
                                col.ValueType.Name.Contains("ICollection"))
                            {
                                col.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }
        void clear()
        {
            txtMaTaiSan.Clear();
            txtTenTaiSan.Clear();
            pictureBoxQR.Image = null;

            // numeric
            numGiaTri.Value = 0;

            // combobox
            if (cbbPhong.Items.Count > 0)
                cbbPhong.SelectedIndex = 0;

            if (cbbTrangThai.Items.Count > 0)
                cbbTrangThai.SelectedIndex = 0;

            txtMaTaiSan.Enabled = true;
        }
        private void TaiSan_Load(object sender, EventArgs e)
        {
            loaddata();
            loadcbtrangthai();
            loadcbphong();
            clear();
            LoadThongKeTaiSan();
            StyleThongKe();
            RoundGroupBox(groupBox4);
            RoundGroupBox(groupBox5);
            RoundGroupBox(groupBox6);
            RoundGroupBox(groupBox7);
                RoundGroupBox(groupBox8);

        }
        // ================== XỬ LÝ QR CODE ==================
        private void GenerateQRCode(string maTS, string tenTS, string maPhong)
        {
            if (string.IsNullOrEmpty(maTS)) return;

            string content = $"TS:{maTS}|TEN:{tenTS}|PHONG:{maPhong}";
            pictureBoxQR.Image = TaoQR(content);
            pictureBoxQR.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private Bitmap TaoQR(string content)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData data = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
                using (QRCode qr = new QRCode(data))
                {
                    return qr.GetGraphic(20);
                }
            }
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTaiSan.Text) ||
      string.IsNullOrWhiteSpace(txtTenTaiSan.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                // 🔥 Tạo QR trước
                string qrContent = $"TS:{txtMaTaiSan.Text}|TEN:{txtTenTaiSan.Text}|PHONG:{cbbPhong.SelectedValue}";
                GenerateQRCode(txtMaTaiSan.Text, txtTenTaiSan.Text, cbbPhong.SelectedValue.ToString());

                TaiSan_DTO ts = new TaiSan_DTO()
                {
                    MaTaiSan = txtMaTaiSan.Text.Trim(),
                    TenTaiSan = txtTenTaiSan.Text.Trim(),
                    MaPhong = cbbPhong.SelectedValue.ToString(),
                    GiaTri = (decimal)numGiaTri.Value,
                    Ma_QR_TS = qrContent,
                    TrangThai = cbbTrangThai.Text
                };

                string kq = taiSan_BUS.Insert(ts);

                if (kq == "success")
                {
                    MessageBox.Show("Thêm tài sản thành công!");

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

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTaiSan.Text))
            {
                MessageBox.Show("Vui lòng chọn tài sản cần sửa!");
                return;
            }

            try
            {
                string qrContent = $"TS:{txtMaTaiSan.Text}|TEN:{txtTenTaiSan.Text}|PHONG:{cbbPhong.SelectedValue}";
                GenerateQRCode(txtMaTaiSan.Text, txtTenTaiSan.Text, cbbPhong.SelectedValue.ToString());

                TaiSan_DTO ts = new TaiSan_DTO()
                {
                    MaTaiSan = txtMaTaiSan.Text.Trim(),
                    TenTaiSan = txtTenTaiSan.Text.Trim(),
                    MaPhong = cbbPhong.SelectedValue.ToString(),
                    GiaTri = (decimal)numGiaTri.Value,
                    Ma_QR_TS = qrContent,
                    TrangThai = cbbTrangThai.Text
                };

                string kq = taiSan_BUS.Update(ts);

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
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMaTaiSan.Text))
            {
                MessageBox.Show("Vui lòng chọn tài sản cần xóa!");
                return;
            }

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa tài sản này không?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (rs == DialogResult.No)
                return;

            try
            {
                string kq = taiSan_BUS.Delete(txtMaTaiSan.Text);

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

        private void dataGridViewTaiSan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;

                DataGridViewRow row = dataGridViewTaiSan.Rows[e.RowIndex];

                txtMaTaiSan.Text = row.Cells["MaTaiSan"].Value?.ToString();
                txtTenTaiSan.Text = row.Cells["TenTaiSan"].Value?.ToString();

                cbbPhong.SelectedValue = row.Cells["MaPhong"].Value?.ToString();

                if (row.Cells["GiaTri"].Value != null)
                    numGiaTri.Value = Convert.ToDecimal(row.Cells["GiaTri"].Value);

                cbbTrangThai.Text = row.Cells["TrangThai"].Value?.ToString();

                // 🔥 Hiển thị lại QR
                string qr = row.Cells["Ma_QR_TS"].Value?.ToString();

                if (!string.IsNullOrEmpty(qr))
                {
                    pictureBoxQR.Image = TaoQR(qr);
                    pictureBoxQR.SizeMode = PictureBoxSizeMode.Zoom;
                }

                txtMaTaiSan.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi chọn dòng: " + ex.Message);
            }
        }

        private void btnxuat_Click(object sender, EventArgs e)
        {
            // gọi hàm đúng
            var list = taiSan_BUS.GetAll();

            string tempFile = Path.Combine(Path.GetTempPath(), "QR_TaiSan.xlsx");

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("TaiSan");

                // header
                ws.Cell(1, 1).Value = "Mã TS";
                ws.Cell(1, 2).Value = "Tên TS";
                ws.Cell(1, 3).Value = "QR Code";

                int row = 2;

                foreach (DAL.TaiSan item in list)
                {
                    ws.Cell(row, 1).Value = item.MaTaiSan;
                    ws.Cell(row, 2).Value = item.TenTaiSan;

                    string content =
                        "TS:" + item.MaTaiSan +
                        "|TEN:" + item.TenTaiSan +
                        "|PHONG:" + item.MaPhong;

                    Bitmap qr = TaoQR(content);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        qr.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                        ms.Position = 0;

                        ws.AddPicture(ms)
                          .MoveTo(ws.Cell(row, 3))
                          .Scale(0.5);
                    }

                    ws.Row(row).Height = 80;

                    row++;
                }
                wb.SaveAs(tempFile);
            }

            MessageBox.Show("Xuất Excel thành công!");

            // mở file
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = tempFile;
            psi.UseShellExecute = true;

            Process.Start(psi);
        }

        private void dataGridViewTaiSan_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                TaiSanDuocChon =
                    new TaiSan_DTO();

                TaiSanDuocChon.MaTaiSan =
                    dataGridViewTaiSan.Rows[e.RowIndex]
                    .Cells["MaTaiSan"]
                    .Value.ToString();

                TaiSanDuocChon.TenTaiSan =
                    dataGridViewTaiSan.Rows[e.RowIndex]
                    .Cells["TenTaiSan"]
                    .Value.ToString();

                DialogResult = DialogResult.OK;

                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnlammoi_Click(object sender, EventArgs e)
        {
            clear();
            loaddata();
        }
        private void LoadThongKeTaiSan()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                lblTongTaiSan.Text =
                    db.TaiSans.Count().ToString();

                lblDangSuDung.Text =
                    db.TaiSans.Count(x => x.TrangThai == "Đang sử dụng")
                    .ToString();

                lblHong.Text =
                    db.TaiSans.Count(x => x.TrangThai == "Hỏng")
                    .ToString();
                lblbaotri.Text =
                    db.TaiSans.Count(x => x.TrangThai == "Đang bảo trì")
                    .ToString();
               lblgiatri.Text = db.TaiSans.Sum(x => x.GiaTri ?? 0).ToString("N0");
                   
            }
        }
        private void StyleThongKe()
        {
            // Tổng tài sản - Xanh dương
            groupBox4.BackColor = Color.FromArgb(219, 234, 254);
            groupBox4.ForeColor = Color.FromArgb(37, 99, 235);

            lblTongTaiSan.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblTongTaiSan.ForeColor = Color.FromArgb(37, 99, 235);
            lblTongTaiSan.Dock = DockStyle.Fill;
            lblTongTaiSan.TextAlign = ContentAlignment.MiddleCenter;

            // Đang sử dụng - Xanh lá
            groupBox5.BackColor = Color.FromArgb(220, 252, 231);
            groupBox5.ForeColor = Color.FromArgb(22, 163, 74);

            lblDangSuDung.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblDangSuDung.ForeColor = Color.FromArgb(22, 163, 74);
            lblDangSuDung.Dock = DockStyle.Fill;
            lblDangSuDung.TextAlign = ContentAlignment.MiddleCenter;

            // Hỏng - Đỏ
            groupBox6.BackColor = Color.FromArgb(254, 226, 226);
            groupBox6.ForeColor = Color.FromArgb(220, 38, 38);

            lblHong.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblHong.ForeColor = Color.FromArgb(220, 38, 38);
            lblHong.Dock = DockStyle.Fill;
            lblHong.TextAlign = ContentAlignment.MiddleCenter;

            // Bảo trì - Cam
            groupBox7.BackColor = Color.FromArgb(255, 237, 213);
            groupBox7.ForeColor = Color.FromArgb(234, 88, 12);

            lblbaotri.Font = new Font("Segoe UI", 22, FontStyle.Bold);
            lblbaotri.ForeColor = Color.FromArgb(234, 88, 12);
            lblbaotri.Dock = DockStyle.Fill;
            lblbaotri.TextAlign = ContentAlignment.MiddleCenter;

            // Giá trị tài sản - Tím
            groupBox8.BackColor = Color.FromArgb(237, 233, 254);
            groupBox8.ForeColor = Color.FromArgb(124, 58, 237);

            lblgiatri.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblgiatri.ForeColor = Color.FromArgb(124, 58, 237);
            lblgiatri.Dock = DockStyle.Fill;
            lblgiatri.TextAlign = ContentAlignment.MiddleCenter;
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
    }
    }

