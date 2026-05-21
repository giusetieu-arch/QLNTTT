using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class HoaDon_BUS
    {
        HoaDon_DAL dal =
          new HoaDon_DAL();

        public ThongTinLapHoaDon_DTO
      GetThongTin(string maPhong)
        {
            return dal.GetThongTin(maPhong);
        }
        public string Insert(HoaDon_DTO hd)
        {
            return dal.Insert(hd);
        }
        public List<HoaDon> GetAll1()
        {
            return dal.GetAll1();
        }

        public List<HoaDon_DTO> GetAll()
        {
            return dal.GetAll();
        }
        public HoaDon GetHoaDon(string maHD)
        {
            return dal.GetHoaDon(maHD);
        }
     

        public string Update(HoaDon_DTO dto)
        {
            return dal.Update(dto);
        }
        public List<ChiTietHoaDon>
        GetChiTiet(string maHD)
        {
            return dal.GetChiTiet(maHD);
        }
        public bool KiemTraHoaDonTonTai(string maPhong,string kyHoaDon)
        {
            return dal.KiemTraHoaDonTonTai(
            maPhong,
            kyHoaDon);
        }
        public decimal TongCongNo()
        {
            return dal.TongCongNo();
        }
    }
}
