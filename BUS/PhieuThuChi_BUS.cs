using DAL;
using DTO;
using System.Collections.Generic;

namespace BUS
{
    public class PhieuThuChi_BUS
    {
        PhieuThuChi_DAL dal =
            new PhieuThuChi_DAL();

        public string Insert(
            PhieuThuChi_DTO dto)
        {
            return dal.Insert(dto);
        }

        public object GetAll()
        {
            return dal.GetAll();
        }
        public List<PhieuThuChi_DTO> GetAll1()
        {
            return dal.GetAll1();
        }
        public string TaoMa()
        {
            return dal.TaoMa();
        }
        public List<PhieuThuChi_DTO> GetLichSuThanhToan(string maHD)
        {
            return dal.GetLichSuThanhToan(maHD);
        }
        public List<PhieuThuChi_DTO> TimKiem(
    string loai,
    string keyword)
        {
            return dal.TimKiem(
                loai,
                keyword);
        }
        public string UpdateThanhToanGop(
            PhieuThuChi_DTO pt,
            SoQuy_DTO sq,
            HoaDon_DTO hd)
        {
            // Có thể validate tại BUS nếu cần

            if (pt == null)
                return "Phiếu thu không hợp lệ";

            if (sq == null)
                return "Sổ quỹ không hợp lệ";

            if (hd == null)
                return "Hóa đơn không hợp lệ";

            return dal.UpdateThanhToanGộp(
                pt,
                sq,
                hd);
        }
    }
}