using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ChiTietHoaDon_DTO
    {
        public int ID { get; set; }

        public string MaHoaDon { get; set; }

        public string LoaiChiTiet { get; set; }

        public string TenDanhMuc { get; set; }

        public decimal? ChiSoCu { get; set; }

        public decimal? ChiSoMoi { get; set; }

        public decimal? SoLuong { get; set; }

        public decimal? DonGia { get; set; }

        public decimal? ThanhTien { get; set; }

        public string AnhChiSo { get; set; }

        public string GhiChu { get; set; }
    }
}
