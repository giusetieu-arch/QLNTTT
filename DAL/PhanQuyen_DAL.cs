using DTO;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class PhanQuyen_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();

        // Lấy tất cả phân quyền
        public List<PhanQuyen> GetAll()
        {
            return db.PhanQuyens.ToList();
        }
    }
}