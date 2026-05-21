using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class HopDongTaiSan
{
    public int Id { get; set; }

    public string? MaHopDong { get; set; }

    public string? MaTaiSan { get; set; }

    public int? SoLuong { get; set; }

    public string? TinhTrangBanDau { get; set; }

    public string? TinhTrangKhiTra { get; set; }

    public DateTime? NgayBanGiao { get; set; }

    public DateTime? NgayThuHoi { get; set; }

    public decimal? TienDenBu { get; set; }

    public string? GhiChu { get; set; }

    public string? TrangThai { get; set; }

    public virtual HopDong? MaHopDongNavigation { get; set; }

    public virtual TaiSan? MaTaiSanNavigation { get; set; }
}
