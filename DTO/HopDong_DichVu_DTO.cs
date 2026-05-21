using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class HopDong_DichVu_DTO
    {

        public int ID { get; set; }

        public string MaHopDong { get; set; }

        public string MaDichVu { get; set; }

        public string TenDichVu { get; set; }

        public decimal? DonGia { get; set; }

        public string HinhThucTinh { get; set; }

        public DateTime? NgayBatDau { get; set; }

        public DateTime? NgayNgung { get; set; }

        public string TrangThai { get; set; }   
    }
}
