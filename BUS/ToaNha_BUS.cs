using System;
using System.Collections.Generic;
using DAL;
using DTO;

namespace BUS
{
    public class ToaNha_BUS
    {
        ToaNha_DAL dal = new ToaNha_DAL();

        // 🔹 Lấy tất cả
        public List<ToaNha> GetAll()
        {
            return dal.GetAll();
        }

        // 🔹 Lấy theo mã
        public ToaNha GetById(string ma)
        {
            return dal.GetById(ma);
        }

        // 🔹 Thêm
        public string Insert_ToaNha(ToaNha_DTO dto)
        {
            if (string.IsNullOrEmpty(dto.MaToaNha))
                return "Mã không được rỗng";

            if (string.IsNullOrEmpty(dto.TenToaNha))
                return "Tên không được rỗng";

            return dal.Insert(dto);
        }

        // 🔹 Update
        public string Update_ToaNha(ToaNha_DTO dto)
        {
            if (string.IsNullOrEmpty(dto.MaToaNha))
                return "Chưa chọn tòa nhà";

            return dal.Update(dto);
        }

        // 🔹 Delete
        public string Delete_ToaNha(string ma)
        {
            if (string.IsNullOrEmpty(ma))
                return "Thiếu mã";

            return dal.Delete(ma);
        }

        // 🔹 Search
        public List<ToaNha> Search(string keyword)
        {
            return dal.Search(keyword);
        }
    }
}