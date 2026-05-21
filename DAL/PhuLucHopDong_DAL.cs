using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PhuLucHopDong_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public string Insert(PhuLucHopDong_DTO dto)
        {
            try
            {
                PhuLucHopDong pl =
                    new PhuLucHopDong();

                pl.MaHopDong =
                    dto.MaHopDong;

                pl.LoaiPhuLuc =
                    dto.LoaiPhuLuc;

                pl.GiaThueMoi =
                    dto.GiaThueMoi;

                pl.GiaDienMoi =
                    dto.GiaDienMoi;

                pl.GiaNuocMoi =
                    dto.GiaNuocMoi;

                pl.GiaCocMoi =
                    dto.GiaCocMoi;

                pl.ThoiGianMoi =
                    dto.ThoiGianMoi;

                pl.NoiDung =
                    dto.NoiDung;
                pl.NgayApDung =
                    dto.NgayApDung;

                pl.NgayTao =
                    dto.NgayTao;

                pl.NguoiThucHien =
                    dto.NguoiThucHien;

                pl.TrangThai =
                    dto.TrangThai;

                db.PhuLucHopDongs.Add(pl);

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // =========================
        // GET BY HỢP ĐỒNG
        // =========================

        public List<PhuLucHopDong_DTO>
            GetByHopDong(string maHD)
        {
            return db.PhuLucHopDongs
                .Where(x =>
                    x.MaHopDong == maHD)
                .Select(x =>
                    new PhuLucHopDong_DTO()
                    {
                        ID = x.ID,

                        MaHopDong =
                            x.MaHopDong,

                        LoaiPhuLuc =
                            x.LoaiPhuLuc,

                        GiaThueMoi =
                            x.GiaThueMoi,

                        GiaDienMoi =
                            x.GiaDienMoi,

                        GiaNuocMoi =
                            x.GiaNuocMoi,

                        GiaCocMoi =
                            x.GiaCocMoi,

                        ThoiGianMoi =
                            x.ThoiGianMoi,

                        NoiDung =
                            x.NoiDung,
                        NgayApDung =
                            x.NgayApDung,

                        NgayTao =
                            x.NgayTao,

                        NguoiThucHien =
                            x.NguoiThucHien,

                        TrangThai =
                            x.TrangThai
                    })
                .OrderByDescending(x =>
                    x.NgayTao)
                .ToList();
        }
        public string ApDungPhuLuc()
        {
            var ds =
                db.PhuLucHopDongs
                .Where(x =>
                    x.TrangThai == "Chờ áp dụng"
                    &&
                    x.NgayApDung <= DateTime.Now)
                .ToList();

            foreach (var pl in ds)
            {
                var hd =
                    db.HopDongs
                    .FirstOrDefault(x =>
                        x.MaHopDong ==
                        pl.MaHopDong);

                if (hd == null)
                    continue;

                // ======================
                // GIÁ THUÊ
                // ======================

                if (pl.GiaThueMoi != null)
                {
                    hd.GiaThue =
                        pl.GiaThueMoi;
                }

                // ======================
                // GIÁ ĐIỆN
                // ======================

                if (pl.GiaDienMoi != null)
                {
                    hd.GiaDienChot =
                        pl.GiaDienMoi;
                }

                // ======================
                // GIÁ NƯỚC
                // ======================

                if (pl.GiaNuocMoi != null)
                {
                    hd.GiaNuocChot =
                        pl.GiaNuocMoi;
                }

                // ======================
                // TIỀN CỌC
                // ======================

                if (pl.GiaCocMoi != null)
                {
                    hd.TienCoc =
                        pl.GiaCocMoi;
                }

                // ======================
                // GIA HẠN
                // ======================

                if (pl.ThoiGianMoi != null)
                {
                    hd.NgayKetThuc =
                        pl.ThoiGianMoi;
                }

                pl.TrangThai =
                    "Đã áp dụng";
            }

            db.SaveChanges();

            return "success";
        }
    }
}
