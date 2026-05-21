using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HopDong_TaiSan_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public List<HopDong_TaiSan> GetTS(string maHD)
        {
            return db.HopDong_TaiSan
                .Where(x => x.MaHopDong == maHD)
                .ToList();
        }
        // =====================
        // LOAD
        // =====================

        public List<HopDong_TaiSan_DTO>
            GetByHopDong(string maHD)
        {
            try
            {
                return db.HopDong_TaiSan
                    .Where(x =>
                        x.MaHopDong == maHD)
                    .Select(x =>
                        new HopDong_TaiSan_DTO()
                        {
                            ID = x.ID,

                            MaHopDong =
                                x.MaHopDong,

                            MaTaiSan =
                                x.MaTaiSan,

                            TenTaiSan =
                                x.TaiSan.TenTaiSan,

                            SoLuong =
                                x.SoLuong,

                            GiaTri =
                                x.TaiSan.GiaTri,

                            TinhTrangBanDau =
                                x.TinhTrangKhiTra,

                            NgayBanGiao =
                                x.NgayBanGiao,

                            NgayThuHoi =
                                x.NgayThuHoi,

                            TrangThai =
                                x.TrangThai
                        })
                    .ToList();
            }
            catch
            {
                return new List
                    <HopDong_TaiSan_DTO>();
            }
        }

        // =====================
        // INSERT
        // =====================

        public string Insert(
            HopDong_TaiSan_DTO dto)
        {
            try
            {
                bool check =
                    db.HopDong_TaiSan.Any(x =>
                        x.MaHopDong
                            == dto.MaHopDong
                        &&
                        x.MaTaiSan
                            == dto.MaTaiSan
                        &&
                        x.TrangThai
                            == "Đang sử dụng");

                if (check)
                {
                    return
                        "Tài sản đã tồn tại";
                }

                HopDong_TaiSan ts =
                    new HopDong_TaiSan();

                ts.MaHopDong =
                    dto.MaHopDong;

                ts.MaTaiSan =
                    dto.MaTaiSan;

                ts.SoLuong =
                    dto.SoLuong;

                ts.TinhTrangKhiTra = dto.TinhTrangKhiTra;

                ts.NgayBanGiao =
                    dto.NgayBanGiao;

                ts.TrangThai =
                    dto.TrangThai;

                db.HopDong_TaiSan.Add(ts);

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =====================
        // THU HỒI
        // =====================

        public string ThuHoi(
            string maHD,
            string maTS)
        {
            try
            {
                var ts =
                    db.HopDong_TaiSan
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaTaiSan == maTS
                        &&
                        x.TrangThai
                            == "Đang sử dụng");

                if (ts == null)
                {
                    return
                        "Không tìm thấy tài sản";
                }

                ts.TrangThai =
                    "Đã thu hồi";

                ts.NgayThuHoi =
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

        // =====================
        // BÁO HỎNG
        // =====================

        public string BaoHong(
            string maHD,
            string maTS)
        {
            try
            {
                var ts =
                    db.HopDong_TaiSan
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaTaiSan == maTS
                        &&
                        x.TrangThai
                            == "Đang sử dụng");

                if (ts == null)
                {
                    return
                        "Không tìm thấy tài sản";
                }

                ts.TinhTrangKhiTra =
                    "Hỏng";

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string SuDungLai(
     string maHD,
     string maTS)
        {
            try
            {
                maHD = maHD.Trim();
                maTS = maTS.Trim();

                var ts = db.HopDong_TaiSan
                    .FirstOrDefault(x =>
                        x.MaHopDong.Trim() == maHD
                        &&
                        x.MaTaiSan.Trim() == maTS);

                if (ts == null)
                {
                    return "Không tìm thấy tài sản trong hợp đồng";
                }

                ts.TrangThai = "Đang dùng";

                ts.NgayThuHoi = null;

                ts.NgayBanGiao = DateTime.Now;

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
