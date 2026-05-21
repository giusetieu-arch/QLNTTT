using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class PhieuThuChi_DAL
    {
        QLNT_DoVanTieuEntities db =
            new QLNT_DoVanTieuEntities();
        public List<PhieuThuChi_DTO> TimKiem(
    string loai,
    string keyword)
        {
           

            var query =
                db.PhieuThuChis.AsQueryable();

            // ======================
            // LỌC LOẠI PHIẾU
            // ======================

            if (loai != "Tất cả")
            {
                query = query.Where(x =>
                    x.LoaiPhieu == loai);
            }

            // ======================
            // TÌM KIẾM
            // ======================

            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x =>
                    x.MaPhieu.Contains(keyword)
                    || x.NoiDung.Contains(keyword)
                    || x.NguoiNopNhan.Contains(keyword));
            }

            // ======================
            // RETURN
            // ======================

            return query
                .OrderByDescending(x =>
                    x.NgayGiaoDich)
                .Select(x => new PhieuThuChi_DTO
                {
                    MaPhieu = x.MaPhieu,
                    LoaiPhieu = x.LoaiPhieu,
                    MaHoaDon = x.MaHoaDon,
                    MaCongViec = x.MaCongViec,
                    SoTien = x.SoTien,
                    NgayGiaoDich = x.NgayGiaoDich,
                    NguoiNopNhan = x.NguoiNopNhan,
                    GhiChu = x.GhiChu,
                    PhuongThuc = x.PhuongThuc,
                    NoiDung = x.NoiDung
                })
                .ToList();
        }

        // =========================
        // INSERT
        // =========================

        public string Insert(
            PhieuThuChi_DTO dto)
        {
            try
            {
                PhieuThuChi p =
                    new PhieuThuChi();

                p.MaPhieu =
                    dto.MaPhieu;

                p.LoaiPhieu =
                    dto.LoaiPhieu;

                p.MaHoaDon =
                    dto.MaHoaDon;

                p.SoTien =
                    dto.SoTien;

                p.NgayGiaoDich =
                    dto.NgayGiaoDich;

                p.NguoiNopNhan =
                    dto.NguoiNopNhan;

                p.PhuongThuc =
                    dto.PhuongThuc;

                p.NoiDung =
                    dto.NoiDung;

                db.PhieuThuChis.Add(p);

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =========================
        // GET ALL
        // =========================

        public object GetAll()
        {
            return db.PhieuThuChis
                .OrderByDescending(x =>
                    x.NgayGiaoDich)
                .ToList();
        }
        public List<PhieuThuChi_DTO> GetAll1()
        {
            
            return db.PhieuThuChis
                .Select(x => new PhieuThuChi_DTO
                {
                    MaPhieu = x.MaPhieu,
                    LoaiPhieu = x.LoaiPhieu,
                    NgayGiaoDich = x.NgayGiaoDich,
                    SoTien = x.SoTien,
                    PhuongThuc = x.PhuongThuc,
                    NoiDung = x.NoiDung,
                    NguoiNopNhan = x.NguoiNopNhan,
                    MaHoaDon = x.MaHoaDon
                })
                .ToList();
        }

        // =========================
        // AUTO MÃ
        // =========================

        public string TaoMa()
        {
            int count =
                db.PhieuThuChis.Count()
                + 1;

            return "PT"
                + count.ToString("0000");
        }
        public List<PhieuThuChi_DTO> GetLichSuThanhToan(string maHD)
        {
            QLNT_DoVanTieuEntities db =
                new QLNT_DoVanTieuEntities();

            var ds = db.PhieuThuChis
                .Where(x =>
                    x.MaHoaDon == maHD
                    && x.LoaiPhieu == "Thu")
                .OrderByDescending(x =>
                    x.NgayGiaoDich)
                .Select(x => new PhieuThuChi_DTO
                {
                    MaPhieu = x.MaPhieu,
                    NgayGiaoDich = x.NgayGiaoDich,
                    SoTien = x.SoTien,
                    PhuongThuc = x.PhuongThuc,
                    NoiDung = x.NoiDung
                });

            return ds.ToList();
        }
        public string UpdateThanhToanGộp(PhieuThuChi_DTO pt, SoQuy_DTO sq, HoaDon_DTO hd)
        {
            using (var context = new QLNT_DoVanTieuEntities()) // Thay bằng DbContext thực tế của bạn
            {
                // Khởi tạo Transaction bảo vệ dữ liệu độc lập
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        // 1. Thêm phiếu thu chi vào Database
                        var entityPhieu = new PhieuThuChi
                        {
                            MaPhieu = pt.MaPhieu,
                            LoaiPhieu = pt.LoaiPhieu,
                            MaHoaDon = pt.MaHoaDon,
                            SoTien = pt.SoTien,
                            NgayGiaoDich = pt.NgayGiaoDich,
                            NguoiNopNhan = pt.NguoiNopNhan,
                            PhuongThuc = pt.PhuongThuc,
                            NoiDung = pt.NoiDung,
                            GhiChu = "Đã duyệt" // Đánh dấu đã qua Admin đối soát
                        };
                        context.PhieuThuChis.Add(entityPhieu);

                        // 2. Thêm bản ghi vào Sổ Quỹ
                        var entitySoQuy = new SoQuy
                        {
                            MaPhieu = sq.MaPhieu,
                            NgayGiaoDich = sq.NgayGiaoDich,
                            LoaiGiaoDich = sq.LoaiGiaoDich,
                            Thu = sq.Thu,
                            Chi = sq.Chi,
                            SoDu = sq.SoDuSauGD,
                            NoiDung = sq.NoiDung,
                            NguoiLap = sq.NguoiLap
                        };
                        context.SoQuys.Add(entitySoQuy);

                        // 3. Tìm và cập nhật hóa đơn tương ứng
                        var entityHoaDon = context.HoaDons.FirstOrDefault(h => h.MaHoaDon == hd.MaHoaDon);
                        if (entityHoaDon == null)
                        {
                            return "Không tìm thấy hóa đơn cần cập nhật";
                        }

                        entityHoaDon.DaThanhToan = hd.DaThanhToan;
                        entityHoaDon.ConNo = hd.ConNo;
                        entityHoaDon.TrangThai = hd.TrangThai;

                        // Lưu tất cả đồng thời xuống SQL Server
                        context.SaveChanges();

                        // Nếu mọi thứ mượt mà, chính thức xác nhận thay đổi (Commit)
                        transaction.Commit();
                        return "success";
                    }
                    catch (Exception ex)
                    {
                        // Nếu bất kỳ bước nào lỗi, hủy bỏ toàn bộ dữ liệu tạm, đưa DB về trạng thái cũ
                        transaction.Rollback();
                        return "Lỗi hệ thống: " + ex.Message;
                    }
                }
            }
        }
    }
}