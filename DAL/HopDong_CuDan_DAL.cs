using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HopDong_CuDan_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public List<CuDan> GetCuDan(string maHD)
        {
            return db.HopDong_CuDan
                .Where(x => x.MaHopDong == maHD)
                .Select(x => x.CuDan)
                .ToList();
        }
        // =========================
        // LOAD THEO HỢP ĐỒNG
        // =========================
        public int DemNguoiDangO(
    string maHD)
        {
            try
            {
                return db.HopDong_CuDan
                    .Count(x =>
                        x.MaHopDong == maHD
                        &&
                        x.TrangThai
                            == "Đang ở");
            }
            catch
            {
                return 0;
            }
        }
        public List<HopDong_CuDan_DTO>
            GetByHopDong(string maHD)
        {
            try
            {
                var ds = db.HopDong_CuDan
                    .Where(x =>
                        x.MaHopDong == maHD)
                    .Select(x =>
                        new HopDong_CuDan_DTO()
                        {
                            ID = x.ID,

                            MaHopDong = x.MaHopDong,

                            MaCuDan = x.MaCuDan,

                            TenCuDan =
                                x.CuDan.TenCuDan,

                            VaiTro = x.VaiTro,

                            NgayThamGia =
                                x.NgayThamGia,

                            NgayRoiKhoi =
                                x.NgayRoiKhoi,

                            TrangThai =
                                x.TrangThai
                        })
                    .ToList();

                return ds;
            }
            catch
            {
                return new List<HopDong_CuDan_DTO>();
            }
        }

        // =========================
        // THÊM CƯ DÂN
        // =========================

        public string Insert(
     HopDong_CuDan_DTO dto)
        {
            try
            {
                // =====================
                // CHECK ĐANG Ở PHÒNG KHÁC
                // =====================

                bool dangOPHongKhac =
                    db.HopDong_CuDan.Any(x =>
                        x.MaCuDan == dto.MaCuDan
                        &&
                        x.TrangThai == "Đang ở"
                        &&
                        x.MaHopDong != dto.MaHopDong);

                if (dangOPHongKhac)
                    return "Cư dân đang ở phòng khác";

                // =====================
                // CHECK ĐÃ TỪNG THUÊ
                // =====================

                var old =
                    db.HopDong_CuDan
                    .FirstOrDefault(x =>
                        x.MaHopDong == dto.MaHopDong
                        &&
                        x.MaCuDan == dto.MaCuDan);

                // =====================
                // ĐÃ TỒN TẠI -> UPDATE
                // =====================

                if (old != null)
                {
                    old.TrangThai =
                        "Đang ở";

                    old.NgayThamGia =
                        dto.NgayThamGia;

                    old.NgayRoiKhoi =
                        null;

                    old.VaiTro =
                        dto.VaiTro;

                    return db.SaveChanges() > 0
                        ? "success"
                        : "fail";
                }

                // =====================
                // INSERT MỚI
                // =====================

                HopDong_CuDan hdcd =
                    new HopDong_CuDan();

                hdcd.MaHopDong =
                    dto.MaHopDong;

                hdcd.MaCuDan =
                    dto.MaCuDan;

                hdcd.VaiTro =
                    dto.VaiTro;

                hdcd.NgayThamGia =
                    dto.NgayThamGia;

                hdcd.TrangThai =
                    dto.TrangThai;

                db.HopDong_CuDan.Add(hdcd);

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =========================
        // CHUYỂN ĐI
        // =========================

        public string ChuyenDi(
            string maHD,
            string maCuDan)
        {
            try
            {
                var item =
                    db.HopDong_CuDan
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaCuDan == maCuDan
                        &&
                        x.TrangThai == "Đang ở");

                if (item == null)
                    return "Không tìm thấy cư dân";

                item.TrangThai =
                    "Đã chuyển đi";

                item.NgayRoiKhoi =
                    DateTime.Now;

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =========================
        // ĐỔI ĐẠI DIỆN
        // =========================

        public string DoiDaiDien(
            string maHD,
            string maCuDanMoi)
        {
            try
            {
                // =====================
                // ĐẠI DIỆN CŨ
                // =====================

                var daiDienCu =
                    db.HopDong_CuDan
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.VaiTro == "Đại diện"
                        &&
                        x.TrangThai == "Đang ở");

                if (daiDienCu != null)
                {
                    daiDienCu.VaiTro =
                        "Thành viên";
                }

                // =====================
                // ĐẠI DIỆN MỚI
                // =====================

                var daiDienMoi =
                    db.HopDong_CuDan
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaCuDan == maCuDanMoi
                        &&
                        x.TrangThai == "Đang ở");

                if (daiDienMoi == null)
                    return "Không tìm thấy cư dân";

                daiDienMoi.VaiTro =
                    "Đại diện";

                // =====================
                // UPDATE HỢP ĐỒNG
                // =====================

                var hd =
                    db.HopDongs
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD);

                if (hd != null)
                {
                    hd.MaNguoiDaiDien =
                        maCuDanMoi;
                }

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
