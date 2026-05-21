using BUS;
using DTO;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

using WordDocument =
    DocumentFormat.OpenXml.Wordprocessing.Document;

using System.IO;
//using Microsoft.VisualBasic.Interaction.InputBox;

namespace QLNTTT
{
    public partial class ChiTietHopDong : Form
    {
        public ChiTietHopDong(string maHD)
        {
            InitializeComponent();
            this.maHD = maHD;
        }
        string maHD = "";
        HopDong_BUS hopDong_BUS = new HopDong_BUS();

        Phong_BUS phong_BUS = new Phong_BUS();
        HopDong_CuDan_BUS hopDong_CuDan_BUS =
    new HopDong_CuDan_BUS();
        HopDong_DichVu_BUS
    hopDong_DichVu_BUS =
    new HopDong_DichVu_BUS();
        HopDong_TaiSan_BUS
    hopDong_TaiSan_BUS =
    new HopDong_TaiSan_BUS();
        PhuLucHopDong_BUS phuLuc_BUS = new PhuLucHopDong_BUS();
        TaiSan_BUS taiSan_BUS = new TaiSan_BUS();
        void LoadPhuLuc()
        {
            dgvPhuLuc.DataSource =
                phuLuc_BUS
                .GetByHopDong(maHD);

            dgvPhuLuc.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvPhuLuc.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvPhuLuc.ReadOnly = true;

            dgvPhuLuc.AllowUserToAddRows = false;

            // =====================
            // HEADER
            // =====================

            dgvPhuLuc.Columns["ID"]
                .Visible = false;

            dgvPhuLuc.Columns["MaHopDong"]
                .Visible = false;

            dgvPhuLuc.Columns["LoaiPhuLuc"]
                .HeaderText = "Loại phụ lục";

            dgvPhuLuc.Columns["GiaThueMoi"]
                .HeaderText = "Giá thuê mới";

            dgvPhuLuc.Columns["GiaDienMoi"]
                .HeaderText = "Giá điện mới";

            dgvPhuLuc.Columns["GiaNuocMoi"]
                .HeaderText = "Giá nước mới";

            dgvPhuLuc.Columns["GiaCocMoi"]
                .HeaderText = "Tiền cọc mới";

            dgvPhuLuc.Columns["ThoiGianMoi"]
                .HeaderText = "Hết hạn mới";

            dgvPhuLuc.Columns["NgayTao"]
                .HeaderText = "Ngày tạo";

            dgvPhuLuc.Columns["NguoiThucHien"]
                .HeaderText = "Người thực hiện";
            dgvPhuLuc.Columns["NgayApDung"]
  .HeaderText = "Ngày áp dụng";

            dgvPhuLuc.Columns["NgayApDung"]
                .DefaultCellStyle.Format =
                "dd/MM/yyyy";

            // =====================
            // FORMAT
            // =====================

            dgvPhuLuc.Columns["GiaThueMoi"]
                .DefaultCellStyle.Format = "N0";

            dgvPhuLuc.Columns["GiaDienMoi"]
                .DefaultCellStyle.Format = "N0";

            dgvPhuLuc.Columns["GiaNuocMoi"]
                .DefaultCellStyle.Format = "N0";

            dgvPhuLuc.Columns["GiaCocMoi"]
                .DefaultCellStyle.Format = "N0";

            dgvPhuLuc.Columns["NgayTao"]
                .DefaultCellStyle.Format =
                "dd/MM/yyyy HH:mm";

            dgvPhuLuc.Columns["ThoiGianMoi"]
                .DefaultCellStyle.Format =
                "dd/MM/yyyy";
        }
        void LoadTaiSan()
        {
            try
            {
                // =====================
                // CLEAR
                // =====================

                dgvTaiSan.Rows.Clear();

                // =====================
                // TẠO CỘT
                // =====================

                if (dgvTaiSan.Columns.Count == 0)
                {
                    dgvTaiSan.Columns.Add("MaTaiSan", "Mã TS");
                    dgvTaiSan.Columns.Add("TenTaiSan", "Tên tài sản");
                    dgvTaiSan.Columns.Add("SoLuong", "Số lượng");
                    dgvTaiSan.Columns.Add("TinhTrangBanDau", "Tình trạng");
                    dgvTaiSan.Columns.Add("GiaTri", "Giá trị");
                    dgvTaiSan.Columns["GiaTri"].DefaultCellStyle.Format = "N0";
                    dgvTaiSan.Columns.Add("NgayBanGiao", "Ngày bàn giao");
                    dgvTaiSan.Columns.Add("NgayThuHoi", "Ngày thu hồi");
                    dgvTaiSan.Columns.Add("TrangThai", "Trạng thái");
                }

                // =====================
                // FORMAT
                // =====================

                dgvTaiSan.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvTaiSan.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvTaiSan.ReadOnly = true;

                dgvTaiSan.AllowUserToAddRows = false;

                dgvTaiSan.MultiSelect = false;

                dgvTaiSan.RowHeadersVisible = false;

                // =====================
                // LOAD TÀI SẢN HỢP ĐỒNG
                // =====================

                var ds =
                    hopDong_TaiSan_BUS
                    .GetByHopDong(maHD);

                // =====================
                // NẾU CHƯA CÓ
                // -> LẤY TÀI SẢN PHÒNG
                // =====================

                if (ds == null || ds.Count == 0)
                {
                    var hd =
                        hopDong_BUS.GetById(maHD);

                    if (hd == null)
                        return;

                    var dsPhong =
                        taiSan_BUS.GetByPhong(hd.MaPhong);

                    if (dsPhong == null)
                        return;

                    foreach (var ts in dsPhong)
                    {
                        dgvTaiSan.Rows.Add(
                            ts.MaTaiSan,
                            ts.TenTaiSan,
                            1,
                            "Tốt",
                            ts.GiaTri,
                            DateTime.Now.ToString("dd/MM/yyyy"),
                            "",
                            ts.TrangThai
                        );
                    }

                    return;
                }

                // =====================
                // LOAD TÀI SẢN HỢP ĐỒNG
                // =====================

                foreach (var item in ds)
                {
                    dgvTaiSan.Rows.Add(
                        item.MaTaiSan,
                        item.TenTaiSan,
                        item.SoLuong,
                        item.TinhTrangBanDau,
                        item.GiaTri,
                        item.NgayBanGiao.HasValue
                            ? item.NgayBanGiao.Value
                                .ToString("dd/MM/yyyy")
                            : "",

                        item.NgayThuHoi.HasValue
                            ? item.NgayThuHoi.Value
                                .ToString("dd/MM/yyyy")
                            : "",

                        item.TrangThai
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi load tài sản: " + ex.Message);
            }
        }
        void LoadDichVu()
        {
            try
            {
                dgvDichVu.Rows.Clear();

                if (dgvDichVu.Columns.Count == 0)
                {
                    dgvDichVu.Columns.Add(
                        "MaDichVu",
                        "Mã DV");

                    dgvDichVu.Columns.Add(
                        "TenDichVu",
                        "Tên dịch vụ");

                    dgvDichVu.Columns.Add(
                        "DonGia",
                        "Đơn giá");
                    dgvDichVu.Columns["DonGia"].DefaultCellStyle.Format = "N0";

                    dgvDichVu.Columns.Add(
                        "HinhThucTinh",
                        "Hình thức tính");

                    dgvDichVu.Columns.Add(
                        "NgayBatDau",
                        "Ngày bắt đầu");

                    dgvDichVu.Columns.Add(
                        "NgayNgung",
                        "Ngày ngưng");

                    dgvDichVu.Columns.Add(
                        "TrangThai",
                        "Trạng thái");
                }

                var ds =
                    hopDong_DichVu_BUS
                    .GetByHopDong(maHD);

                foreach (var item in ds)
                {
                    dgvDichVu.Rows.Add(
                        item.MaDichVu,
                        item.TenDichVu,
                        item.DonGia,
                        item.HinhThucTinh,
                        item.NgayBatDau,
                        item.NgayNgung,
                        item.TrangThai
                    );
                }

                dgvDichVu.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvDichVu.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvDichVu.ReadOnly = true;

                dgvDichVu.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void LoadCuDan()
        {
            try
            {
                dgvCuDan.Rows.Clear();

                if (dgvCuDan.Columns.Count == 0)
                {
                    dgvCuDan.Columns.Add(
                        "MaCuDan",
                        "Mã cư dân");

                    dgvCuDan.Columns.Add(
                        "TenCuDan",
                        "Tên cư dân");

                    dgvCuDan.Columns.Add(
                        "VaiTro",
                        "Vai trò");

                    dgvCuDan.Columns.Add(
                        "NgayThamGia",
                        "Ngày tham gia");

                    dgvCuDan.Columns.Add(
                        "TrangThai",
                        "Trạng thái");
                }

                var ds =
                    hopDong_CuDan_BUS
                    .GetByHopDong(maHD);

                foreach (var item in ds)
                {
                    dgvCuDan.Rows.Add(
                        item.MaCuDan,
                        item.TenCuDan,
                        item.VaiTro,
                        item.NgayThamGia,
                        item.TrangThai
                    );
                }

                dgvCuDan.AutoSizeColumnsMode =
                    DataGridViewAutoSizeColumnsMode.Fill;

                dgvCuDan.SelectionMode =
                    DataGridViewSelectionMode.FullRowSelect;

                dgvCuDan.ReadOnly = true;

                dgvCuDan.AllowUserToAddRows = false;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void LoadTrangThai()
        {
            cbbTrangThai.Items.Clear();

            cbbTrangThai.Items.Add(
                "Đang hiệu lực");

            cbbTrangThai.Items.Add(
                "Đã kết thúc");

            cbbTrangThai.Items.Add(
                "Đã thanh lý");
        }
        void LoadPhong()
        {
            var dsPhong = phong_BUS.GetAll();

            if (dsPhong == null || dsPhong.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu phòng");
                return;
            }

            cbbPhong.DataSource = dsPhong;
            cbbPhong.DisplayMember = "TenPhong";
            cbbPhong.ValueMember = "MaPhong";
        }
        void KhoaChucNang()
        {
            // khóa thông tin hợp đồng
            cbbPhong.Enabled = false;
            dtpBatDau.Enabled = false;
            dtpKetThuc.Enabled = false;

            numGiaThue.Enabled = false;
            numTienCoc.Enabled = false;

            cbbTrangThai.Enabled = false;

            // khóa button chính
            btnluu.Enabled = false;

            btnThemCuDan.Enabled = false;
            btnXoaCuDan.Enabled = false;
            btnDoiVaiTro.Enabled = false;

            btnThemDichVu.Enabled = false;
            btnNgungDichVu.Enabled = false;
            btnDoiGia.Enabled = false;

            btnThemTaiSan.Enabled = false;
            btnThuHoi.Enabled = false;
            btnBaoHong.Enabled = false;

            btnThucHienPhuLuc.Enabled = false;

            // đã thanh lý thì không cho thanh lý nữa
            btnThanhLy.Enabled = false;

            // optional
            dgvCuDan.ReadOnly = true;
            dgvDichVu.ReadOnly = true;
            dgvTaiSan.ReadOnly = true;
            dgvPhuLuc.ReadOnly = true;
        }
        void LoadHopDong()
        {
            try
            {
                var hd = hopDong_BUS.GetById(maHD);

                if (hd == null)
                {
                    MessageBox.Show("Không tìm thấy hợp đồng");
                    return;
                }

                txtMaHopDong.Text = hd.MaHopDong;

                if (hd.MaPhong != null)
                    cbbPhong.SelectedValue = hd.MaPhong;

                dtpBatDau.Value =
                    hd.NgayBatDau ?? DateTime.Now;

                dtpKetThuc.Value =
                    hd.NgayKetThuc ?? DateTime.Now;

                numGiaThue.Value =
                    hd.GiaThue ?? 0;

                numTienCoc.Value =
                    hd.TienCoc ?? 0;

                cbbTrangThai.Text =
                    hd.TrangThai;
                // =====================
                // KHÓA NẾU ĐÃ THANH LÝ
                // =====================

                if (hd.TrangThai == "Đã thanh lý")
                {
                    KhoaChucNang();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        void StyleGrid(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.None;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            dgv.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(59, 130, 246);
            dgv.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.White;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(30, 64, 175);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font =
                new Font("Segoe UI", 9, FontStyle.Bold);

            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 28;
            dgv.GridColor = System.Drawing.Color.FromArgb(230, 230, 230);

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.RowHeadersVisible = false;
        }
        private void ChiTietHopDong_Load(object sender, EventArgs e)
        {
            LoadTrangThai();

            LoadPhong();

            LoadHopDong();
            LoadCuDan();
            LoadDichVu();
            LoadTaiSan();
            LoadPhuLuc();
            LoadLoaiPhuLuc();

            StyleGrid(dgvCuDan);
            StyleGrid(dgvDichVu);
            StyleGrid(dgvTaiSan);
            StyleGrid(dgvPhuLuc);
        }

        private void btnluu_Click(object sender, EventArgs e)
        {
            try
            {
                // validate

                if (dtpKetThuc.Value
                    <= dtpBatDau.Value)
                {
                    MessageBox.Show(
                        "Ngày kết thúc không hợp lệ");

                    return;
                }

                // dto

                HopDong_DTO hd =
                    new HopDong_DTO();

                hd.MaHopDong =
                    txtMaHopDong.Text;

                /*hd.MaPhong =
                    cbbPhong.SelectedValue
                    .ToString();*/
                if (cbbPhong.SelectedValue == null)
                {
                    MessageBox.Show("Chưa chọn phòng");
                    return;
                }

                hd.MaPhong =
                    cbbPhong.SelectedValue.ToString();

                hd.NgayBatDau =
                    dtpBatDau.Value;

                hd.NgayKetThuc =
                    dtpKetThuc.Value;

                hd.GiaThue =
                    numGiaThue.Value;

                hd.TienCoc =
                    numTienCoc.Value;

                hd.TrangThai =
                    cbbTrangThai.Text;

                // update

                string kq =
                    hopDong_BUS.Update(hd);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Cập nhật thành công");
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        LoaiPhong_BUS loaiPhong_BUS = new LoaiPhong_BUS();
        private void btnThemCuDan_Click(object sender, EventArgs e)
        {
            try
            {
                CuDan f = new CuDan();
                var phong =
    phong_BUS.GetById(
        cbbPhong.SelectedValue
        .ToString());

                var loaiPhong =
                    loaiPhong_BUS
                    .GetById(
                        phong.MaLoaiPhong);
                int soNguoiDangO =
                    dgvCuDan.Rows
                    .Cast<DataGridViewRow>()
                    .Count(r =>
                        r.Cells["TrangThai"].Value != null
                        &&
                        r.Cells["TrangThai"]
                        .Value.ToString() == "Đang ở");

                if (soNguoiDangO
                    >= loaiPhong.SoNguoiToiDa)
                {
                    MessageBox.Show(
                        "Phòng đã đầy");

                    return;
                }
                if (f.ShowDialog() == DialogResult.OK)
                {
                    var cd = f.CuDanDuocChon;

                    if (cd == null)
                        return;

                    // =====================
                    // CHECK TRÙNG
                    // =====================
                    foreach (DataGridViewRow row
                        in dgvCuDan.Rows)
                    {
                        if (row.Cells["MaCuDan"].Value != null)
                        {
                            string ma =
                                row.Cells["MaCuDan"]
                                .Value.ToString();

                            string trangThai =
                                row.Cells["TrangThai"]
                                .Value?.ToString();

                            // CHỈ CHECK NGƯỜI ĐANG Ở
                            if (ma == cd.MaCuDan
                                &&
                                trangThai == "Đang ở")
                            {
                                MessageBox.Show(
                                    "Cư dân đã tồn tại");

                                return;
                            }
                        }
                    }

                    // =====================
                    // DTO
                    // =====================

                    HopDong_CuDan_DTO dto =
                        new HopDong_CuDan_DTO();

                    dto.MaHopDong = maHD;

                    dto.MaCuDan = cd.MaCuDan;

                    dto.VaiTro = "Thành viên";

                    dto.NgayThamGia =
                        DateTime.Now;

                    dto.TrangThai =
                        "Đang ở";

                    // =====================
                    // SAVE
                    // =====================

                    string kq =
                        hopDong_CuDan_BUS
                        .Insert(dto);

                    if (kq == "success")
                    {
                        MessageBox.Show(
                            "Thêm cư dân thành công");

                        LoadCuDan();
                    }
                    else
                    {
                        MessageBox.Show(kq);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnXoaCuDan_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCuDan.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Chọn cư dân");

                    return;
                }

                string maCuDan =
                    dgvCuDan.CurrentRow
                    .Cells["MaCuDan"]
                    .Value.ToString();

                string kq =
                    hopDong_CuDan_BUS
                    .ChuyenDi(
                        maHD,
                        maCuDan);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Đã chuyển đi");

                    LoadCuDan();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDoiVaiTro_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvCuDan.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Chọn cư dân");

                    return;
                }

                string maCuDan =
                    dgvCuDan.CurrentRow
                    .Cells["MaCuDan"]
                    .Value.ToString();

                string kq =
                    hopDong_CuDan_BUS
                    .DoiDaiDien(
                        maHD,
                        maCuDan);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Đổi đại diện thành công");

                    LoadCuDan();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThemDichVu_Click(object sender, EventArgs e)
        {
            try
            {
                DanhMucDichVu f =
                    new DanhMucDichVu();

                if (f.ShowDialog()
                    == DialogResult.OK)
                {
                    var dv =
                        f.DichVuDuocChon;

                    if (dv == null)
                        return;

                    // =====================
                    // CHECK ĐÃ TỒN TẠI
                    // =====================

                    var ds =
                        hopDong_DichVu_BUS
                        .GetByHopDong(maHD);

                    var dvCu =
                        ds.FirstOrDefault(x =>
                            x.MaDichVu == dv.MaDichVu);

                    // =====================
                    // ĐANG DÙNG
                    // =====================

                    if (dvCu != null
                        &&
                        dvCu.TrangThai == "Đang dùng")
                    {
                        MessageBox.Show(
                            "Dịch vụ đã tồn tại");

                        return;
                    }

                    // =====================
                    // ĐÃ NGƯNG -> DÙNG LẠI
                    // =====================

                    if (dvCu != null
                        &&
                        dvCu.TrangThai == "Ngưng")
                    {
                        string kqSua =
                            hopDong_DichVu_BUS
                            .SuDungLai(
                                maHD,
                                dv.MaDichVu);

                        if (kqSua == "success")
                        {
                            MessageBox.Show(
                                "Đã sử dụng lại dịch vụ");

                            LoadDichVu();
                        }
                        else
                        {
                            MessageBox.Show(kqSua);
                        }

                        return;
                    }

                    // =====================
                    // THÊM MỚI
                    // =====================

                    HopDong_DichVu_DTO dto =
    new HopDong_DichVu_DTO();

                    dto.MaHopDong =
                        maHD;

                    dto.MaDichVu =
                        dv.MaDichVu;

                    dto.TenDichVu =
                        dv.TenDichVu;

                    dto.DonGia =
                        dv.DonGia;

                    dto.HinhThucTinh =
                        dv.HinhThucTinh;

                    dto.NgayBatDau =
                        DateTime.Now;

                    dto.TrangThai =
                        "Đang dùng";

                    string kq =
                        hopDong_DichVu_BUS
                        .Insert(dto);

                    if (kq == "success")
                    {
                        MessageBox.Show(
                            "Thêm dịch vụ thành công");

                        LoadDichVu();
                    }
                    else
                    {
                        MessageBox.Show(kq);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnNgungDichVu_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvDichVu.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Chọn dịch vụ");

                    return;
                }

                string maDV =
                    dgvDichVu.CurrentRow
                    .Cells["MaDichVu"]
                    .Value.ToString();

                string kq =
                    hopDong_DichVu_BUS
                    .NgungDichVu(
                        maHD,
                        maDV);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Ngưng dịch vụ thành công");

                    LoadDichVu();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDoiGia_Click(object sender, EventArgs e)
        {
            try
            {
                // =====================
                // CHECK CHỌN
                // =====================

                if (dgvDichVu.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Chọn dịch vụ");

                    return;
                }

                // =====================
                // LẤY MÃ DV
                // =====================

                string maDV =
                    dgvDichVu.CurrentRow
                    .Cells["MaDichVu"]
                    .Value.ToString();

                // =====================
                // INPUT GIÁ MỚI
                // =====================

                string input =
                    Interaction.InputBox(
                        "Nhập giá mới",
                        "Đổi giá dịch vụ",
                        "0");

                if (input == "")
                    return;

                decimal giaMoi;

                bool check =
                    decimal.TryParse(
                        input,
                        out giaMoi);

                if (!check)
                {
                    MessageBox.Show(
                        "Giá không hợp lệ");

                    return;
                }

                if (giaMoi <= 0)
                {
                    MessageBox.Show(
                        "Giá phải lớn hơn 0");

                    return;
                }

                // =====================
                // UPDATE
                // =====================

                string kq =
                    hopDong_DichVu_BUS
                    .DoiGia(
                        maHD,
                        maDV,
                        giaMoi);

                // =====================
                // RESULT
                // =====================

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Đổi giá thành công");

                    LoadDichVu();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThemTaiSan_Click(object sender, EventArgs e)
        {
            try
            {
                TaiSan f = new TaiSan();

                if (f.ShowDialog() != DialogResult.OK)
                    return;

                var ts = f.TaiSanDuocChon;

                if (ts == null)
                    return;

                var ds =
                    hopDong_TaiSan_BUS
                    .GetByHopDong(maHD);

                var tsCu =
                    ds.FirstOrDefault(x =>
                        x.MaTaiSan == ts.MaTaiSan);

                // =====================
                // ĐANG DÙNG
                // =====================

                if (tsCu != null &&
                    tsCu.TrangThai == "Đang dùng")
                {
                    MessageBox.Show(
                        "Tài sản đang được sử dụng");

                    return;
                }

                // =====================
                // ĐÃ HỎNG
                // =====================

                if (tsCu != null &&
                    tsCu.TrangThai == "Đã hỏng")
                {
                    MessageBox.Show(
                        "Tài sản đã hỏng, không thể bàn giao lại");

                    return;
                }

                // =====================
                // DÙNG LẠI
                // =====================

                if (tsCu != null &&
                    tsCu.TrangThai == "Đã thu hồi")
                {
                    DialogResult rs =
                        MessageBox.Show(
                            "Tài sản đã thu hồi trước đó. Bàn giao lại?",
                            "Xác nhận",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Question);

                    if (rs != DialogResult.Yes)
                        return;

                    string kqSua =
                        hopDong_TaiSan_BUS
                        .SuDungLai(maHD, ts.MaTaiSan);

                    if (kqSua == "success")
                    {
                        MessageBox.Show(
                            "Bàn giao lại thành công");

                        LoadTaiSan();
                    }
                    else
                    {
                        MessageBox.Show(kqSua);
                    }

                    return;
                }

                // =====================
                // THÊM MỚI
                // =====================

                HopDong_TaiSan_DTO dto =
                    new HopDong_TaiSan_DTO();

                dto.MaHopDong = maHD;

                dto.MaTaiSan = ts.MaTaiSan;
                dto.TenTaiSan = ts.TenTaiSan;

                dto.SoLuong = 1;

                dto.TinhTrangBanDau = "Tốt";

                dto.GiaTri = ts.GiaTri;
                

                dto.NgayBanGiao = DateTime.Now;

                dto.TrangThai = "Đang dùng";

                string kq =
                    hopDong_TaiSan_BUS
                    .Insert(dto);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Thêm tài sản thành công");

                    LoadTaiSan();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnThuHoi_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTaiSan.CurrentRow == null)
                {
                    MessageBox.Show("Chọn tài sản");
                    return;
                }

                string maTS =
                    dgvTaiSan.CurrentRow
                    .Cells["MaTaiSan"]
                    .Value.ToString();

                string trangThai =
                    dgvTaiSan.CurrentRow
                    .Cells["TrangThai"]
                    .Value.ToString();

                // =====================
                // CHECK
                // =====================

                if (trangThai == "Đã thu hồi")
                {
                    MessageBox.Show(
                        "Tài sản đã thu hồi");

                    return;
                }

                if (trangThai == "Đã hỏng")
                {
                    MessageBox.Show(
                        "Tài sản đã hỏng");

                    return;
                }

                DialogResult rs =
                    MessageBox.Show(
                        "Xác nhận thu hồi tài sản?",
                        "Thu hồi",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                if (rs != DialogResult.Yes)
                    return;

                string kq =
                    hopDong_TaiSan_BUS
                    .ThuHoi(maHD, maTS);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Thu hồi thành công");

                    LoadTaiSan();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnBaoHong_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvTaiSan.CurrentRow == null)
                {
                    MessageBox.Show(
                        "Chọn tài sản");

                    return;
                }

                string maTS =
                    dgvTaiSan.CurrentRow
                    .Cells["MaTaiSan"]
                    .Value.ToString();

                string trangThai =
                    dgvTaiSan.CurrentRow
                    .Cells["TrangThai"]
                    .Value.ToString();

                // =====================
                // CHECK
                // =====================

                if (trangThai == "Đã hỏng")
                {
                    MessageBox.Show(
                        "Tài sản đã báo hỏng");

                    return;
                }

                if (trangThai == "Đã thu hồi")
                {
                    MessageBox.Show(
                        "Tài sản đã thu hồi");

                    return;
                }

                DialogResult rs =
                    MessageBox.Show(
                        "Xác nhận báo hỏng?",
                        "Báo hỏng",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);

                if (rs != DialogResult.Yes)
                    return;

                string kq =
                    hopDong_TaiSan_BUS
                    .BaoHong(maHD, maTS);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Báo hỏng thành công");

                    LoadTaiSan();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void LoadLoaiPhuLuc()
        {
            cbbLoaiPhuLuc.Items.Clear();

            cbbLoaiPhuLuc.Items.Add(
                "Gia hạn");

            cbbLoaiPhuLuc.Items.Add(
                "Tăng giá thuê");

            cbbLoaiPhuLuc.Items.Add(
                "Đổi giá điện");

            cbbLoaiPhuLuc.Items.Add(
                "Đổi giá nước");

            cbbLoaiPhuLuc.Items.Add(
                "Đổi tiền cọc");
            cbbLoaiPhuLuc.Items.Add(
                "Ngày Áp Dụng Mới");

            cbbLoaiPhuLuc.SelectedIndex = 0;
        }

        private void cbbLoaiPhuLuc_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnThucHienPhuLuc_Click(object sender, EventArgs e)
        {

        }

        private void btnThanhLy_Click(object sender, EventArgs e)
        {

            try
            {
                // =====================
                // XÁC NHẬN
                // =====================

                DialogResult rs = MessageBox.Show(
                    "Bạn có chắc muốn thanh lý hợp đồng này không?",
                    "Xác nhận thanh lý",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (rs != DialogResult.Yes)
                    return;

                // =====================
                // GỌI BUS
                // =====================

                string kq = hopDong_BUS.ThanhLy(maHD);

                // =====================
                // KẾT QUẢ
                // =====================

                if (kq.StartsWith("success"))
                {
                    // Trường hợp thanh lý trước hạn
                    if (kq.Contains("|"))
                    {
                        string thongBao = kq.Split('|')[1];

                        MessageBox.Show(
                            "Thanh lý hợp đồng thành công.\n\n" +
                            thongBao,
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show(
                            "Thanh lý hợp đồng thành công.",
                            "Thông báo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show(
                        kq,
                        "Không thể thanh lý",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi: " + ex.Message,
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnXuatPDF_Click(object sender, EventArgs e)
        {
            try
            {
                // =========================
                // SAVE FILE
                // =========================

                SaveFileDialog sfd =
                    new SaveFileDialog();

                sfd.Filter =
                    "PDF files (*.pdf)|*.pdf";

                sfd.FileName =
                    "HopDong_" + maHD + ".pdf";

                if (sfd.ShowDialog()
                    != DialogResult.OK)
                {
                    return;
                }

                // =========================
                // LOAD DATA
                // =========================

                var hd =
                    hopDong_BUS.GetById(maHD);

                if (hd == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hợp đồng");

                    return;
                }

                var dsCuDan =
                    hopDong_CuDan_BUS
                    .GetByHopDong(maHD);

                var dsDV =
                    hopDong_DichVu_BUS
                    .GetByHopDong(maHD);

                var dsTS =
                    hopDong_TaiSan_BUS
                    .GetByHopDong(maHD);

                // =========================
                // PDF
                // =========================

                QuestPDF.Fluent.Document
                .Create(container =>
                {
                    container.Page(page =>
                    {
                        // =========================
                        // PAGE
                        // =========================

                        page.Size(PageSizes.A4);

                        page.Margin(30);

                        page.DefaultTextStyle(x =>
                            x.FontSize(11));

                        // =========================
                        // HEADER
                        // =========================

                        page.Header()
                            .AlignCenter()
                            .Text(
                                "HỢP ĐỒNG THUÊ PHÒNG")
                            .Bold()
                            .FontSize(22);

                        // =========================
                        // CONTENT
                        // =========================

                        page.Content()
                            .Column(col =>
                            {
                                // =========================
                                // THÔNG TIN HỢP ĐỒNG
                                // =========================

                                col.Item()
                                    .Text(
                                        $"Mã hợp đồng: {hd.MaHopDong}");

                                col.Item()
                                    .Text(
                                        $"Ngày bắt đầu: " +
                                        $"{Convert.ToDateTime(hd.NgayBatDau):dd/MM/yyyy}");

                                col.Item()
                                    .Text(
                                        $"Ngày kết thúc: " +
                                        $"{Convert.ToDateTime(hd.NgayKetThuc):dd/MM/yyyy}");

                                col.Item()
                                    .Text(
                                        $"Giá thuê: " +
                                        $"{Convert.ToDecimal(hd.GiaThue):N0} VNĐ");

                                col.Item()
                                    .Text(
                                        $"Tiền cọc: " +
                                        $"{Convert.ToDecimal(hd.TienCoc):N0} VNĐ");

                                col.Item()
                                    .Text(
                                        $"Giá điện: " +
                                        $"{Convert.ToDecimal(hd.GiaDienChot):N0} VNĐ");

                                col.Item()
                                    .Text(
                                        $"Giá nước: " +
                                        $"{Convert.ToDecimal(hd.GiaNuocChot):N0} VNĐ");

                                col.Item()
                                    .PaddingBottom(15);

                                // =========================
                                // CƯ DÂN
                                // =========================

                                col.Item()
                                    .Text(
                                        "DANH SÁCH CƯ DÂN")
                                    .Bold()
                                    .FontSize(14);

                                col.Item()
                                    .Table(table =>
                                    {
                                        // cột

                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        // header

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Họ tên")
                                                .Bold();

                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Vai trò")
                                                .Bold();

                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Trạng thái")
                                                .Bold();
                                        });

                                        // data

                                        foreach (var cd in dsCuDan)
                                        {
                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(cd.TenCuDan);

                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(cd.VaiTro);

                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(cd.TrangThai);
                                        }
                                    });

                                col.Item()
                                    .PaddingBottom(15);

                                // =========================
                                // DỊCH VỤ
                                // =========================

                                col.Item()
                                    .Text("DỊCH VỤ")
                                    .Bold()
                                    .FontSize(14);

                                col.Item()
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                            columns.RelativeColumn();
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Tên DV")
                                                .Bold();

                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Giá")
                                                .Bold();

                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Trạng thái")
                                                .Bold();
                                        });

                                        foreach (var dv in dsDV)
                                        {
                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(dv.TenDichVu);

                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(
                                                    Convert.ToDecimal(
                                                        dv.DonGia)
                                                    .ToString("N0"));

                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(dv.TrangThai);
                                        }
                                    });

                                col.Item()
                                    .PaddingBottom(15);

                                // =========================
                                // TÀI SẢN
                                // =========================

                                col.Item()
                                    .Text("TÀI SẢN")
                                    .Bold()
                                    .FontSize(14);

                                col.Item()
                                    .Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.RelativeColumn();
                                            columns.ConstantColumn(70);
                                            columns.RelativeColumn();
                                        });

                                        table.Header(header =>
                                        {
                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Tên TS")
                                                .Bold();

                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("SL")
                                                .Bold();

                                            header.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text("Tình trạng")
                                                .Bold();
                                        });

                                        foreach (var ts in dsTS)
                                        {
                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(ts.TenTaiSan);

                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(
                                                    ts.SoLuong
                                                    .ToString());

                                            table.Cell()
                                                .Border(1)
                                                .Padding(5)
                                                .Text(ts.TinhTrangBanDau);
                                        }
                                    });

                                col.Item()
                                    .PaddingBottom(20);

                                // =========================
                                // ĐIỀU KHOẢN
                                // =========================

                                col.Item()
                                    .Text("ĐIỀU KHOẢN")
                                    .Bold()
                                    .FontSize(14);

                                col.Item()
                                    .Text(
                                        "- Thanh toán tiền phòng đúng hạn.");

                                col.Item()
                                    .Text(
                                        "- Không gây mất trật tự.");

                                col.Item()
                                    .Text(
                                        "- Không tự ý sửa chữa tài sản.");

                                col.Item()
                                    .Text(
                                        "- Không cho thuê lại phòng.");

                                col.Item()
                                    .Text(
                                        "- Khi trả phòng phải bàn giao đầy đủ tài sản.");

                                col.Item()
                                    .Text(
                                        "- Nếu làm hỏng tài sản phải bồi thường.");

                                col.Item()
                                    .PaddingBottom(40);

                                // =========================
                                // CHỮ KÝ
                                // =========================

                                col.Item()
                                    .Row(row =>
                                    {
                                        row.RelativeItem()
                                            .AlignCenter()
                                            .Column(x =>
                                            {
                                                x.Item()
                                                    .Text("BÊN CHO THUÊ")
                                                    .Bold();

                                                x.Item()
                                                    .PaddingTop(60)
                                                    .Text("(Ký tên)");
                                            });

                                        row.RelativeItem()
                                            .AlignCenter()
                                            .Column(x =>
                                            {
                                                x.Item()
                                                    .Text("BÊN THUÊ")
                                                    .Bold();

                                                x.Item()
                                                    .PaddingTop(60)
                                                    .Text("(Ký tên)");
                                            });
                                    });
                            });

                        // =========================
                        // FOOTER
                        // =========================

                        page.Footer()
                            .AlignCenter()
                            .Text(
                                $"Ngày xuất: {DateTime.Now:dd/MM/yyyy}");
                    });
                })
                .GeneratePdf(sfd.FileName);

                // =========================
                // DONE
                // =========================

                MessageBox.Show(
                    "Xuất PDF thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btndong_Click(object sender, EventArgs e)
        {
            DanhSachHopDong ds = new DanhSachHopDong();
            ds.ShowDialog();

        }
        private DateTime? NhapNgayApDung()
        {
            string inputNgay =
                Interaction.InputBox(
                    "Nhập ngày áp dụng (dd/MM/yyyy)",
                    "Ngày áp dụng",
                    DateTime.Now
                        .AddMonths(1)
                        .ToString("dd/MM/yyyy"));

            if (inputNgay == "")
                return null;

            DateTime ngayAD;

            bool check =
                DateTime.TryParse(
                    inputNgay,
                    out ngayAD);

            if (!check)
            {
                MessageBox.Show(
                    "Ngày áp dụng không hợp lệ");

                return null;
            }

            if (ngayAD < DateTime.Now.Date)
            {
                MessageBox.Show(
                    "Ngày áp dụng phải >= hôm nay");

                return null;
            }

            return ngayAD;
        }
        private void btnThucHienPhuLuc_Click_1(object sender, EventArgs e)
        {
            try
            {
                var hd =
                    hopDong_BUS.GetById(maHD);

                if (hd == null)
                {
                    MessageBox.Show(
                        "Không tìm thấy hợp đồng");

                    return;
                }

                PhuLucHopDong_DTO pl =
                    new PhuLucHopDong_DTO();

                pl.MaHopDong = maHD;

                pl.LoaiPhuLuc =
                    cbbLoaiPhuLuc.Text;

                pl.NgayTao =
                    DateTime.Now;

                pl.NguoiThucHien =
                    "Admin";

                pl.TrangThai =
                    "Chờ áp dụng";

                // =========================
                // GIA HẠN
                // =========================

                if (cbbLoaiPhuLuc.Text
                    == "Gia hạn")
                {
                    string input =
                        Interaction.InputBox(
                            "Nhập số tháng gia hạn",
                            "Gia hạn",
                            "6");

                    if (input == "")
                        return;

                    int thangThem;

                    bool check =
                        int.TryParse(
                            input,
                            out thangThem);

                    if (!check || thangThem <= 0)
                    {
                        MessageBox.Show(
                            "Số tháng không hợp lệ");

                        return;
                    }

                    pl.ThoiGianMoi =
                        Convert.ToDateTime(
                            hd.NgayKetThuc)
                        .AddMonths(thangThem);

                    pl.NgayApDung =
                        DateTime.Now;
                }

                // =========================
                // TĂNG GIÁ THUÊ
                // =========================

                else if (cbbLoaiPhuLuc.Text
                    == "Tăng giá thuê")
                {
                    string input =
                        Interaction.InputBox(
                            "Nhập giá thuê mới",
                            "Đổi giá thuê",
                            hd.GiaThue.ToString());

                    if (input == "")
                        return;

                    decimal giaMoi;

                    bool check =
                        decimal.TryParse(
                            input,
                            out giaMoi);

                    if (!check || giaMoi <= 0)
                    {
                        MessageBox.Show(
                            "Giá thuê không hợp lệ");

                        return;
                    }

                    pl.GiaThueMoi =
                        giaMoi;

                    var ngayAD =
                        NhapNgayApDung();

                    if (ngayAD == null)
                        return;

                    pl.NgayApDung =
                        ngayAD;
                }

                // =========================
                // ĐỔI GIÁ ĐIỆN
                // =========================

                else if (cbbLoaiPhuLuc.Text
                    == "Đổi giá điện")
                {
                    string input =
                        Interaction.InputBox(
                            "Nhập giá điện mới",
                            "Đổi giá điện",
                            hd.GiaDienChot.ToString());

                    if (input == "")
                        return;

                    decimal giaMoi;

                    bool check =
                        decimal.TryParse(
                            input,
                            out giaMoi);

                    if (!check || giaMoi <= 0)
                    {
                        MessageBox.Show(
                            "Giá điện không hợp lệ");

                        return;
                    }

                    pl.GiaDienMoi =
                        giaMoi;

                    var ngayAD =
                        NhapNgayApDung();

                    if (ngayAD == null)
                        return;

                    pl.NgayApDung =
                        ngayAD;
                }

                // =========================
                // ĐỔI GIÁ NƯỚC
                // =========================

                else if (cbbLoaiPhuLuc.Text
                    == "Đổi giá nước")
                {
                    string input =
                        Interaction.InputBox(
                            "Nhập giá nước mới",
                            "Đổi giá nước",
                            hd.GiaNuocChot.ToString());

                    if (input == "")
                        return;

                    decimal giaMoi;

                    bool check =
                        decimal.TryParse(
                            input,
                            out giaMoi);

                    if (!check || giaMoi <= 0)
                    {
                        MessageBox.Show(
                            "Giá nước không hợp lệ");

                        return;
                    }

                    pl.GiaNuocMoi =
                        giaMoi;

                    var ngayAD =
                        NhapNgayApDung();

                    if (ngayAD == null)
                        return;

                    pl.NgayApDung =
                        ngayAD;
                }

                // =========================
                // ĐỔI TIỀN CỌC
                // =========================

                else if (cbbLoaiPhuLuc.Text
                    == "Đổi tiền cọc")
                {
                    string input =
                        Interaction.InputBox(
                            "Nhập tiền cọc mới",
                            "Đổi tiền cọc",
                            hd.TienCoc.ToString());

                    if (input == "")
                        return;

                    decimal giaMoi;

                    bool check =
                        decimal.TryParse(
                            input,
                            out giaMoi);

                    if (!check || giaMoi < 0)
                    {
                        MessageBox.Show(
                            "Tiền cọc không hợp lệ");

                        return;
                    }

                    pl.GiaCocMoi =
                        giaMoi;

                    var ngayAD =
                        NhapNgayApDung();

                    if (ngayAD == null)
                        return;

                    pl.NgayApDung =
                        ngayAD;
                }

                // =========================
                // LƯU PHỤ LỤC
                // =========================

                string kq =
                    phuLuc_BUS.Insert(pl);

                if (kq == "success")
                {
                    MessageBox.Show(
                        "Tạo phụ lục thành công");

                    LoadPhuLuc();

                    LoadHopDong();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
