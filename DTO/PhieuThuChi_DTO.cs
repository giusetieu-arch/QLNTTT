using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class PhieuThuChi_DTO
    {
        public string MaPhieu { get; set; }

        public string LoaiPhieu { get; set; }
        // Thu / Chi

        public string MaHoaDon { get; set; }

        public decimal? SoTien { get; set; }

        public DateTime? NgayGiaoDich { get; set; }

        public string NguoiNopNhan { get; set; }

        public string PhuongThuc { get; set; }
        // Tiền mặt / Chuyển khoản

        public string NoiDung { get; set; }

        public string GhiChu { get; set; }

        public string MaCongViec { get; set; }
    }
}
