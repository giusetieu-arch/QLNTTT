using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class Phong
{
    public string MaPhong { get; set; } = null!;

    public string? TenPhong { get; set; }

    public string? MaToaNha { get; set; }

    public string? MaLoaiPhong { get; set; }

    public double? DienTich { get; set; }

    public double? SoDienCu { get; set; }

    public double? SoNuocCu { get; set; }

    public string? ActdCu { get; set; }

    public string? ActnCu { get; set; }

    public decimal? TienCoc { get; set; }

    public string? TrangThai { get; set; }

    public decimal? GiaThue { get; set; }

    public virtual ICollection<CongViec> CongViecs { get; set; } = new List<CongViec>();

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<LichSuThietBi> LichSuThietBis { get; set; } = new List<LichSuThietBi>();

    public virtual LoaiPhong? MaLoaiPhongNavigation { get; set; }

    public virtual ToaNha? MaToaNhaNavigation { get; set; }

    public virtual ICollection<TaiSan> TaiSans { get; set; } = new List<TaiSan>();
}
