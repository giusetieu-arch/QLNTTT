using DAL;
using System.Collections.Generic;
using DTO;
namespace BUS
{
    public class HopDong_TaiSan_BUS
    {
        HopDong_TaiSan_DAL dal =
            new HopDong_TaiSan_DAL();

        // =========================
        // GET TÀI SẢN THEO HỢP ĐỒNG
        // =========================
        public List<HopDong_TaiSan> GetTS(string maHD)
        {
            return dal.GetTS(maHD);
        }
        public List<HopDong_TaiSan_DTO>
           GetByHopDong(string maHD)
        {
            return dal.GetByHopDong(maHD);
        }

        public string Insert(
            HopDong_TaiSan_DTO dto)
        {
            return dal.Insert(dto);
        }

        public string ThuHoi(
            string maHD,
            string maTS)
        {
            return dal.ThuHoi(
                maHD,
                maTS);
        }

        public string BaoHong(
            string maHD,
            string maTS)
        {
            return dal.BaoHong(
                maHD,
                maTS);
        }
        public string SuDungLai(
    string maHD,
    string maTS)
        {
            return dal.SuDungLai(maHD, maTS);
        }
    }
}