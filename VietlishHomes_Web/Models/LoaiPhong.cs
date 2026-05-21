using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class LoaiPhong
{
    public string MaLoaiPhong { get; set; } = null!;

    public string? TenLoaiPhong { get; set; }

    public int? SoNguoiToiDa { get; set; }

    public decimal? GiaThueMacDinh { get; set; }

    public decimal? DonGiaDien { get; set; }

    public decimal? DonGiaNuoc { get; set; }

    public string? MoTa { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<Phong> Phongs { get; set; } = new List<Phong>();
}
