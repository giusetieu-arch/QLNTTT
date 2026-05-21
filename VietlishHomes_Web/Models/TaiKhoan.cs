using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class TaiKhoan
{
    public string MaTaiKhoan { get; set; } = null!;

    public string? Username { get; set; }

    public string? Password { get; set; }

    public string? MaQuyen { get; set; }

    public string? MaCuDan { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? TrangThai { get; set; }

    public virtual CuDan? MaCuDanNavigation { get; set; }

    public virtual PhanQuyen? MaQuyenNavigation { get; set; }
}
