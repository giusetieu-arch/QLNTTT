using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ChiTietHoaDon_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public string Insert(
    ChiTietHoaDon_DTO dto)
        {
            try
            {
                ChiTietHoaDon ct =
                    new ChiTietHoaDon();

                ct.MaHoaDon =
                    dto.MaHoaDon;

                ct.LoaiChiTiet =
                    dto.LoaiChiTiet;

                ct.TenDanhMuc =
                    dto.TenDanhMuc;

                ct.ChiSoCu =
                    dto.ChiSoCu;

                ct.ChiSoMoi =
                    dto.ChiSoMoi;

                ct.SoLuong =
                    dto.SoLuong;

                ct.DonGia =
                    dto.DonGia;

                ct.ThanhTien =
                    dto.ThanhTien;

                ct.AmhChiSo =
                    dto.AnhChiSo;

                ct.GhiChu =
                    dto.GhiChu;

                db.ChiTietHoaDons.Add(ct);

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
