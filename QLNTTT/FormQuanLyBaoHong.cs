using BUS;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DAL;
namespace QLNTTT
{
    public partial class FormQuanLyBaoHong : Form
    {
        string maCongViec = "";
        public FormQuanLyBaoHong()
        {
            InitializeComponent();
        }
        CongViec_BUS bus =new CongViec_BUS();
        void LoadBaoHong()
        {
            dgvBaoHong.DataSource =
                bus.GetAll();
        }
        void FormatDGV()
        {
            dgvBaoHong.Columns["MaCongViec"]
                .HeaderText = "Mã CV";

            dgvBaoHong.Columns["MaPhong"]
                .HeaderText = "Phòng";

            dgvBaoHong.Columns["TieuDe"]
                .HeaderText = "Tiêu đề";

            dgvBaoHong.Columns["TrangThai"]
                .HeaderText = "Trạng thái";

            dgvBaoHong.Columns["NgayBao"]
                .HeaderText = "Ngày báo";

            // ẨN BỚT

            dgvBaoHong.Columns["MoTa"]
                .Visible = false;

            dgvBaoHong.Columns["AnhBaoHong"]
                .Visible = false;

            dgvBaoHong.Columns["MaTaiSan"]
                .Visible = false;

            dgvBaoHong.Columns["MaCuDan"]
                .Visible = false;

            dgvBaoHong.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }
        void loadtrangthai()
        {
            cbbTrangThai.Items.Add("Mới báo");
            cbbTrangThai.Items.Add("Đang xử lý");
            cbbTrangThai.Items.Add("Đã sửa");
            cbbTrangThai.Items.Add("Hoàn thành");
            cbbTrangThai.SelectedIndex = 0;
        }   
        void loadphuongthuc()
        {
            cbbPhuongThuc.Items.Add("Tiền mặt");
            cbbPhuongThuc.Items.Add("Chuyển khoản");
            cbbPhuongThuc.Items.Add("Momo");
            cbbPhuongThuc.SelectedIndex = 0;
        }
        void loadnguyennhan()
        {
            cbbNguyenNhan.Items.Add("Cư dân làm hỏng");
            cbbNguyenNhan.Items.Add("Tự hỏng");
            cbbNguyenNhan.Items.Add("Hao mòn");

            cbbNguyenNhan.SelectedIndex = 0;
        }
        private void FormQuanLyBaoHong_Load(object sender, EventArgs e)
        {
            LoadBaoHong();
            FormatDGV();
           loadtrangthai();
           loadphuongthuc();
            loadnguyennhan();   
        }

        private void dgvBaoHong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                // =====================
                // MÃ CÔNG VIỆC
                // =====================

                maCongViec =
                    dgvBaoHong.CurrentRow
                    .Cells["MaCongViec"]
                    .Value
                    .ToString();

                // =====================
                // PHÒNG
                // =====================

                lblPhong.Text =
                    dgvBaoHong.CurrentRow
                    .Cells["MaPhong"]
                    .Value
                    .ToString();

                // =====================
                // TÀI SẢN
                // =====================

                lblTaiSan.Text =
                    dgvBaoHong.CurrentRow
                    .Cells["MaTaiSan"]
                    .Value
                    .ToString();

                // =====================
                // MÔ TẢ
                // =====================

                txtMoTa.Text =
                    dgvBaoHong.CurrentRow
                    .Cells["MoTa"]
                    .Value
                    .ToString();

                // =====================
                // TRẠNG THÁI
                // =====================

                cbbTrangThai.Text =
                    dgvBaoHong.CurrentRow
                    .Cells["TrangThai"]
                    .Value
                    .ToString();

                // =====================
                // ẢNH
                // =====================

                string fileName =
    dgvBaoHong.CurrentRow
    .Cells["AnhBaoHong"]
    .Value?.ToString();

                if (!string.IsNullOrEmpty(fileName))
                {
                    try
                    {
                        string imageUrl =
                            "https://localhost:7249/uploads/" + fileName;

                        picBaoHong.Load(imageUrl);
                    }
                    catch (Exception ex)
                    {
                        picBaoHong.Image = null;
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    picBaoHong.Image = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTaoPhieu_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBaoHong.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn báo hỏng");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoTien.Text))
                {
                    MessageBox.Show("Nhập chi phí");
                    return;
                }

                decimal soTien = decimal.Parse(txtSoTien.Text);

                // =========================
                // CƯ DÂN LÀM HỎNG
                // =========================

                if (cbbNguyenNhan.Text == "Cư dân làm hỏng")
                {
                    CongViec cv = bus.GetByID(maCongViec);

                    if (cv == null)
                    {
                        MessageBox.Show("Không tìm thấy công việc");
                        return;
                    }

                    cv.TienDenBu = soTien;

                    string kqCV = bus.Update(cv);

                    if (kqCV != "success")
                    {
                        MessageBox.Show(kqCV);
                        return;
                    }

                    MessageBox.Show(
                        "Đã ghi nhận tiền đền bù vào hóa đơn tháng sau");

                    return;
                }

                // =========================
                // TỰ HỎNG / HAO MÒN
                // =========================

                PhieuThuChi_DTO pt =
                    new PhieuThuChi_DTO();

                pt.MaPhieu =
                    "PTC" +
                    DateTime.Now.ToString("yyyyMMddHHmmss");

                pt.LoaiPhieu = "Chi";

                pt.SoTien = soTien;

                pt.NgayGiaoDich =
                    DateTime.Now;

                pt.PhuongThuc =
                    cbbPhuongThuc.Text;

                pt.NoiDung =
                    txtNoiDung.Text;

                pt.NguoiNopNhan =
                    "Đơn vị sửa chữa";

                pt.MaCongViec =
                    maCongViec;

                PhieuThuChi_BUS ptBUS =
                    new PhieuThuChi_BUS();

                string kq =
                    ptBUS.Insert(pt);

                if (kq != "success")
                {
                    MessageBox.Show(kq);
                    return;
                }

                // =========================
                // GHI SỔ QUỸ
                // =========================

                SoQuy_DTO sq =
                    new SoQuy_DTO();

                sq.MaPhieu =
                    pt.MaPhieu;

                sq.NgayGiaoDich =
                    DateTime.Now;

                sq.LoaiGiaoDich =
                    "Chi";

                sq.Thu = 0;

                sq.Chi = soTien;

                sq.NoiDung =
                    txtNoiDung.Text;

                sq.NguoiLap =
                    "Admin";

                SoQuy_BUS sqBUS =
                    new SoQuy_BUS();

                string kqSQ =
                    sqBUS.Insert(sq);

                if (kqSQ != "success")
                {
                    MessageBox.Show(kqSQ);
                    return;
                }

                MessageBox.Show(
                    "Tạo phiếu chi thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCapNhat_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvBaoHong.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Vui lòng chọn");

                    return;
                }

                string maCV =
                    dgvBaoHong.CurrentRow
                    .Cells["MaCongViec"]
                    .Value
                    .ToString();

                CongViec cv =
                    bus.GetByID(maCV);

                if (cv == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy");

                    return;
                }

                cv.TrangThai =
                    cbbTrangThai.Text;

                cv.NgayXuLy =
                    DateTime.Now;

                string kq =
                    bus.Update(cv);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Cập nhật thành công");

                    LoadBaoHong();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnhoanthanh_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(cbbNguyenNhan.Text))
                {
                    MessageBox.Show("Chọn nguyên nhân");
                    return;
                }
                if (dgvBaoHong.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn báo hỏng");
                    return;
                }

                string maCV =
                    dgvBaoHong.CurrentRow.Cells["MaCongViec"]
                    .Value.ToString();

                CongViec cv = bus.GetByID(maCV);

                if (cv == null)
                {
                    MessageBox.Show("Không tìm thấy công việc");
                    return;
                }

                // =========================
                // HOÀN THÀNH CÔNG VIỆC
                // =========================

                cv.TrangThai = "Hoàn thành";
                cv.NgayXuLy = DateTime.Now;

                decimal chiPhi = 0;

                decimal.TryParse(txtSoTien.Text, out chiPhi);

                if (cbbNguyenNhan.Text == "Cư dân làm hỏng")
                {
                    cv.TienDenBu = chiPhi;
                }
                else
                {
                    cv.TienDenBu = 0;
                }

                string kq = bus.Update(cv);

                if (kq != "success")
                {
                    MessageBox.Show(kq);
                    return;
                }

                // =========================
                // LƯU LỊCH SỬ THIẾT BỊ
                // =========================

                if (!string.IsNullOrEmpty(txtSoTien.Text))
                {
                    decimal.TryParse(txtSoTien.Text, out chiPhi);
                }

                LichSuThietBi_DTO ls =
                    new LichSuThietBi_DTO();

                ls.MaLichSu =
                    "LS" + DateTime.Now.ToString("yyyyMMddHHmmss");

                ls.MaTaiSan =
                    dgvBaoHong.CurrentRow.Cells["MaTaiSan"]
                    .Value.ToString();

                ls.MaPhong =
                    dgvBaoHong.CurrentRow.Cells["MaPhong"]
                    .Value.ToString();

                ls.LoaiSuKien = "Sửa chữa";

                ls.MoTa = txtNoiDung.Text;

                ls.ChiPhi = chiPhi;

                ls.NgayThucHien = DateTime.Now;

                ls.MaCongViec = maCV;

                LichSuThietBi_BUS lsBUS =
                    new LichSuThietBi_BUS();

                string kqLS = lsBUS.Insert(ls);

                if (kqLS != "success")
                {
                    MessageBox.Show(kqLS);
                    return;
                }

               

                MessageBox.Show("Đã hoàn thành sửa chữa");

                LoadBaoHong();

                picBaoHong.Image = null;

                txtMoTa.Clear();

                txtNoiDung.Clear();

                txtSoTien.Clear();

                lblPhong.Text = "";

                lblTaiSan.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
