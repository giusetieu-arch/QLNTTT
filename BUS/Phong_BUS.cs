using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUS
{
    public class Phong_BUS
    {
        Phong_DAL dal = new Phong_DAL();
        public string UpdateChiSo(
     string maPhong,
     int dienMoi,
     int nuocMoi,
     string anhDien,
     string anhNuoc)
        {
            return dal.UpdateChiSo(
                maPhong,
                dienMoi,
                nuocMoi,
                anhDien,
                anhNuoc);
        }

        // 🔹 Lấy danh sách và chuyển đổi sang DTO
        public List<Phong_DTO> GetAll()
        {
            // Lấy list thực thể từ DAL
            var listEntity = dal.GetAll();

            // Chuyển đổi sang list DTO để tầng GUI sử dụng an toàn
            return listEntity.Select(p => new Phong_DTO
            {
                MaPhong = p.MaPhong,
                TenPhong = p.TenPhong,
                MaToaNha = p.MaToaNha,
                MaLoaiPhong = p.MaLoaiPhong,
                DienTich = p.DienTich,
                SoDienCu = p.SoDienCu,
                SoNuocCu = p.SoNuocCu,
                ACTD_Cu = p.ACTD_Cu,
                ACTN_Cu = p.ACTN_Cu,
                TienCoc = p.TienCoc,
                TrangThai = p.TrangThai,
                GiaThue = p.GiaThue
            }).ToList();
        }
        public Phong GetById(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return null;

            return dal.GetById(ma);
        }
        // 🔹 Validate chung: Kiểm tra tính hợp lệ của dữ liệu đầu vào
        private string Validate(Phong_DTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaPhong))
                return "Mã phòng không được để trống!";

            if (string.IsNullOrWhiteSpace(dto.TenPhong))
                return "Tên phòng không được để trống!";

            if (dto.GiaThue < 0)
                return "Giá thuê phải lớn hơn hoặc bằng 0!";

            if (dto.TienCoc < 0)
                return "Tiền cọc phải lớn hơn hoặc bằng 0!";

            return "ok";
        }

        // 🔹 Thêm phòng mới
        public string Insert_Phong(Phong_DTO dto)
        {
            string check = Validate(dto);
            if (check != "ok")
                return check;

            return dal.Insert(dto);
        }

        // 🔹 Cập nhật thông tin phòng
        public string Update_Phong(Phong_DTO dto)
        {
            string check = Validate(dto);
            if (check != "ok")
                return check;

            return dal.Update(dto);
        }

        // 🔹 Xóa phòng
        public string Delete_Phong(string maPhong)
        {
            if (string.IsNullOrWhiteSpace(maPhong))
                return "Vui lòng chọn phòng cần xóa!";

            return dal.Delete(maPhong);
        }

        // 🔹 Tìm kiếm (Bổ sung thêm để đồng bộ với DAL)
        public List<Phong_DTO> Search(string keyword)
        {
            var listEntity = dal.Search(keyword);
            return listEntity.Select(p => new Phong_DTO
            {
                MaPhong = p.MaPhong,
                TenPhong = p.TenPhong,
                MaToaNha = p.MaToaNha,
                MaLoaiPhong = p.MaLoaiPhong,
                DienTich = p.DienTich,
                SoDienCu = p.SoDienCu,
                SoNuocCu = p.SoNuocCu,
                ACTD_Cu = p.ACTD_Cu,
                ACTN_Cu = p.ACTN_Cu,
                TienCoc = p.TienCoc,
                TrangThai = p.TrangThai,
                GiaThue = p.GiaThue
            }).ToList();
        }
        public int TongPhong()
        {
            return dal.TongPhong();
        }
        public int PhongDangThue()
        {
            return dal.PhongDangThue();
        }
        public int PhongTrong()
        {
            return dal.PhongTrong();
        }
    }
}