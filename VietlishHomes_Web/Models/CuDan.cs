using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class CuDan
{
    public string MaCuDan { get; set; } = null!;

    public string? TenCuDan { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public string? Email { get; set; }

    public string? Cccd { get; set; }

    public string? Sdt { get; set; }

    public string? QueQuan { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<CongViec> CongViecs { get; set; } = new List<CongViec>();

    public virtual ICollection<HopDongCuDan> HopDongCuDans { get; set; } = new List<HopDongCuDan>();

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
