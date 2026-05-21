using System;
using System.Collections.Generic;

namespace VietlishHomes_Web.Models;

public partial class LichSuThietBi
{
    public int Id { get; set; }

    public string? MaTaiSan { get; set; }

    public string? MaPhong { get; set; }

    public string? LoaiSuKien { get; set; }

    public string? MoTa { get; set; }

    public decimal? ChiPhi { get; set; }

    public DateTime? NgayThucHien { get; set; }

    public string? NguoiThucHien { get; set; }

    public virtual Phong? MaPhongNavigation { get; set; }

    public virtual TaiSan? MaTaiSanNavigation { get; set; }
}
