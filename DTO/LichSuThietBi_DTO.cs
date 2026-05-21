using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LichSuThietBi_DTO
    {
        public string MaLichSu { get; set; }

        public string MaTaiSan { get; set; }

        public string MaPhong { get; set; }

        public string LoaiSuKien { get; set; }

        public string MoTa { get; set; }

        public decimal? ChiPhi { get; set; }

        public DateTime? NgayThucHien { get; set; }

        public string MaCongViec { get; set; }
    }
}
