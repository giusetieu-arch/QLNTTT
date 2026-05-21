using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
namespace DAL
{
    public class TaiSan_DAL
    {
        // 🔹 Lấy tất cả
        public List<TaiSan> GetAll()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                return db.TaiSans.ToList();
            }
        }

        // 🔹 Lấy theo mã
        public TaiSan GetById(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.TaiSans.Find(ma);
            }
        }

        // 🔹 Kiểm tra tồn tại
        public bool Exists(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.TaiSans.Any(x => x.MaTaiSan == ma);
            }
        }

        // 🔹 Thêm
        public string Insert(TaiSan_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    if (db.TaiSans.Any(x => x.MaTaiSan == dto.MaTaiSan))
                        return "Mã tài sản đã tồn tại";

                    TaiSan ts = new TaiSan()
                    {
                        MaTaiSan = dto.MaTaiSan,
                        TenTaiSan = dto.TenTaiSan,
                        MaPhong = dto.MaPhong,
                        GiaTri = dto.GiaTri,
                        Ma_QR_TS = dto.Ma_QR_TS,

                       
                        TrangThai = "Đang sử dụng"
                    };

                    db.TaiSans.Add(ts);

                    return db.SaveChanges() > 0
                        ? "success"
                        : "fail";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi thêm: " + ex.Message;
            }
        }

        // 🔹 Cập nhật
        public string Update(TaiSan_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var ts = db.TaiSans.Find(dto.MaTaiSan);

                    if (ts == null)
                        return "Không tìm thấy tài sản";

                    ts.TenTaiSan = dto.TenTaiSan;
                    ts.MaPhong = dto.MaPhong;
                    ts.GiaTri = dto.GiaTri;
                    ts.Ma_QR_TS = dto.Ma_QR_TS;
                    ts.TrangThai = dto.TrangThai;

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi cập nhật: " + ex.Message;
            }
        }

        // 🔹 Xóa
        public string Delete(string ma)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var ts = db.TaiSans.Find(ma);

                    if (ts == null)
                        return "Tài sản không tồn tại";

                    db.TaiSans.Remove(ts);

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi ràng buộc: " + ex.Message;
            }
        }
        public List<TaiSan> GetByPhong(string maPhong)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.TaiSans
                    .Where(x => x.MaPhong == maPhong)
                    .ToList();
            }
        }

        // 🔹 Tìm kiếm
        public List<TaiSan> Search(string keyword)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                db.Configuration.LazyLoadingEnabled = false;

                return db.TaiSans
                    .Where(x =>
                        x.TenTaiSan.Contains(keyword) ||
                        x.MaTaiSan.Contains(keyword))
                    .ToList();
            }
        }
    }
}
