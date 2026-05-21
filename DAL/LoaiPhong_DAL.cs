using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity; // Cần thêm để dùng AsNoTracking

namespace DAL
{
    public class LoaiPhong_DAL
    {
        // 🔹 Lấy tất cả - Tối ưu với AsNoTracking
        public List<LoaiPhong> GetAll()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                // AsNoTracking giúp ngắt các proxy gây lỗi Lazy Loading khi db bị dispose
                return db.LoaiPhongs.AsNoTracking().ToList();
            }
        }
        public LoaiPhong GetById(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.LoaiPhongs.Find(ma);
            }
        }

        // 🔹 Kiểm tra tồn tại
        public bool Exists(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.LoaiPhongs.Any(x => x.MaLoaiPhong == ma);
            }
        }

        // 🔹 Thêm (Giữ nguyên logic của bạn)
        public string Insert(LoaiPhong_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    if (db.LoaiPhongs.Any(x => x.MaLoaiPhong == dto.MaLoaiPhong))
                        return "Mã loại phòng đã tồn tại!";

                    LoaiPhong lp = new LoaiPhong()
                    {
                        MaLoaiPhong = dto.MaLoaiPhong,
                        TenLoaiPhong = dto.TenLoaiPhong,
                        SoNguoiToiDa = dto.SoNguoiToiDa,
                        GiaThueMacDinh = dto.GiaThueMacDinh,
                        DonGiaDien = dto.DonGiaDien,
                        DonGiaNuoc = dto.DonGiaNuoc,
                        MoTa = dto.MoTa,
                        TrangThai = dto.TrangThai
                    };

                    db.LoaiPhongs.Add(lp);
                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // 🔹 Update
        public string Update(LoaiPhong_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var lp = db.LoaiPhongs.Find(dto.MaLoaiPhong);
                    if (lp == null) return "Không tìm thấy!";

                    lp.TenLoaiPhong = dto.TenLoaiPhong;
                    lp.SoNguoiToiDa = dto.SoNguoiToiDa;
                    lp.GiaThueMacDinh = dto.GiaThueMacDinh;
                    lp.DonGiaDien = dto.DonGiaDien;
                    lp.DonGiaNuoc = dto.DonGiaNuoc;
                    lp.MoTa = dto.MoTa;
                    lp.TrangThai = dto.TrangThai;

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // 🔹 Delete
        public string Delete(string ma)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var lp = db.LoaiPhongs.Find(ma);
                    if (lp == null) return "Không tồn tại!";

                    db.LoaiPhongs.Remove(lp);
                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception)
            {
                return "Không thể xóa (có ràng buộc dữ liệu với các phòng hiện có)";
            }
        }


            QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();

            // Tổng số loại phòng
            public int TongLoaiPhong()
            {
                return db.LoaiPhongs.Count();
            }

            // Tìm loại phòng theo số người ở
            public List<LoaiPhong> TimTheoSoNguoi(int soNguoi)
            {
                return db.LoaiPhongs
                         .Where(x => x.SoNguoiToiDa >= soNguoi)
                         .ToList();
            }
    }
}