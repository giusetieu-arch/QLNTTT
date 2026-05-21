using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class TaiSan_BUS
    {
        TaiSan_DAL dal = new TaiSan_DAL();

        // 🔹 Lấy tất cả
        public List<TaiSan> GetAll()
        {
            return dal.GetAll();
        }

        // 🔹 Lấy theo mã
        public TaiSan GetById(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return null;

            return dal.GetById(ma);
        }
        public List<TaiSan> GetByPhong(string maPhong)
        {
            return dal.GetByPhong(maPhong);
        }
        // 🔹 Thêm
        public string Insert(TaiSan_DTO dto)
        {
            if (dto == null)
                return "Dữ liệu không hợp lệ";

            if (string.IsNullOrWhiteSpace(dto.MaTaiSan))
                return "Mã tài sản không được trống";

            if (string.IsNullOrWhiteSpace(dto.TenTaiSan))
                return "Tên tài sản không được trống";

            return dal.Insert(dto);
        }

        // 🔹 Sửa
        public string Update(TaiSan_DTO dto)
        {
            if (dto == null)
                return "Dữ liệu không hợp lệ";

            if (string.IsNullOrWhiteSpace(dto.MaTaiSan))
                return "Chưa có mã tài sản";

            return dal.Update(dto);
        }

        // 🔹 Xóa
        public string Delete(string ma)
        {
            if (string.IsNullOrWhiteSpace(ma))
                return "Chưa chọn tài sản";

            return dal.Delete(ma);
        }

        // 🔹 Search
        public List<TaiSan> Search(string keyword)
        {
            return dal.Search(keyword);
        }
    }
}
