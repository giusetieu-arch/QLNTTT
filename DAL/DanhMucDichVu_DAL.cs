using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DanhMucDichVu_DAL
    {
        // 🔹 Lấy tất cả danh mục (Fix lỗi Disposed Context)
        public List<DanhMucDichVu> GetAll()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                db.Configuration.ProxyCreationEnabled = false;
                db.Configuration.LazyLoadingEnabled = false;
                return db.DanhMucDichVus.AsNoTracking().ToList();
            }
        }

        // 🔹 Thêm mới
        public string Insert(DanhMucDichVu_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    if (db.DanhMucDichVus.Any(x => x.MaDichVu == dto.MaDichVu))
                        return "Mã dịch vụ đã tồn tại trong hệ thống.";

                    var dv = new DanhMucDichVu
                    {
                        MaDichVu = dto.MaDichVu,
                        TenDichVu = dto.TenDichVu,
                        DonGia = dto.DonGia,
                        DonViTinh = dto.DonViTinh,
                        HinhThucTinh = dto.HinhThucTinh ?? "",  
                        GhiChu = dto.GhiChu,
                        TrangThai = dto.TrangThai
                    };

                    db.DanhMucDichVus.Add(dv);
                    db.SaveChanges();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi DAL: " + ex.Message;
            }
        }

        // 🔹 Cập nhật
        public string Update(DanhMucDichVu_DTO dto)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var dv = db.DanhMucDichVus.Find(dto.MaDichVu);
                    if (dv == null) return "Không tìm thấy dịch vụ.";

                    dv.TenDichVu = dto.TenDichVu;
                    dv.DonGia = dto.DonGia;
                    dv.DonViTinh = dto.DonViTinh;
                    dv.HinhThucTinh = dto.HinhThucTinh;
                    dv.GhiChu = dto.GhiChu;
                    dv.TrangThai = dto.TrangThai;

                    db.SaveChanges();
                    return "success";
                }
            }
            catch (Exception ex)
            {
                return "Lỗi DAL: " + ex.Message;
            }
        }

        // 🔹 Xóa (Bắt lỗi ràng buộc khóa ngoại)
        public string Delete(string ma)
        {
            try
            {
                using (var db = new QLNT_DoVanTieuEntities())
                {
                    var dv = db.DanhMucDichVus.Find(ma);
                    if (dv == null) return "Dịch vụ không tồn tại.";

                    db.DanhMucDichVus.Remove(dv);
                    db.SaveChanges();
                    return "success";
                }
            }
            catch (Exception)
            {
                return "Lỗi: Dịch vụ này đã được dùng trong Hóa đơn/Hợp đồng, không thể xóa!";
            }
        }
    }
}
