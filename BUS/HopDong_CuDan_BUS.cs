using DAL;
using System.Collections.Generic;
using DTO;
namespace BUS
{
    public class HopDong_CuDan_BUS
    {
        HopDong_CuDan_DAL dal =
            new HopDong_CuDan_DAL();

        // =========================
        // GET CƯ DÂN THEO HỢP ĐỒNG
        // =========================
        public List<CuDan> GetCuDan(string maHD)
        {
            return dal.GetCuDan(maHD);
        }
        // =========================
        // LOAD
        // =========================
        public int DemNguoiDangO(
    string maHD)
        {
            return dal
                .DemNguoiDangO(maHD);
        }
        public List<HopDong_CuDan_DTO>
            GetByHopDong(string maHD)
        {
            return dal.GetByHopDong(maHD);
        }

        // =========================
        // INSERT
        // =========================

        public string Insert(
            HopDong_CuDan_DTO dto)
        {
            return dal.Insert(dto);
        }

        // =========================
        // CHUYỂN ĐI
        // =========================

        public string ChuyenDi(
            string maHD,
            string maCuDan)
        {
            return dal.ChuyenDi(
                maHD,
                maCuDan);
        }

        // =========================
        // ĐỔI ĐẠI DIỆN
        // =========================

        public string DoiDaiDien(
            string maHD,
            string maCuDan)
        {
            return dal.DoiDaiDien(
                maHD,
                maCuDan);
        }
    }
}