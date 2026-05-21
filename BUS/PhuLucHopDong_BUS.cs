using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BUS
{
    public class PhuLucHopDong_BUS
    {
        PhuLucHopDong_DAL dao = new PhuLucHopDong_DAL   ();
        public List<PhuLucHopDong_DTO>
           GetByHopDong(string maHD)
        {
            return dao.GetByHopDong(maHD);
        }
        public string ApDungPhuLuc()
        {
            return dao.ApDungPhuLuc();
        }
        public string Insert(PhuLucHopDong_DTO dto)
        {
            // Kiểm tra dữ liệu đầu vào
            if (dto == null)
            {
                return "Dữ liệu không hợp lệ";
            }

            if (string.IsNullOrEmpty(dto.MaHopDong))
            {
                return "Mã hợp đồng không được để trống";
            }

            if (string.IsNullOrEmpty(dto.LoaiPhuLuc))
            {
                return "Loại phụ lục không được để trống";
            }

            // Có thể bổ sung thêm validate nếu cần

            return dao.Insert(dto);
        }
    }
}
