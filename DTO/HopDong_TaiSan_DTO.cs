using System;

namespace DTO
{
    public class HopDong_TaiSan_DTO
    {
        public int ID { get; set; }

        public string MaHopDong { get; set; }

        public string MaTaiSan { get; set; }

        // JOIN từ bảng TaiSan
        public string TenTaiSan { get; set; }

        public int? SoLuong { get; set; }

        // Giá trị tài sản
        public decimal? GiaTri { get; set; }

        // Tình trạng khi bàn giao
        public string TinhTrangBanDau { get; set; }

        // Tình trạng khi trả
        public string TinhTrangKhiTra { get; set; }

        public DateTime? NgayBanGiao { get; set; }

        public DateTime? NgayThuHoi { get; set; }

        // Tiền đền bù nếu hỏng
        public decimal? TienDenBu { get; set; }

        public string GhiChu { get; set; }

        public string TrangThai { get; set; }
    }
}