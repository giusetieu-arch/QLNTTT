using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class HoaDon
{
    public string MaHoaDon { get; set; } = null!;

    public string? MaHopDong { get; set; }

    public string? MaPhong { get; set; }

    public decimal? TongTien { get; set; }

    public DateTime? NgayLap { get; set; }

    public string? KyHoaDon { get; set; }

    public DateTime? NgayThanhToan { get; set; }

    public decimal? DaThanhToan { get; set; }

    public decimal? ConNo { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual HopDong? MaHopDongNavigation { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }

    public virtual ICollection<PhieuThuChi> PhieuThuChis { get; set; } = new List<PhieuThuChi>();
}
