using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class TaiSan
{
    public string MaTaiSan { get; set; } = null!;

    public string? TenTaiSan { get; set; }

    public string? MaPhong { get; set; }

    public decimal? GiaTri { get; set; }

    public string? MaQrTs { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<CongViec> CongViecs { get; set; } = new List<CongViec>();

    public virtual ICollection<HopDongTaiSan> HopDongTaiSans { get; set; } = new List<HopDongTaiSan>();

    public virtual ICollection<LichSuThietBi> LichSuThietBis { get; set; } = new List<LichSuThietBi>();

    public virtual Phong? MaPhongNavigation { get; set; }
}
