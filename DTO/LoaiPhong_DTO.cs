using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class LoaiPhong_DTO
    {
        public string MaLoaiPhong { get; set; }
        public string TenLoaiPhong { get; set; }
        public int? SoNguoiToiDa { get; set; }
        public decimal? GiaThueMacDinh { get; set; }
        public decimal? DonGiaDien { get; set; }
        public decimal? DonGiaNuoc { get; set; }
        public string MoTa { get; set; }
        public string TrangThai { get; set; }
    }
}
