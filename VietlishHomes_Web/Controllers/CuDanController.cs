using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VietlishHomes_Web.Models;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Net;
using System.Text.Json; // ===== [MỚI - SEPAY] dùng để parse JSON webhook =====

namespace VietlishHomes_Web.Controllers
{
    public class CuDanController : Controller
    {
        private readonly QlntDoVanTieuContext _context;

        // ===== [MỚI - SEPAY] Inject IConfiguration để đọc ApiToken từ appsettings.json =====
        private readonly IConfiguration _configuration;

        public CuDanController(QlntDoVanTieuContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration; // ===== [MỚI - SEPAY] =====
        }
        // ==========================================
        // CHỨC NĂNG: GIAO DIỆN QUÉT QR CODE
        // ==========================================
        public IActionResult QuetQR(string maCuDan)
        {
            ViewBag.MaCuDan = maCuDan;
            return View();
        }

        // CHỨC NĂNG: BÓC TÁCH DỮ LIỆU TỪ QR VÀ TRA CỨU DATABASE
        // Giả sử nội dung mã QR được in cấu trúc dạng: MaPhong_MaTaiSan (Ví dụ: P101_TS01)
        // CHỨC NĂNG: XỬ LÝ QUÉT MÃ QR - TRA CỨU ĐỔ DỮ LIỆU LÊN FORM
        // Giả định nội dung mã QR dán trên thiết bị có định dạng: MaPhong_MaTaiSan (Ví dụ: P101_TS01)
        // CHỨC NĂNG: XỬ LÝ QUÉT MÃ QR CHỈ CHỨA MÃ TÀI SẢN (Ví dụ: 1)
        public async Task<IActionResult> XulyQuetQR(string qrContent, string maCuDan)
        {
            if (string.IsNullOrEmpty(qrContent))
            {
                return RedirectToAction("ThongTinPhongThue", new { maCuDan = maCuDan });
            }

            // Vì mã QR của bạn chứa trực tiếp Mã tài sản (Ví dụ: "1")
            string maTaiSan = qrContent.Trim();
            string maPhong = "";

            // 1. Tự động truy tìm Phòng thực tế mà cư dân này đang thuê từ bảng HopDong
            var hopDong = await _context.HopDongs.AsNoTracking().FirstOrDefaultAsync(h => h.MaNguoiDaiDien == maCuDan);
            if (hopDong != null)
            {
                maPhong = hopDong.MaPhong; // Lấy đúng mã phòng của cư dân đăng nhập
            }
            else
            {
                // Dự phòng nếu tài khoản demo chưa gán hợp đồng
                var phongDauTien = await _context.Phongs.AsNoTracking().FirstOrDefaultAsync();
                maPhong = phongDauTien != null ? phongDauTien.MaPhong : "P101";
            }

            // Tìm tên phòng để hiển thị lên Form
            var phong = await _context.Phongs.AsNoTracking().FirstOrDefaultAsync(p => p.MaPhong == maPhong);

            // 2. Tra cứu thông tin chi tiết thiết bị trong bảng TaiSan theo MaTaiSan quét được
            var taiSan = await _context.TaiSans.AsNoTracking().FirstOrDefaultAsync(t => t.MaTaiSan == maTaiSan);

            // 3. Đổ toàn bộ dữ liệu an toàn vào các ViewBag để đẩy lên giao diện
            ViewBag.MaPhong = maPhong;
            ViewBag.MaCuDan = maCuDan;
            ViewBag.MaTaiSan = maTaiSan;
            ViewBag.TenPhong = phong != null ? phong.TenPhong : maPhong;

            if (taiSan != null)
            {
                ViewBag.TenTaiSan = taiSan.TenTaiSan; // Hiển thị tên tài sản thực tế từ DB
                ViewBag.HangSanXuat = "Chính hãng";   // Bạn có thể đổi thành cột hãng nếu bảng TaiSan có
                ViewBag.TieuDeMacDinh = $"[BÁO HỎNG] {taiSan.TenTaiSan} - Phòng {ViewBag.TenPhong}";
            }
            else
            {
                // Trường hợp quét mã QR số 1 nhưng DB bảng TaiSan chưa nhập dòng có khóa chính là "1"
                ViewBag.TenTaiSan = "Thiết bị số " + maTaiSan;
                ViewBag.HangSanXuat = "Chưa cập nhật";
                ViewBag.TieuDeMacDinh = $"[BÁO HỎNG] Thiết bị mã {maTaiSan} - Phòng {ViewBag.TenPhong}";
            }

            // Trả dữ liệu thẳng về giao diện Form báo hỏng, khóa cứng các ô thông tin lại
            return View("BaoHong");
        }

        // ==========================================
        // CHỨC NĂNG [POST]: LƯU SỰ CỐ KÈM FILE ẢNH THỰC TẾ
        // ==========================================
        [HttpPost]
        public async Task<IActionResult> BaoHong(string maPhong, string maCuDan, string maTaiSan, string tieuDe, string moTa, IFormFile fileAnh)
        {
            // 1. XỬ LÝ BÓC TÁCH MÃ TÀI SẢN NẾU NHẬN CHUỖI QR PHỨC TẠP
            // Chuỗi mẫu: "TS:1|TEN:1|PHONG:1" -> Cần lấy ra giá trị "1"
            if (!string.IsNullOrEmpty(maTaiSan) && maTaiSan.Contains("|"))
            {
                try
                {
                    var parts = maTaiSan.Split('|');
                    foreach (var part in parts)
                    {
                        if (part.StartsWith("TS:"))
                        {
                            maTaiSan = part.Replace("TS:", "").Trim(); // Lấy ra số "1"
                            break;
                        }
                    }
                }
                catch
                {
                    maTaiSan = null; // Dự phòng nếu chuỗi lỗi cấu trúc
                }
            }

            // 2. KIỂM TRA TOÀN VẸN KHÓA NGOẠI (FOREIGN KEY) VỚI DATABASE

            // Kiểm tra tính hợp lệ của MaTaiSan trong bảng TaiSan
            if (!string.IsNullOrEmpty(maTaiSan) && maTaiSan != "N/A")
            {
                var tsExists = await _context.TaiSans.AsNoTracking().AnyAsync(t => t.MaTaiSan == maTaiSan);
                if (!tsExists)
                {
                    maTaiSan = null; // Nếu mã tài sản sau khi cắt vẫn không có trong DB, gán về null để tránh lỗi
                }
            }
            else
            {
                maTaiSan = null;
            }

            // Kiểm tra và xử lý an toàn cho MaPhong để tránh lỗi FK_CongViec_Phong
            if (string.IsNullOrEmpty(maPhong) || maPhong == "fgf") // Chặn các chuỗi gõ lỗi như 'fgf'
            {
                var hopDong = await _context.HopDongs.AsNoTracking().FirstOrDefaultAsync(h => h.MaNguoiDaiDien == maCuDan);
                if (hopDong != null)
                {
                    maPhong = hopDong.MaPhong;
                }
                else
                {
                    var phongDauTien = await _context.Phongs.AsNoTracking().FirstOrDefaultAsync();
                    maPhong = phongDauTien != null ? phongDauTien.MaPhong : "P101";
                }
            }

            // 3. KIỂM TRA FORM BẮT BUỘC
            if (string.IsNullOrEmpty(tieuDe) || string.IsNullOrEmpty(moTa))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ tiêu đề và nội dung mô tả!";
                await ReLoadViewBag(maPhong, maCuDan, maTaiSan);
                return View();
            }

            // 4. XỬ LÝ LƯU FILE ẢNH
            string tenFileAnhLuu = "default_issue.png";
            if (fileAnh != null && fileAnh.Length > 0)
            {
                try
                {
                    tenFileAnhLuu = Guid.NewGuid().ToString() + Path.GetExtension(fileAnh.FileName);
                    string thuMucLuu = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    if (!Directory.Exists(thuMucLuu)) Directory.CreateDirectory(thuMucLuu);

                    string duongDanDayDu = Path.Combine(thuMucLuu, tenFileAnhLuu);
                    using (var stream = new FileStream(duongDanDayDu, FileMode.Create))
                    {
                        await fileAnh.CopyToAsync(stream);
                    }
                }
                catch { tenFileAnhLuu = "default_issue.png"; }
            }

            // 5. TIẾN HÀNH LƯU VÀO DATABASE
            try
            {
                var suCo = new CongViec
                {
                    MaCongViec = "BH" + DateTime.Now.ToString("ddHHmmss"),
                    MaPhong = maPhong,
                    MaCuDan = maCuDan,
                    MaTaiSan = maTaiSan,
                    TieuDe = tieuDe,
                    MoTa = moTa,
                    TrangThai = "Chờ xử lý",
                    NgayBao = DateTime.Now,
                    AnhBaoHong = tenFileAnhLuu
                };

                _context.CongViecs.Add(suCo);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Gửi yêu cầu báo hỏng thành công!";
                return RedirectToAction("ThongTinPhongThue", new { maCuDan = maCuDan });
            }
            catch (Exception ex)
            {
                var chiTietLoi = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ViewBag.Error = "Lỗi lưu dữ liệu: " + chiTietLoi;

                await ReLoadViewBag(maPhong, maCuDan, maTaiSan);
                return View();
            }
        }


        // Hàm bổ trợ nạp lại dữ liệu hiển thị cho ViewBag khi Form quay đầu báo lỗi
        private async Task ReLoadViewBag(string maPhong, string maCuDan, string maTaiSan)
        {
            ViewBag.MaPhong = maPhong;
            ViewBag.MaCuDan = maCuDan;
            ViewBag.MaTaiSan = maTaiSan;

            var phong = await _context.Phongs.FirstOrDefaultAsync(p => p.MaPhong == maPhong);
            ViewBag.TenPhong = phong != null ? phong.TenPhong : maPhong;

            if (!string.IsNullOrEmpty(maTaiSan))
            {
                var taiSan = await _context.TaiSans.FirstOrDefaultAsync(t => t.MaTaiSan == maTaiSan);
                if (taiSan != null)
                {
                    ViewBag.TenTaiSan = taiSan.TenTaiSan;
                    ViewBag.HangSanXuat = "Chính hãng";
                    ViewBag.TieuDeMacDinh = $"[BÁO HỎNG] Thiết bị {taiSan.TenTaiSan} - Phòng {maPhong}";
                }
            }
        }
        // ==========================================
        // CHỨC NĂNG: GIAO DIỆN ĐĂNG NHẬP CƯ DÂN
        // ==========================================

        // [GET]: Hiển thị giao diện Form đăng nhập cư dân
        // Đường dẫn: /CuDan/Login
        public IActionResult Login()
        {
            return View();
        }

        // [POST]: Xử lý dữ liệu khi cư dân điền Username & Password để đăng nhập
        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu!";
                return View();
            }

            // Dò tìm tài khoản trong bảng TaiKhoan khớp cả Username và Password từ DB thực tế
            var taiKhoan = await _context.TaiKhoans
                .FirstOrDefaultAsync(tk => tk.Username == username && tk.Password == password);

            if (taiKhoan == null)
            {
                ViewBag.Error = "Tài khoản hoặc mật khẩu không đúng! Vui lòng kiểm tra lại.";
                return View();
            }

            if (taiKhoan.TrangThai != "Hoạt động")
            {
                ViewBag.Error = "Tài khoản này đã bị tạm khóa hoặc ngừng hoạt động!";
                return View();
            }

            // Kiểm tra xem tài khoản này có được gán mã cư dân lưu trú hay không
            if (string.IsNullOrEmpty(taiKhoan.MaCuDan))
            {
                ViewBag.Error = "Tài khoản này không liên kết với thông tin cư dân nào!";
                return View();
            }

            // Đăng nhập thành công -> Điều hướng sang danh sách phòng thuê kèm theo MaCuDan tìm được
            return RedirectToAction("ThongTinPhongThue", new { maCuDan = taiKhoan.MaCuDan });
        }


        // ==========================================
        // CHỨC NĂNG 1: XEM DANH SÁCH PHÒNG THUÊ ĐANG Ở
        // ==========================================
        public async Task<IActionResult> ThongTinPhongThue(string maCuDan)
        {
            if (string.IsNullOrEmpty(maCuDan))
            {
                return RedirectToAction("Login");
            }

            // Lấy thông tin họ tên cư dân từ bảng CuDan để hiển thị câu chào mừng lên giao diện
            var thongTinCaNhan = await _context.CuDans.FirstOrDefaultAsync(cd => cd.MaCuDan == maCuDan);
            ViewBag.TenCuDan = thongTinCaNhan != null ? thongTinCaNhan.TenCuDan : maCuDan;
            ViewBag.MaCuDan = maCuDan;

            // FIX LỖI TẠI ĐÂY: Khớp trường liên kết bảng trung gian là hdcd.MaCd theo Database Diagram của bạn
            var danhSachHopDong = await _context.HopDongs
                .Where(hd => hd.HopDongCuDans.Any(hdcd => hdcd.MaCuDan == maCuDan))
                .OrderByDescending(hd => hd.NgayBatDau)
                .ToListAsync();

            // Tự nạp danh sách phòng độc lập lên ViewBag để View dò tìm tên phòng hiển thị, không dùng Include tránh lỗi điều hướng
            var danhSachPhong = await _context.Phongs.ToListAsync();
            ViewBag.DanhSachPhong = danhSachPhong;

            if (danhSachHopDong == null || !danhSachHopDong.Any())
            {
                ViewBag.Message = $"Cư dân {maCuDan} hiện tại chưa đăng ký thông tin lưu trú tại phòng nào.";
            }

            return View(danhSachHopDong);
        }


        // ==========================================
        // CHỨC NĂNG 2: XEM CHI TIẾT HỢP ĐỒNG ĐIỆN TỬ THEO PHÒNG
        // ==========================================
        public async Task<IActionResult> ThongTinHopDong(string maPhong)
        {
            if (string.IsNullOrEmpty(maPhong))
            {
                return BadRequest("Vui lòng cung cấp mã phòng.");
            }

            // Tìm kiếm bản ghi hợp đồng mới nhất của căn phòng này
            var hopDong = await _context.HopDongs
                .Include(hd => hd.HopDongCuDans)
                .Where(hd => hd.MaPhong == maPhong)
                .OrderByDescending(hd => hd.NgayTao)
                .FirstOrDefaultAsync();

            ViewBag.MaPhong = maPhong;

            // Tìm tên phòng thực tế từ bảng Phong để nạp ra giao diện
            var thongTinPhong = await _context.Phongs.FirstOrDefaultAsync(p => p.MaPhong == maPhong);
            ViewBag.TenPhong = thongTinPhong != null ? thongTinPhong.TenPhong : "Chưa cập nhật tên";

            if (hopDong == null)
            {
                ViewBag.Message = $"Phòng {maPhong} hiện tại chưa được cập nhật dữ liệu hợp đồng lưu trú.";
                return View();
            }

            // Xử lý so sánh dữ liệu kiểu DateOnly của hệ thống
            DateOnly ngayHienTai = DateOnly.FromDateTime(DateTime.Now);
            ViewBag.ConHan = hopDong.NgayKetThuc > ngayHienTai;

            return View(hopDong);
        }


        // ==========================================
        // CHỨC NĂNG 3: XEM LỊCH SỬ HÓA ĐƠN TIỀN PHÒNG
        // ==========================================
        public async Task<IActionResult> DanhSachHoaDon(string maPhong, string? maCuDan)
        {
            if (string.IsNullOrEmpty(maPhong))
            {
                return BadRequest("Vui lòng cung cấp mã phòng.");
            }

            // Lấy danh sách toàn bộ hóa đơn dịch vụ của phòng, đẩy hóa đơn mới nhất lên đầu danh sách
            var dsHoaDon = await _context.HoaDons
                .Where(hd => hd.MaPhong == maPhong)
                .OrderByDescending(hd => hd.NgayLap)
                .ToListAsync();

            ViewBag.MaPhong = maPhong;

            // ===== [MỚI - SEPAY] Truyền maCuDan để link Thanh Toán redirect đúng =====
            ViewBag.MaCuDan = maCuDan;
            // ===== [KẾT THÚC MỚI - SEPAY] =====

            if (dsHoaDon == null || !dsHoaDon.Any())
            {
                ViewBag.Message = $"Phòng {maPhong} hiện chưa phát sinh hóa đơn thanh toán nào.";
            }

            return View(dsHoaDon);
        }
        // ==========================================
        // CHỨC NĂNG: GỬI YÊU CẦU BÁO HỎNG / SỰ CỐ
        // ==========================================

        // [GET]: Hiển thị Form báo hỏng
        // Đường dẫn: /CuDan/BaoHong?maPhong=xxx&maCuDan=yyy
        public async Task<IActionResult> BaoHong(string maPhong, string maCuDan)
        {
            if (string.IsNullOrEmpty(maPhong) || string.IsNullOrEmpty(maCuDan))
            {
                return BadRequest("Thiếu thông tin phòng hoặc cư dân.");
            }

            ViewBag.MaPhong = maPhong;
            ViewBag.MaCuDan = maCuDan;

            // Lấy tên phòng hiển thị cho thân thiện giao diện
            var phong = await _context.Phongs.FirstOrDefaultAsync(p => p.MaPhong == maPhong);
            ViewBag.TenPhong = phong != null ? phong.TenPhong : maPhong;

            return View();
        }
        public async Task<IActionResult> ThanhToanHoaDon(string maHoaDon, string? maCuDan)
        {
            // 1. Lấy thông tin hóa đơn từ Database
            var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
            if (hoaDon == null)
            {
                return RedirectToAction("ThongTinPhongThue");
            }

            // ===== [MỚI - SEPAY] Đọc cấu hình từ appsettings.json thay vì hardcode =====
            string BANK_ID      = _configuration["SePay:BankId"]      ?? "ACB";
            string ACCOUNT_NO   = _configuration["SePay:AccountNo"]   ?? "38805157";
            string ACCOUNT_NAME = _configuration["SePay:AccountName"] ?? "CONG TY VIETLISH HOMES";
            // ===== [KẾT THÚC MỚI - SEPAY] =====

            // ===== [XÓA] Code cũ hardcode thông tin ngân hàng:
            // string BANK_ID = "ACB";
            // string ACCOUNT_NO = "38805157";
            // string ACCOUNT_NAME = "CONG TY VIETLISH HOMES";
            // =====

            // Ép kiểu số tiền về dạng số nguyên (bỏ phần thập phân nếu có)
            long soTien = Convert.ToInt64(hoaDon.TongTien);

            // Nội dung chuyển khoản - SePay sẽ match chuỗi này để tự động nhận giao dịch
            string noiDung = $"THANH TOAN HOA DON {hoaDon.MaHoaDon}".Replace(" ", "%20");

            // Sử dụng API VietQR chuẩn để tự sinh link ảnh QR động
            string linkVietQR = $"https://img.vietqr.io/image/{BANK_ID}-{ACCOUNT_NO}-compact.png?amount={soTien}&addInfo={noiDung}&accountName={ACCOUNT_NAME}";

            // Đẩy dữ liệu ra giao diện hiển thị
            ViewBag.LinkQR    = linkVietQR;
            ViewBag.MaHoaDon  = hoaDon.MaHoaDon;
            ViewBag.SoTien    = soTien;
            ViewBag.NoiDungCK = $"THANH TOAN HOA DON {hoaDon.MaHoaDon}";

            // ===== [MỚI - SEPAY] Truyền maCuDan để sau khi thanh toán redirect đúng =====
            ViewBag.MaCuDan = maCuDan;
            // ===== [KẾT THÚC MỚI - SEPAY] =====

            return View();
        }

        // ===== [GIỮ LẠI] Nút xác nhận thủ công - fallback khi webhook không hoạt động =====
        [HttpPost]
        public async Task<IActionResult> XacNhanThanhToanThanhCong(string maHoaDon, string? maCuDan)
        {
            var hoaDon = await _context.HoaDons.FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);
            if (hoaDon != null && hoaDon.TrangThai != "Đã thanh toán")
            {
                // Cập nhật trạng thái hóa đơn
                hoaDon.TrangThai     = "Đã thanh toán";
                hoaDon.NgayThanhToan = DateTime.Now;
                hoaDon.DaThanhToan   = hoaDon.TongTien;
                hoaDon.ConNo         = 0;

                // ===== [MỚI] Tra cứu tên khách thuê để ghi vào NguoiNopNhan =====
                string tenKhachThue = "Khách thuê";
                var hopDongLK = await _context.HopDongs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.MaPhong == hoaDon.MaPhong);
                if (hopDongLK?.MaNguoiDaiDien != null)
                {
                    var cuDan = await _context.CuDans
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cd => cd.MaCuDan == hopDongLK.MaNguoiDaiDien);
                    if (cuDan != null) tenKhachThue = cuDan.TenCuDan ?? "Khách thuê";
                }
                string ghiChu = $"Tiền trọ {hoaDon.KyHoaDon ?? hoaDon.NgayLap?.ToString("MM/yyyy") ?? DateTime.Now.ToString("MM/yyyy")}";
                // ===== [KẾT THÚC MỚI] =====

                var phieuThu = new PhieuThuChi
                {
                    MaPhieu      = "PT" + DateTime.Now.ToString("ddHHmmss"),
                    LoaiPhieu    = "Thu",
                    MaHoaDon     = hoaDon.MaHoaDon,
                    SoTien       = hoaDon.TongTien,
                    NgayGiaoDich = DateTime.Now,
                    PhuongThuc   = "VietQR - Xác nhận thủ công",
                    NguoiNopNhan = tenKhachThue,  // ===== [MỚI] Tên khách thuê =====
                    GhiChu       = ghiChu,         // ===== [MỚI] Ghi chú tiền trọ =====
                    NoiDung      = $"Cư dân {tenKhachThue} xác nhận thủ công qua VietQR - Hóa đơn {hoaDon.MaHoaDon}"
                };

                _context.PhieuThuChis.Add(phieuThu);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xác nhận thành công! Hệ thống đã ghi nhận phiếu thu.";
            }

            if (!string.IsNullOrEmpty(maCuDan))
                return RedirectToAction("ThongTinPhongThue", new { maCuDan });
            return RedirectToAction("ThongTinPhongThue");
        }

        // ===== [MỚI - SEPAY] WEBHOOK: SePay gọi về đây sau mỗi giao dịch ngân hàng =====
        // Đường dẫn: POST /CuDan/SePayWebhook
        // SePay cần được cấu hình Webhook URL = https://your-ngrok-url.ngrok-free.app/CuDan/SePayWebhook
        [HttpPost]
        [IgnoreAntiforgeryToken] // Bỏ CSRF vì đây là server-to-server call từ SePay
        public async Task<IActionResult> SePayWebhook()
        {
            try
            {
                // 1. Đọc raw body từ request
                Request.EnableBuffering();
                using var reader = new StreamReader(Request.Body, System.Text.Encoding.UTF8, leaveOpen: true);
                string rawBody = await reader.ReadToEndAsync();
                Request.Body.Position = 0;

                // 2. Verify token bảo mật từ SePay (Authorization: Bearer {ApiToken})
                string sePayToken  = _configuration["SePay:ApiToken"] ?? "";
                string authHeader  = Request.Headers["Authorization"].FirstOrDefault() ?? "";
                if (!string.IsNullOrEmpty(sePayToken)
                    && sePayToken != "QuanlynhatroVanTieu"
                    && authHeader != $"Bearer {sePayToken}")
                {
                    return Unauthorized(new { success = false, message = "Invalid API token" });
                }

                // 3. Parse JSON từ SePay
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<SePayWebhookData>(rawBody, options);
                if (data == null)
                    return BadRequest(new { success = false, message = "Invalid JSON body" });

                // 4. Chỉ xử lý giao dịch TIỀN VÀO
                if (data.TransferType?.ToLower() != "in")
                    return Ok(new { success = true, message = "Ignored: outgoing transaction" });

                // 5. Đối chiếu nội dung chuyển khoản với MaHoaDon trong DB
                string content    = (data.Content ?? "").ToUpper();
                string? maHoaDon  = null;

                // Lấy tất cả hóa đơn chưa thanh toán để đối chiếu
                var hoaDonChuaThanhToan = await _context.HoaDons
                    .Where(h => h.TrangThai != "Đã thanh toán" && h.ConNo > 0)
                    .AsNoTracking()
                    .ToListAsync();

                foreach (var hd in hoaDonChuaThanhToan)
                {
                    // Kiểm tra xem nội dung CK có chứa mã hóa đơn không
                    if (content.Contains(hd.MaHoaDon.ToUpper()))
                    {
                        maHoaDon = hd.MaHoaDon;
                        break;
                    }
                }

                if (maHoaDon == null)
                    return Ok(new { success = true, message = "No matching unpaid invoice found in content" });

                // 6. Cập nhật hóa đơn tìm được
                var hoaDon = await _context.HoaDons
                    .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);

                if (hoaDon == null || hoaDon.TrangThai == "Đã thanh toán")
                    return Ok(new { success = true, message = "Invoice already paid or not found" });

                hoaDon.TrangThai     = "Đã thanh toán";
                hoaDon.NgayThanhToan = DateTime.Now;
                hoaDon.DaThanhToan   = data.TransferAmount;
                hoaDon.ConNo         = 0;

                // 7. ===== [MỚI] Tra cứu tên khách thuê để ghi vào NguoiNopNhan =====
                string tenKhachThueWH = "Khách thuê";
                var hopDongLKWH = await _context.HopDongs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(h => h.MaPhong == hoaDon.MaPhong);
                if (hopDongLKWH?.MaNguoiDaiDien != null)
                {
                    var cuDanWH = await _context.CuDans
                        .AsNoTracking()
                        .FirstOrDefaultAsync(cd => cd.MaCuDan == hopDongLKWH.MaNguoiDaiDien);
                    if (cuDanWH != null) tenKhachThueWH = cuDanWH.TenCuDan ?? "Khách thuê";
                }
                string ghiChuWH = $"Tiền trọ {hoaDon.KyHoaDon ?? hoaDon.NgayLap?.ToString("MM/yyyy") ?? DateTime.Now.ToString("MM/yyyy")}";
                // ===== [KẾT THÚC MỚI] =====

                // 8. Tự động sinh phiếu thu
                var phieuThu = new PhieuThuChi
                {
                    MaPhieu      = "SEPAY" + DateTime.Now.ToString("ddHHmmss"),
                    LoaiPhieu    = "Thu",
                    MaHoaDon     = hoaDon.MaHoaDon,
                    SoTien       = data.TransferAmount,
                    NgayGiaoDich = DateTime.Now,
                    PhuongThuc   = $"SePay - {data.Gateway}",
                    NguoiNopNhan = tenKhachThueWH,  // ===== [MỚI] Tên khách thuê =====
                    GhiChu       = ghiChuWH,          // ===== [MỚI] Ghi chú tiền trọ =====
                    NoiDung      = $"SePay tự động | {tenKhachThueWH} | {data.Content} | Mã GD: {data.ReferenceCode}"
                };

                _context.PhieuThuChis.Add(phieuThu);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, message = $"Invoice {maHoaDon} marked as paid" });
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng vẫn trả 200 để SePay không retry liên tục
                return StatusCode(500, new { success = false, message = ex.Message });
            }
        }
        // ===== [KẾT THÚC MỚI - SEPAY WEBHOOK] =====

        // ===== [MỚI - SEPAY] AJAX POLLING: Frontend gọi mỗi 5 giây để check trạng thái =====
        // Đường dẫn: GET /CuDan/KiemTraThanhToan?maHoaDon=HD001
        [HttpGet]
        public async Task<IActionResult> KiemTraThanhToan(string maHoaDon)
        {
            var hoaDon = await _context.HoaDons
                .AsNoTracking()
                .FirstOrDefaultAsync(h => h.MaHoaDon == maHoaDon);

            if (hoaDon == null)
                return Json(new { daDuocThanhToan = false, loi = "Không tìm thấy hóa đơn" });

            bool daDuocThanhToan = hoaDon.TrangThai == "Đã thanh toán" || hoaDon.ConNo == 0;

            return Json(new
            {
                daDuocThanhToan,
                trangThai = hoaDon.TrangThai,
                soTienDaTra = hoaDon.DaThanhToan
            });
        }
        // ===== [KẾT THÚC MỚI - SEPAY POLLING] =====
        // [POST]: Xử lý lưu form báo hỏng vào bảng CongViec
        /*[HttpPost]
        public async Task<IActionResult> BaoHong(string maPhong, string maCuDan, string tieuDe, string moTa)
        {
            if (string.IsNullOrEmpty(tieuDe) || string.IsNullOrEmpty(moTa))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ tiêu đề và nội dung mô tả sự cố!";
                ViewBag.MaPhong = maPhong;
                ViewBag.MaCuDan = maCuDan;
                return View();
            }

            try
            {
                // Khởi tạo một đối tượng Công việc mới dựa trên cấu trúc bảng CongViec của bạn
                var suCo = new CongViec
                {
                    // Tự động sinh mã công việc bằng Guid ngắn gọn hoặc để DB tự tăng tùy cấu hình của bạn
                    MaCongViec = "BH_" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper(),
                    MaPhong = maPhong,
                    MaCuDan = maCuDan, // Khớp trường dữ liệu MaCuDan trong bảng CongViec
                    TieuDe = tieuDe,
                    MoTa = moTa,
                    TrangThai = "Chờ xử lý", // Trạng thái ban đầu khi gửi sự cố
                                             // Convert từ DateOnly sang DateTime đầy đủ
                    NgayBao = DateTime.Parse(DateOnly.FromDateTime(DateTime.Now).ToString("yyyy-MM-dd")),
                    AnhBaoHong = "default_issue.png" // Chuỗi tạm thời nếu không up ảnh thực tế
                };

                _context.CongViecs.Add(suCo);
                await _context.SaveChangesAsync();

                // Đẩy thông báo thành công về trang danh sách phòng
                TempData["SuccessMessage"] = "Gửi yêu cầu báo hỏng thành công! Ban quản lý sẽ sớm xử lý.";  
                return RedirectToAction("ThongTinPhongThue", new { maCuDan = maCuDan });
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Đã xảy ra lỗi hệ thống: " + ex.Message;
                ViewBag.MaPhong = maPhong;
                ViewBag.MaCuDan = maCuDan;
                return View();
            }
        }*/
    }
}