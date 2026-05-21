using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using VietlishHomes_Web.Models;

namespace VietlishHomes_Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // ===== [MỚI] Inject DbContext để load dữ liệu thực từ DB =====
        private readonly QlntDoVanTieuContext _context;

        public HomeController(ILogger<HomeController> logger, QlntDoVanTieuContext context)
        {
            _logger  = logger;
            _context = context;
        }
        // ===== [KẾT THÚC MỚI] =====

        // ===== [MỚI] Index: Load dashboard thống kê thực tế từ DB =====
        public async Task<IActionResult> Index()
        {
            // Thống kê tổng quan
            ViewBag.TongPhong       = await _context.Phongs.CountAsync();
            ViewBag.PhongDangThue   = await _context.Phongs.CountAsync(p => p.TrangThai == "Đang thuê" || p.TrangThai == "Có người thuê");
            ViewBag.PhongTrong      = await _context.Phongs.CountAsync(p => p.TrangThai == "Còn trống" || p.TrangThai == "Trống");
            ViewBag.TongCuDan       = await _context.CuDans.CountAsync(cd => cd.TrangThai == "Hoạt động" || cd.TrangThai == "Đang ở");

            // Hóa đơn chưa thanh toán
            ViewBag.HoaDonChuaTT    = await _context.HoaDons.CountAsync(h => h.TrangThai != "Đã thanh toán" && h.ConNo > 0);

            // Báo hỏng chưa xử lý
            ViewBag.BaoHongChuaXL   = await _context.CongViecs.CountAsync(cv => cv.TrangThai == "Chờ xử lý");

            // Danh sách phòng + tên toà nhà để hiển thị trên dashboard
            var danhSachPhong = await _context.Phongs
                .Include(p => p.MaToaNhaNavigation)
                .OrderBy(p => p.MaPhong)
                .Take(20)
                .ToListAsync();
            ViewBag.DanhSachPhong = danhSachPhong;

            // 5 hóa đơn gần nhất chưa thanh toán
            var hoaDonGanDay = await _context.HoaDons
                .Where(h => h.TrangThai != "Đã thanh toán" && h.ConNo > 0)
                .OrderByDescending(h => h.NgayLap)
                .Take(5)
                .ToListAsync();
            ViewBag.HoaDonGanDay = hoaDonGanDay;

            // 5 báo hỏng mới nhất
            var baoHongGanDay = await _context.CongViecs
                .Where(cv => cv.TrangThai == "Chờ xử lý")
                .OrderByDescending(cv => cv.NgayBao)
                .Take(5)
                .ToListAsync();
            ViewBag.BaoHongGanDay = baoHongGanDay;

            return View();
        }
        // ===== [KẾT THÚC MỚI] =====

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
