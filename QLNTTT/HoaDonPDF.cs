using DTO;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
namespace QLNTTT
{
    public class HoaDonPDF
    {
        public static string XuatPDF(
            HoaDon_DTO hd,
            List<ChiTietHoaDon_DTO> ds)
        {
            QuestPDF.Settings.License =
                LicenseType.Community;

            string filePath =
                Path.Combine(
                    Environment.GetFolderPath(
                        Environment.SpecialFolder.Desktop),
                    "HoaDon_" + hd.MaHoaDon + ".pdf");

            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(25);

                    // =========================
                    // HEADER
                    // =========================

                    page.Header().Column(col =>
                    {
                        col.Item().Text(
                            "HÓA ĐƠN NHÀ TRỌ")
                            .FontSize(24)
                            .Bold()
                            .FontColor(Colors.Blue.Darken2);

                        col.Item().Text(
                            "Hệ thống quản lý nhà trọ")
                            .FontSize(11);

                        col.Item().PaddingTop(10);

                        col.Item().Row(row =>
                        {
                            row.RelativeItem().Column(x =>
                            {
                                x.Item().Text(
                                    $"Mã hóa đơn: {hd.MaHoaDon}");

                                x.Item().Text(
                                    $"Phòng: {hd.MaPhong}");

                                x.Item().Text(
                                    $"Ngày lập: {hd.NgayLap:dd/MM/yyyy}");
                            });

                            row.RelativeItem().AlignRight().Column(x =>
                            {
                                x.Item().Text(
                                    $"Tổng tiền: {hd.TongTien:N0} VNĐ")
                                    .Bold();

                                x.Item().Text(
                                    $"Đã thanh toán: {hd.DaThanhToan:N0} VNĐ");

                                x.Item().Text(
                                    $"Còn nợ: {hd.ConNo:N0} VNĐ")
                                    .FontColor(Colors.Red.Medium)
                                    .Bold();
                            });
                        });
                    });

                    // =========================
                    // CONTENT
                    // =========================

                    page.Content().PaddingVertical(20).Column(col =>
                    {
                        // =====================
                        // TABLE
                        // =====================

                        col.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn(1);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(2);
                            });

                            // HEADER

                            table.Header(header =>
                            {
                                header.Cell()
                                    .Background(Colors.Green.Darken2)
                                    .Padding(5)
                                    .Text("STT")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Green.Darken2)
                                    .Padding(5)
                                    .Text("Khoản")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Green.Darken2)
                                    .Padding(5)
                                    .Text("SL")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Green.Darken2)
                                    .Padding(5)
                                    .Text("Đơn giá")
                                    .FontColor(Colors.White)
                                    .Bold();

                                header.Cell()
                                    .Background(Colors.Green.Darken2)
                                    .Padding(5)
                                    .Text("Thành tiền")
                                    .FontColor(Colors.White)
                                    .Bold();
                            });

                            // DATA

                            int stt = 1;

                            foreach (var item in ds)
                            {
                                table.Cell()
                                    .Border(1)
                                    .Padding(5)
                                    .Text(stt.ToString());

                                table.Cell()
                                    .Border(1)
                                    .Padding(5)
                                    .Text(item.TenDanhMuc);

                                table.Cell()
                                    .Border(1)
                                    .Padding(5)
                                    .Text(item.SoLuong.ToString());

                                table.Cell()
                                    .Border(1)
                                    .Padding(5)
                                    .AlignRight()
                                    .Text(
                                        $"{item.DonGia:N0}");

                                table.Cell()
                                    .Border(1)
                                    .Padding(5)
                                    .AlignRight()
                                    .Text(
                                        $"{item.ThanhTien:N0}");

                                stt++;
                            }
                        });

                        col.Item().PaddingTop(20);

                        // =====================
                        // QR + TOTAL
                        // =====================

                        col.Item().Row(row =>
                        {
                            // QR
                            row.RelativeItem().Column(qr =>
                            {
                                qr.Item().Text(
                                    "Quét mã để thanh toán")
                                    .Bold();

                                // =======================
                                // LINK QR
                                // =======================

                                string qrUrl =
     "https://img.vietqr.io/image/ACB-38805157-compact2.png"
     + "?amount="
     + ((int)hd.ConNo.Value).ToString()
     + "&addInfo=ThanhToan_" + hd.MaHoaDon;

                                // file tạm
                                string qrPath =
                                    Path.Combine(
                                        Path.GetTempPath(),
                                        "qr.png");

                                // tải qr về máy
                                using (WebClient wc = new WebClient())
                                {
                                    wc.DownloadFile(
                                        qrUrl,
                                        qrPath);
                                }

                                // hiển thị qr
                                qr.Item()
                                    .Height(120)
                                    .Width(120)
                                    .Image(qrPath);
                            });

                            // TOTAL
                            row.RelativeItem().AlignRight().Column(total =>
                            {
                                total.Item().Text(
                                    $"Tổng tiền: {hd.TongTien:N0} VNĐ")
                                    .FontSize(14);

                                total.Item().Text(
                                    $"Đã thanh toán: {hd.DaThanhToan:N0} VNĐ")
                                    .FontSize(14);

                                total.Item().Text(
                                    $"Còn nợ: {hd.ConNo:N0} VNĐ")
                                    .FontSize(18)
                                    .Bold()
                                    .FontColor(Colors.Red.Medium);
                            });
                        });
                    });

                    // =========================
                    // FOOTER
                    // =========================

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Cảm ơn bạn đã sử dụng dịch vụ")
                                .FontSize(10);
                        });
                });
            })
            .GeneratePdf(filePath);

            return filePath;
        }
    }
}