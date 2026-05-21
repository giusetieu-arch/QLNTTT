using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS
{
    public class LichSuThietBi_BUS
    {
        LichSuThietBi_DAL dal =
        new LichSuThietBi_DAL();

        public string Insert(
            LichSuThietBi_DTO dto)
        {
            return dal.Insert(dto);
        }
    }
}
