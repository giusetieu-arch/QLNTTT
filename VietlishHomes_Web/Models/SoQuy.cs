using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class SoQuy
{
    public int Id { get; set; }

    public string? MaPhieu { get; set; }

    public DateTime? NgayGiaoDich { get; set; }

    public string? LoaiGiaoDich { get; set; }

    public decimal? Thu { get; set; }

    public decimal? Chi { get; set; }

    public decimal? SoDu { get; set; }

    public string? NoiDung { get; set; }

    public string? NguoiLap { get; set; }

    public string? GhiChu { get; set; }

    public virtual PhieuThuChi? MaPhieuNavigation { get; set; }
}
