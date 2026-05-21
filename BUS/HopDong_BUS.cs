using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BUS
{
    public class HopDong_BUS
    {
        HopDong_DAL dal = new HopDong_DAL();

        public string Insert(HopDong_DTO dto)
        {
            if (string.IsNullOrEmpty(dto.MaHopDong))
                return "Chưa nhập mã hợp đồng";
            if (string.IsNullOrEmpty(dto.MaNguoiDaiDien))
                return "Chưa chọn người đại diện";
            if (string.IsNullOrEmpty(dto.MaPhong))
                return "Chưa chọn phòng";

            return dal.Insert(dto);
        }
        public HopDong GetHopDongByPhong(string maPhong)
        {
            return dal.GetAll()
                .FirstOrDefault(x =>
                    x.MaPhong == maPhong
                    && x.TrangThai == "Đang hiệu lực");
        }
        public List<HopDong_DTO>GetDangHieuLuc()
        {
            return dal
                .GetDangHieuLuc();
        }
        // GET ALL
        // =========================
        public List<HopDong> GetAll()
        {
            return dal.GetAll();
        }
        public HopDong GetById(string maHD)
        {
            if (string.IsNullOrWhiteSpace(maHD))
                return null;

            return dal.GetById(maHD);
        }
        public string ThanhLy(string maHD)
        {
            return dal.ThanhLy(maHD);
        }
        public HopDong_DTO GetById2(
    string maHD)
        {
            return dal.GetById2(maHD);
        }
        public HopDong_DTO GetById1(string maHD)
        {
            return dal.GetById1(maHD);
        }
        public string Update(HopDong_DTO dto)
        {
            return dal.Update(dto);
        }
        public string LapHopDong(
    HopDong_DTO hd,
    DataGridView dgvCuDan,
    DataGridView dgvDichVu,
    DataGridView dgvTaiSan)
{
    return dal.LapHopDong(
        hd,
        dgvCuDan,
        dgvDichVu,
        dgvTaiSan);
}
        public int HopDongSapHetHan()
        {
            return dal.HopDongSapHetHan();
        }
    }
}
