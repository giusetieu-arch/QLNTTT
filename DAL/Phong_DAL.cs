using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // Quan trọng: Thêm thư viện này để dùng .AsNoTracking()
using DTO;

namespace DAL
{
    public class Phong_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public string UpdateChiSo(
      string maPhong,
      int dienMoi,
      int nuocMoi,
      string anhDien,
      string anhNuoc)
        {
            try
            {
                var p =
                    db.Phongs
                    .FirstOrDefault(x =>
                        x.MaPhong
                            == maPhong);

                if (p == null)
                    return "Không tìm thấy phòng";

                p.SoDienCu =
                    dienMoi;

                p.SoNuocCu =
                    nuocMoi;

                p.ACTD_Cu =
                    anhDien;

                p.ACTN_Cu =
                    anhNuoc;

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // 🔹 Lấy tất cả: Thêm AsNoTracking để tăng tốc và tránh lỗi Proxy
        public List<Phong> GetAll()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return db.Phongs.AsNoTracking().ToList();
            }
        }

        // 🔹 Lấy theo mã
        public Phong GetById(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                return db.Phongs.AsNoTracking().FirstOrDefault(x => x.MaPhong == ma);
            }
        }

        // 🔹 Kiểm tra tồn tại: Dùng Any sẽ nhanh hơn lấy cả Object ra
        public bool Exists(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.Phongs.Any(x => x.MaPhong == ma);
            }
        }

        // 🔹 Thêm
        public string Insert(Phong_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    if (db.Phongs.Any(x => x.MaPhong == dto.MaPhong))
                        return "Mã phòng đã tồn tại";

                    Phong p = new Phong()
                    {
                        MaPhong = dto.MaPhong,
                        TenPhong = dto.TenPhong,
                        MaToaNha = dto.MaToaNha,
                        MaLoaiPhong = dto.MaLoaiPhong,
                        DienTich = dto.DienTich,
                        SoDienCu = dto.SoDienCu,
                        SoNuocCu = dto.SoNuocCu,
                        ACTD_Cu = dto.ACTD_Cu,
                        ACTN_Cu = dto.ACTN_Cu,
                        TienCoc = dto.TienCoc,
                        TrangThai = dto.TrangThai,
                        GiaThue = dto.GiaThue
                    };

                    db.Phongs.Add(p);
                    db.SaveChanges();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi thêm mới: " + ex.Message;
            }
        }

        // 🔹 Cập nhật
        public string Update(Phong_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var p = db.Phongs.Find(dto.MaPhong);
                    if (p == null) return "Không tìm thấy phòng";

                    // Chỉ cập nhật nếu có thay đổi để tối ưu hiệu suất
                    p.TenPhong = dto.TenPhong;
                    p.MaToaNha = dto.MaToaNha;
                    p.MaLoaiPhong = dto.MaLoaiPhong;
                    p.DienTich = dto.DienTich;
                    p.SoDienCu = dto.SoDienCu;
                    p.SoNuocCu = dto.SoNuocCu;
                    p.ACTD_Cu = dto.ACTD_Cu;
                    p.ACTN_Cu = dto.ACTN_Cu;
                    p.TienCoc = dto.TienCoc;
                    p.TrangThai = dto.TrangThai;
                    p.GiaThue = dto.GiaThue;

                    db.SaveChanges();
                    return "success";
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
                    var p = db.Phongs.Find(ma);
                    if (p == null) return "Phòng không tồn tại";

                    db.Phongs.Remove(p);
                    db.SaveChanges();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                // Thường lỗi ở đây là do ràng buộc khóa ngoại (phòng đang có hợp đồng)
                return "Không thể xóa phòng này do đang có dữ liệu liên quan (Hợp đồng, Hóa đơn...)";
            }
        }

        // 🔹 Tìm kiếm nâng cao
        public List<Phong> Search(string keyword)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                string kw = keyword.ToLower(); // Chuyển về chữ thường để tìm kiếm không phân biệt

                return db.Phongs.AsNoTracking()
                    .Where(x => x.TenPhong.ToLower().Contains(kw)
                             || x.MaPhong.ToLower().Contains(kw))
                    .ToList();
            }
        }
        // tổng số phòng
        public int TongPhong()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.Phongs.Count();
            }
        }
        // phòng dang thuê
        public int PhongDangThue()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.Phongs
                         .Count(x =>
                         x.TrangThai == "Đang thuê");
            }
        }
        // phòng trống
        public int PhongTrong()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.Phongs
                         .Count(x =>
                         x.TrangThai == "Trống");
            }
        }
    }
}