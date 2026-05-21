using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class ChiTietHoaDon
{
    public int Id { get; set; }

    public string? MaHoaDon { get; set; }

    public string? LoaiChiTiet { get; set; }

    public string? TenDanhMuc { get; set; }

    public decimal? ChiSoCu { get; set; }

    public decimal? ChiSoMoi { get; set; }

    public decimal? SoLuong { get; set; }

    public decimal? DonGia { get; set; }

    public decimal? ThanhTien { get; set; }

    public string? AmhChiSo { get; set; }

    public string? GhiChu { get; set; }

    public virtual HoaDon? MaHoaDonNavigation { get; set; }
}
