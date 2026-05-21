using BUS;
using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace QLNTTT
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        CongViec_BUS  cvBUS      = new CongViec_BUS();
        Phong_BUS     phongBUS   = new Phong_BUS();
        CuDan_BUS     cdBUS      = new CuDan_BUS();
        SoQuy_BUS     sqBUS      = new SoQuy_BUS();
        HoaDon_BUS    hdBUS      = new HoaDon_BUS();
        HopDong_BUS   hopDongBUS = new HopDong_BUS();

        // ── Màu stat-cards ────────────────────────────────────────────────
        void loadmaugroup()
        {
            groupBoxTongPhong.BackColor  = UIHelper.PrimaryLight;
            groupBoxDangThue.BackColor   = UIHelper.SuccessLight;
            groupBoxPhongTrong.BackColor = UIHelper.WarningLight;
            groupBoxCuDan.BackColor      = UIHelper.PurpleLight;
            groupBoxDoanhThu.BackColor   = UIHelper.TealLight;
            groupBoxCongNo.BackColor     = UIHelper.DangerLight;
            groupBoxBaoHong.BackColor    = Color.FromArgb(255, 237, 213);
            groupBoxHopDong.BackColor    = Color.FromArgb(226, 232, 240);
        }

        // ── Màu số liệu ───────────────────────────────────────────────────
        void loadchu()
        {
            lblTongPhong.ForeColor  = UIHelper.Primary;
            lblDangThue.ForeColor   = UIHelper.Success;
            lblPhongTrong.ForeColor = UIHelper.Warning;
            lblCuDan.ForeColor      = UIHelper.Purple;
            lblDoanhThu.ForeColor   = UIHelper.Teal;
            lblCongNo.ForeColor     = UIHelper.Danger;
            lblBaoHong.ForeColor    = Color.FromArgb(194, 65, 12);
            lblHopDong.ForeColor    = UIHelper.TextSecond;
        }

        // ── Font phù hợp cho số liệu stat cards ─────────────────────────
        void loadcochu()
        {
            // Dùng font 18pt thay vì 22pt để vừa với GroupBox
            var numFont = new Font("Segoe UI", 18, FontStyle.Bold);
            var moneyFont = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTongPhong.Font  = numFont;
            lblDangThue.Font   = numFont;
            lblPhongTrong.Font = numFont;
            lblCuDan.Font      = numFont;
            lblDoanhThu.Font   = moneyFont;
            lblCongNo.Font     = moneyFont;
            lblBaoHong.Font    = numFont;
            lblHopDong.Font    = numFont;
        }

        // ── Biểu đồ doanh thu theo tháng ─────────────────────────────────
        void LoadChartDoanhThu()
        {
            chartDoanhThu.Series.Clear();
            var s = chartDoanhThu.Series.Add("Doanh Thu");
            s.ChartType = SeriesChartType.Column;

            using (var db = new QLNT_DoVanTieuEntities())
            {
                var data = db.SoQuys
                    .GroupBy(x => new { x.NgayGiaoDich.Value.Year, x.NgayGiaoDich.Value.Month })
                    .Select(g => new
                    {
                        Thang    = g.Key.Month + "/" + g.Key.Year,
                        DoanhThu = g.Sum(x => x.Thu)
                    })
                    .OrderBy(x => x.Thang)
                    .ToList();

                foreach (var item in data)
                    s.Points.AddXY(item.Thang, item.DoanhThu);
            }
        }

        void LoadHopDongSapHetHan() =>
            lblHopDong.Text = hopDongBUS.HopDongSapHetHan().ToString();

        void LoadBaoHong()
        {
            lblBaoHong.Text = cvBUS.BaoHongChuaXuLy().ToString();
            UIHelper.StyleGrid(dgvBaoHongMoi, UIHelper.Danger);
        }

        void LoadCongNo()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                decimal t = db.HoaDons
                    .Where(x => x.TrangThai == "Chưa thanh toán")
                    .Sum(x => (decimal?)x.TongTien) ?? 0;
                lblCongNo.Text = t.ToString("N0") + " VNĐ";
            }
        }

        void LoadDoanhThu()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                int thang = DateTime.Now.Month;
                int nam = DateTime.Now.Year;

                decimal doanhThu =
                    db.SoQuys
                    .Where(x =>
                        x.LoaiGiaoDich == "Thu" &&
                        x.NgayGiaoDich.Value.Month == thang &&
                        x.NgayGiaoDich.Value.Year == nam)
                    .Sum(x => (decimal?)x.Thu) ?? 0;

                lblDoanhThu.Text =
                    doanhThu.ToString("N0") + " VNĐ";
            }
        }
        void LoadCuDan()      => lblCuDan.Text      = cdBUS.TongCuDan().ToString();
        void LoadPhongTrong() => lblPhongTrong.Text  = phongBUS.PhongTrong().ToString();
        void LoadPhongDangThue() => lblDangThue.Text = phongBUS.PhongDangThue().ToString();
        void LoadTongPhong()  => lblTongPhong.Text   = phongBUS.TongPhong().ToString();

        void LoadBaoHongMoi()
        {
            dgvBaoHongMoi.DataSource = cvBUS.GetTopBaoHongMoi();

            dgvBaoHongMoi.Columns["MaCongViec"].HeaderText = "Mã Công Việc";
            dgvBaoHongMoi.Columns["MaPhong"].HeaderText    = "Phòng";
            dgvBaoHongMoi.Columns["TieuDe"].HeaderText     = "Tiêu đề";
            dgvBaoHongMoi.Columns["TrangThai"].HeaderText  = "Trạng thái";
            dgvBaoHongMoi.Columns["NgayBao"].HeaderText    = "Ngày báo";
            dgvBaoHongMoi.Columns["NgayXuLy"].HeaderText   = "Ngày Xử Lý";

            string[] hideColumns = { "MoTa", "AnhBaoHong", "MaTaiSan", "MaCuDan",
                                     "CuDan", "Phong", "TaiSan", "PhieuThuChis", "LichSuThietbis" };
            foreach (var col in hideColumns)
                if (dgvBaoHongMoi.Columns[col] != null)
                    dgvBaoHongMoi.Columns[col].Visible = false;

            dgvBaoHongMoi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        // ── Navigation helpers ────────────────────────────────────────────
        void hideSubMenu()
        {
            pnlDanhMuc.Visible  = false;
            pnlHopDong.Visible  = false;
            pnlHoaDon1.Visible  = false;
            pnlCongViec.Visible = false;
            pnlBaoCao.Visible   = false;
            pnlHeThong.Visible  = false;
        }

        private Form activeForm = null;

        private void OpenChildForm(Form childForm)
        {
            panelDashboard.Visible = false;
            panelMain.Visible      = true;

            activeForm?.Close();
            panelMain.Controls.Clear();

            activeForm = childForm;
            childForm.TopLevel        = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock            = DockStyle.Fill;

            panelMain.Controls.Add(childForm);
            childForm.Show();
        }

        // ── Menu color helpers ────────────────────────────────────────────
        private void ResetMenuColor()
        {
            Color inactive = Color.FromArgb(30, 41, 59);
            Button[] btns = { btnTrangChu, btnDanhMuc, btnHopDong, btnHoaDon, btnCongViec, btnBaoCao, btnHeThong };
            foreach (var btn in btns)
            {
                btn.BackColor                 = inactive;
                btn.ForeColor                 = Color.White;
                btn.FlatStyle                 = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }
        }

        private void ActiveMenu(Button btn)
        {
            ResetMenuColor();
            btn.BackColor = UIHelper.Primary;
            btn.ForeColor = Color.White;
        }

        // ── UI Styling ────────────────────────────────────────────────────
        private void MauHeader()
        {
            panelHeader.BackColor = Color.FromArgb(15, 23, 42);
            panelHeader.Height    = 48;

            lblTieuDe.ForeColor = Color.White;
            lblTieuDe.Font      = new Font("Segoe UI", 13f, FontStyle.Bold);
            lblTieuDe.BackColor = Color.Transparent;

            lblXinChao.ForeColor = Color.FromArgb(148, 163, 184);
            lblXinChao.Font      = new Font("Segoe UI", 9.5f, FontStyle.Regular);
            lblXinChao.BackColor = Color.Transparent;

            lblTime.ForeColor = Color.FromArgb(100, 116, 139);
            lblTime.Font      = new Font("Segoe UI", 9f, FontStyle.Regular);
            lblTime.BackColor = Color.Transparent;

            panelCon.BackColor = Color.FromArgb(15, 23, 42);
            panelCha.BackColor = Color.FromArgb(15, 23, 42);

            panelDashboard.BackColor = UIHelper.BgPage;
            panelMain.BackColor      = UIHelper.BgPage;
        }

        private void StyleMenuButtons()
        {
            Button[] mainBtns = { btnTrangChu, btnDanhMuc, btnHopDong, btnHoaDon, btnCongViec, btnBaoCao, btnHeThong };
            foreach (var btn in mainBtns)
            {
                btn.FlatStyle                 = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor                 = Color.FromArgb(30, 41, 59);
                btn.ForeColor                 = Color.White;
                btn.Font                      = new Font("Segoe UI", 9f, FontStyle.Bold);
                btn.Cursor                    = Cursors.Hand;
                btn.TextAlign                 = ContentAlignment.MiddleCenter;
            }
            btnTrangChu.BackColor = UIHelper.Primary;

            Panel[] subPanels = { pnlDanhMuc, pnlHopDong, pnlHoaDon1, pnlCongViec, pnlBaoCao, pnlHeThong };
            foreach (var pnl in subPanels)
            {
                pnl.BackColor = Color.FromArgb(15, 23, 42);
                StyleSubPanelButtons(pnl);
            }
        }

        private void StyleSubPanelButtons(Panel pnl)
        {
            foreach (Control c in pnl.Controls)
            {
                if (c is TableLayoutPanel tlp)
                {
                    foreach (Control ctrl in tlp.Controls)
                    {
                        if (ctrl is Button b)
                        {
                            b.FlatStyle                       = FlatStyle.Flat;
                            b.FlatAppearance.BorderSize       = 1;
                            b.FlatAppearance.BorderColor      = Color.FromArgb(51, 65, 85);
                            b.BackColor                       = Color.FromArgb(30, 41, 59);
                            b.ForeColor                       = Color.FromArgb(203, 213, 225);
                            b.Font                            = new Font("Segoe UI", 8.5f, FontStyle.Regular);
                            b.Cursor                          = Cursors.Hand;
                            b.TextAlign                       = ContentAlignment.MiddleCenter;
                        }
                    }
                }
            }
        }

        private void StyleDashboardCards()
        {
            // ── Fix layout: tăng chiều cao rows stat card ─────────────────
            // tableLayoutPanel2 chứa dashboard: row 0 & 1 quá nhỏ (10%) → tăng lên
            tableLayoutPanel2.RowStyles[0] = new RowStyle(SizeType.Percent, 20F); // stat row 1
            tableLayoutPanel2.RowStyles[1] = new RowStyle(SizeType.Percent, 18F); // stat row 2
            tableLayoutPanel2.RowStyles[2] = new RowStyle(SizeType.Percent, 38F); // chart
            tableLayoutPanel2.RowStyles[3] = new RowStyle(SizeType.Percent, 24F); // baohong table

            // ── Fix vị trí labels trong GroupBox ──────────────────────────
            // Labels được đặt tại Y=46 nhưng GroupBox chỉ cao 32px → đặt lại Dock.Fill
            Label[] statLabels = {
                lblTongPhong, lblDangThue, lblPhongTrong, lblCuDan,
                lblDoanhThu,  lblCongNo,   lblBaoHong,    lblHopDong
            };
            foreach (var lbl in statLabels)
            {
                lbl.Dock      = DockStyle.Fill;
                lbl.TextAlign = ContentAlignment.MiddleCenter;
                lbl.Padding   = new Padding(0, 16, 0, 0); // padding top để tránh GroupBox title
            }

            // ── GroupBox stat card styling ─────────────────────────────────
            GroupBox[] cards = {
                groupBoxTongPhong, groupBoxDangThue, groupBoxPhongTrong, groupBoxCuDan,
                groupBoxDoanhThu,  groupBoxCongNo,   groupBoxBaoHong,    groupBoxHopDong
            };
            foreach (var gb in cards)
            {
                gb.Font        = new Font("Segoe UI", 8.5f, FontStyle.Bold);
                gb.ForeColor   = UIHelper.TextSecond;
                gb.Padding     = new Padding(4, 16, 4, 4);
            }

            // ── Section headers ────────────────────────────────────────────
            groupBox9.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            groupBox9.ForeColor = UIHelper.Primary;
            groupBox9.BackColor = Color.White;

            groupBox10.Font      = new Font("Segoe UI", 9.5f, FontStyle.Bold);
            groupBox10.ForeColor = UIHelper.Primary;
            groupBox10.BackColor = Color.White;

            StyleChartControl(chartDoanhThu);
            UIHelper.StyleGrid(dgvBaoHongMoi, UIHelper.Primary);
        }

        private void StyleChartControl(System.Windows.Forms.DataVisualization.Charting.Chart chart)
        {
            chart.BackColor       = Color.White;
            chart.BorderlineColor = UIHelper.Border;

            if (chart.ChartAreas.Count > 0)
            {
                var ca = chart.ChartAreas[0];
                ca.BackColor                  = Color.White;
                ca.AxisX.MajorGrid.LineColor  = Color.FromArgb(241, 245, 249);
                ca.AxisY.MajorGrid.LineColor  = Color.FromArgb(241, 245, 249);
                ca.AxisX.LineColor            = UIHelper.Border;
                ca.AxisY.LineColor            = UIHelper.Border;
                ca.AxisX.LabelStyle.ForeColor = UIHelper.TextSecond;
                ca.AxisY.LabelStyle.ForeColor = UIHelper.TextSecond;
                ca.AxisX.LabelStyle.Font      = new Font("Segoe UI", 8f);
                ca.AxisY.LabelStyle.Font      = new Font("Segoe UI", 8f);
                ca.AxisY.LabelStyle.Format    = "N0";
            }

            if (chart.Series.Count > 0)
            {
                chart.Series[0].Color       = UIHelper.Primary;
                chart.Series[0].BorderColor = UIHelper.PrimaryHover;
                chart.Series[0].BorderWidth = 1;
            }
        }

        // ── Form Load ─────────────────────────────────────────────────────
        private void Main_Load(object sender, EventArgs e)
        {
            panelMain.Visible      = false;
            panelDashboard.Visible = true;
            panelDashboard.BringToFront();

            LoadBaoHongMoi();
            LoadTongPhong();
            LoadPhongDangThue();
            LoadPhongTrong();
            LoadCuDan();
            LoadDoanhThu();
            LoadCongNo();
            LoadBaoHong();
            LoadHopDongSapHetHan();
            LoadChartDoanhThu();

            loadmaugroup();
            loadchu();
            loadcochu();

            MauHeader();
            StyleMenuButtons();
            StyleDashboardCards();

            lblXinChao.Text = "👋 Xin chào Admin";
            lblTime.Text    = "🕒 " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

            timer1.Interval = 1000;
            timer1.Start();
        }

        // ── Trang Chủ click ───────────────────────────────────────────────
        private void btnTrangChu_Click(object sender, EventArgs e)
        {
            activeForm?.Close();
            activeForm = null;

            panelMain.Controls.Clear();
            panelMain.Visible = false;

            panelDashboard.Visible = true;
            panelDashboard.BringToFront();

            ResetMenuColor();
            btnTrangChu.BackColor = UIHelper.Primary;
            btnTrangChu.ForeColor = Color.White;
            hideSubMenu();
        }

        // ── Timer tick ────────────────────────────────────────────────────
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }

        // ── Menu click handlers ───────────────────────────────────────────
        private void btnDanhMuc_Click(object sender, EventArgs e)  { hideSubMenu(); pnlDanhMuc.Visible  = true; ActiveMenu(btnDanhMuc); }
        private void btnHopDong_Click(object sender, EventArgs e)  { hideSubMenu(); pnlHopDong.Visible  = true; ActiveMenu(btnHopDong); }
        private void btnHoaDon_Click(object sender, EventArgs e)   { hideSubMenu(); pnlHoaDon1.Visible  = true; ActiveMenu(btnHoaDon); }
        private void btnCongViec_Click(object sender, EventArgs e) { hideSubMenu(); pnlCongViec.Visible = true; ActiveMenu(btnCongViec); }
        private void btnBaoCao_Click(object sender, EventArgs e)   { hideSubMenu(); pnlBaoCao.Visible   = true; ActiveMenu(btnBaoCao); }
        private void btnHeThong_Click(object sender, EventArgs e)  { hideSubMenu(); pnlHeThong.Visible  = true; ActiveMenu(btnHeThong); }

        // ── Sub-menu item clicks ──────────────────────────────────────────
        private void btntoanha_Click(object sender, EventArgs e)        => OpenChildForm(new ToaNha());
        private void btnloaiphong_Click(object sender, EventArgs e)     => OpenChildForm(new LoaiPhong());
        private void btnphong_Click(object sender, EventArgs e)         => OpenChildForm(new Phong());
        private void btntaisan_Click(object sender, EventArgs e)        => OpenChildForm(new TaiSan());
        private void btncudan_Click(object sender, EventArgs e)         => OpenChildForm(new CuDan());
        private void btndichvu_Click(object sender, EventArgs e)        => OpenChildForm(new DanhMucDichVu());
        private void btntaikhoan_Click(object sender, EventArgs e)      => OpenChildForm(new TaiKhoan());
        private void btnlaphopdong_Click(object sender, EventArgs e)    => OpenChildForm(new LapHopDong());
        private void btndanhsachhopdong_Click(object sender, EventArgs e) => OpenChildForm(new DanhSachHopDong());
        private void btnlaphoadon_Click(object sender, EventArgs e)     => OpenChildForm(new LapHoaDon());
        private void btnphieuthuchi_Click(object sender, EventArgs e)   => OpenChildForm(new PhieuThuChi());
        private void btnsoquy_Click(object sender, EventArgs e)         => OpenChildForm(new SoQuy());
        private void btnqlihoadon_Click(object sender, EventArgs e)     => OpenChildForm(new QuanLyHoaDon());
        private void btnbaohong_Click(object sender, EventArgs e)       => OpenChildForm(new FormQuanLyBaoHong());

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
