using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HoaDon_DTO
    {
        public string MaHoaDon { get; set; }

        public string MaHopDong { get; set; }

        public string MaPhong { get; set; }

        public string KyHoaDon { get; set; }

        public DateTime? NgayLap { get; set; }

        public DateTime? NgayThanhToan { get; set; }

        public decimal? TongTien { get; set; }

        public decimal? DaThanhToan { get; set; }

        public decimal? ConNo { get; set; }

        public string TrangThai { get; set; }
    }
}
