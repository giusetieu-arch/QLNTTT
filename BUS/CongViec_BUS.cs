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
    public class CongViec_BUS
    {
        CongViec_DAL dal = new CongViec_DAL();
        public List<CongViec_DTO> GetAll()
        {
            return dal.GetAll();
        }

        public CongViec GetByID(string maCV)
        {
            return dal.GetByID(maCV);
        }

        public string Update(CongViec cv)
        {
            return dal.Update(cv);
        }
        public List<CongViec> GetTopBaoHongMoi()
        {
            return dal.GetTopBaoHongMoi();
        }
        public int BaoHongChuaXuLy()
        {
            return dal.BaoHongChuaXuLy();
        }
    }
}
