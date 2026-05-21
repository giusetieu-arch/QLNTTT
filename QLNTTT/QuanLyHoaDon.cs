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
    public partial class QuanLyHoaDon : Form
    {
        public QuanLyHoaDon()
        {
            InitializeComponent();
        }
        HoaDon_BUS hoaDonBUS = new HoaDon_BUS();
        void LoadHoaDon()
        {
            dgvHoaDon.DataSource = hoaDonBUS.GetAll1();

            FormatDGV();
        }
        // =========================
        // ĐỊNH DẠNG DATAGRIDVIEW
        // =========================
        void FormatDGV()
        {
            if (dgvHoaDon.Columns.Count == 0)
                return;

            dgvHoaDon.Columns["MaHoaDon"].HeaderText = "Mã HĐ";
            dgvHoaDon.Columns["MaPhong"].HeaderText = "Phòng";
            dgvHoaDon.Columns["NgayLap"].HeaderText = "Ngày lập";
            dgvHoaDon.Columns["TongTien"].HeaderText = "Tổng tiền";
           // dgvHoaDon.Columns["DaThu"].HeaderText = "Đã thu";
            dgvHoaDon.Columns["ConNo"].HeaderText = "Còn nợ";
            dgvHoaDon.Columns["TrangThai"].HeaderText = "Trạng thái";

            dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Format = "N0";
           // dgvHoaDon.Columns["DaThu"].DefaultCellStyle.Format = "N0";
            dgvHoaDon.Columns["ConNo"].DefaultCellStyle.Format = "N0";

            dgvHoaDon.Columns["TongTien"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            /*dgvHoaDon.Columns["DaThu"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;*/

            dgvHoaDon.Columns["ConNo"].DefaultCellStyle.Alignment =
                DataGridViewContentAlignment.MiddleRight;

            dgvHoaDon.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvHoaDon.ReadOnly = true;
            dgvHoaDon.AllowUserToAddRows = false;
        }

        // =========================
        // TÔ MÀU DÒNG
        // =========================
        private void dgvHoaDon_RowPrePaint(
            object sender,
            DataGridViewRowPrePaintEventArgs e)
        {
            if (dgvHoaDon.Rows[e.RowIndex]
                .Cells["TrangThai"].Value == null)
                return;

            string tt = dgvHoaDon.Rows[e.RowIndex]
                .Cells["TrangThai"]
                .Value.ToString();

            if (tt == "Đã thanh toán")
            {
                dgvHoaDon.Rows[e.RowIndex]
                    .DefaultCellStyle.BackColor =
                    Color.LightGreen;
            }
            else if (tt == "Thanh toán một phần")
            {
                dgvHoaDon.Rows[e.RowIndex]
                    .DefaultCellStyle.BackColor =
                    Color.Khaki;
            }
            else
            {
                dgvHoaDon.Rows[e.RowIndex]
                    .DefaultCellStyle.BackColor =
                    Color.LightPink;
            }
        }

        private void QuanLyHoaDon_Load(object sender, EventArgs e)
        {
            LoadHoaDon();
            dgvHoaDon.SelectionMode =
    DataGridViewSelectionMode.FullRowSelect;

            dgvHoaDon.MultiSelect = false;
            cbbTrangThai.Items.Clear();

            cbbTrangThai.Items.Add("Tất cả");
            cbbTrangThai.Items.Add("Chưa thanh toán");
            cbbTrangThai.Items.Add("Thanh toán một phần");
            cbbTrangThai.Items.Add("Đã thanh toán");

            cbbTrangThai.SelectedIndex = 0;

            dgvHoaDon.RowPrePaint += dgvHoaDon_RowPrePaint;
        }

        private void btnChiTiet_Click(object sender, EventArgs e)
        {
            // Kiểm tra có dữ liệu hay không
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn hóa đơn!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            // Kiểm tra giá trị mã hóa đơn
            if (dgvHoaDon.CurrentRow.Cells["MaHoaDon"].Value == null)
            {
                MessageBox.Show(
                    "Không tìm thấy mã hóa đơn!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            string maHD =
                dgvHoaDon.CurrentRow
                .Cells["MaHoaDon"]
                .Value.ToString();

            ChiTietHoaDon f =
                new ChiTietHoaDon(maHD);

            f.ShowDialog();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            if (dgvHoaDon.CurrentRow == null)
            {
                MessageBox.Show(
                    "Vui lòng chọn hóa đơn!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            if (dgvHoaDon.CurrentRow.Cells["MaHoaDon"].Value == null)
            {
                MessageBox.Show(
                    "Không tìm thấy mã hóa đơn!",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            string maHD =
                dgvHoaDon.CurrentRow
                .Cells["MaHoaDon"]
                .Value.ToString();

            ThuTienHoaDon f =
                new ThuTienHoaDon(maHD);

            f.ShowDialog();

            LoadHoaDon();
        }
    }
}
