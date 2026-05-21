using BUS;
using DAL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Globalization;
using System.Text;
namespace QLNTTT
{
    public partial class LapHoaDon : Form
    {
       
        
        public LapHoaDon()
        {
            InitializeComponent();
            QuestPDF.Settings.License =
        LicenseType.Community;
        }
        Phong_BUS phongBUS = new Phong_BUS();
        PhuLucHopDong_BUS phuLuc_BUS =
    new PhuLucHopDong_BUS();
        HoaDon_BUS hoaDonBUS = new HoaDon_BUS();
        ChiTietHoaDon_BUS chiTietBUS =  new ChiTietHoaDon_BUS();
        bool isLoaded = false;
        void LoadPhong()
        {
            isLoaded = false;

            cbbPhong.DataSource =
                phongBUS.GetAll();

            cbbPhong.DisplayMember =
                "TenPhong";

            cbbPhong.ValueMember =
                "MaPhong";

            cbbPhong.SelectedIndex = -1;

            isLoaded = true;
        }
        private void TaoCotDGV()
        {
            dgvChiTiet.Columns.Clear();

            dgvChiTiet.Columns.Add(
                "Ten",
                "Khoản thu");

            dgvChiTiet.Columns.Add(
                "SoLuong",
                "Số lượng");

            dgvChiTiet.Columns.Add(
                "DonGia",
                "Đơn giá");

            dgvChiTiet.Columns.Add(
                "ThanhTien",
                "Thành tiền");

            // FORMAT TIỀN

            dgvChiTiet.Columns["DonGia"]
                .DefaultCellStyle.Format = "N0";

            dgvChiTiet.Columns["ThanhTien"]
                .DefaultCellStyle.Format = "N0";
        }
        void loadDichVu()
        {
            dgvDichVu.AutoGenerateColumns = true;
            dgvDichVu.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDichVu.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDichVu.ReadOnly = true;
        }
        private void LapHoaDon_Load(object sender, EventArgs e)
        {
            LoadPhong();
            
            phuLuc_BUS.ApDungPhuLuc();

            dtNgayLap.Value =
                DateTime.Now;
            TaoCotDGV();
            loadDichVu();

        }
        decimal giaPhong = 0;
        decimal giaDien = 0;
        decimal giaNuoc = 0;

        int soNguoi = 0;

        List<HopDong_DichVu_DTO> dsDichVu =
            new List<HopDong_DichVu_DTO>();
        private void cbbPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!isLoaded)
                return;

            if (cbbPhong.SelectedValue == null)
                return;

            string maPhong =
                cbbPhong.SelectedValue
                .ToString();

            var data =
     hoaDonBUS
     .GetThongTin(maPhong);

            if (data == null)
            {
                MessageBox.Show(
                    "Phòng chưa có hợp đồng");

                return;
            }

            maHopDong = data.MaHopDong;

            // =====================
            // GIÁ
            // =====================

            giaPhong =
                data.GiaPhong;

            giaDien =
                data.GiaDien;

            giaNuoc =
                data.GiaNuoc;

            soNguoi =
                data.SoNguoi;

            // =====================
            // CHỈ SỐ
            // =====================

            txtSoDienCu.Text =
                data.SoDienCu.ToString();

            txtSoNuocCu.Text =
                data.SoNuocCu.ToString();

            // =====================
            // ẢNH CŨ
            // =====================

            if (!string.IsNullOrEmpty(
                data.AnhDienCu))
            {
                picAnhDienCu.ImageLocation =
                    data.AnhDienCu;
            }

            if (!string.IsNullOrEmpty(
                data.AnhNuocCu))
            {
                picAnhNuocCu.ImageLocation =
                    data.AnhNuocCu;
            }
            lblGiaPhong.Text ="Giá phòng: " +
    giaPhong.ToString("N0") + " VNĐ";

            lblGiaDien.Text =
                "Giá điện: " + giaDien.ToString("N0") + " VNĐ";
            lblGiaNuoc.Text =
                "Giá nước: " + giaNuoc.ToString("N0") + " VNĐ";

            lblSoNguoi.Text =
                "Số người: " + soNguoi.ToString();
            // =====================
            // DỊCH VỤ
            // =====================

            dsDichVu =
                data.DichVus;

            dgvDichVu.DataSource =dsDichVu;

            // Đổi tên cột
            dgvDichVu.Columns["MaDichVu"].HeaderText = "Mã dịch vụ";
            dgvDichVu.Columns["TenDichVu"].HeaderText = "Tên dịch vụ";
            dgvDichVu.Columns["DonGia"].HeaderText = "Đơn giá";
            dgvDichVu.Columns["HinhThucTinh"].HeaderText = "Hình thức tính";

            // Format tiền
            dgvDichVu.Columns["DonGia"].DefaultCellStyle.Format = "N0";

            // Ẩn các cột không cần thiết
            dgvDichVu.Columns["MaHopDong"].Visible = false;
            dgvDichVu.Columns["NgayBatDau"].Visible = false;
            dgvDichVu.Columns["NgayNgung"].Visible = false;
            dgvDichVu.Columns["TrangThai"].Visible = false;
        }
        decimal tongTien = 0;
        private string RemoveDiacritics(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return "";

            string normalized = text.Normalize(NormalizationForm.FormD);
            StringBuilder sb = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c)
                    != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
        void TinhTongTien()
        {
            try
            {
                dgvChiTiet.Rows.Clear();

                tongTien = 0;

                // =====================
                // VALIDATE
                // =====================

                if (string.IsNullOrWhiteSpace(txtSoDienCu.Text))
                {
                    MessageBox.Show("Chưa có số điện cũ");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoNuocCu.Text))
                {
                    MessageBox.Show("Chưa có số nước cũ");
                    return;
                }

                int dienCu = 0;
                int dienMoi = 0;

                int nuocCu = 0;
                int nuocMoi = 0;

                // =====================
                // CHECK ĐIỆN
                // =====================

                if (!int.TryParse(txtSoDienCu.Text, out dienCu))
                {
                    MessageBox.Show("Số điện cũ không hợp lệ");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoDienMoi.Text))
                {
                    dienMoi = dienCu;
                }
                else
                {
                    if (!int.TryParse(txtSoDienMoi.Text, out dienMoi))
                    {
                        MessageBox.Show("Số điện mới không hợp lệ");
                        return;
                    }
                }

                if (dienMoi < dienCu)
                {
                    MessageBox.Show(
                        "Số điện mới không được nhỏ hơn số cũ");

                    txtSoDienMoi.Focus();
                    return;
                }

                // =====================
                // CHECK NƯỚC
                // =====================

                if (!int.TryParse(txtSoNuocCu.Text, out nuocCu))
                {
                    MessageBox.Show("Số nước cũ không hợp lệ");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSoNuocMoi.Text))
                {
                    nuocMoi = nuocCu;
                }
                else
                {
                    if (!int.TryParse(txtSoNuocMoi.Text, out nuocMoi))
                    {
                        MessageBox.Show("Số nước mới không hợp lệ");
                        return;
                    }
                }

                if (nuocMoi < nuocCu)
                {
                    MessageBox.Show(
                        "Số nước mới không được nhỏ hơn số cũ");

                    txtSoNuocMoi.Focus();
                    return;
                }

                // =====================
                // TIỀN PHÒNG
                // =====================

                dgvChiTiet.Rows.Add(
                    "Tiền phòng",
                    1,
                    giaPhong,
                    giaPhong);

                tongTien += giaPhong;

                // =====================
                // ĐIỆN
                // =====================

                int soDien = dienMoi - dienCu;

                decimal tienDien =
                    soDien * giaDien;

                txtTienDien.Text =
                    tienDien.ToString("N0");

                dgvChiTiet.Rows.Add(
                    "Điện",
                    soDien,
                    giaDien,
                    tienDien);

                tongTien += tienDien;

                // =====================
                // NƯỚC
                // =====================

                int soNuoc = nuocMoi - nuocCu;

                decimal tienNuoc =
                    soNuoc * giaNuoc;

                txtTienNuoc.Text =
                    tienNuoc.ToString("N0");

                dgvChiTiet.Rows.Add(
                    "Nước",
                    soNuoc,
                    giaNuoc,
                    tienNuoc);

                tongTien += tienNuoc;

                // =====================
                // DỊCH VỤ
                // =====================

                if (dsDichVu != null && dsDichVu.Count > 0)
                {
                    foreach (var dv in dsDichVu)
                    {
                        decimal sl = 1;
                        decimal donGia = dv.DonGia ?? 0;
                        decimal thanhTien = 0;

                        string tenDV =
                            string.IsNullOrWhiteSpace(dv.TenDichVu)
                            ? dv.MaDichVu
                            : dv.TenDichVu;

                        // =====================
                        // CHUẨN HÓA HÌNH THỨC
                        // =====================

                        string hinhThuc = RemoveDiacritics(dv.HinhThucTinh ?? "")
                            .Trim()
                            .ToLower();

                        // =====================
                        // DEBUG (nếu cần)
                        // =====================
                        // MessageBox.Show(dv.MaDichVu + " - " + hinhThuc);

                        bool isDauNguoi = hinhThuc.Contains("dau_nguoi");
                        bool isPhong = hinhThuc.Contains("phong");
                        bool isXe = hinhThuc.Contains("xe");
                        bool isTieuThu = hinhThuc.Contains("tieu_thu");

                        // =====================
                        // XỬ LÝ
                        // =====================

                        if (isDauNguoi)
                        {
                            sl = soNguoi > 0 ? soNguoi : 1;
                            thanhTien = donGia * sl;
                        }
                        else if (isPhong)
                        {
                            sl = 1;
                            thanhTien = donGia;
                        }
                        else if (isXe)
                        {
                            int soXe = 2;
                            sl = soXe;
                            thanhTien = donGia * soXe;
                        }
                        else if (isTieuThu)
                        {
                            sl = 1;
                            thanhTien = donGia;
                        }
                        else
                        {
                            // ❗ fallback an toàn nhưng có cảnh báo
                            MessageBox.Show("Không nhận dạng Hình Thức Tính: " + dv.HinhThucTinh);

                            sl = 1;
                            thanhTien = donGia;
                        }

                        dgvChiTiet.Rows.Add(
                            tenDV,
                            sl,
                            donGia,
                            thanhTien
                        );

                        tongTien += thanhTien;
                    }
                }
                // =====================
                // HIỂN THỊ TỔNG
                // =====================

                lblTongTien.Text =
                    tongTien.ToString("N0")
                    + " VNĐ";
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi tính tiền: " + ex.Message);
            }
        }
        string pathAnhDien = "";

        string pathAnhNuoc = "";
        string maHopDong = "";
        private void txtSoDienMoi_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void txtSoNuocMoi_TextChanged(object sender, EventArgs e)
        {
            TinhTongTien();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // =====================
                // VALIDATE
                // =====================

                if (string.IsNullOrWhiteSpace(
                    txtMaHoaDon.Text))
                {
                    MessageBox.Show(
                        "Vui lòng nhập mã hóa đơn");

                    return;
                }

                if (cbbPhong.SelectedValue == null)
                {
                    MessageBox.Show(
                        "Vui lòng chọn phòng");

                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    txtSoDienMoi.Text))
                {
                    MessageBox.Show(
                        "Vui lòng nhập số điện mới");

                    return;
                }

                if (string.IsNullOrWhiteSpace(
                    txtSoNuocMoi.Text))
                {
                    MessageBox.Show(
                        "Vui lòng nhập số nước mới");

                    return;
                }

                if (string.IsNullOrEmpty(
                    maHopDong))
                {
                    MessageBox.Show(
                        "Phòng chưa có hợp đồng");

                    return;
                }

                // =====================
                // HEADER
                // =====================

                HoaDon_DTO hd =
                    new HoaDon_DTO();

                hd.MaHoaDon =
                    txtMaHoaDon.Text.Trim();

                hd.MaHopDong =
                    maHopDong;

                hd.MaPhong =
                    cbbPhong.SelectedValue
                    .ToString();

                hd.KyHoaDon =
                    DateTime.Now.Month
                    + "/"
                    + DateTime.Now.Year;

                hd.NgayLap =
  dtNgayLap.Value;

                // =====================
                // TIỀN ĐỀN BÙ
                // =====================

                decimal tienDenBu = 0;

                using (QLNT_DoVanTieuEntities db =
                    new QLNT_DoVanTieuEntities())
                {
                    tienDenBu =
                        db.CongViecs
                        .Where(x =>
                            x.MaPhong == hd.MaPhong
                            &&
                            x.TrangThai == "Hoàn thành"
                            &&
                            x.TienDenBu > 0)
                        .Sum(x => (decimal?)x.TienDenBu)
                        ?? 0;
                }

                // cộng vào hóa đơn

                tongTien += tienDenBu;

                hd.TongTien =
                    tongTien;

                hd.DaThanhToan =
                    0;

                hd.ConNo =
                    tongTien;

                hd.TrangThai =
                    "Chưa thanh toán";

                // =====================
                // KIỂM TRA TRÙNG
                // =====================

                bool daTonTai =
                    hoaDonBUS.KiemTraHoaDonTonTai(
                        hd.MaPhong,
                        hd.KyHoaDon);

                if (daTonTai)
                {
                    MessageBox.Show(
                        "Phòng này đã lập hóa đơn tháng "
                        + hd.KyHoaDon);

                    return;
                }

                // =====================
                // INSERT HÓA ĐƠN
                // =====================

                string kq =
                    hoaDonBUS.Insert(hd);

                if (kq != "success")
                {
                    MessageBox.Show(kq);

                    return;
                }

                // =====================
                // DETAIL
                // =====================

                foreach (
                    DataGridViewRow row
                    in dgvChiTiet.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    if (row.Cells["Ten"].Value == null)
                        continue;

                    ChiTietHoaDon_DTO ct =
                        new ChiTietHoaDon_DTO();

                    ct.MaHoaDon =
                        hd.MaHoaDon;

                    ct.TenDanhMuc =
                        row.Cells["Ten"]
                        .Value.ToString();

                    ct.SoLuong =
                        Convert.ToDecimal(
                            row.Cells["SoLuong"]
                            .Value);

                    ct.DonGia =
                        Convert.ToDecimal(
                            row.Cells["DonGia"]
                            .Value);

                    ct.ThanhTien =
                        Convert.ToDecimal(
                            row.Cells["ThanhTien"]
                            .Value);

                    // =====================
                    // ĐIỆN
                    // =====================

                    if (ct.TenDanhMuc
                        == "Điện")
                    {
                        ct.LoaiChiTiet =
                            "Điện";

                        ct.ChiSoCu =
                            decimal.Parse(
                                txtSoDienCu.Text);

                        ct.ChiSoMoi =
                            decimal.Parse(
                                txtSoDienMoi.Text);

                        ct.AnhChiSo =
                            pathAnhDien;
                    }

                    // =====================
                    // NƯỚC
                    // =====================

                    else if (
                        ct.TenDanhMuc
                        == "Nước")
                    {
                        ct.LoaiChiTiet =
                            "Nước";

                        ct.ChiSoCu =
                            decimal.Parse(
                                txtSoNuocCu.Text);

                        ct.ChiSoMoi =
                            decimal.Parse(
                                txtSoNuocMoi.Text);

                        ct.AnhChiSo =
                            pathAnhNuoc;
                    }

                    // =====================
                    // DỊCH VỤ
                    // =====================

                    else
                    {
                        ct.LoaiChiTiet =
                            "Dịch vụ";
                    }

                    string kqCT =
                        chiTietBUS.Insert(ct);

                    if (kqCT != "success")
                    {
                        MessageBox.Show(kqCT);

                        return;
                    }
                }

                // =====================
                // ĐỀN BÙ HƯ HỎNG
                // =====================

                if (tienDenBu > 0)
                {
                    ChiTietHoaDon_DTO ctDenBu =
                        new ChiTietHoaDon_DTO();

                    ctDenBu.MaHoaDon =
                        hd.MaHoaDon;

                    ctDenBu.LoaiChiTiet =
                        "Đền bù";

                    ctDenBu.TenDanhMuc =
                        "Đền bù hư hỏng";

                    ctDenBu.SoLuong = 1;

                    ctDenBu.DonGia =
                        tienDenBu;

                    ctDenBu.ThanhTien =
                        tienDenBu;

                    chiTietBUS.Insert(ctDenBu);
                }
                using (QLNT_DoVanTieuEntities db =
                    new QLNT_DoVanTieuEntities())
                {
                    var dsCV =
                        db.CongViecs
                        .Where(x =>
                            x.MaPhong == hd.MaPhong
                            &&
                            x.TienDenBu > 0)
                        .ToList();

                    foreach (var cv in dsCV)
                    {
                        cv.TienDenBu = 0;
                    }

                    db.SaveChanges();
                }

                // =====================
                // UPDATE CHỈ SỐ
                // =====================

                phongBUS.UpdateChiSo(
                    cbbPhong.SelectedValue
                    .ToString(),

                    int.Parse(
                        txtSoDienMoi.Text),

                    int.Parse(
                        txtSoNuocMoi.Text),

                    pathAnhDien,

                    pathAnhNuoc);

                // =====================
                // SUCCESS
                // =====================

                MessageBox.Show(
                    "Lập hóa đơn thành công");

                dgvChiTiet.Rows.Clear();

                lblTongTien.Text =
                    "0 VNĐ";
            }
            catch (Exception ex)
            {
                string msg = ex.Message;

                if (ex.InnerException != null)
                {
                    msg +=
                        "\n\n" +
                        ex.InnerException.Message;
                }

                if (ex.InnerException
                    ?.InnerException != null)
                {
                    msg +=
                        "\n\n" +
                        ex.InnerException
                        .InnerException
                        .Message;
                }

                MessageBox.Show(msg);
            }
        }

        private void btnAnhDien_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd =
       new OpenFileDialog();

            ofd.Filter =
                "Image Files|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog()
                == DialogResult.OK)
            {
                // lưu path

                pathAnhDien =
                    ofd.FileName;

                // preview ảnh

                picDien.Image =
     System.Drawing.Image.FromFile(
         pathAnhDien);
            }
        }

        private void btnAnhNuoc_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd =
       new OpenFileDialog();

            ofd.Filter =
                "Image Files|*.jpg;*.jpeg;*.png";

            if (ofd.ShowDialog()
                == DialogResult.OK)
            {
                pathAnhNuoc =
                    ofd.FileName;
                picNuoc.Image =
                    System.Drawing.Image.FromFile(
                        pathAnhNuoc);
            }
        }
        QuestPDF.Infrastructure.IContainer CellStyle(
      QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(1)
                .BorderColor(
                    Colors.Grey.Lighten2)
                .Padding(5);
        }
        private void XuatPDF()
        {
            try
            {
                // =====================
                // FILE NAME
                // =====================

                string fileName =
                    "HoaDon_" +
                    txtMaHoaDon.Text +
                    ".pdf";

                string path =
                    Path.Combine(
                        Environment.GetFolderPath(
                            Environment
                            .SpecialFolder
                            .Desktop),
                        fileName);

                // =====================
                // CREATE PDF
                // =====================

                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        // =====================
                        // PAGE
                        // =====================

                        page.Margin(30);

                        page.Size(PageSizes.A4);

                        page.DefaultTextStyle(x =>
                            x.FontSize(12));

                        // =====================
                        // HEADER
                        // =====================

                        page.Header()
                        .AlignCenter()
                        .Text("HÓA ĐƠN NHÀ TRỌ")
                        .Bold()
                        .FontSize(22)
                        .FontColor(
                            Colors.Blue.Medium);

                        // =====================
                        // CONTENT
                        // =====================

                        page.Content()
                        .Column(col =>
                        {
                            // =====================
                            // THÔNG TIN
                            // =====================

                            col.Item().Text(
                                "Mã hóa đơn: "
                                + txtMaHoaDon.Text);

                            col.Item().Text(
                                "Phòng: "
                                + cbbPhong.Text);

                            col.Item().Text(
                                "Ngày lập: "
                                + DateTime.Now
                                .ToString("dd/MM/yyyy"));

                            col.Item().Text(
                                "Tổng tiền: "
                                + lblTongTien.Text);

                            col.Item()
                            .PaddingBottom(15);

                            // =====================
                            // TABLE
                            // =====================

                            col.Item()
                            .Table(table =>
                            {
                                // =====================
                                // COLUMN
                                // =====================

                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn(3);

                                    columns.RelativeColumn(2);

                                    columns.RelativeColumn(2);

                                    columns.RelativeColumn(3);
                                });

                                // =====================
                                // HEADER
                                // =====================

                                table.Header(header =>
                                {
                                    header.Cell()
                                    .Element(CellStyle)
                                    .Text("Khoản");

                                    header.Cell()
                                    .Element(CellStyle)
                                    .Text("Số lượng");

                                    header.Cell()
                                    .Element(CellStyle)
                                    .Text("Đơn giá");

                                    header.Cell()
                                    .Element(CellStyle)
                                    .Text("Thành tiền");
                                });

                                // =====================
                                // DATA
                                // =====================

                                foreach (
                                    DataGridViewRow row
                                    in dgvChiTiet.Rows)
                                {
                                    if (row.IsNewRow)
                                        continue;

                                    table.Cell()
                                    .Element(CellStyle)
                                    .Text(
                                        row.Cells["Ten"]
                                        .Value?
                                        .ToString());

                                    table.Cell()
                                    .Element(CellStyle)
                                    .Text(
                                        row.Cells["SoLuong"]
                                        .Value?
                                        .ToString());

                                    table.Cell()
                                    .Element(CellStyle)
                                    .Text(
                                        row.Cells["DonGia"]
                                        .Value?
                                        .ToString());

                                    table.Cell()
                                    .Element(CellStyle)
                                    .Text(
                                        row.Cells["ThanhTien"]
                                        .Value?
                                        .ToString());
                                }
                            });

                            // =====================
                            // TỔNG TIỀN
                            // =====================

                            col.Item()
                            .AlignRight()
                            .PaddingTop(20)
                            .Text(
                                "TỔNG TIỀN: "
                                + lblTongTien.Text)
                            .Bold()
                            .FontSize(16);

                            // =====================
                            // ẢNH ĐIỆN
                            // =====================

                            if (
                                !string.IsNullOrEmpty(
                                    pathAnhDien)
                                &&
                                File.Exists(
                                    pathAnhDien))
                            {
                                col.Item()
                                .PaddingTop(20)
                                .Text(
                                    "Ảnh công tơ điện")
                                .Bold();

                                col.Item()
                                .Height(180)
                                .Image(
                                    pathAnhDien);
                            }

                            // =====================
                            // ẢNH NƯỚC
                            // =====================

                            if (
                                !string.IsNullOrEmpty(
                                    pathAnhNuoc)
                                &&
                                File.Exists(
                                    pathAnhNuoc))
                            {
                                col.Item()
                                .PaddingTop(20)
                                .Text(
                                    "Ảnh công tơ nước")
                                .Bold();

                                col.Item()
                                .Height(180)
                                .Image(
                                    pathAnhNuoc);
                            }
                        });

                        // =====================
                        // FOOTER
                        // =====================

                        page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span(
                                "Xuất bởi hệ thống ");

                            x.Span(
                                "QL Nhà Trọ")
                            .SemiBold();
                        });
                    });
                })
                .GeneratePdf(path);

                // =====================
                // SUCCESS
                // =====================

                MessageBox.Show(
                    "Xuất PDF thành công");

                System.Diagnostics
                .Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message);
            }
        }
        private void btnInPDF_Click(object sender, EventArgs e)
        {
            XuatPDF();
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            QuanLyHoaDon ql = new QuanLyHoaDon();
            ql.ShowDialog();
        }
    }
}
