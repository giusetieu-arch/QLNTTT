    using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class CuDan_BUS
    {
        CuDan_DAL dal = new CuDan_DAL();

        // ================= GET ALL =================
        public List<CuDan> GetAll()
        {
            return dal.GetAll();
        }
        public List<CuDan> LayCuDanHopLe()
        {
            return dal.LayCuDanHopLe();
        }
        // ================= GET BY ID =================
        public CuDan GetById(string ma)
        {
            return dal.GetById(ma);
        }

        // ================= EXISTS =================
        public bool Exists(string ma)
        {
            return dal.Exists(ma);
        }

        // ================= INSERT =================
        public string Insert(CuDan_DTO dto)
        {
            // validate
            if (string.IsNullOrWhiteSpace(dto.MaCuDan))
                return "Mã cư dân không được trống";

            if (string.IsNullOrWhiteSpace(dto.TenCuDan))
                return "Tên cư dân không được trống";

            if (string.IsNullOrWhiteSpace(dto.CCCD))
                return "CCCD không được trống";

            if (dto.CCCD.Length != 12)
                return "CCCD phải 12 số";

            if (string.IsNullOrWhiteSpace(dto.SDT))
                return "SĐT không được trống";

            return dal.Insert(dto);
        }

        // ================= UPDATE =================
        public string Update(CuDan_DTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.TenCuDan))
                return "Tên cư dân không được trống";

            if (dto.CCCD.Length != 12)
                return "CCCD phải 12 số";

            return dal.Update(dto);
        }

        // ================= DELETE =================
        public string Delete(string ma)
        {
            return dal.Delete(ma);
        }

        // ================= SEARCH =================
        public List<CuDan> Search(string keyword)
        {
            return dal.Search(keyword);
        }
        public int TongCuDan()
        {
            return dal.TongCuDan();
        }
    }
}
