using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace QLNTTT
{
    /// <summary>
    /// ===== [MỚI] UIHelper – Bộ style chuẩn toàn ứng dụng =====
    /// Palette sáng, chuyên nghiệp: White / Slate / Blue
    /// </summary>
    public static class UIHelper
    {
        // ── Màu chính ──────────────────────────────────────────────
        public static readonly Color Primary     = Color.FromArgb(37, 99, 235);   // #2563EB xanh chính
        public static readonly Color PrimaryHover= Color.FromArgb(29, 78, 216);   // #1D4ED8 hover
        public static readonly Color PrimaryLight= Color.FromArgb(219, 234, 254); // #DBEAFE xanh nhạt
        public static readonly Color Success     = Color.FromArgb(22, 163, 74);   // #16A34A xanh lá
        public static readonly Color SuccessLight= Color.FromArgb(220, 252, 231); // nhạt
        public static readonly Color Warning     = Color.FromArgb(217, 119, 6);   // #D97706 vàng
        public static readonly Color WarningLight= Color.FromArgb(254, 243, 199); // nhạt
        public static readonly Color Danger      = Color.FromArgb(220, 38, 38);   // #DC2626 đỏ
        public static readonly Color DangerLight = Color.FromArgb(254, 226, 226); // nhạt
        public static readonly Color Purple      = Color.FromArgb(124, 58, 237);  // tím
        public static readonly Color PurpleLight = Color.FromArgb(237, 233, 254); // nhạt
        public static readonly Color Teal        = Color.FromArgb(13, 148, 136);  // xanh teal
        public static readonly Color TealLight   = Color.FromArgb(204, 251, 241); // nhạt

        // ── Màu nền & viền ──────────────────────────────────────────
        public static readonly Color BgPage      = Color.FromArgb(248, 250, 252); // #F8FAFC nền trang
        public static readonly Color BgCard      = Color.White;                    // nền card
        public static readonly Color BgSidebar   = Color.FromArgb(15, 23, 42);    // sidebar tối
        public static readonly Color BgHeader    = Color.White;                    // header trắng
        public static readonly Color Border      = Color.FromArgb(226, 232, 240); // #E2E8F0
        public static readonly Color BorderFocus = Color.FromArgb(37, 99, 235);

        // ── Màu chữ ─────────────────────────────────────────────────
        public static readonly Color TextPrimary = Color.FromArgb(15, 23, 42);    // #0F172A
        public static readonly Color TextSecond  = Color.FromArgb(71, 85, 105);   // #475569
        public static readonly Color TextMuted   = Color.FromArgb(148, 163, 184); // #94A3B8
        public static readonly Color TextWhite   = Color.White;

        // ── Font ────────────────────────────────────────────────────
        public static Font FontH1  => new Font("Segoe UI", 18, FontStyle.Bold);
        public static Font FontH2  => new Font("Segoe UI", 14, FontStyle.Bold);
        public static Font FontH3  => new Font("Segoe UI", 11, FontStyle.Bold);
        public static Font FontBody=> new Font("Segoe UI", 10, FontStyle.Regular);
        public static Font FontSm  => new Font("Segoe UI",  9, FontStyle.Regular);
        public static Font FontNum => new Font("Segoe UI", 22, FontStyle.Bold);
        public static Font FontSidebar => new Font("Segoe UI", 10, FontStyle.Regular);

        // ══════════════════════════════════════════════════════════
        // STYLE METHODS
        // ══════════════════════════════════════════════════════════

        /// <summary>Style DataGridView chuyên nghiệp sáng</summary>
        public static void StyleGrid(DataGridView dgv, Color headerBg = default)
        {
            if (headerBg == default) headerBg = Primary;

            dgv.BorderStyle               = BorderStyle.None;
            dgv.BackgroundColor           = BgCard;
            dgv.GridColor                 = Border;
            dgv.RowHeadersVisible         = false;
            dgv.SelectionMode             = DataGridViewSelectionMode.FullRowSelect;
            dgv.AllowUserToAddRows        = false;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AutoSizeColumnsMode       = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.CellBorderStyle           = DataGridViewCellBorderStyle.SingleHorizontal;

            // Header
            dgv.ColumnHeadersDefaultCellStyle.BackColor  = headerBg;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor  = TextWhite;
            dgv.ColumnHeadersDefaultCellStyle.Font       = new Font("Segoe UI", 9, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Padding    = new Padding(8, 0, 0, 0);
            dgv.ColumnHeadersHeight                      = 38;
            dgv.ColumnHeadersBorderStyle                 = DataGridViewHeaderBorderStyle.None;

            // Row
            dgv.DefaultCellStyle.BackColor  = BgCard;
            dgv.DefaultCellStyle.ForeColor  = TextPrimary;
            dgv.DefaultCellStyle.Font       = FontBody;
            dgv.DefaultCellStyle.SelectionBackColor = PrimaryLight;
            dgv.DefaultCellStyle.SelectionForeColor = Primary;
            dgv.DefaultCellStyle.Padding    = new Padding(6, 4, 6, 4);

            // Alternate row
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
            dgv.AlternatingRowsDefaultCellStyle.SelectionBackColor = PrimaryLight;
            dgv.AlternatingRowsDefaultCellStyle.SelectionForeColor = Primary;

            dgv.RowTemplate.Height = 36;
        }

        /// <summary>Style Button chính (filled)</summary>
        public static void StyleButtonPrimary(Button btn)
        {
            btn.FlatStyle                     = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize     = 0;
            btn.BackColor                     = Primary;
            btn.ForeColor                     = TextWhite;
            btn.Font                          = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor                        = Cursors.Hand;
            btn.Height                        = 36;
            btn.MouseEnter += (s, e) => ((Button)s).BackColor = PrimaryHover;
            btn.MouseLeave += (s, e) => ((Button)s).BackColor = Primary;
        }

        /// <summary>Style Button secondary (outline)</summary>
        public static void StyleButtonSecondary(Button btn)
        {
            btn.FlatStyle                       = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize       = 1;
            btn.FlatAppearance.BorderColor      = Border;
            btn.BackColor                       = BgCard;
            btn.ForeColor                       = TextSecond;
            btn.Font                            = new Font("Segoe UI", 10, FontStyle.Regular);
            btn.Cursor                          = Cursors.Hand;
            btn.Height                          = 36;
            btn.MouseEnter += (s, e) => { ((Button)s).BackColor = BgPage; };
            btn.MouseLeave += (s, e) => { ((Button)s).BackColor = BgCard; };
        }

        /// <summary>Style TextBox / ComboBox</summary>
        public static void StyleInput(Control ctrl)
        {
            ctrl.Font      = FontBody;
            ctrl.ForeColor = TextPrimary;
            ctrl.BackColor = BgCard;
            if (ctrl is TextBox tb) { tb.BorderStyle = BorderStyle.FixedSingle; }
        }

        /// <summary>Style Label tiêu đề section</summary>
        public static void StyleLabelSection(Label lbl)
        {
            lbl.Font      = FontH3;
            lbl.ForeColor = TextPrimary;
        }

        /// <summary>Style Card panel (viền nhẹ)</summary>
        public static void StyleCard(Panel pnl)
        {
            pnl.BackColor   = BgCard;
            pnl.BorderStyle = BorderStyle.FixedSingle;
        }

        /// <summary>Style Sidebar Button</summary>
        public static void StyleSidebarButton(Button btn, bool active = false)
        {
            btn.FlatStyle                 = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.BackColor                 = active
                ? Color.FromArgb(37, 99, 235)
                : Color.FromArgb(30, 41, 59);
            btn.ForeColor = TextWhite;
            btn.Font      = FontSidebar;
            btn.Cursor    = Cursors.Hand;
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Padding   = new Padding(16, 0, 0, 0);

            btn.MouseEnter += (s, e) =>
            {
                if (((Button)s).BackColor != Primary)
                    ((Button)s).BackColor = Color.FromArgb(51, 65, 85);
            };
            btn.MouseLeave += (s, e) =>
            {
                if (((Button)s).BackColor != Primary)
                    ((Button)s).BackColor = Color.FromArgb(30, 41, 59);
            };
        }

        /// <summary>Tạo stat card Panel đẹp</summary>
        public static Panel CreateStatCard(string title, string value,
            Color accentColor, Color lightColor, string icon)
        {
            var card = new Panel
            {
                Width     = 180,
                Height    = 100,
                BackColor = lightColor,
                Cursor    = Cursors.Default
            };

            // Icon label
            var lblIcon = new Label
            {
                Text      = icon,
                Font      = new Font("Segoe UI", 20),
                ForeColor = accentColor,
                AutoSize  = true,
                Location  = new Point(12, 12)
            };

            // Title
            var lblTitle = new Label
            {
                Text      = title.ToUpper(),
                Font      = new Font("Segoe UI", 7, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize  = true,
                Location  = new Point(50, 14)
            };

            // Value
            var lblValue = new Label
            {
                Text      = value,
                Font      = new Font("Segoe UI", 20, FontStyle.Bold),
                ForeColor = accentColor,
                AutoSize  = true,
                Location  = new Point(12, 52)
            };

            card.Controls.Add(lblIcon);
            card.Controls.Add(lblTitle);
            card.Controls.Add(lblValue);
            return card;
        }

        /// <summary>Tô màu xen kẽ rows DataGridView theo sự kiện</summary>
        public static void AttachAlternateRowColor(DataGridView dgv)
        {
            dgv.RowPrePaint += (s, e) =>
            {
                var row = ((DataGridView)s).Rows[e.RowIndex];
                if ((e.RowIndex % 2) == 0)
                    row.DefaultCellStyle.BackColor = BgCard;
                else
                    row.DefaultCellStyle.BackColor = BgPage;
            };
        }

        /// <summary>Tự động style tất cả control của một Form</summary>
        public static void StyleForm(Form frm)
        {
            frm.BackColor = BgPage;
            frm.Font      = FontBody;
            StyleControlsRecursive(frm.Controls);
        }

        private static void StyleControlsRecursive(Control.ControlCollection controls)
        {
            foreach (Control c in controls)
            {
                if (c is DataGridView dgv)
                {
                    StyleGrid(dgv);
                }
                else if (c is Button btn)
                {
                    string text = btn.Text.ToLower().Trim();

                    // Primary action buttons
                    if (text.Contains("thêm")   || text.Contains("lưu")   ||
                        text.Contains("ghi")    || text.Contains("xác nhận") ||
                        text.Contains("tạo")    || text.Contains("lập")   ||
                        text.Contains("cập nhật") || text.Contains("in")  ||
                        text.Contains("xuất")   || text.Contains("tìm")   ||
                        text.Contains("tìm kiếm") || text.Contains("thanh toán"))
                    {
                        StyleButtonPrimary(btn);
                    }
                    // Danger action buttons
                    else if (text.Contains("xóa") || text.Contains("hủy") || text.Contains("thoát"))
                    {
                        btn.FlatStyle                     = FlatStyle.Flat;
                        btn.FlatAppearance.BorderSize     = 1;
                        btn.FlatAppearance.BorderColor    = Color.FromArgb(252, 165, 165);
                        btn.BackColor                     = Color.FromArgb(254, 242, 242);
                        btn.ForeColor                     = Danger;
                        btn.Font                          = new Font("Segoe UI", 9.5f, FontStyle.Regular);
                        btn.Cursor                        = Cursors.Hand;
                        btn.Height                        = 34;
                        btn.MouseEnter += (s, ev) => { ((Button)s).BackColor = DangerLight; };
                        btn.MouseLeave += (s, ev) => { ((Button)s).BackColor = Color.FromArgb(254, 242, 242); };
                    }
                    // Secondary/neutral buttons
                    else
                    {
                        StyleButtonSecondary(btn);
                    }
                }
                else if (c is TextBox tb)
                {
                    tb.Font        = FontBody;
                    tb.ForeColor   = TextPrimary;
                    tb.BackColor   = BgCard;
                    tb.BorderStyle = BorderStyle.FixedSingle;
                }
                else if (c is ComboBox cb)
                {
                    cb.Font      = FontBody;
                    cb.ForeColor = TextPrimary;
                    cb.BackColor = BgCard;
                    cb.FlatStyle = FlatStyle.Flat;
                }
                else if (c is NumericUpDown nud)
                {
                    nud.Font      = FontBody;
                    nud.ForeColor = TextPrimary;
                    nud.BackColor = BgCard;
                }
                else if (c is DateTimePicker dtp)
                {
                    dtp.Font      = FontBody;
                    dtp.ForeColor = TextPrimary;
                    dtp.CalendarForeColor = TextPrimary;
                }
                else if (c is Label lbl)
                {
                    // Giữ nguyên AutoSize, chỉ điều chỉnh font và màu
                    float sz = lbl.Font.Size;
                    bool bold = lbl.Font.Bold;

                    if (sz >= 12 || bold)
                    {
                        lbl.Font      = new Font("Segoe UI", Math.Max(sz, 10f), FontStyle.Bold);
                        lbl.ForeColor = TextPrimary;
                    }
                    else
                    {
                        lbl.Font      = FontBody;
                        lbl.ForeColor = TextSecond;
                    }
                }
                else if (c is GroupBox gb)
                {
                    gb.Font      = new Font("Segoe UI", 9f, FontStyle.Bold);
                    gb.ForeColor = Primary;
                    gb.BackColor = BgCard;
                    StyleControlsRecursive(gb.Controls);
                }
                else if (c is Panel pnl)
                {
                    // Chỉ style panel nếu nó chưa có màu đặc biệt
                    if (pnl.BackColor == SystemColors.Control)
                        pnl.BackColor = BgPage;
                    StyleControlsRecursive(pnl.Controls);
                }
                else if (c is TabControl tc)
                {
                    tc.Font = FontBody;
                    foreach (TabPage tp in tc.TabPages)
                    {
                        tp.BackColor = BgPage;
                        tp.Font      = FontBody;
                        StyleControlsRecursive(tp.Controls);
                    }
                }
                else if (c is SplitContainer sc)
                {
                    sc.BackColor = BgPage;
                    StyleControlsRecursive(sc.Panel1.Controls);
                    StyleControlsRecursive(sc.Panel2.Controls);
                }
                else if (c.Controls.Count > 0)
                {
                    StyleControlsRecursive(c.Controls);
                }
            }
        }
    }
}
