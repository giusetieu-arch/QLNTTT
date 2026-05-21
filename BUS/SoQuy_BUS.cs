using DAL;
using DTO;
using System.Collections.Generic;

namespace BUS
{
    public class SoQuy_BUS
    {
        SoQuy_DAL dal =
            new SoQuy_DAL();

        public string Insert(
            SoQuy_DTO dto)
        {
            return dal.Insert(dto);
        }

        public decimal LaySoDuCuoi()
        {
            return dal.LaySoDuCuoi();
        }

        public object GetAll()
        {
            return dal.GetAll();
        }
        public decimal GetSoDuHienTai()
        {
            return dal.GetSoDuHienTai();
        }
        public List<SoQuy_DTO> GetAll1()
        {
            return dal.GetAll1();
        }
        public decimal DoanhThuThang()
        {
            return dal.DoanhThuThang();
        }
        public List<dynamic> DoanhThu12Thang()
        {
            return dal.DoanhThu12Thang();
        }
    }
}