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
    public partial class DanhSachHopDong : Form
    {
        public DanhSachHopDong()
        {
            InitializeComponent();
            this.Load += DanhSachHopDong_Load;
        }
        HopDong_BUS hopDong_BUS =new HopDong_BUS();
        void LoadGrid()
        {
            try
            {
                dgvHopDong.DataSource =
                    hopDong_BUS.GetAll();
                // =====================
                // ẨN CÁC CỘT LIÊN KẾT
                // =====================

                string[] hiddenCols =
                {
    "HopDong_CuDan",
    "HopDong_DichVu",
    "HopDong_TaiSan",
    "HoaDons",
    "PhuLucHopDongs"
};

                foreach (string col in hiddenCols)
                {
                    if (dgvHopDong.Columns[col] != null)
                        dgvHopDong.Columns[col].Visible = false;
                }
                // =====================
                // CHECK COLUMN
                // =====================

                if (dgvHopDong.Columns["MaHopDong"] != null)
                    dgvHopDong.Columns["MaHopDong"]
                        .HeaderText = "Mã HĐ";

                if (dgvHopDong.Columns["MaPhong"] != null)
                    dgvHopDong.Columns["MaPhong"]
                        .HeaderText = "Phòng";

                if (dgvHopDong.Columns["NgayBatDau"] != null)
                    dgvHopDong.Columns["NgayBatDau"]
                        .HeaderText = "Ngày bắt đầu";

                if (dgvHopDong.Columns["NgayKetThuc"] != null)
                    dgvHopDong.Columns["NgayKetThuc"]
                        .HeaderText = "Ngày kết thúc";

                if (dgvHopDong.Columns["GiaThue"] != null)
                    dgvHopDong.Columns["GiaThue"]
                        .HeaderText = "Giá thuê";

                if (dgvHopDong.Columns["GiaDienChot"] != null)
                    dgvHopDong.Columns["GiaDienChot"]
                        .HeaderText = "Giá điện chốt";

                if (dgvHopDong.Columns["GiaNuocChot"] != null)
                    dgvHopDong.Columns["GiaNuocChot"]
                        .HeaderText = "Giá nước chốt";

                if (dgvHopDong.Columns["NgayTao"] != null)
                    dgvHopDong.Columns["NgayTao"]
                        .HeaderText = "Ngày tạo";

                if (dgvHopDong.Columns["GhiChu"] != null)
                    dgvHopDong.Columns["GhiChu"]
                        .HeaderText = "Ghi chú";

                if (dgvHopDong.Columns["TienCoc"] != null)
                    dgvHopDong.Columns["TienCoc"]
                        .HeaderText = "Tiền cọc";

                if (dgvHopDong.Columns["TrangThai"] != null)
                    dgvHopDong.Columns["TrangThai"]
                        .HeaderText = "Trạng thái";

                if (dgvHopDong.Columns["MaNguoiDaiDien"] != null)
                    dgvHopDong.Columns["MaNguoiDaiDien"]
                        .HeaderText = "Người đại diện";

                // =====================
                // FORMAT
                // =====================

                dgvHopDong.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvHopDong.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvHopDong.ReadOnly = true;

                dgvHopDong.AllowUserToAddRows = false;
                dgvHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvHopDong.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
                dgvHopDong.RowHeadersVisible = false;
                dgvHopDong.AllowUserToResizeRows = false;

                // =====================
                // FORMAT TIỀN
                // =====================

                if (dgvHopDong.Columns["GiaThue"] != null)
                    dgvHopDong.Columns["GiaThue"]
                        .DefaultCellStyle.Format = "N0";

                if (dgvHopDong.Columns["TienCoc"] != null)
                    dgvHopDong.Columns["TienCoc"]
                        .DefaultCellStyle.Format = "N0";

                // =====================
                // FORMAT DATE
                // =====================

                if (dgvHopDong.Columns["NgayBatDau"] != null)
                    dgvHopDong.Columns["NgayBatDau"]
                        .DefaultCellStyle.Format = "dd/MM/yyyy";

                if (dgvHopDong.Columns["NgayKetThuc"] != null)
                    dgvHopDong.Columns["NgayKetThuc"]
                        .DefaultCellStyle.Format = "dd/MM/yyyy";
                if (dgvHopDong.Columns["GiaThue"] != null)
                    dgvHopDong.Columns["GiaThue"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;

                if (dgvHopDong.Columns["TienCoc"] != null)
                    dgvHopDong.Columns["TienCoc"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleRight;

                if (dgvHopDong.Columns["NgayBatDau"] != null)
                    dgvHopDong.Columns["NgayBatDau"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;

                if (dgvHopDong.Columns["NgayKetThuc"] != null)
                    dgvHopDong.Columns["NgayKetThuc"].DefaultCellStyle.Alignment =
                        DataGridViewContentAlignment.MiddleCenter;
                dgvHopDong.ColumnHeadersHeight = 35;
                dgvHopDong.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgvHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvHopDong.ScrollBars = ScrollBars.Both;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void MauTrangThai()
        {
            foreach (DataGridViewRow row in dgvHopDong.Rows)
            {
                if (row.Cells["TrangThai"].Value == null)
                    continue;

                string tt = row.Cells["TrangThai"].Value.ToString();

                if (tt == "Đang hiệu lực")
                    row.DefaultCellStyle.BackColor = Color.FromArgb(220, 252, 231); // xanh

                else if (tt == "Đã kết thúc")
                    row.DefaultCellStyle.BackColor = Color.FromArgb(254, 243, 199); // vàng

                else if (tt == "Đã thanh lý")
                    row.DefaultCellStyle.BackColor = Color.FromArgb(254, 226, 226); // đỏ
            }
        }
        private void DanhSachHopDong_Load(object sender, EventArgs e)
        {
            LoadGrid();
            StyleGrid();
            MauTrangThai();
        }
        private void StyleGrid()
        {
            dgvHopDong.BorderStyle = BorderStyle.None;
            dgvHopDong.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);

            dgvHopDong.DefaultCellStyle.SelectionBackColor = Color.FromArgb(59, 130, 246);
            dgvHopDong.DefaultCellStyle.SelectionForeColor = Color.White;

            dgvHopDong.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(30, 64, 175);
            dgvHopDong.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvHopDong.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 10, FontStyle.Bold);

            dgvHopDong.EnableHeadersVisualStyles = false;
            dgvHopDong.RowTemplate.Height = 30;

            dgvHopDong.GridColor = Color.FromArgb(230, 230, 230);
        }
        private void dgvHopDong_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                string maHD =
                    dgvHopDong.Rows[e.RowIndex]
                    .Cells["MaHopDong"]
                    .Value.ToString();
                MessageBox.Show(maHD);

                ChiTietHopDong f = new ChiTietHopDong(maHD);

                f.ShowDialog();

                // reload sau khi sửa

                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHopDong.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Chọn hợp đồng");

                    return;
                }

                string maHD =
                    dgvHopDong.CurrentRow
                    .Cells["MaHopDong"]
                    .Value.ToString();

                ChiTietHopDong f =
                    new ChiTietHopDong(maHD);

                f.ShowDialog();

                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            LapHopDong f =
       new LapHopDong();

            f.ShowDialog();

            LoadGrid();
        }
    }
}
