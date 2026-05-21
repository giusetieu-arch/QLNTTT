using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class Phong_DTO
    {
        public string MaPhong { get; set; }
        public string TenPhong { get; set; }
        public string MaToaNha { get; set; }
        public string MaLoaiPhong { get; set; }

        public double? DienTich { get; set; }
        public double? SoDienCu { get; set; }
        public double? SoNuocCu { get; set; }

        public string ACTD_Cu { get; set; }
        public string ACTN_Cu { get; set; }

        public decimal? TienCoc { get; set; }
        public string TrangThai { get; set; }
        public decimal? GiaThue { get; set; }
    }
}
