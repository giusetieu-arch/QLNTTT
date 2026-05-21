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

namespace QLNTTT
{
    public partial class ThuTienHoaDon : Form
    {
        string _maHD = "";
        public ThuTienHoaDon(string maHD)
        {
            InitializeComponent();
            _maHD = maHD;
        }
       
HoaDon_BUS hoaDonBUS =
    new HoaDon_BUS();
        SoQuy_BUS sqBUS =
            new SoQuy_BUS();
        void LoadHoaDon()
        {
            var hd =
                hoaDonBUS
                .GetHoaDon(_maHD);

            if (hd == null)
                return;

            lblMaHD.Text =
     "Mã hóa đơn: " +
     hd.MaHoaDon;

            lblPhong.Text =
                "Phòng: " +
                hd.MaPhong;

            lblTongTien.Text =
                "Tổng tiền: " +
                hd.TongTien
                ?.ToString("N0")
                + " VNĐ";

            lblDaThanhToan.Text =
                "Đã thanh toán: " +
                hd.DaThanhToan
                ?.ToString("N0")
                + " VNĐ";

            lblConNo.Text =
                "Còn nợ: " +
                hd.ConNo
                ?.ToString("N0")
                + " VNĐ";

            lblTrangThai.Text =
                "Trạng thái: " +
                hd.TrangThai;
        }
        private void ThuTienHoaDon_Load(object sender, EventArgs e)
        {
           LoadHoaDon();
            cbbHinhThuc.Items.Add("Tiền mặt"); 
            cbbHinhThuc.Items.Add("Chuyển khoản"); 
            cbbHinhThuc.Items.Add("Momo"); 
            cbbHinhThuc.SelectedIndex = 0;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                // ==========================================
                // 1. KIỂM TRA HÓA ĐƠN & TRẠNG THÁI CÔNG NỢ
                // ==========================================
                HoaDon_BUS hdBUS = new HoaDon_BUS();
                var hd = hdBUS.GetHoaDon(_maHD);

                if (hd == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (hd.TrangThai == "Đã thanh toán")
                {
                    MessageBox.Show("Hóa đơn này đã được thanh toán đầy đủ trước đó!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ==========================================
                // 2. KIỂM TRA SỐ TIỀN ADMIN NHẬP THỰC TẾ
                // ==========================================
                decimal soTienThu = 0;
                if (!decimal.TryParse(txtSoTienThu.Text, out soTienThu) || soTienThu <= 0)
                {
                    MessageBox.Show("Vui lòng nhập số tiền thu thực tế hợp lệ (phải lớn hơn 0)!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                decimal conNoCu = hd.ConNo ?? 0;
                if (soTienThu > conNoCu)
                {
                    MessageBox.Show($"Số tiền thu thực tế ({soTienThu:N0}đ) vượt quá số tiền còn nợ của hóa đơn ({conNoCu:N0}đ)!", "Lỗi đối soát", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Phục vụ tạo mã phiếu đồng bộ
                string maPhieuTuDong = "PT" + DateTime.Now.ToString("yyyyMMddHHmmss");

                // ==========================================
                // 3. ĐÓNG GÓI DỮ LIỆU ĐỐI TƯỢNG (DTO)
                // ==========================================

                // A. Đối tượng Phiếu Thu Chi
                PhieuThuChi_DTO pt = new PhieuThuChi_DTO
                {
                    MaPhieu = maPhieuTuDong,
                    LoaiPhieu = "Thu",
                    MaHoaDon = hd.MaHoaDon,
                    SoTien = soTienThu,
                    NgayGiaoDich = DateTime.Now,
                    NguoiNopNhan = hd.MaPhong,
                    PhuongThuc = cbbHinhThuc.Text,
                    NoiDung = $"Đối soát thủ công - Thanh toán hóa đơn {hd.MaHoaDon}",
                    GhiChu = "Đã duyệt"
                };

                // B. Đối tượng Sổ Quỹ
                SoQuy_BUS sqBUS = new SoQuy_BUS();
                decimal soDuCu = sqBUS.GetSoDuHienTai();

                SoQuy_DTO sq = new SoQuy_DTO
                {
                    MaPhieu = maPhieuTuDong,
                    NgayGiaoDich = DateTime.Now,
                    LoaiGiaoDich = "Thu tiền hóa đơn",
                    Thu = soTienThu,
                    Chi = 0,
                    SoDuSauGD = soDuCu + soTienThu,
                    NoiDung = $"Thu tiền hóa đơn {hd.MaHoaDon} phòng {hd.MaPhong}",
                    NguoiLap = "Admin"
                };

                // C. Đối tượng Hóa Đơn cập nhật
                decimal daThanhToanMoi = (hd.DaThanhToan ?? 0) + soTienThu;
                decimal conNoMoi = conNoCu - soTienThu;

                HoaDon_DTO dtoHoaDon = new HoaDon_DTO
                {
                    MaHoaDon = hd.MaHoaDon,
                    DaThanhToan = daThanhToanMoi,
                    ConNo = conNoMoi,
                    TrangThai = (conNoMoi == 0) ? "Đã thanh toán" : "Thanh toán một phần"
                };

                // ==========================================
                // 4. GỌI HÀM BUS THỰC THI TRANSACTION ĐỒNG BỘ
                // ==========================================
                PhieuThuChi_BUS ptBUS = new PhieuThuChi_BUS();
                string ketQuaThanhToan = ptBUS.UpdateThanhToanGop(pt, sq, dtoHoaDon);

                if (ketQuaThanhToan == "success")
                {
                    MessageBox.Show("Xác nhận thanh toán và cập nhật công nợ thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Tải lại danh sách trên Form
                    LoadHoaDon();
                    txtSoTienThu.Clear();
                }
                else
                {
                    // Hiển thị thông báo chi tiết nếu có lỗi phát sinh trong Transaction
                    MessageBox.Show(ketQuaThanhToan, "Lỗi xử lý", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi phát sinh trên giao diện: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
