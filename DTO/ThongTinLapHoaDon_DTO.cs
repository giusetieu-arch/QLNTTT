using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class ThongTinLapHoaDon_DTO
    {
        public string MaHopDong { get; set; }

        public decimal GiaPhong { get; set; }

        public decimal GiaDien { get; set; }

        public decimal GiaNuoc { get; set; }

        public int SoDienCu { get; set; }

        public int SoNuocCu { get; set; }

        public int SoNguoi { get; set; }

        public string AnhDienCu { get; set; }

        public string AnhNuocCu { get; set; }

        public List<HopDong_DichVu_DTO>
            DichVus
        { get; set; }
    }
}
