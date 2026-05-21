using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class CongViec_DTO
    {
        public string MaCongViec { get; set; }
        public string MaPhong { get; set; }
        public string MaTaiSan { get; set; }
        public string MaCuDan { get; set; }

        public string TieuDe { get; set; }
        public string MoTa { get; set; }

        public string AnhBaoHong { get; set; }
        public decimal? TienDenBu { get; set; }
        public string NguyenNhan { get; set; }
        public string TrangThai { get; set; }

        public DateTime? NgayBao { get; set; }
        public DateTime? NgayXuLy { get; set; }

    }
}
