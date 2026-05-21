using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class DanhMucDichVu_BUS
    {
        private DanhMucDichVu_DAL dal = new DanhMucDichVu_DAL();

        public List<DanhMucDichVu> LayTatCa()
        {
            return dal.GetAll();
        }

        public string Them(DanhMucDichVu_DTO dto)
        {
            // 1. Kiểm tra để trống các trường bắt buộc
            if (string.IsNullOrWhiteSpace(dto.MaDichVu)) return "Mã dịch vụ bắt buộc nhập.";
            if (string.IsNullOrWhiteSpace(dto.TenDichVu)) return "Tên dịch vụ bắt buộc nhập.";

            // 2. Kiểm tra giá trị âm
            if (dto.DonGia.HasValue && dto.DonGia < 0)
                return "Đơn giá không được là số âm.";

            // 3. Chuẩn hóa dữ liệu (Viết hoa mã)
            dto.MaDichVu = dto.MaDichVu.Trim().ToUpper();

            return dal.Insert(dto);
        }

        public string Sua(DanhMucDichVu_DTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.MaDichVu)) return "Mã dịch vụ không hợp lệ.";
            if (string.IsNullOrWhiteSpace(dto.TenDichVu)) return "Tên dịch vụ không được để trống.";

            if (dto.DonGia.HasValue && dto.DonGia < 0)
                return "Đơn giá không được là số âm.";

            return dal.Update(dto);
        }

        public string Xoa(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma)) return "Mã không hợp lệ.";

            

            return dal.Delete(ma);
        }
    }
}
