using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VietlishHomes_Web.Models;

public partial class QlntDoVanTieuContext : DbContext
{
    public QlntDoVanTieuContext()
    {
    }

    public QlntDoVanTieuContext(DbContextOptions<QlntDoVanTieuContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<CongViec> CongViecs { get; set; }

    public virtual DbSet<CuDan> CuDans { get; set; }

    public virtual DbSet<DanhMucDichVu> DanhMucDichVus { get; set; }

    public virtual DbSet<HoaDon> HoaDons { get; set; }

    public virtual DbSet<HopDong> HopDongs { get; set; }

    public virtual DbSet<HopDongCuDan> HopDongCuDans { get; set; }

    public virtual DbSet<HopDongDichVu> HopDongDichVus { get; set; }

    public virtual DbSet<HopDongTaiSan> HopDongTaiSans { get; set; }

    public virtual DbSet<LichSuThietBi> LichSuThietBis { get; set; }

    public virtual DbSet<LoaiPhong> LoaiPhongs { get; set; }

    public virtual DbSet<PhanQuyen> PhanQuyens { get; set; }

    public virtual DbSet<PhieuThuChi> PhieuThuChis { get; set; }

    public virtual DbSet<Phong> Phongs { get; set; }

    public virtual DbSet<PhuLucHopDong> PhuLucHopDongs { get; set; }

    public virtual DbSet<SoQuy> SoQuies { get; set; }

    public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }

    public virtual DbSet<TaiSan> TaiSans { get; set; }

    public virtual DbSet<ToaNha> ToaNhas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-Q39RSO5\\SQLEXPRESS;Initial Catalog=QLNT_DoVanTieu;Integrated Security=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.ChiSoCu).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ChiSoMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoaiChiTiet).HasMaxLength(50);
            entity.Property(e => e.MaHoaDon).HasMaxLength(50);
            entity.Property(e => e.SoLuong).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK_ChiTietHoaDon_HoaDon");
        });

        modelBuilder.Entity<CongViec>(entity =>
        {
            entity.HasKey(e => e.MaCongViec);

            entity.ToTable("CongViec");

            entity.Property(e => e.MaCongViec).HasMaxLength(50);
            entity.Property(e => e.MaCuDan).HasMaxLength(50);
            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.MaTaiSan).HasMaxLength(50);
            entity.Property(e => e.MoTa).HasMaxLength(50);
            entity.Property(e => e.NgayBao).HasColumnType("datetime");
            entity.Property(e => e.NgayXuLy).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaCuDanNavigation).WithMany(p => p.CongViecs)
                .HasForeignKey(d => d.MaCuDan)
                .HasConstraintName("FK_CongViec_CuDan");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.CongViecs)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK_CongViec_Phong");

            entity.HasOne(d => d.MaTaiSanNavigation).WithMany(p => p.CongViecs)
                .HasForeignKey(d => d.MaTaiSan)
                .HasConstraintName("FK_CongViec_TaiSan");
        });

        modelBuilder.Entity<CuDan>(entity =>
        {
            entity.HasKey(e => e.MaCuDan);

            entity.ToTable("CuDan");

            entity.Property(e => e.MaCuDan).HasMaxLength(50);
            entity.Property(e => e.Cccd)
                .HasMaxLength(12)
                .HasColumnName("CCCD");
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.GioiTinh).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Sdt)
                .HasMaxLength(11)
                .HasColumnName("SDT");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<DanhMucDichVu>(entity =>
        {
            entity.HasKey(e => e.MaDichVu);

            entity.ToTable("DanhMucDichVu");

            entity.Property(e => e.MaDichVu).HasMaxLength(50);
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DonViTinh).HasMaxLength(50);
            entity.Property(e => e.HinhThucTinh).HasMaxLength(50);
            entity.Property(e => e.TenDichVu).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<HoaDon>(entity =>
        {
            entity.HasKey(e => e.MaHoaDon);

            entity.ToTable("HoaDon");

            entity.Property(e => e.MaHoaDon).HasMaxLength(50);
            entity.Property(e => e.ConNo).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DaThanhToan).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaHopDong).HasMaxLength(50);
            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.NgayLap).HasColumnType("datetime");
            entity.Property(e => e.NgayThanhToan).HasColumnType("datetime");
            entity.Property(e => e.TongTien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaHopDong)
                .HasConstraintName("FK_HoaDon_HopDong");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.HoaDons)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK_HoaDon_Phong");
        });

        modelBuilder.Entity<HopDong>(entity =>
        {
            entity.HasKey(e => e.MaHopDong);

            entity.ToTable("HopDong");

            entity.Property(e => e.MaHopDong).HasMaxLength(50);
            entity.Property(e => e.GiaDienChot).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaNuocChot).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaThue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaNguoiDaiDien).HasMaxLength(50);
            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.TienCoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<HopDongCuDan>(entity =>
        {
            entity.ToTable("HopDong_CuDan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.MaCuDan).HasMaxLength(50);
            entity.Property(e => e.MaHopDong).HasMaxLength(50);
            entity.Property(e => e.NgayRoiKhoi).HasColumnType("datetime");
            entity.Property(e => e.NgayThamGia).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.VaiTro).HasMaxLength(50);

            entity.HasOne(d => d.MaCuDanNavigation).WithMany(p => p.HopDongCuDans)
                .HasForeignKey(d => d.MaCuDan)
                .HasConstraintName("FK_HopDong_CuDan_CuDan");

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.HopDongCuDans)
                .HasForeignKey(d => d.MaHopDong)
                .HasConstraintName("FK_HopDong_CuDan_HopDong");
        });

        modelBuilder.Entity<HopDongDichVu>(entity =>
        {
            entity.ToTable("HopDong_DichVu");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.HinhThucTinh).HasMaxLength(50);
            entity.Property(e => e.MaDichVu).HasMaxLength(50);
            entity.Property(e => e.MaHopDong).HasMaxLength(50);
            entity.Property(e => e.NgayBatDau).HasColumnType("datetime");
            entity.Property(e => e.NgayNgung).HasColumnType("datetime");
            entity.Property(e => e.TenDichVu).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaDichVuNavigation).WithMany(p => p.HopDongDichVus)
                .HasForeignKey(d => d.MaDichVu)
                .HasConstraintName("FK_HopDong_DichVu_DanhMucDichVu");

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.HopDongDichVus)
                .HasForeignKey(d => d.MaHopDong)
                .HasConstraintName("FK_HopDong_DichVu_HopDong");
        });

        modelBuilder.Entity<HopDongTaiSan>(entity =>
        {
            entity.ToTable("HopDong_TaiSan");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GhiChu).HasMaxLength(50);
            entity.Property(e => e.MaHopDong).HasMaxLength(50);
            entity.Property(e => e.MaTaiSan).HasMaxLength(50);
            entity.Property(e => e.NgayBanGiao).HasColumnType("datetime");
            entity.Property(e => e.NgayThuHoi).HasColumnType("datetime");
            entity.Property(e => e.TienDenBu).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TinhTrangBanDau).HasMaxLength(50);
            entity.Property(e => e.TinhTrangKhiTra).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.HopDongTaiSans)
                .HasForeignKey(d => d.MaHopDong)
                .HasConstraintName("FK_HopDong_TaiSan_HopDong");

            entity.HasOne(d => d.MaTaiSanNavigation).WithMany(p => p.HopDongTaiSans)
                .HasForeignKey(d => d.MaTaiSan)
                .HasConstraintName("FK_HopDong_TaiSan_TaiSan");
        });

        modelBuilder.Entity<LichSuThietBi>(entity =>
        {
            entity.ToTable("LichSuThietBi");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.ChiPhi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoaiSuKien).HasMaxLength(50);
            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.MaTaiSan).HasMaxLength(50);
            entity.Property(e => e.NgayThucHien).HasColumnType("datetime");

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.LichSuThietBis)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK_LichSuThietBi_Phong");

            entity.HasOne(d => d.MaTaiSanNavigation).WithMany(p => p.LichSuThietBis)
                .HasForeignKey(d => d.MaTaiSan)
                .HasConstraintName("FK_LichSuThietBi_TaiSan");
        });

        modelBuilder.Entity<LoaiPhong>(entity =>
        {
            entity.HasKey(e => e.MaLoaiPhong);

            entity.ToTable("LoaiPhong");

            entity.Property(e => e.MaLoaiPhong).HasMaxLength(50);
            entity.Property(e => e.DonGiaDien).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DonGiaNuoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaThueMacDinh).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        modelBuilder.Entity<PhanQuyen>(entity =>
        {
            entity.HasKey(e => e.MaQuyen);

            entity.ToTable("PhanQuyen");

            entity.Property(e => e.MaQuyen).HasMaxLength(50);
            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<PhieuThuChi>(entity =>
        {
            entity.HasKey(e => e.MaPhieu);

            entity.ToTable("PhieuThuChi");

            entity.Property(e => e.MaPhieu).HasMaxLength(50);
            entity.Property(e => e.LoaiPhieu).HasMaxLength(50);
            entity.Property(e => e.MaHoaDon).HasMaxLength(50);
            entity.Property(e => e.NgayGiaoDich).HasColumnType("datetime");
            entity.Property(e => e.NguoiNopNhan).HasMaxLength(50);
            entity.Property(e => e.PhuongThuc).HasMaxLength(50);
            entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaHoaDonNavigation).WithMany(p => p.PhieuThuChis)
                .HasForeignKey(d => d.MaHoaDon)
                .HasConstraintName("FK_PhieuThuChi_HoaDon");
        });

        modelBuilder.Entity<Phong>(entity =>
        {
            entity.HasKey(e => e.MaPhong);

            entity.ToTable("Phong");

            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.ActdCu).HasColumnName("ACTD_Cu");
            entity.Property(e => e.ActnCu).HasColumnName("ACTN_Cu");
            entity.Property(e => e.GiaThue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaLoaiPhong).HasMaxLength(50);
            entity.Property(e => e.MaToaNha).HasMaxLength(50);
            entity.Property(e => e.TienCoc).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaLoaiPhongNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.MaLoaiPhong)
                .HasConstraintName("FK_Phong_LoaiPhong");

            entity.HasOne(d => d.MaToaNhaNavigation).WithMany(p => p.Phongs)
                .HasForeignKey(d => d.MaToaNha)
                .HasConstraintName("FK_Phong_ToaNha");
        });

        modelBuilder.Entity<PhuLucHopDong>(entity =>
        {
            entity.ToTable("PhuLucHopDong");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.GiaCocMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaDienMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaNuocMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.GiaThueMoi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoaiPhuLuc).HasMaxLength(50);
            entity.Property(e => e.MaHopDong).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.NguoiThucHien).HasMaxLength(100);
            entity.Property(e => e.ThoiGianMoi).HasColumnType("datetime");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaHopDongNavigation).WithMany(p => p.PhuLucHopDongs)
                .HasForeignKey(d => d.MaHopDong)
                .HasConstraintName("FK_PhuLucHopDong_HopDong");
        });

        modelBuilder.Entity<SoQuy>(entity =>
        {
            entity.ToTable("SoQuy");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Chi).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.LoaiGiaoDich).HasMaxLength(50);
            entity.Property(e => e.MaPhieu).HasMaxLength(50);
            entity.Property(e => e.NgayGiaoDich).HasColumnType("datetime");
            entity.Property(e => e.NguoiLap).HasMaxLength(50);
            entity.Property(e => e.SoDu).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Thu).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.MaPhieuNavigation).WithMany(p => p.SoQuies)
                .HasForeignKey(d => d.MaPhieu)
                .HasConstraintName("FK_SoQuy_PhieuThuChi");
        });

        modelBuilder.Entity<TaiKhoan>(entity =>
        {
            entity.HasKey(e => e.MaTaiKhoan);

            entity.ToTable("TaiKhoan");

            entity.Property(e => e.MaTaiKhoan).HasMaxLength(50);
            entity.Property(e => e.MaCuDan).HasMaxLength(50);
            entity.Property(e => e.MaQuyen).HasMaxLength(50);
            entity.Property(e => e.NgayTao).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);

            entity.HasOne(d => d.MaCuDanNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaCuDan)
                .HasConstraintName("FK_TaiKhoan_CuDan");

            entity.HasOne(d => d.MaQuyenNavigation).WithMany(p => p.TaiKhoans)
                .HasForeignKey(d => d.MaQuyen)
                .HasConstraintName("FK_TaiKhoan_PhanQuyen");
        });

        modelBuilder.Entity<TaiSan>(entity =>
        {
            entity.HasKey(e => e.MaTaiSan);

            entity.ToTable("TaiSan");

            entity.Property(e => e.MaTaiSan).HasMaxLength(50);
            entity.Property(e => e.GiaTri).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MaPhong).HasMaxLength(50);
            entity.Property(e => e.MaQrTs).HasColumnName("Ma_QR_TS");
            entity.Property(e => e.TrangThai).HasMaxLength(50);

            entity.HasOne(d => d.MaPhongNavigation).WithMany(p => p.TaiSans)
                .HasForeignKey(d => d.MaPhong)
                .HasConstraintName("FK_TaiSan_Phong");
        });

        modelBuilder.Entity<ToaNha>(entity =>
        {
            entity.HasKey(e => e.MaToaNha);

            entity.ToTable("ToaNha");

            entity.Property(e => e.MaToaNha).HasMaxLength(50);
            entity.Property(e => e.TrangThai).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
