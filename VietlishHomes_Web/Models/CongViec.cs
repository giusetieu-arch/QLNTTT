using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class CongViec
{
    public string MaCongViec { get; set; } = null!;

    public string? MaPhong { get; set; }

    public string? MaTaiSan { get; set; }

    public string? MaCuDan { get; set; }

    public string? TieuDe { get; set; }

    public string? MoTa { get; set; }

    public string? AnhBaoHong { get; set; }

    public string? TrangThai { get; set; }

    public DateTime? NgayBao { get; set; }

    public DateTime? NgayXuLy { get; set; }

    public virtual CuDan? MaCuDanNavigation { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }

    public virtual TaiSan? MaTaiSanNavigation { get; set; }
}
