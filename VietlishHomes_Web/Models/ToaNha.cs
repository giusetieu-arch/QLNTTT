using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class ToaNha
{
    public string MaToaNha { get; set; } = null!;

    public string? TenToaNha { get; set; }

    public string? DiaChi { get; set; }

    public int? SoTang { get; set; }

    public int? SoPhong { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
