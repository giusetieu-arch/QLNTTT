using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDong_DTO
    {
        public string MaHopDong { get; set; }

        public string MaPhong { get; set; }

        public string MaNguoiDaiDien { get; set; }

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        public decimal? TienCoc { get; set; }

        public decimal? GiaThue { get; set; }

        public decimal? GiaDienChot { get; set; }

        public decimal? GiaNuocChot { get; set; }

        public DateTime? NgayTao { get; set; }

        public string GhiChu { get; set; }

        public string TrangThai { get; set; }
    }
}
