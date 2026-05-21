using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HoaDon_DAL
    {
        HoaDon_DTO hd = new HoaDon_DTO();
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public List<HoaDon> GetHoaDonChuaThanhToan()
        {
            return db.HoaDons
                .Where(x => x.ConNo > 0)
                .ToList();
        }
        public string Update(HoaDon_DTO dto)
        {
            try
            {
                var hd = db.HoaDons
                    .FirstOrDefault(x =>
                        x.MaHoaDon == dto.MaHoaDon);

                if (hd == null)
                    return "Không tìm thấy";

                hd.DaThanhToan =
                    dto.DaThanhToan;

                hd.ConNo =
                    dto.ConNo;

                hd.TrangThai =
                    dto.TrangThai;

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public HoaDon GetHoaDon(string maHD)
        {
          

            return db.HoaDons
                .FirstOrDefault(x =>
                    x.MaHoaDon == maHD);
        }
        public List<ChiTietHoaDon> GetChiTiet(string maHD)
        {
            // 1. Lấy những chi tiết cố định đã lưu trong hóa đơn (Tiền phòng, điện, nước)
            var listKetQua = db.ChiTietHoaDons
                               .Where(x => x.MaHoaDon == maHD)
                               .ToList();

            // 2. Tìm thông tin Hóa đơn để lấy MaPhong (hoặc MaHopDong nếu có) và Ngày Lập
            var hoaDon = db.HoaDons.FirstOrDefault(x => x.MaHoaDon == maHD);
            if (hoaDon == null) return listKetQua;

            string maPhong = hoaDon.MaPhong;
            DateTime? ngayLap = hoaDon.NgayLap;

            // Định nghĩa khoảng thời gian của tháng này để lọc phát sinh (Ví dụ: từ đầu tháng đến ngày lập)
            if (ngayLap.HasValue)
            {
                DateTime tuNgay = new DateTime(ngayLap.Value.Year, ngayLap.Value.Month, 1);
                DateTime denNgay = ngayLap.Value;

                // 3. GOM DỊCH VỤ TỪ BẢNG TRUNG GIAN HỢP ĐỒNG
                // Giả sử bảng trung gian của bạn tên là ChiTietDichVuHopDongs
                var listDichVuPhatSinh = db.HopDong_DichVu
                    .Where(x => x.MaHopDong == hoaDon.MaHopDong && x.TrangThai == "Đang dùng")
                    .ToList()
                    .Select(dv => new ChiTietHoaDon
                    {
                        MaHoaDon = maHD,
                        LoaiChiTiet = "Dịch vụ",
                        TenDanhMuc = dv.TenDichVu, // Thay bằng cột chứa tên dịch vụ bên bạn (ví dụ: dv.DichVu.TenDV)
                        SoLuong = 1,
                        DonGia = dv.DonGia,
                        ThanhTien = dv.DonGia
                    }).ToList();

                listKetQua.AddRange(listDichVuPhatSinh);

                

                // 5. GOM PHÁT SINH CƯ DÂN (Nếu có tính phí thêm người, phụ thu xe cộ...)
                // Tương tự, bạn truy vấn từ bảng cư dân trung gian phát sinh trong tháng
                /*
                var listCuDanPhatSinh = db.PhuThuCuDans.Where(...).ToList().Select(...);
                listKetQua.AddRange(listCuDanPhatSinh);
                */
            }

            return listKetQua;
        }
        public List<HoaDon> GetAll1()
        {


            return db.HoaDons
                .OrderByDescending(x => x.NgayLap)
                .ToList();
        }
        public List<HoaDon_DTO> GetAll()
        {
            return db.HoaDons
                .Select(x => new HoaDon_DTO
                {
                    MaHoaDon = x.MaHoaDon,

                    MaPhong = x.MaPhong,

                    KyHoaDon = x.KyHoaDon,

                    TongTien = x.TongTien,

                    DaThanhToan = x.DaThanhToan,

                    ConNo = x.ConNo,

                    TrangThai = x.TrangThai,

                    NgayLap = x.NgayLap
                })
                .OrderByDescending(x =>
                    x.NgayLap)
                .ToList();
        }
        public string Insert(HoaDon_DTO dto)
        {
            try
            {
                HoaDon hd = new HoaDon();

                hd.MaHoaDon = dto.MaHoaDon;

                hd.MaHopDong = dto.MaHopDong;

                hd.MaPhong = dto.MaPhong;

                hd.KyHoaDon = dto.KyHoaDon;

                hd.NgayLap = dto.NgayLap;

                hd.TongTien = dto.TongTien;

                hd.DaThanhToan = dto.DaThanhToan;

                hd.ConNo = dto.ConNo;

                hd.TrangThai = dto.TrangThai;

                db.HoaDons.Add(hd);

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

                if (ex.InnerException != null)
                    msg += "\n" + ex.InnerException.Message;

                if (ex.InnerException?.InnerException != null)
                    msg += "\n" + ex.InnerException.InnerException.Message;

                return msg;
            }
        }
        public ThongTinLapHoaDon_DTO GetThongTin(string maPhong)
        {
            // =====================
            // HỢP ĐỒNG
            // =====================

            var hd =
                db.HopDongs
                .FirstOrDefault(x =>
                    x.MaPhong == maPhong
                    &&
                    x.TrangThai
                        == "Đang hiệu lực");

            if (hd == null)
                return null;

            // =====================
            // PHÒNG
            // =====================

            var phong =
                db.Phongs
                .FirstOrDefault(x =>
                    x.MaPhong == maPhong);

            // =====================
            // GIÁ GỐC
            // =====================

            decimal giaPhong =
                hd.GiaThue ?? 0;

            decimal giaDien =
                hd.GiaDienChot ?? 0;

            decimal giaNuoc =
                hd.GiaNuocChot ?? 0;

            // =====================
            // LẤY PHỤ LỤC MỚI NHẤT
            // =====================

            var dsPhuLuc =
     db.PhuLucHopDongs
     .Where(x =>
         x.MaHopDong == hd.MaHopDong
         &&
         x.NgayApDung != null
         &&
         x.NgayApDung <= DateTime.Now
     )
     .OrderBy(x => x.NgayApDung)
     .ToList();

            // =====================
            // ƯU TIÊN GIÁ MỚI
            // =====================

            foreach (var pl in dsPhuLuc)
            {
                // =====================
                // GIÁ THUÊ
                // =====================

                if (pl.GiaThueMoi != null)
                {
                    giaPhong =
                        pl.GiaThueMoi.Value;
                }

                // =====================
                // GIÁ ĐIỆN
                // =====================

                if (pl.GiaDienMoi != null)
                {
                    giaDien =
                        pl.GiaDienMoi.Value;
                }

                // =====================
                // GIÁ NƯỚC
                // =====================

                if (pl.GiaNuocMoi != null)
                {
                    giaNuoc =
                        pl.GiaNuocMoi.Value;
                }

                // =====================
                // TIỀN CỌC
                // =====================

                if (pl.GiaCocMoi != null)
                {
                    hd.TienCoc =
                        pl.GiaCocMoi.Value;
                }

                // =====================
                // GIA HẠN
                // =====================

                if (pl.ThoiGianMoi != null)
                {
                    hd.NgayKetThuc =
                        pl.ThoiGianMoi.Value;
                }
            }

            // =====================
            // DTO
            // =====================

            ThongTinLapHoaDon_DTO dto =
                new ThongTinLapHoaDon_DTO();

            dto.MaHopDong =
                hd.MaHopDong;

            dto.GiaPhong =
                giaPhong;

            dto.GiaDien =
                giaDien;

            dto.GiaNuoc =
                giaNuoc;

            dto.SoDienCu =
                (int)(phong.SoDienCu ?? 0);

            dto.SoNuocCu =
                (int)(phong.SoNuocCu ?? 0);

            dto.AnhDienCu =
                phong.ACTD_Cu;

            dto.AnhNuocCu =
                phong.ACTN_Cu;

            // =====================
            // SỐ NGƯỜI
            // =====================

            dto.SoNguoi =
                db.HopDong_CuDan
                .Count(x =>
                    x.MaHopDong
                        == hd.MaHopDong
                    &&
                    x.TrangThai
                        == "Đang ở");

            // =====================
            // DỊCH VỤ
            // =====================

            dto.DichVus =
                db.HopDong_DichVu
                .Where(x =>
                    x.MaHopDong
                        == hd.MaHopDong
                    &&
                    x.TrangThai
                        == "Đang dùng")
                .Select(x =>
                    new HopDong_DichVu_DTO()
                    {
                        ID = x.ID,

                        MaDichVu =
                            x.MaDichVu,

                        TenDichVu =
                            x.TenDichVu,

                        DonGia =
                            x.DonGia,

                        HinhThucTinh =
                            x.HinhThucTinh
                    })
                .ToList();


            return dto;
        }
        public bool KiemTraHoaDonTonTai(string maPhong,string kyHoaDon)
        {
            return db.HoaDons.Any(x =>
            x.MaPhong == maPhong
            &&
            x.KyHoaDon == kyHoaDon);
        }
        public decimal TongCongNo()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                return db.HoaDons
                         .Where(x =>
                         x.TrangThai == "Chưa thanh toán")
                         .Sum(x => (decimal?)x.TongTien)
                         ?? 0;
            }
        }
    }
    }

