using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class HopDongDichVu
{
    public int Id { get; set; }

    public string? MaHopDong { get; set; }

    public string? MaDichVu { get; set; }

    public string? TenDichVu { get; set; }

    public decimal? DonGia { get; set; }

    public string? HinhThucTinh { get; set; }

    public DateTime? NgayNgung { get; set; }

    public DateTime? NgayBatDau { get; set; }

    public string? TrangThai { get; set; }

    public virtual DanhMucDichVu? MaDichVuNavigation { get; set; }

    public virtual HopDong? MaHopDongNavigation { get; set; }
}
