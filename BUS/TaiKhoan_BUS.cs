using DAL;
using DTO;
using System.Collections.Generic;

namespace BUS
{
    public class TaiKhoan_BUS
    {
        TaiKhoan_DAL dal = new TaiKhoan_DAL();

        // Lấy tất cả
        public List<TaiKhoan_DTO> GetAll()
        {
            return dal.GetAll();
        }

        // Thêm
        public bool Insert(TaiKhoan_DTO dto)
        {
            return dal.Insert(dto);
        }

        // Sửa
        public bool Update(TaiKhoan_DTO dto)
        {
            return dal.Update(dto);
        }

        // Xóa
        public bool Delete(string maTK)
        {
            return dal.Delete(maTK);
        }

        // Tìm theo mã
        public TaiKhoan_DTO GetById(string maTK)
        {
            return dal.GetById(maTK);
        }

        // Đăng nhập
        public TaiKhoan_DTO Login(string username, string password)
        {
            return dal.Login(username, password);
        }
    }
}