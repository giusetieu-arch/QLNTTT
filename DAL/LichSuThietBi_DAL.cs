using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class LichSuThietBi_DAL
    {
        QLNT_DoVanTieuEntities db =
       new QLNT_DoVanTieuEntities();

        public string Insert(
            LichSuThietBi_DTO dto)
        {
            try
            {
                LichSuThietBi ls =
                    new LichSuThietBi();

               

                ls.MaTaiSan =
                    dto.MaTaiSan;

                ls.MaPhong =
                    dto.MaPhong;

                ls.LoaiSuKien =
                    dto.LoaiSuKien;

                ls.MoTa =
                    dto.MoTa;

                ls.ChiPhi =
                    dto.ChiPhi;

                ls.NgayThucHien =
                    dto.NgayThucHien;

                ls.MaCongViec =
                    dto.MaCongViec;

                db.LichSuThietBis.Add(ls);

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}

