using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class CuDan_DAL
    {
        // ================= GET ALL =================
        public List<CuDan> GetAll()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans.ToList();
            }
        }

        // ================= GET BY ID =================
        public CuDan GetById(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans.Find(ma);
            }
        }
        public List<CuDan> LayCuDanHopLe()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans
                    .Where(cd =>
                        !db.HopDong_CuDan.Any(hd =>
                            hd.MaCuDan == cd.MaCuDan &&
                            hd.TrangThai == "Đang ở"
                        ))
                    .ToList();
            }
        }

        // ================= CHECK TỒN TẠI =================
        public bool Exists(string ma)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans.Any(x => x.MaCuDan == ma);
            }
        }

        // ================= CHECK CCCD =================
        public bool CheckCCCD(string cccd)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans.Any(x => x.CCCD == cccd);
            }
        }

        // ================= CHECK SDT =================
        public bool CheckSDT(string sdt)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans.Any(x => x.SDT == sdt);
            }
        }

        // ================= INSERT =================
        public string Insert(CuDan_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    if (db.CuDans.Any(x => x.MaCuDan == dto.MaCuDan))
                        return "Mã cư dân đã tồn tại";

                    if (db.CuDans.Any(x => x.CCCD == dto.CCCD))
                        return "CCCD đã tồn tại";

                    if (db.CuDans.Any(x => x.SDT == dto.SDT))
                        return "SĐT đã tồn tại";
                    CuDan cd = new CuDan()
                    {
                        MaCuDan = dto.MaCuDan,
                        TenCuDan = dto.TenCuDan,
                        NgaySinh = dto.NgaySinh,
                        GioiTinh = dto.GioiTinh,
                        Email = dto.Email,
                        CCCD = dto.CCCD,
                        SDT = dto.SDT,
                        QueQuan = dto.QueQuan,
                        NgayTao = dto.NgayTao,
                        TrangThai = dto.TrangThai
                    };

                    db.CuDans.Add(cd);

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // ================= UPDATE =================
        public string Update(CuDan_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var cd = db.CuDans.Find(dto.MaCuDan);

                    if (cd == null)
                        return "Không tìm thấy cư dân";

                    // check CCCD trùng
                    bool cccdExists = db.CuDans.Any(x =>
                        x.CCCD == dto.CCCD &&
                        x.MaCuDan != dto.MaCuDan);

                    if (cccdExists)
                        return "CCCD đã tồn tại";

                    // check SDT trùng
                    bool sdtExists = db.CuDans.Any(x =>
                        x.SDT == dto.SDT &&
                        x.MaCuDan != dto.MaCuDan);

                    if (sdtExists)
                        return "SĐT đã tồn tại";

                    cd.TenCuDan = dto.TenCuDan;
                    cd.NgaySinh = dto.NgaySinh;
                    cd.GioiTinh = dto.GioiTinh;
                    cd.Email = dto.Email;
                    cd.CCCD = dto.CCCD;
                    cd.SDT = dto.SDT;
                    cd.QueQuan = dto.QueQuan;
                    cd.TrangThai = dto.TrangThai;

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // ================= DELETE =================
        public string Delete(string ma)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var cd = db.CuDans.Find(ma);

                    if (cd == null)
                        return "Không tồn tại";

                    db.CuDans.Remove(cd);

                    return db.SaveChanges() > 0 ? "success" : "fail";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi ràng buộc: " + ex.Message;
            }
        }

        // ================= SEARCH =================
        public List<CuDan> Search(string keyword)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans
                    .Where(x =>
                        x.TenCuDan.Contains(keyword) ||
                        x.CCCD.Contains(keyword) ||
                        x.SDT.Contains(keyword))
                    .ToList();
            }
        }
        // tông cu dan
        public int TongCuDan()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.CuDans.Count();
            }
        }
    }
}
