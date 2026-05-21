using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CongViec_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public List<CongViec_DTO> GetAll()
        {
            return db.CongViecs
                .Select(x => new CongViec_DTO
                {
                    MaCongViec = x.MaCongViec,

                    MaPhong = x.MaPhong,

                    MaTaiSan = x.MaTaiSan,

                    MaCuDan = x.MaCuDan,

                    TieuDe = x.TieuDe,

                    MoTa = x.MoTa,

                    AnhBaoHong = x.AnhBaoHong,

                    TrangThai = x.TrangThai,

                    NgayBao = x.NgayBao,

                    NgayXuLy = x.NgayXuLy
                })
                .OrderByDescending(x => x.NgayBao)
                .ToList();
        }
        public CongViec GetByID(string maCV)
        {
            return db.CongViecs
                .FirstOrDefault(x =>
                    x.MaCongViec == maCV);
        }
        public string Update(CongViec cv)
        {
            try
            {
                var data =
                    db.CongViecs
                    .FirstOrDefault(x =>
                        x.MaCongViec ==
                        cv.MaCongViec);

                if (data == null)
                    return "Không tìm thấy";

                data.TrangThai =
                    cv.TrangThai;

                data.MoTa =
                    cv.MoTa;

                data.NgayXuLy =
                    cv.NgayXuLy;

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public List<CongViec> GetTopBaoHongMoi()
        {
            return db.CongViecs
                     .OrderByDescending(x => x.NgayBao)
                     .Take(5)
                     .ToList();
        }
        public int BaoHongChuaXuLy()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CongViecs
                         .Count(x =>
                            x.TrangThai != "Hoàn thành");
            }
        }
    }
}
