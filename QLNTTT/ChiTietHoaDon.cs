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
using DTO;

namespace QLNTTT
{
    public partial class ChiTietHoaDon : Form
    {
        string _maHD = "";
        public ChiTietHoaDon(string maHD)
        {
            InitializeComponent();
            _maHD = maHD;
        }
        HoaDon_BUS bus= new HoaDon_BUS();
        PhieuThuChi_BUS busPTC = new PhieuThuChi_BUS();
        CuDan_BUS cuDanBUS =
    new CuDan_BUS();

        HopDong_BUS hopDongBUS =
            new HopDong_BUS();
        void LoadLichSuThanhToan()
        {
            PhieuThuChi_BUS busPT =
                new PhieuThuChi_BUS();

            dgvLichSuThanhToan.DataSource =
                busPT.GetLichSuThanhToan(_maHD);

            dgvLichSuThanhToan.Columns["MaPhieu"]
                .HeaderText = "Mã phiếu";

            dgvLichSuThanhToan.Columns["NgayGiaoDich"]
                .HeaderText = "Ngày thu";

            dgvLichSuThanhToan.Columns["SoTien"]
                .HeaderText = "Số tiền";

            dgvLichSuThanhToan.Columns["PhuongThuc"]
                .HeaderText = "Phương thức";

            dgvLichSuThanhToan.Columns["NoiDung"]
                .HeaderText = "Nội dung";

            dgvLichSuThanhToan.Columns["SoTien"]
                .DefaultCellStyle.Format = "N0";
        }
        void LoadData()
        {
            // =====================
            // HEADER
            // =====================

            var hd =
                bus.GetHoaDon(_maHD);

            if (hd == null)
                return;

            lblMaHD.Text =
                "Mã hóa đơn: " + hd.MaHoaDon;

            lblPhong.Text =
                "Phòng: " + hd.MaPhong;

            lblNgayLap.Text =
                "Ngày lập: " +
                hd.NgayLap
                ?.ToString("dd/MM/yyyy");

            lblTongTien.Text =
                "Tổng tiền: " +
                hd.TongTien
                ?.ToString("N0")
                + " VNĐ";

            lblDaThanhToan.Text =
                "Đã thanh toán: " +
                hd.DaThanhToan?
                .ToString("N0")
                + " VNĐ";

            lblConNo.Text =
                "Còn nợ: " +
                hd.ConNo?
                .ToString("N0")
                + " VNĐ";

            lblTrangThai.Text =
                "Trạng thái: " +
                hd.TrangThai;

            // =====================
            // DETAIL
            // =====================

            var ds =
                bus.GetChiTiet(_maHD);

            dgvChiTiet.DataSource =
                ds;

            // =====================
            // ẢNH
            // =====================

            var dien =
                ds.FirstOrDefault(x =>
                    x.LoaiChiTiet == "Điện");

            if (dien != null)
            {
                if (!string.IsNullOrEmpty(
                    dien.AmhChiSo))
                {
                    picDien.ImageLocation =
                        dien.AmhChiSo;
                }
            }

            var nuoc =
                ds.FirstOrDefault(x =>
                    x.LoaiChiTiet == "Nước");

            if (nuoc != null)
            {
                if (!string.IsNullOrEmpty(
                    nuoc.AmhChiSo))
                {
                    picNuoc.ImageLocation =
                        nuoc.AmhChiSo;
                }
            }
            if (hd.TrangThai
    == "Đã thanh toán")
            {
                lblTrangThai.ForeColor =
                    Color.Green;
            }
            else if (hd.TrangThai
                == "Thanh toán một phần")
            {
                lblTrangThai.ForeColor =
                    Color.Orange;
            }
            else
            {
                lblTrangThai.ForeColor =
                    Color.Red;
            }
        }
        private void ChiTietHoaDon_Load(object sender, EventArgs e)
        {
            LoadData();
            FormatDGV();
            LoadLichSuThanhToan();
        }
        void FormatDGV()
        {
            dgvChiTiet.Columns["TenDanhMuc"]
                .HeaderText = "Khoản";

            dgvChiTiet.Columns["SoLuong"]
                .HeaderText = "SL";

            dgvChiTiet.Columns["DonGia"]
                .HeaderText = "Đơn giá";

            dgvChiTiet.Columns["ThanhTien"]
                .HeaderText = "Thành tiền";

            dgvChiTiet.Columns["DonGia"]
                .DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["ThanhTien"]
                .DefaultCellStyle.Format = "N0";
        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            try
            {
                // ======================
                // HÓA ĐƠN
                // ======================

                var hd =
                    bus.GetHoaDon(_maHD);

                if (hd == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hóa đơn");

                    return;
                }

                // ======================
                // CHI TIẾT
                // ======================

                var ds =
                    bus.GetChiTiet(_maHD);

                // Convert DAL.ChiTietHoaDon to DTO.ChiTietHoaDon_DTO
                var dsDTO = ds.Select(x => new ChiTietHoaDon_DTO
                {
                    ID = x.ID,
                    MaHoaDon = x.MaHoaDon,
                    LoaiChiTiet = x.LoaiChiTiet,
                    TenDanhMuc = x.TenDanhMuc,
                    ChiSoCu = x.ChiSoCu,
                    ChiSoMoi = x.ChiSoMoi,
                    SoLuong = x.SoLuong,
                    DonGia = x.DonGia,
                    ThanhTien = x.ThanhTien,
                    AnhChiSo = x.AmhChiSo,
                    GhiChu = x.GhiChu
                }).ToList();
                // ======================
                // TẠO DTO HÓA ĐƠN
                // ======================
                HoaDon_DTO hdDTO = new HoaDon_DTO
                {
                    MaHoaDon = hd.MaHoaDon,
                    MaPhong = hd.MaPhong,
                    NgayLap = hd.NgayLap,
                    TongTien = hd.TongTien,
                    DaThanhToan = hd.DaThanhToan,
                    ConNo = hd.ConNo,
                    TrangThai = hd.TrangThai
                    // Thêm các thuộc tính khác nếu cần thiết
                };
                // ======================
                // XUẤT PDF
                // ======================

                string filePDF =
                    HoaDonPDF.XuatPDF(hdDTO, dsDTO);

                // ======================
                // HỢP ĐỒNG
                // ======================

                var hopDong =
                    hopDongBUS
                    .GetHopDongByPhong(
                        hd.MaPhong);

                if (hopDong == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hợp đồng");

                    return;
                }

                // ======================
                // CƯ DÂN
                // ======================

                var cuDan =
                    cuDanBUS.GetById(
                        hopDong.MaNguoiDaiDien);

                if (cuDan == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy cư dân");

                    return;
                }

                // ======================
                // DEBUG EMAIL
                // ======================

                MessageBox.Show(
                    cuDan.Email);

                // ======================
                // GỬI MAIL
                // ======================

                string kq =
                    GuiMail.SendMail(
                        cuDan.Email,
                        cuDan.TenCuDan,
                        hd.MaHoaDon,
                        filePDF);

                // ======================
                // RESULT
                // ======================

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Gửi mail thành công");
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

        private void btnInPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // ======================
                // LẤY HÓA ĐƠN
                // ======================

                var hdEntity =
                    bus.GetHoaDon(_maHD);

                if (hdEntity == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hóa đơn");

                    return;
                }

                // ======================
                // ĐỔI SANG DTO
                // ======================

                HoaDon_DTO hd =
                    new HoaDon_DTO();

                hd.MaHoaDon =
                    hdEntity.MaHoaDon;

                hd.MaPhong =
                    hdEntity.MaPhong;

                hd.NgayLap =
                    hdEntity.NgayLap;

                hd.TongTien =
                    hdEntity.TongTien;

                hd.DaThanhToan =
                    hdEntity.DaThanhToan;

                hd.ConNo =
                    hdEntity.ConNo;

                // ======================
                // CHI TIẾT
                // ======================

                var dsEntity =
                    bus.GetChiTiet(_maHD);

                List<ChiTietHoaDon_DTO> ds =
                    dsEntity.Select(x =>
                    new ChiTietHoaDon_DTO
                    {
                        TenDanhMuc = x.TenDanhMuc,
                        SoLuong = x.SoLuong,
                        DonGia = x.DonGia,
                        ThanhTien = x.ThanhTien
                    })
                    .ToList();

                // ======================
                // XUẤT PDF
                // ======================

                string filePDF =
                    HoaDonPDF.XuatPDF(
                        hd,
                        ds);

                // ======================
                // SUCCESS
                // ======================

                MessageBox.Show(
                    "Xuất PDF thành công");

                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = filePDF,
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btninPDF_Click_1(object sender, EventArgs e)
        {
            try
            {
                // ======================
                // LẤY HÓA ĐƠN
                // ======================

                var hdEntity =
                    bus.GetHoaDon(_maHD);

                if (hdEntity == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hóa đơn");

                    return;
                }

                // ======================
                // ĐỔI SANG DTO
                // ======================

                HoaDon_DTO hd =
                    new HoaDon_DTO();

                hd.MaHoaDon =
                    hdEntity.MaHoaDon;

                hd.MaPhong =
                    hdEntity.MaPhong;

                hd.NgayLap =
                    hdEntity.NgayLap;

                hd.TongTien =
                    hdEntity.TongTien;

                hd.DaThanhToan =
                    hdEntity.DaThanhToan;

                hd.ConNo =
                    hdEntity.ConNo;

                // ======================
                // CHI TIẾT
                // ======================

                var dsEntity =
                    bus.GetChiTiet(_maHD);

                List<ChiTietHoaDon_DTO> ds =
                    dsEntity.Select(x =>
                    new ChiTietHoaDon_DTO
                    {
                        TenDanhMuc = x.TenDanhMuc,
                        SoLuong = x.SoLuong,
                        DonGia = x.DonGia,
                        ThanhTien = x.ThanhTien
                    })
                    .ToList();

                // ======================
                // XUẤT PDF
                // ======================

                string filePDF =
                    HoaDonPDF.XuatPDF(
                        hd,
                        ds);

                // ======================
                // SUCCESS
                // ======================

                MessageBox.Show(
                    "Xuất PDF thành công");

                System.Diagnostics.Process.Start(
                    new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = filePDF,
                        UseShellExecute = true
                    });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
