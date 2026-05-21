using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class PhanQuyen
{
    public string MaQuyen { get; set; } = null!;

    public string? TenQuyen { get; set; }

    public virtual ICollection<TaiKhoan> TaiKhoans { get; set; } = new List<TaiKhoan>();
}
