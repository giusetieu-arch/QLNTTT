using System;

namespace DTO
{
    public class PhuLucHopDong_DTO
    {
        public int ID { get; set; }

        public string MaHopDong { get; set; }

        public string LoaiPhuLuc { get; set; }

        public decimal? GiaThueMoi { get; set; }

        public decimal? GiaDienMoi { get; set; }

        public decimal? GiaNuocMoi { get; set; }

        public decimal? GiaCocMoi { get; set; }

        public DateTime? ThoiGianMoi { get; set; }
        public DateTime? NgayApDung { get; set; }

        public string NoiDung { get; set; }

        public DateTime? NgayTao { get; set; }

        public string NguoiThucHien { get; set; }

        public string TrangThai { get; set; }
    }
}