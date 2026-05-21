using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDong_CuDan_DTO
    {
        public int ID { get; set; }

        public string MaHopDong { get; set; }

        public string MaCuDan { get; set; }
        // THÊM
        public string TenCuDan { get; set; }
        public string VaiTro { get; set; }

        public DateTime? NgayThamGia { get; set; }

        public DateTime? NgayRoiKhoi { get; set; }

        public string TrangThai { get; set; }

    }
}
