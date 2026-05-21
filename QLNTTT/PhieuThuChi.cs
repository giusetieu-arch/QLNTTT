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
    public partial class PhieuThuChi : Form
    {
        public PhieuThuChi()
        {
            InitializeComponent();
        }
        PhieuThuChi_BUS PhieuThuChi_BUS =
            new PhieuThuChi_BUS();
        void LoadData()
        {
            dgvPhieuThuChi.DataSource =
            PhieuThuChi_BUS.GetAll();

            dgvPhieuThuChi.Columns["MaPhieu"]
                .HeaderText = "Mã phiếu";

            dgvPhieuThuChi.Columns["LoaiPhieu"]
                .HeaderText = "Loại";

            dgvPhieuThuChi.Columns["NgayGiaoDich"]
                .HeaderText = "Ngày";

            dgvPhieuThuChi.Columns["SoTien"]
                .HeaderText = "Số tiền";

            dgvPhieuThuChi.Columns["PhuongThuc"]
                .HeaderText = "Phương thức";

            dgvPhieuThuChi.Columns["NoiDung"]
                .HeaderText = "Nội dung";

            dgvPhieuThuChi.Columns["NguoiNopNhan"]
                .HeaderText = "Người nộp/nhận";

            dgvPhieuThuChi.Columns["MaHoaDon"]
                .HeaderText = "Mã HĐ";

            dgvPhieuThuChi.Columns["SoTien"]
                .DefaultCellStyle.Format = "N0";
            dgvPhieuThuChi.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPhieuThuChi.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPhieuThuChi.ReadOnly = true;

        }
        void loadCbb()
        {
            cbbLoaiPhieu.Items.Add("Tất cả");
            cbbLoaiPhieu.Items.Add("Thu");
            cbbLoaiPhieu.Items.Add("Chi");
            cbbLoaiPhieu.SelectedIndex = 0;
        }
        private void PhieuThuChi_Load(object sender, EventArgs e)
        {
            LoadData();
            loadCbb();
            dtTuNgay.Value =
                DateTime.Now.AddMonths(-1);

            dtDenNgay.Value =
                DateTime.Now;
        }

        private void btnTim_Click(object sender, EventArgs e)
        {
            try
            {
                // =====================
                // LOAD DỮ LIỆU
                // =====================

                var ds =
                    PhieuThuChi_BUS
                    .GetAll1();

                // =====================
                // TÌM THEO LOẠI
                // =====================

                string loai =
                    cbbLoaiPhieu.Text;

                if (loai != "Tất cả")
                {
                    ds = ds
                        .Where(x =>
                            x.LoaiPhieu == loai)
                        .ToList();
                }

                // =====================
                // TỪ NGÀY - ĐẾN NGÀY
                // =====================

                DateTime tuNgay =
                    dtTuNgay.Value.Date;

                DateTime denNgay =
                    dtDenNgay.Value.Date;

                ds = ds
                    .Where(x =>
                        x.NgayGiaoDich >= tuNgay
                        &&
                        x.NgayGiaoDich <= denNgay)
                    .ToList();

                // =====================
                // TÌM TEXT
                // =====================

                string tuKhoa =
                    txtTim.Text
                    .Trim()
                    .ToLower();

                if (!string.IsNullOrEmpty(
                    tuKhoa))
                {
                    ds = ds
                        .Where(x =>

                            (x.MaPhieu != null &&
                             x.MaPhieu
                             .ToLower()
                             .Contains(tuKhoa))

                             ||

                            (x.MaHoaDon != null &&
                             x.MaHoaDon
                             .ToLower()
                             .Contains(tuKhoa))

                             ||

                            (x.NoiDung != null &&
                             x.NoiDung
                             .ToLower()
                             .Contains(tuKhoa))

                             ||

                            (x.NguoiNopNhan != null &&
                             x.NguoiNopNhan
                             .ToLower()
                             .Contains(tuKhoa))
                        )
                        .ToList();
                }

                // =====================
                // HIỂN THỊ
                // =====================

                dgvPhieuThuChi.DataSource =
                    ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }

        private void dgvPhieuThuChi_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string maPhieu =
                dgvPhieuThuChi
                .Rows[e.RowIndex]
                .Cells["MaPhieu"]
                .Value
                .ToString();

            MessageBox.Show(
                "Bạn chọn phiếu: "
                + maPhieu);
        }
    }
}
