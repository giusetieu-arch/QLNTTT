using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using System.Data.Entity;

namespace DAL
{
    public class ToaNha_DAL
    {
        // 🔹 Lấy tất cả
        public List<ToaNha> GetAll()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.ToaNhas.ToList();
            }
        }

        // 🔹 Lấy theo mã
        public ToaNha GetById(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.ToaNhas.Find(ma);
            }
        }

        // 🔹 Kiểm tra tồn tại
        public bool Exists(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.ToaNhas.Any(x => x.MaToaNha == ma);
            }
        }

        // 🔹 Thêm
        public string Insert(ToaNha_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    if (db.ToaNhas.Any(x => x.MaToaNha == dto.MaToaNha))
                        return "Mã đã tồn tại";

                    ToaNha tn = new ToaNha()
                    {
                        MaToaNha = dto.MaToaNha,
                        TenToaNha = dto.TenToaNha,
                        DiaChi = dto.DiaChi,
                        SoTang = dto.SoTang,
                        SoPhong = dto.SoPhong,
                        TrangThai = dto.TrangThai
                    };

                    db.ToaNhas.Add(tn);
                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // 🔹 Cập nhật
        public string Update(ToaNha_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var tn = db.ToaNhas.Find(dto.MaToaNha);

                    if (tn == null)
                        return "Không tìm thấy";

                    tn.TenToaNha = dto.TenToaNha;
                    tn.DiaChi = dto.DiaChi;
                    tn.SoTang = dto.SoTang;
                    tn.SoPhong = dto.SoPhong;
                    tn.TrangThai = dto.TrangThai;

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // 🔹 Xóa
        public string Delete(string ma)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var tn = db.ToaNhas.Find(ma);

                    if (tn == null)
                        return "Không tồn tại";

                    db.ToaNhas.Remove(tn);
                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi ràng buộc: " + ex.Message;
            }
        }

        // 🔹 Tìm kiếm (LIKE)
        public List<ToaNha> Search(string keyword)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.ToaNhas
                    .Where(x => x.TenToaNha.Contains(keyword) || x.DiaChi.Contains(keyword))
                    .ToList();
            }
        }
    }
}