using DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class HopDong_DichVu_DAL
    {
        QLNT_DoVanTieuEntities db = new QLNT_DoVanTieuEntities();
        public List<HopDong_DichVu> GetDV(string maHD)
        {
            return db.HopDong_DichVu
                .Where(x => x.MaHopDong == maHD)
                .ToList();
        }
        // =====================
        // LOAD
        // =====================

        public List<HopDong_DichVu_DTO>
            GetByHopDong(string maHD)
        {
            try
            {
                return db.HopDong_DichVu
                    .Where(x =>
                        x.MaHopDong == maHD)
                    .Select(x =>
                        new HopDong_DichVu_DTO()
                        {
                            ID = x.ID,

                            MaHopDong =
                                x.MaHopDong,

                            MaDichVu =
                                x.MaDichVu,

                            TenDichVu =
                                x.DanhMucDichVu.TenDichVu,

                            DonGia =
                                x.DonGia,

                            HinhThucTinh =
                                x.DanhMucDichVu.HinhThucTinh,

                            NgayBatDau =
                                x.NgayBatDau,

                            NgayNgung =
                                x.NgayNgung,

                            TrangThai =
                                x.TrangThai
                        })
                    .ToList();
            }
            catch
            {
                return new List
                    <HopDong_DichVu_DTO>();
            }
        }

        // =====================
        // INSERT
        // =====================

        public string Insert(
            HopDong_DichVu_DTO dto)
        {
            try
            {
                bool check =
                    db.HopDong_DichVu.Any(x =>
                        x.MaHopDong
                            == dto.MaHopDong
                        &&
                        x.MaDichVu
                            == dto.MaDichVu
                        &&
                        x.TrangThai
                            == "Đang dùng");

                if (check)
                {
                    return
                        "Dịch vụ đã tồn tại";
                }

                HopDong_DichVu hd =
                    new HopDong_DichVu();

                hd.MaHopDong =
                    dto.MaHopDong;

                hd.MaDichVu =
                    dto.MaDichVu;
                hd.TenDichVu =
                    dto.TenDichVu;

                hd.DonGia =
                    dto.DonGia;

                hd.NgayBatDau =
                    dto.NgayBatDau;
                hd.HinhThucTinh =
                    dto.HinhThucTinh;

                hd.TrangThai =
                    dto.TrangThai;

                db.HopDong_DichVu.Add(hd);

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =====================
        // NGƯNG DỊCH VỤ
        // =====================

        public string NgungDichVu(
            string maHD,
            string maDV)
        {
            try
            {
                var dv =
                    db.HopDong_DichVu
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaDichVu == maDV
                        &&
                        x.TrangThai
                            == "Đang dùng");

                if (dv == null)
                {
                    return
                        "Không tìm thấy dịch vụ";
                }

                dv.TrangThai =
                    "Ngưng dùng";

                dv.NgayNgung =
                    DateTime.Now;

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        // =====================
        // ĐỔI GIÁ
        // =====================

        public string DoiGia(
            string maHD,
            string maDV,
            decimal giaMoi)
        {
            try
            {
                var dv =
                    db.HopDong_DichVu
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaDichVu == maDV
                        &&
                        x.TrangThai
                            == "Đang dùng");

                if (dv == null)
                {
                    return
                        "Không tìm thấy dịch vụ";
                }

                dv.DonGia = giaMoi;

                return db.SaveChanges() > 0
                    ? "success"
                    : "fail";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public string SuDungLai(
    string maHD,
    string maDV)
        {
            try
            {
                var item =
                    db.HopDong_DichVu
                    .FirstOrDefault(x =>
                        x.MaHopDong == maHD
                        &&
                        x.MaDichVu == maDV);

                if (item == null)
                    return "Không tìm thấy dịch vụ";

                item.TrangThai =
                    "Đang dùng";

                item.NgayNgung =
                    null;

                item.NgayBatDau =
                    DateTime.Now;

                db.SaveChanges();

                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
