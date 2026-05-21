using BUS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLNTTT
{
    public partial class SoQuy : Form
    {
        public SoQuy()
        {
            InitializeComponent();
        }
        SoQuy_BUS bus = new SoQuy_BUS();

        void LoadData()
        {
            var ds = bus.GetAll1();

            dgvSoQuy.DataSource = ds;

            // =====================
            // TIÊU ĐỀ CỘT
            // =====================

            dgvSoQuy.Columns["NgayGiaoDich"].HeaderText = "Ngày";
            dgvSoQuy.Columns["LoaiGiaoDich"].HeaderText = "Loại GD";
            dgvSoQuy.Columns["Thu"].HeaderText = "Thu";
            dgvSoQuy.Columns["Chi"].HeaderText = "Chi";
            dgvSoQuy.Columns["SoDuSauGD"].HeaderText = "Số dư";
            dgvSoQuy.Columns["NoiDung"].HeaderText = "Nội dung";
            dgvSoQuy.Columns["NguoiLap"].HeaderText = "Người lập";

            // Ẩn mã phiếu nếu không cần hiển thị
            dgvSoQuy.Columns["MaPhieu"].Visible = false;

            // =====================
            // FORMAT TIỀN
            // =====================

            dgvSoQuy.Columns["Thu"].DefaultCellStyle.Format = "N0";
            dgvSoQuy.Columns["Chi"].DefaultCellStyle.Format = "N0";
            dgvSoQuy.Columns["SoDuSauGD"].DefaultCellStyle.Format = "N0";

            // Căn phải số tiền
            dgvSoQuy.Columns["Thu"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            dgvSoQuy.Columns["Chi"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            dgvSoQuy.Columns["SoDuSauGD"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            // Format ngày
            dgvSoQuy.Columns["NgayGiaoDich"].DefaultCellStyle.Format =
                "dd/MM/yyyy";

            dgvSoQuy.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            // =====================
            // THỐNG KÊ
            // =====================

            decimal tongThu = ds.Sum(x => x.Thu ?? 0);
            decimal tongChi = ds.Sum(x => x.Chi ?? 0);
            decimal soDu = tongThu - tongChi;

            lblTongThu.Text = "Tổng thu: " + tongThu.ToString("N0") + " VNĐ";

            lblTongChi.Text = "Tổng chi: " + tongChi.ToString("N0") + " VNĐ";

            lblSoDu.Text = "Số dư hiện tại: " + soDu.ToString("N0") + " VNĐ";
            dgvSoQuy.Columns["Thu"].DefaultCellStyle.ForeColor = Color.Blue;
            dgvSoQuy.Columns["Chi"].DefaultCellStyle.ForeColor = Color.Red;
            dgvSoQuy.ReadOnly = true;
            dgvSoQuy.AllowUserToAddRows = false;
            dgvSoQuy.SelectionMode =
    DataGridViewSelectionMode.FullRowSelect;
            dgvSoQuy.AutoGenerateColumns = true;
        }
        private void SoQuy_Load(object sender, EventArgs e)
        {
            
            cbbLoai.Items.Add("Tất cả");
            cbbLoai.Items.Add("Thu");
            cbbLoai.Items.Add("Chi");

            cbbLoai.SelectedIndex = 0;

            dtTuNgay.Value =
                DateTime.Now.AddMonths(-1);

            dtDenNgay.Value =
                DateTime.Now;

            LoadData();
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            var ds = bus.GetAll1();

            // =====================
            // LỌC THEO NGÀY
            // =====================

            DateTime tuNgay = dtTuNgay.Value.Date;

            // lấy hết ngày đến
            DateTime denNgay = dtDenNgay.Value.Date.AddDays(1).AddSeconds(-1);

            ds = ds.Where(x =>
                    x.NgayGiaoDich >= tuNgay
                    && x.NgayGiaoDich <= denNgay)
                .ToList();

            // =====================
            // LỌC THEO LOẠI
            // =====================

            if (cbbLoai.Text != "Tất cả")
            {
                if (cbbLoai.Text == "Thu")
                {
                    ds = ds.Where(x => (x.Thu ?? 0) > 0)
                           .ToList();
                }
                else if (cbbLoai.Text == "Chi")
                {
                    ds = ds.Where(x => (x.Chi ?? 0) > 0)
                           .ToList();
                }
            }

            // =====================
            // HIỂN THỊ
            // =====================

            dgvSoQuy.DataSource = ds;

            // =====================
            // THỐNG KÊ
            // =====================

            decimal tongThu = ds.Sum(x => x.Thu ?? 0);
            decimal tongChi = ds.Sum(x => x.Chi ?? 0);
            decimal soDu = tongThu - tongChi;

            lblTongThu.Text = $"Tổng thu: {tongThu:N0} VNĐ";
            lblTongChi.Text = $"Tổng chi: {tongChi:N0} VNĐ";
            lblSoDu.Text = $"Số dư: {soDu:N0} VNĐ";
        }
        void ResetBoLoc()
        {
            cbbLoai.SelectedIndex = 0;

            dtTuNgay.Value = DateTime.Now.AddMonths(-1);

            dtDenNgay.Value = DateTime.Now;
        }
        private void btnReload_Click(object sender, EventArgs e)
        {
            // =====================
            // RESET BỘ LỌC
            // =====================

            cbbLoai.SelectedIndex = 0;

            dtTuNgay.Value = DateTime.Now.AddMonths(-1);

            dtDenNgay.Value = DateTime.Now;

            // =====================
            // LOAD LẠI DỮ LIỆU
            // =====================
            ResetBoLoc();
            LoadData();
        }
    }
}
