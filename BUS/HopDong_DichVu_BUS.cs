using DAL;
using System.Collections.Generic;
using DTO;
namespace BUS
{
    public class HopDong_DichVu_BUS
    {
        HopDong_DichVu_DAL dal =
            new HopDong_DichVu_DAL();

        // =========================
        // GET DỊCH VỤ THEO HỢP ĐỒNG
        // =========================
        public List<HopDong_DichVu> GetDV(string maHD)
        {
            return dal.GetDV(maHD);
        }
        public List<HopDong_DichVu_DTO>
           GetByHopDong(string maHD)
        {
            return dal.GetByHopDong(maHD);
        }

        public string Insert(
            HopDong_DichVu_DTO dto)
        {
            return dal.Insert(dto);
        }

        public string NgungDichVu(
            string maHD,
            string maDV)
        {
            return dal.NgungDichVu(
                maHD,
                maDV);
        }

        public string DoiGia(
            string maHD,
            string maDV,
            decimal giaMoi)
        {
            return dal.DoiGia(
                maHD,
                maDV,
                giaMoi);
        }
        public string SuDungLai(
    string maHD,
    string maDV)
        {
            return dal
                .SuDungLai(
                    maHD,
                    maDV);
        }
    }
}