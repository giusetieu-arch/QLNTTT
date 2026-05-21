using System;
using System.Drawing;
using System.Windows.Forms;

namespace QLNTTT
{
    public partial class TrangChu : Form
    {
        // Panel chính
        private Panel sidebar;
        private Panel header;
        private Panel desktop;

        // Form hiện tại
        private Form currentForm = null;

        public TrangChu()
        {
            InitializeComponent();
            SetupUI();
        }

        // ===== [MỚI] SetupUI dùng UIHelper – light professional theme =====
        private void SetupUI()
        {
            // FORM
            this.Text            = "VIETLISH HOMES – QUẢN LÝ NHÀ TRỌ THÔNG MINH";
            this.WindowState     = FormWindowState.Maximized;
            this.StartPosition   = FormStartPosition.CenterScreen;
            this.BackColor       = UIHelper.BgPage;
            this.IsMdiContainer  = false;
            this.Font            = UIHelper.FontBody;

            // ── HEADER ───────────────────────────────────────────
            header = new Panel();
            header.Dock      = DockStyle.Top;
            header.Height    = 64;
            header.BackColor = UIHelper.BgHeader;
            // Đường viền dưới header
            header.Paint += (s, e) =>
            {
                var p = (Panel)s;
                using (var pen = new System.Drawing.Pen(UIHelper.Border, 1))
                {
                    e.Graphics.DrawLine(pen, 0, p.Height - 1, p.Width, p.Height - 1);
                }
            };
            this.Controls.Add(header);

            // Logo + Tên
            var pnlBrand = new Panel { AutoSize = true, Location = new Point(20, 10) };
            var lblLogo  = new Label
            {
                Text = "🏠",
                Font = new Font("Segoe UI", 20),
                AutoSize = true,
                Location = new Point(0, 2)
            };
            var lblTitle = new Label
            {
                Text      = "VIETLISH HOMES",
                Font      = new Font("Segoe UI", 15, FontStyle.Bold),
                ForeColor = UIHelper.Primary,
                AutoSize  = true,
                Location  = new Point(38, 4)
            };
            var lblSub = new Label
            {
                Text      = "Hệ thống Quản lý Nhà trọ chuyên nghiệp",
                Font      = new Font("Segoe UI", 9),
                ForeColor = UIHelper.TextMuted,
                AutoSize  = true,
                Location  = new Point(40, 30)
            };
            header.Controls.Add(lblLogo);
            header.Controls.Add(lblTitle);
            header.Controls.Add(lblSub);

            // Đồng hồ bên phải header
            var lblClock = new Label
            {
                Name      = "lblClock",
                Text      = DateTime.Now.ToString("HH:mm:ss"),
                Font      = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = UIHelper.TextSecond,
                AutoSize  = true
            };
            header.Controls.Add(lblClock);
            header.Resize += (s, e) =>
            {
                lblClock.Location = new Point(header.Width - lblClock.Width - 20, (header.Height - lblClock.Height) / 2);
            };

            var timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += (s, e) => lblClock.Text = DateTime.Now.ToString("dd/MM/yyyy  HH:mm:ss");
            timer.Start();

            // ── SIDEBAR ───────────────────────────────────────────
            sidebar = new Panel();
            sidebar.Dock      = DockStyle.Left;
            sidebar.Width     = 230;
            sidebar.BackColor = UIHelper.BgSidebar;
            this.Controls.Add(sidebar);

            // Tên sidebar
            var lblSidebarTitle = new Label
            {
                Text      = "MENU CHÍNH",
                Font      = new Font("Segoe UI", 8, FontStyle.Bold),
                ForeColor = Color.FromArgb(71, 85, 105),
                AutoSize  = true,
                Location  = new Point(16, 18)
            };
            sidebar.Controls.Add(lblSidebarTitle);

            // ── DESKTOP ───────────────────────────────────────────
            desktop = new Panel();
            desktop.Dock      = DockStyle.Fill;
            desktop.BackColor = UIHelper.BgPage;
            this.Controls.Add(desktop);
            desktop.BringToFront();

            // ── MENU BUTTONS ─────────────────────────────────────
            int startY = 48;
            int gap    = 48;
            AddMenuButton("🏙️  Tòa nhà",          startY + gap * 0,  cậpNhậtToàNhàToolStripMenuItem_Click);
            AddMenuButton("🏠  Loại phòng",        startY + gap * 1,  cậpNhậtLoạiPhòngToolStripMenuItem_Click);
            AddMenuButton("🔑  Quản lý phòng",     startY + gap * 2,  cậpNhậtPhòngToolStripMenuItem_Click);
            AddMenuButton("📦  Tài sản",           startY + gap * 3,  cậpNhậtTàiSảnToolStripMenuItem_Click);
            AddMenuButton("👥  Cư dân",            startY + gap * 4,  cậpNhậtCưDânToolStripMenuItem_Click);
            AddMenuButton("⚙️  Dịch vụ",           startY + gap * 5,  cậpNhậtDịchVụToolStripMenuItem_Click);

            // Separator
            var sep = new Panel { Left = 12, Top = startY + gap * 6 + 2, Width = 206, Height = 1, BackColor = Color.FromArgb(51,65,85) };
            sidebar.Controls.Add(sep);

            AddMenuButton("📝  Lập hợp đồng",     startY + gap * 6 + 14, lậpHợpĐồngToolStripMenuItem_Click);
            AddMenuButton("📋  Danh sách HĐ",     startY + gap * 7 + 14, chiTiếtHợpĐồngToolStripMenuItem_Click);
            AddMenuButton("🧾  Lập hóa đơn",      startY + gap * 8 + 14, lậpHoáĐơnToolStripMenuItem_Click);
            AddMenuButton("👤  Tài khoản CĐ",     startY + gap * 9 + 14, tàiKhoảnCưDânToolStripMenuItem_Click);
            AddMenuButton("🔧  Báo hỏng",         startY + gap * 10 + 14, quảnLíBáoHỏngToolStripMenuItem_Click);
        }
        // ===== [KẾT THÚC MỚI] =====

        // ===== [MỚI] AddMenuButton – Sidebar button chuyên nghiệp =====
        private Button _activeMenuBtn = null;
        private void AddMenuButton(string text, int top, EventHandler clickEvent)
        {
            var btn = new Button
            {
                Text      = text,
                Width     = 210,
                Height    = 44,
                Left      = 10,
                Top       = top,
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(30, 41, 59),
                ForeColor = Color.FromArgb(203, 213, 225),
                Font      = new Font("Segoe UI", 10, FontStyle.Regular),
                Cursor    = Cursors.Hand,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding   = new Padding(12, 0, 0, 0),
                Tag       = text
            };
            btn.FlatAppearance.BorderSize  = 0;
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(51, 65, 85);

            btn.MouseEnter += (s, e) =>
            {
                if ((Button)s != _activeMenuBtn)
                    ((Button)s).BackColor = Color.FromArgb(51, 65, 85);
            };
            btn.MouseLeave += (s, e) =>
            {
                if ((Button)s != _activeMenuBtn)
                    ((Button)s).BackColor = Color.FromArgb(30, 41, 59);
            };
            btn.Click += (s, e) =>
            {
                // Reset all
                foreach (Control c in sidebar.Controls)
                    if (c is Button b && b != _activeMenuBtn)
                        b.BackColor = Color.FromArgb(30, 41, 59);

                _activeMenuBtn            = (Button)s;
                _activeMenuBtn.BackColor  = UIHelper.Primary;
                _activeMenuBtn.ForeColor  = Color.White;
                clickEvent?.Invoke(s, e);
            };

            sidebar.Controls.Add(btn);
        }
        // ===== [KẾT THÚC MỚI] =====


        // ==========================
        // MỞ FORM CON
        // ==========================
        private void OpenChildForm(Form childForm)
        {
            // đóng form cũ
            if (currentForm != null)
            {
                currentForm.Close();
            }

            currentForm = childForm;

            // setup form con
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            desktop.Controls.Clear();
            desktop.Controls.Add(childForm);

            childForm.BringToFront();
            childForm.Show();
        }

        // ==========================
        // EVENT BUTTON
        // ==========================

        private void cậpNhậtToàNhàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new ToaNha());
        }

        private void cậpNhậtLoạiPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new LoaiPhong());
        }

        private void cậpNhậtPhòngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new Phong());
        }

        private void cậpNhậtTàiSảnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TaiSan());
        }

        private void cậpNhậtCưDânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new CuDan());
        }

        private void cậpNhậtDịchVụToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DanhMucDichVu());
        }

        private void lậpHợpĐồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new LapHopDong());
        }

        private void chiTiếtHợpĐồngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new DanhSachHopDong());
        }

        private void lậpHoáĐơnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new LapHoaDon());
        }

        private void tàiKhoảnCưDânToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new TaiKhoan());
        }

        private void quảnLíBáoHỏngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new FormQuanLyBaoHong());
        }

        private void phiếuThuChiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new PhieuThuChi());
        }

        private void sổQuỹToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenChildForm(new SoQuy());
        }

        private void quảnLíCôngViệcToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}