using DAL;
using System.Collections.Generic;

namespace BUS
{
    public class PhanQuyen_BUS
    {
        PhanQuyen_DAL dal = new PhanQuyen_DAL();

        public List<PhanQuyen> GetAll()
        {
            return dal.GetAll();
        }
    }
}