using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class SoQuy_DTO
    {
        public int ID { get; set; }
     

        public string MaPhieu { get; set; }

        public DateTime? NgayGiaoDich { get; set; }

        public string LoaiGiaoDich { get; set; }

        public decimal? Thu { get; set; }

        public decimal? Chi { get; set; }

        public decimal? SoDuSauGD { get; set; }

        public string NoiDung { get; set; }

        public string NguoiLap { get; set; }

        public string GhiChu { get; set; }
    }
}
