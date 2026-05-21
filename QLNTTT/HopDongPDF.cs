using DAL;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

public class HopDongPDF
{
    // Helper để định dạng ô bảng nhanh và sạch
    static IContainer CellStyle(IContainer container) => container.Border(0.5f).Padding(5).AlignMiddle();

    public static void XuatPDF(
    HopDong hd,
    List<CuDan> dsCuDan,
    List<HopDong_DichVu> dsDV,
    List<HopDong_TaiSan> dsTS,
    string path)
    {
        // 1. Cấu hình License (Bắt buộc cho QuestPDF Community)
        QuestPDF.Settings.License = LicenseType.Community;

        // 2. Đường dẫn lưu file ra Desktop
       /* string path = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
            $"HopDong_{hd.MaHopDong}.pdf");*/

        // 3. Tạo tài liệu
        Document.Create(container =>
        {
            container.Page(page =>
            {
                // Cấu hình trang
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(11).FontFamily("Times New Roman"));

                // TOÀN BỘ NỘI DUNG NẰM TRONG COLUMN NÀY
                page.Content().PaddingVertical(10).Column(col =>
                {
                    // === PHẦN TIÊU ĐỀ ===
                    col.Item().AlignCenter().Text("CỘNG HÒA XÃ HỘI CHỦ NGHĨA VIỆT NAM").Bold().FontSize(12);
                    col.Item().AlignCenter().Text("Độc lập - Tự do - Hạnh phúc").Bold().FontSize(11);
                    col.Item().AlignCenter().Text("---------------").FontSize(10);
                    col.Item().PaddingTop(15).AlignCenter().Text("HỢP ĐỒNG THUÊ PHÒNG TRỌ").FontSize(18).Bold();
                    col.Item().AlignCenter().Text($"(Mã hợp đồng: {hd.MaHopDong})").Italic().FontSize(10);
                    col.Item().PaddingTop(10).Text($"Hôm nay, ngày {DateTime.Now.Day} tháng {DateTime.Now.Month} năm {DateTime.Now.Year}, Chúng tôi ký tên dưới đây gồm có:");
                    // === I. THÔNG TIN PHÒNG ===
                    col.Item().PaddingTop(20).Text("I. THÔNG TIN PHÒNG THUÊ").Bold().Underline();
                    col.Item().PaddingLeft(10).Column(c =>
                    {
                        c.Item().Text($"- Mã phòng: {hd.MaPhong}");
                        c.Item().Text($"- Thời hạn thuê: Từ ngày {hd.NgayBatDau?.ToString("dd/MM/yyyy")} đến ngày {hd.NgayKetThuc?.ToString("dd/MM/yyyy")}");
                        c.Item().Text($"- Giá thuê phòng: {hd.GiaThue:N0} VNĐ/tháng");
                        c.Item().Text($"- Tiền đặt cọc: {hd.TienCoc:N0} VNĐ");
                        c.Item().Text($"- Giá điện: {hd.GiaDienChot:N0} VNĐ/KWh");

                        c.Item().Text($"- Giá nước: {hd.GiaNuocChot:N0} VNĐ/m³");
                    });

                    // === II. DANH SÁCH CƯ DÂN ===
                    col.Item().PaddingTop(15).Text("II. DANH SÁCH NGƯỜI Ở (CƯ DÂN)").Bold().Underline();
                    col.Item().PaddingTop(5).Table(table =>
                    {
                        table.ColumnsDefinition(c => {
                            c.ConstantColumn(30);
                            c.RelativeColumn();
                            c.RelativeColumn();
                        });
                        table.Header(h => {
                            h.Cell().Element(CellStyle).AlignCenter().Text("STT").Bold();
                            h.Cell().Element(CellStyle).Text("Họ và Tên").Bold();
                            h.Cell().Element(CellStyle).Text("Số CCCD/Passport").Bold();
                        });
                        int stt = 1;
                        foreach (var cd in dsCuDan)
                        {
                            table.Cell().Element(CellStyle).AlignCenter().Text((stt++).ToString());
                            table.Cell().Element(CellStyle).Text(cd.TenCuDan);
                            table.Cell().Element(CellStyle).Text(cd.CCCD);
                        }
                    });

                    // === III. DỊCH VỤ & TÀI SẢN ===
                    col.Item().PaddingTop(15).Row(row =>
                    {
                        // Cột dịch vụ
                        row.RelativeItem().Column(c => {
                            c.Item().Text("III. DỊCH VỤ ĐĂNG KÝ").Bold().Underline();
                            foreach (var dv in dsDV)
                                c.Item().PaddingLeft(5).Text($"+ {dv.DanhMucDichVu.TenDichVu}: {dv.DonGia:N0}đ");
                        });
                        // Cột tài sản
                        row.RelativeItem().Column(c => {
                            c.Item().Text("IV. TÀI SẢN BÀN GIAO").Bold().Underline();
                            foreach (var ts in dsTS)
                                c.Item().PaddingLeft(5).Text($"+ {ts.MaTaiSan}");
                        });
                    });

                    // === VII. ĐIỀU KHOẢN HỢP ĐỒNG ===
                    col.Item().PaddingTop(25).Text("VII. CÁC ĐIỀU KHOẢN THỎA THUẬN").Bold().FontSize(13);

                    col.Item().PaddingTop(5).Column(d => {
                        // Điều 1
                        d.Item().PaddingTop(5).Text("Điều 1. Thanh toán:").Bold();
                        d.Item().PaddingLeft(10).Text("Bên B thanh toán tiền thuê và phí dịch vụ vào ngày 05 hàng tháng. Quá hạn 05 ngày sẽ bị tính phí chậm nộp.");

                        // Điều 2
                        d.Item().PaddingTop(5).Text("Điều 2. Tiền đặt cọc:").Bold();
                        d.Item().PaddingLeft(10).Text("Tiền cọc đảm bảo trách nhiệm bảo quản tài sản. Bên B sẽ nhận lại tiền cọc khi thanh toán đủ các khoản phí và bàn giao phòng đúng hạn.");

                        // Điều 3
                        d.Item().PaddingTop(5).Text("Điều 3. Quy định cư trú:").Bold();
                        d.Item().PaddingLeft(10).Text("Bên B phải đăng ký tạm trú đúng quy định. Không tự ý cho người lạ ở lại qua đêm khi chưa báo trước.");

                        // Điều 4
                        d.Item().PaddingTop(5).Text("Điều 4. Bảo quản tài sản:").Bold();
                        d.Item().PaddingLeft(10).Text("Nếu tài sản hư hỏng do lỗi chủ quan của Bên B, Bên B có trách nhiệm đền bù theo giá trị thị trường.");

                        // Điều 5
                        d.Item().PaddingTop(5).Text("Điều 5. An ninh & PCCC:").Bold();
                        d.Item().PaddingLeft(10).Text("Nghiêm cấm tàng trữ chất dễ cháy nổ, chất cấm. Tuân thủ nội quy chung của khu nhà.");

                        // Điều 6
                        d.Item().PaddingTop(5).Text("Điều 6. Chấm dứt hợp đồng:").Bold();
                        d.Item().PaddingLeft(10).Text("Bên nào muốn chấm dứt trước thời hạn phải thông báo cho bên còn lại trước ít nhất 30 ngày.");

                        // Điều 7
                        d.Item().PaddingTop(5).Text("Điều 7. Cam kết chung:").Bold();
                        d.Item().PaddingLeft(10).Text("Hai bên cam kết thực hiện đúng các điều khoản trên. Hợp đồng này có giá trị pháp lý kể từ ngày ký.");
                    });

                    // === PHẦN CHỮ KÝ ===
                    col.Item().PaddingTop(40).Row(row =>
                    {
                        row.RelativeItem().Column(c => {
                            c.Item().AlignCenter().Text("BÊN CHO THUÊ (BÊN A)").Bold();
                            c.Item().AlignCenter().Text("(Ký và ghi rõ họ tên)").Italic();
                            c.Item().PaddingTop(50);
                        });
                        row.RelativeItem().Column(c => {
                            c.Item().AlignCenter().Text("BÊN THUÊ (BÊN B)").Bold();
                            c.Item().AlignCenter().Text("(Ký và ghi rõ họ tên)").Italic();
                            c.Item().PaddingTop(50);
                        });
                    });
                });

                // Chân trang (Footer)
                page.Footer().AlignCenter().Text(x =>
                {
                    x.Span("Trang ");
                    x.CurrentPageNumber();
                });
            });
        })
        .GeneratePdf(path);

        // Mở file ngay sau khi tạo thành công
        try
        {
            Process.Start(new ProcessStartInfo(path) { UseShellExecute = true });
        }
        catch (Exception) { /* Xử lý nếu máy không có trình đọc PDF mặc định */ }
    }
}