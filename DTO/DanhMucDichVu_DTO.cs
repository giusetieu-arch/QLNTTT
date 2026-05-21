using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class DanhMucDichVu_DTO
    {
        public string MaDichVu { get; set; }
        public string TenDichVu { get; set; }
        public decimal? DonGia { get; set; } // Dùng decimal? vì cho phép Null
        public string DonViTinh { get; set; }
        public string HinhThucTinh { get; set; }
        public string GhiChu { get; set; }
        public string TrangThai { get; set; }
    }
}
