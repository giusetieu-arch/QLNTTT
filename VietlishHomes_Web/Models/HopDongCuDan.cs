using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class HopDongCuDan
{
    public int Id { get; set; }

    public string? MaHopDong { get; set; }

    public string? MaCuDan { get; set; }

    public string? VaiTro { get; set; }

    public DateTime? NgayThamGia { get; set; }

    public DateTime? NgayRoiKhoi { get; set; }

    public string? TrangThai { get; set; }

    public virtual CuDan? MaCuDanNavigation { get; set; }

    public virtual HopDong? MaHopDongNavigation { get; set; }
}
