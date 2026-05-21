using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class PhieuThuChi
{
    public string MaPhieu { get; set; } = null!;

    public string? LoaiPhieu { get; set; }

    public string? MaHoaDon { get; set; }

    public decimal? SoTien { get; set; }

    public DateTime? NgayGiaoDich { get; set; }

    public string? NguoiNopNhan { get; set; }

    public string? GhiChu { get; set; }

    public string? PhuongThuc { get; set; }

    public string? NoiDung { get; set; }

    public virtual HoaDon? MaHoaDonNavigation { get; set; }

    public virtual ICollection<SoQuy> SoQuies { get; set; } = new List<SoQuy>();
}
