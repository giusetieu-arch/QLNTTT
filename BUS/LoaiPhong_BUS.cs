using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BUS
{
    public class LoaiPhong_BUS
    {
        LoaiPhong_DAL dal = new LoaiPhong_DAL();

        // 🔹 Lấy tất cả và chuyển đổi sang DTO
        public List<LoaiPhong_DTO> GetAll()
        {
            // Gọi DAL lấy danh sách Entity (đã dùng AsNoTracking ở DAL)
            var listEntity = dal.GetAll();

            // Mapping: Chuyển từ List<LoaiPhong> sang List<LoaiPhong_DTO>
            // Việc này giúp ngắt hoàn toàn liên kết với Database Proxy ở tầng GUI
            return listEntity.Select(x => new LoaiPhong_DTO
            {
                MaLoaiPhong = x.MaLoaiPhong,
                TenLoaiPhong = x.TenLoaiPhong,
                SoNguoiToiDa = (int)x.SoNguoiToiDa,
                GiaThueMacDinh = (decimal)x.GiaThueMacDinh,
                DonGiaDien = (decimal)x.DonGiaDien,
                DonGiaNuoc = (decimal)x.DonGiaNuoc,
                MoTa = x.MoTa,
                TrangThai = x.TrangThai
            }).ToList();
        }
        public LoaiPhong GetById(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return null;

            return dal.GetById(ma);
        }

        // 🔹 Thêm loại phòng
        public string Insert_LoaiPhong(LoaiPhong_DTO dto)
        {
            string check = Validate(dto);
            if (check != "valid") return check;

            // Kiểm tra trùng mã trước khi gọi DAL (Tăng tính an toàn)
            if (dal.Exists(dto.MaLoaiPhong))
                return "Mã loại phòng này đã tồn tại trong hệ thống!";

            return dal.Insert(dto);
        }

        // 🔹 Cập nhật loại phòng
        public string Update_LoaiPhong(LoaiPhong_DTO dto)
        {
            string check = Validate(dto);
            if (check != "valid") return check;

            return dal.Update(dto);
        }

        // 🔹 Xóa loại phòng
        public string Delete_LoaiPhong(string ma)
        {
            if (string.IsNullOrEmpty(ma))
                return "Vui lòng chọn loại phòng cần xóa!";

            return dal.Delete(ma);
        }
       

        public int TongLoaiPhong()
        {
            return dal.TongLoaiPhong();
        }

        public List<LoaiPhong> TimTheoSoNguoi(int soNguoi)
        {
            return dal.TimTheoSoNguoi(soNguoi);
        }

        // 🔥 Hàm hỗ trợ kiểm tra dữ liệu đầu vào (Validation)
        private string Validate(LoaiPhong_DTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaLoaiPhong))
                return "Mã loại phòng không được để trống!";

            if (string.IsNullOrWhiteSpace(dto.TenLoaiPhong))
                return "Tên loại phòng không được để trống!";

            if (dto.SoNguoiToiDa <= 0)
                return "Số người tối đa phải lớn hơn 0!";

            if (dto.GiaThueMacDinh < 0)
                return "Giá thuê không được là số âm!";

            if (dto.DonGiaDien < 0 || dto.DonGiaNuoc < 0)
                return "Đơn giá điện/nước không hợp lệ!";

            return "valid";
        }
    }
}