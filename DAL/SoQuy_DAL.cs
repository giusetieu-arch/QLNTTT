using DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DAL
{
    public class SoQuy_DAL
    {
        QLNT_DoVanTieuEntities db =
            new QLNT_DoVanTieuEntities();

        // =========================
        // INSERT
        // =========================

        public string Insert(
            SoQuy_DTO dto)
        {
            try
            {
                SoQuy s =
                    new SoQuy();

                s.MaPhieu =
                    dto.MaPhieu;

                s.NgayGiaoDich =
                    dto.NgayGiaoDich;

                s.LoaiGiaoDich =
                    dto.LoaiGiaoDich;

                s.Thu =
                    dto.Thu;

                s.Chi =
                    dto.Chi;

                s.SoDu =
                    dto.SoDuSauGD;

                s.NoiDung =
                    dto.NoiDung;

                s.NguoiLap =
                    dto.NguoiLap;

                s.GhiChu =
                    dto.GhiChu;

                db.SoQuys.Add(s);

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =========================
        // LẤY SỐ DƯ CUỐI
        // =========================
        public decimal GetSoDuHienTai()
        {
            using (QLNT_DoVanTieuEntities db =
                new QLNT_DoVanTieuEntities())
            {
                var soDu =
                    db.SoQuys
                    .OrderByDescending(x => x.NgayGiaoDich)
                    .Select(x => x.SoDu)
                    .FirstOrDefault();

                return soDu ?? 0;
            }
        }
        public decimal LaySoDuCuoi()
        {
            var last =
                db.SoQuys
                .OrderByDescending(x => x.ID)
                .FirstOrDefault();

            if (last == null)
                return 0;

            return last.SoDu ?? 0;
        }

        // =========================
        // GET ALL
        // =========================

        public object GetAll()
        {
            return db.SoQuys
                .OrderByDescending(x =>
                    x.NgayGiaoDich)
                .ToList();
        }
        public List<SoQuy_DTO> GetAll1()
        {
            using (QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities())
            {
                return db.SoQuys
                    .OrderByDescending(x => x.NgayGiaoDich)
                    .Select(x => new SoQuy_DTO
                    {
                        MaPhieu = x.MaPhieu,
                        NgayGiaoDich = x.NgayGiaoDich,
                        LoaiGiaoDich = x.LoaiGiaoDich,
                        Thu = x.Thu,
                        Chi = x.Chi,
                        SoDuSauGD = x.SoDu,
                        NoiDung = x.NoiDung,
                        NguoiLap = x.NguoiLap
                    })
                    .ToList();
            }
        }
        public decimal DoanhThuThang()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                int thang = DateTime.Now.Month;
                int nam = DateTime.Now.Year;

                return db.SoQuys
                         .Where(x =>
                            x.LoaiGiaoDich == "Thu"
                            && x.NgayGiaoDich.HasValue
                            && x.NgayGiaoDich.Value.Month == thang
                            && x.NgayGiaoDich.Value.Year == nam)
                         .Sum(x => (decimal?)x.Thu) ?? 0;
            }
        }
        public List<dynamic> DoanhThu12Thang()
        {
            using (var db = new QLNT_DoVanTieuEntities())
            {
                var data =
                    db.SoQuys
                    .Where(x => x.LoaiGiaoDich == "Thu" && x.NgayGiaoDich.HasValue)
                    .GroupBy(x => new
                    {
                        Year = x.NgayGiaoDich.Value.Year,
                        Month = x.NgayGiaoDich.Value.Month
                    })
                    .Select(x => new
                    {
                        Nam = x.Key.Year,
                        Thang = x.Key.Month,
                        TongTien = x.Sum(y => y.Thu)
                    })
                    .OrderBy(x => x.Nam)
                    .ThenBy(x => x.Thang)
                    .ToList();

                return data.Cast<dynamic>().ToList();
            }
        }
    }
}