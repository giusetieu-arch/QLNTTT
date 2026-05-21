using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class ChiTietHoaDon_BUS
    {
        ChiTietHoaDon_DAL dal =
         new ChiTietHoaDon_DAL();

        public string Insert(
            ChiTietHoaDon_DTO dto)
        {
            return dal.Insert(dto);
        }
    }
}
