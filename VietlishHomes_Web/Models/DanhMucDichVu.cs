using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class DanhMucDichVu
{
    public string MaDichVu { get; set; } = null!;

    public string? TenDichVu { get; set; }

    public decimal? DonGia { get; set; }

    public string? DonViTinh { get; set; }

    public string? HinhThucTinh { get; set; }

    public string? GhiChu { get; set; }

    public string? TrangThai { get; set; }

    public virtual ICollection<HopDongDichVu> HopDongDichVus { get; set; } = new List<HopDongDichVu>();
}
