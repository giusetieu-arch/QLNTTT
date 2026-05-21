using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class HopDong
{
    public string MaHopDong { get; set; } = null!;

    public string? MaPhong { get; set; }

    public DateOnly? NgayBatDau { get; set; }

    public DateOnly? NgayKetThuc { get; set; }

    public decimal? TienCoc { get; set; }

    public decimal? GiaThue { get; set; }

    public decimal? GiaDienChot { get; set; }

    public decimal? GiaNuocChot { get; set; }

    public DateTime? NgayTao { get; set; }

    public string? GhiChu { get; set; }

    public string? TrangThai { get; set; }

    public string? MaNguoiDaiDien { get; set; }

    public virtual ICollection<HoaDon> HoaDons { get; set; } = new List<HoaDon>();

    public virtual ICollection<HopDongCuDan> HopDongCuDans { get; set; } = new List<HopDongCuDan>();

    public virtual ICollection<HopDongDichVu> HopDongDichVus { get; set; } = new List<HopDongDichVu>();

    public virtual ICollection<HopDongTaiSan> HopDongTaiSans { get; set; } = new List<HopDongTaiSan>();

    public virtual ICollection<PhuLucHopDong> PhuLucHopDongs { get; set; } = new List<PhuLucHopDong>();
}
