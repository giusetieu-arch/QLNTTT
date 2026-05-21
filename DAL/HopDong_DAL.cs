using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DAL
{
    public class HopDong_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();

        public string Insert(HopDong_DTO dto)
        {
            try
            {
                if (db.HopDongs.Any(x => x.MaHopDong == dto.MaHopDong))
                    return "Mã hợp đồng đã tồn tại";

                HopDong hd = new HopDong()
                {
                    MaHopDong = dto.MaHopDong,
                    MaPhong = dto.MaPhong,
                    MaNguoiDaiDien = dto.MaNguoiDaiDien,
                    NgayBatDau = dto.NgayBatDau,
                    NgayKetThuc = dto.NgayKetThuc,
                    TienCoc = dto.TienCoc,
                    GiaThue = dto.GiaThue,
                    GiaDienChot = dto.GiaDienChot,
                    GiaNuocChot = dto.GiaNuocChot,
                    NgayTao = dto.NgayTao,
                    GhiChu = dto.GhiChu,
                    TrangThai = dto.TrangThai
                };

                db.HopDongs.Add(hd);

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public List<HopDong> GetAll()
        {
            return db.HopDongs.ToList();
        }

        public string ThanhLy(string maHD)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            using (var tran = db.Database.BeginTransaction())
            {
                try
                {
                    var hd = db.HopDongs
                        .FirstOrDefault(x => x.MaHopDong == maHD);

                    if (hd == null)
                        return "Không tìm thấy hợp đồng";

                    if (hd.TrangThai == "Đã thanh lý")
                        return "Hợp đồng đã thanh lý";

                    // =====================
                    // KIỂM TRA CÔNG NỢ
                    // =====================

                    var hoaDonChuaTT = db.HoaDons
                        .Where(x =>
                            x.MaHopDong == maHD &&
                            x.TrangThai != "Đã thanh toán")
                        .ToList();

                    if (hoaDonChuaTT.Count > 0)
                        return $"Còn {hoaDonChuaTT.Count} hóa đơn chưa thanh toán";

                    bool truocHan =
                        hd.NgayKetThuc.HasValue &&
                        DateTime.Now.Date <
                        hd.NgayKetThuc.Value.Date;

                    // =====================
                    // HỢP ĐỒNG
                    // =====================

                    hd.TrangThai = "Đã thanh lý";

                    hd.GhiChu = truocHan
                        ? $"Thanh lý trước hạn - mất cọc {hd.TienCoc:N0}"
                        : "Thanh lý đúng hạn";

                    // =====================
                    // PHÒNG
                    // =====================

                    var phong = db.Phongs
                        .FirstOrDefault(x =>
                            x.MaPhong == hd.MaPhong);

                    if (phong != null)
                        phong.TrangThai = "Trống";

                    // =====================
                    // CƯ DÂN
                    // =====================

                    var dsCuDan = db.HopDong_CuDan
                        .Where(x =>
                            x.MaHopDong == maHD &&
                            x.TrangThai == "Đang ở")
                        .ToList();

                    foreach (var item in dsCuDan)
                    {
                        item.TrangThai = "Đã chuyển đi";
                        item.NgayRoiKhoi = DateTime.Now;

                        var cd = db.CuDans
                            .FirstOrDefault(x =>
                                x.MaCuDan == item.MaCuDan);

                        if (cd != null)
                            cd.TrangThai = "Không ở";
                    }

                    // =====================
                    // DỊCH VỤ
                    // =====================

                    var dsDV = db.HopDong_DichVu
                        .Where(x =>
                            x.MaHopDong == maHD &&
                            x.TrangThai == "Đang sử dụng")
                        .ToList();

                    foreach (var dv in dsDV)
                    {
                        dv.TrangThai = "Ngưng sử dụng";
                        dv.NgayNgung = DateTime.Now;
                    }

                    // =====================
                    // TÀI SẢN
                    // =====================

                    var dsTS = db.HopDong_TaiSan
                        .Where(x =>
                            x.MaHopDong == maHD &&
                            x.TrangThai == "Đang sử dụng")
                        .ToList();

                    foreach (var ts in dsTS)
                    {
                        ts.TrangThai = "Đã thu hồi";
                        ts.NgayThuHoi = DateTime.Now;
                    }

                    // =====================
                    // LẤY SỐ DƯ CUỐI
                    // =====================

                    decimal soDuCuoi =
                        db.SoQuys
                        .OrderByDescending(x => x.ID)
                        .Select(x => (decimal?)x.SoDu)
                        .FirstOrDefault() ?? 0;

                    // =====================
                    // THANH LÝ TRƯỚC HẠN
                    // =====================

                    if (truocHan)
                    {
                        string maPhieu =
                            "PTC" +
                            DateTime.Now.ToString("yyyyMMddHHmmss");

                        // PHIẾU

                        PhieuThuChi pt = new PhieuThuChi()
                        {
                            MaPhieu = maPhieu,
                            LoaiPhieu = "Thu",
                            SoTien = 0,
                            NgayGiaoDich = DateTime.Now,
                            NguoiNopNhan = "Khách thuê",
                            PhuongThuc = "Tiền mặt",
                            NoiDung = $"Giữ cọc HĐ {maHD}"
                        };

                        db.PhieuThuChis.Add(pt);

                        // SỔ QUỸ

                        SoQuy sq = new SoQuy()
                        {
                            MaPhieu = maPhieu,
                            NgayGiaoDich = DateTime.Now,
                            LoaiGiaoDich = "Giữ cọc",
                            Thu = 0,
                            Chi = 0,
                            SoDu = soDuCuoi,
                            NoiDung = $"Giữ cọc HĐ {maHD}",
                            NguoiLap = "Admin"
                        };

                        db.SoQuys.Add(sq);
                    }
                    else
                    {
                        // =====================
                        // TRẢ CỌC
                        // =====================

                        string maPhieu =
                            "PC" +
                            DateTime.Now.ToString("yyyyMMddHHmmss");

                        PhieuThuChi pt = new PhieuThuChi()
                        {
                            MaPhieu = maPhieu,
                            LoaiPhieu = "Chi",
                            SoTien = hd.TienCoc,
                            NgayGiaoDich = DateTime.Now,
                            NguoiNopNhan = "Khách thuê",
                            PhuongThuc = "Tiền mặt",
                            NoiDung = $"Trả cọc HĐ {maHD}"
                        };

                        db.PhieuThuChis.Add(pt);

                        SoQuy sq = new SoQuy()
                        {
                            MaPhieu = maPhieu,
                            NgayGiaoDich = DateTime.Now,
                            LoaiGiaoDich = "Chi cọc",
                            Thu = 0,
                            Chi = hd.TienCoc,
                            SoDu = soDuCuoi - hd.TienCoc,
                            NoiDung = $"Trả cọc HĐ {maHD}",
                            NguoiLap = "Admin"
                        };

                        db.SoQuys.Add(sq);
                    }

                    db.SaveChanges();

                    tran.Commit();

                    return "success";
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    string loi = ex.Message;

                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        loi += "\n-----------------\n";
                        loi += ex.Message;
                    }

                    return loi;
                }
            }
        }
        public List<HopDong_DTO>GetDangHieuLuc()
        {
            try
            {
                var ds =
                    db.HopDongs
                    .Where(x =>
                        x.TrangThai
                            == "Đang hiệu lực")
                    .Select(x =>
                        new HopDong_DTO()
                        {
                            MaHopDong =
                                x.MaHopDong,

                            MaPhong =
                                x.MaPhong,

                            GiaThue =
                                x.GiaThue,

                            GiaDienChot =
                                x.GiaDienChot,

                            GiaNuocChot =
                                x.GiaNuocChot,

                            NgayBatDau =
                                x.NgayBatDau,

                            NgayKetThuc =
                                x.NgayKetThuc,

                            TrangThai =
                                x.TrangThai
                        })
                    .ToList();

                return ds;
            }
            catch
            {
                return new List<HopDong_DTO>();
            }
        }
        public List<HopDong_TaiSan_DTO> GetByHopDong(string maHD)
        {
            var ds =
            (
                from hdts in db.HopDong_TaiSan

                join ts in db.TaiSans
                on hdts.MaTaiSan equals ts.MaTaiSan

                where hdts.MaHopDong == maHD

                select new HopDong_TaiSan_DTO()
                {
                    ID = hdts.ID,

                    MaHopDong = hdts.MaHopDong,

                    MaTaiSan = hdts.MaTaiSan,

                    // LẤY TÊN TÀI SẢN
                    TenTaiSan = ts.TenTaiSan,

                    SoLuong = hdts.SoLuong,

                    TinhTrangBanDau =
                        hdts.TinhTrangBanDau,

                    TinhTrangKhiTra =
                        hdts.TinhTrangKhiTra,

                    NgayBanGiao =
                        hdts.NgayBanGiao,

                    NgayThuHoi =
                        hdts.NgayThuHoi,

                    TienDenBu =
                        hdts.TienDenBu,

                    GhiChu =
                        hdts.GhiChu,

                    TrangThai =
                        hdts.TrangThai
                }
            ).ToList();

            return ds;
        }
        public HopDong GetById(string maHD)
         {
             return db.HopDongs
                 .FirstOrDefault(x =>
                     x.MaHopDong == maHD);
         }
        public HopDong_DTO GetById2(
    string maHD)
        {
            return db.HopDongs
                .Where(x =>
                    x.MaHopDong == maHD)
                .Select(x =>
                    new HopDong_DTO()
                    {
                        MaHopDong =
                            x.MaHopDong,

                        MaPhong =
                            x.MaPhong,

                        GiaThue =
                            x.GiaThue,

                        GiaDienChot =
                            x.GiaDienChot,

                        GiaNuocChot =
                            x.GiaNuocChot,

                        TienCoc =
                            x.TienCoc,

                        NgayBatDau =
                            x.NgayBatDau,

                        NgayKetThuc =
                            x.NgayKetThuc,

                        TrangThai =
                            x.TrangThai
                    })
                .FirstOrDefault();
        }
        public HopDong_DTO GetById1(string maHD)
        {
            var hd = db.HopDongs
                .FirstOrDefault(x =>
                    x.MaHopDong == maHD);

            if (hd == null)
                return null;

            return new HopDong_DTO()
            {
                MaHopDong = hd.MaHopDong,
                MaPhong = hd.MaPhong,
                NgayBatDau = hd.NgayBatDau,
                NgayKetThuc = hd.NgayKetThuc,
                GiaThue = hd.GiaThue,
                TienCoc = hd.TienCoc,
                TrangThai = hd.TrangThai
            };
        }
        public string Update(HopDong_DTO dto)
        {
            try
            {
                var hd = db.HopDongs
                    .FirstOrDefault(x =>
                        x.MaHopDong == dto.MaHopDong);

                if (hd == null)
                    return "Không tìm thấy hợp đồng";

                // =========================
                // UPDATE
                // =========================

                hd.MaPhong = dto.MaPhong;

                hd.NgayBatDau = dto.NgayBatDau;

                hd.NgayKetThuc = dto.NgayKetThuc;

                hd.TienCoc = dto.TienCoc;

                hd.GiaThue = dto.GiaThue;

                hd.GiaDienChot = dto.GiaDienChot;

                hd.GiaNuocChot = dto.GiaNuocChot;

                hd.GhiChu = dto.GhiChu;

                hd.TrangThai = dto.TrangThai;

                // =========================
                // SAVE
                // =========================

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string LapHopDong(
     HopDong_DTO dto,
     DataGridView dgvCuDan,
     DataGridView dgvDichVu,
     DataGridView dgvTaiSan)
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                using (var tran = db.Database.BeginTransaction())
                {
                    try
                    {
                        // =====================
                        // CHECK TRÙNG
                        // =====================

                        if (db.HopDongs.Any(x =>
                            x.MaHopDong == dto.MaHopDong))
                        {
                            return "Mã hợp đồng đã tồn tại";
                        }

                        // =====================
                        // INSERT HỢP ĐỒNG
                        // =====================

                        HopDong hd = new HopDong();

                        hd.MaHopDong = dto.MaHopDong;
                        hd.MaPhong = dto.MaPhong;
                        hd.NgayBatDau = dto.NgayBatDau;
                        hd.NgayKetThuc = dto.NgayKetThuc;
                        hd.TienCoc = dto.TienCoc;
                        hd.GiaThue = dto.GiaThue;
                        hd.GiaDienChot = dto.GiaDienChot;
                        hd.GiaNuocChot = dto.GiaNuocChot;
                        hd.NgayTao = dto.NgayTao;
                        hd.GhiChu = dto.GhiChu;
                        hd.MaNguoiDaiDien = dto.MaNguoiDaiDien;
                        hd.TrangThai = dto.TrangThai;

                        db.HopDongs.Add(hd);

                        // =====================
                        // CƯ DÂN
                        // =====================

                        foreach (DataGridViewRow row in dgvCuDan.Rows)
                        {
                            if (row.IsNewRow) continue;

                            HopDong_CuDan ct =
                                new HopDong_CuDan();

                            ct.MaHopDong = dto.MaHopDong;

                            ct.MaCuDan =
                                row.Cells["MaCuDan"]
                                .Value.ToString();

                            ct.VaiTro =
                                row.Cells["VaiTro"]
                                .Value.ToString();

                            ct.NgayThamGia =
                                DateTime.Now;

                            ct.TrangThai =
                                "Đang ở";

                            db.HopDong_CuDan.Add(ct);

                            // cập nhật trạng thái cư dân

                            var cd = db.CuDans
                                .FirstOrDefault(x =>
                                    x.MaCuDan == ct.MaCuDan);

                            if (cd != null)
                            {
                                cd.TrangThai = "Đang ở";
                            }
                        }

                        // =====================
                        // DỊCH VỤ
                        // =====================

                        foreach (DataGridViewRow row in dgvDichVu.Rows)
                        {
                            if (row.IsNewRow) continue;

                            HopDong_DichVu dv =
                                new HopDong_DichVu();

                            dv.MaHopDong =
                                dto.MaHopDong;

                            dv.MaDichVu =
                                row.Cells["MaDichVu"]
                                .Value.ToString();

                            dv.DonGia =
                                Convert.ToDecimal(
                                    row.Cells["DonGia"]
                                    .Value);

                            dv.NgayBatDau =
                                DateTime.Now;

                            dv.TrangThai =
                                "Đang sử dụng";

                            db.HopDong_DichVu.Add(dv);
                        }

                        // =====================
                        // TÀI SẢN
                        // =====================

                        foreach (DataGridViewRow row in dgvTaiSan.Rows)
                        {
                            if (row.IsNewRow) continue;

                            HopDong_TaiSan ts =
                                new HopDong_TaiSan();

                            ts.MaHopDong =
                                dto.MaHopDong;

                            ts.MaTaiSan =
                                row.Cells["MaTaiSan"]
                                .Value.ToString();

                            ts.SoLuong =
                                Convert.ToInt32(
                                    row.Cells["SoLuong"]
                                    .Value);

                            ts.TinhTrangBanDau =
                                row.Cells["TinhTrangBanDau"]
                                .Value.ToString();

                            ts.TienDenBu =
                                Convert.ToDecimal(
                                    row.Cells["TienDenBu"]
                                    .Value);

                            ts.TrangThai =
                                "Đang sử dụng";

                            db.HopDong_TaiSan.Add(ts);
                        }

                        // =====================
                        // UPDATE PHÒNG
                        // =====================

                        var phong =
                            db.Phongs.Find(dto.MaPhong);

                        if (phong != null)
                        {
                            phong.TrangThai =
                                "Đang thuê";
                        }
                        // =====================
                        // THU TIỀN CỌC
                        // =====================

                        if (dto.TienCoc > 0)
                        {
                            string maPhieu = "PT" + DateTime.Now.ToString("yyyyMMddHHmmss");

                            // 1. Tạo phiếu thu chi
                            PhieuThuChi pt = new PhieuThuChi();

                            pt.MaPhieu = maPhieu;
                            pt.LoaiPhieu = "Thu";
                            pt.MaHoaDon = null;       // hợp đồng chưa phải hóa đơn
                            pt.MaCongViec = null;
                            pt.SoTien = dto.TienCoc;
                            pt.NgayGiaoDich = DateTime.Now;
                            pt.NguoiNopNhan = "Khách thuê";
                            pt.GhiChu = "Thu tiền cọc hợp đồng " + dto.MaHopDong;
                            pt.PhuongThuc = "Tiền mặt";
                            pt.NoiDung = "Tiền cọc nhà trọ";

                            db.PhieuThuChis.Add(pt);

                            // 2. Lấy số dư hiện tại
                            decimal soDuCuoi = 0;

                            var dongCuoi = db.SoQuys
                                .OrderByDescending(x => x.ID)
                                .FirstOrDefault();

                            if (dongCuoi != null)
                                soDuCuoi = dongCuoi.SoDu ?? 0;

                            // 3. Ghi sổ quỹ
                            SoQuy sq = new SoQuy();

                            sq.MaPhieu = maPhieu;   // ⭐ QUAN TRỌNG
                            sq.NgayGiaoDich = DateTime.Now;
                            sq.LoaiGiaoDich = "Thu";
                            sq.Thu = dto.TienCoc;
                            sq.Chi = 0;
                            sq.SoDu = soDuCuoi + dto.TienCoc;
                            sq.NoiDung = "Thu cọc HĐ " + dto.MaHopDong;
                            sq.NguoiLap = "Admin";

                            db.SoQuys.Add(sq);
                        }


                        // =====================
                        // SAVE
                        // =====================

                        db.SaveChanges();

                        tran.Commit();

                        return "success";
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();

                        string loi = ex.Message;

                        Exception inner = ex.InnerException;

                        while (inner != null)
                        {
                            loi += "\n------------------\n";
                            loi += inner.Message;
                            inner = inner.InnerException;
                        }

                        return loi;
                    }
                }
            }
        }
        public int HopDongSapHetHan()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                DateTime now = DateTime.Now;

                DateTime future =
                    now.AddDays(30);

                return db.HopDongs
                         .Count(x =>
                            x.NgayKetThuc >= now
                            && x.NgayKetThuc <= future);
            }
        }
    }
}
