using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class TaiKhoan_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities ();

        // Lấy tất cả tài khoản
        public List<TaiKhoan_DTO> GetAll()
        {
            return db.TaiKhoans.Select(tk => new TaiKhoan_DTO
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                Username = tk.Username,
                Password = tk.Password,
                MaQuyen = tk.MaQuyen,
                MaCuDan = tk.MaCuDan,
                NgayTao = tk.NgayTao ?? DateTime.Now,
                TrangThai = tk.TrangThai
            }).ToList();
        }

        // Thêm tài khoản
        public bool Insert(TaiKhoan_DTO dto)
        {
            try
            {
                TaiKhoan tk = new TaiKhoan();

                tk.MaTaiKhoan = dto.MaTaiKhoan;
                tk.Username = dto.Username;
                tk.Password = dto.Password;
                tk.MaQuyen = dto.MaQuyen;
                tk.MaCuDan = dto.MaCuDan;
                tk.NgayTao = dto.NgayTao;
                tk.TrangThai = dto.TrangThai;

                db.TaiKhoans.Add(tk);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Sửa tài khoản
        public bool Update(TaiKhoan_DTO dto)
        {
            try
            {
                var tk = db.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == dto.MaTaiKhoan);

                if (tk == null)
                    return false;

                tk.Username = dto.Username;
                tk.Password = dto.Password;
                tk.MaQuyen = dto.MaQuyen;
                tk.MaCuDan = dto.MaCuDan;
                tk.TrangThai = dto.TrangThai;

                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Xóa tài khoản
        public bool Delete(string maTK)
        {
            try
            {
                var tk = db.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == maTK);

                if (tk == null)
                    return false;

                db.TaiKhoans.Remove(tk);
                db.SaveChanges();

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Tìm theo mã
        public TaiKhoan_DTO GetById(string maTK)
        {
            var tk = db.TaiKhoans.FirstOrDefault(x => x.MaTaiKhoan == maTK);

            if (tk == null)
                return null;

            return new TaiKhoan_DTO
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                Username = tk.Username,
                Password = tk.Password,
                MaQuyen = tk.MaQuyen,
                MaCuDan = tk.MaCuDan,
                NgayTao = tk.NgayTao ?? DateTime.Now,
                TrangThai = tk.TrangThai
            };
        }

        // Đăng nhập
        public TaiKhoan_DTO Login(string username, string password)
        {
            var tk = db.TaiKhoans.FirstOrDefault(x =>
                x.Username == username &&
                x.Password == password &&
                x.TrangThai == "Hoạt động");

            if (tk == null)
                return null;

            return new TaiKhoan_DTO
            {
                MaTaiKhoan = tk.MaTaiKhoan,
                Username = tk.Username,
                Password = tk.Password,
                MaQuyen = tk.MaQuyen,
                MaCuDan = tk.MaCuDan,
                NgayTao = tk.NgayTao ?? DateTime.Now,
                TrangThai = tk.TrangThai
            };
        }
    }
}