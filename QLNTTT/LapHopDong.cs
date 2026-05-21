using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;
using BUS;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.IO;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
namespace QLNTTT
{
    public partial class LapHopDong : Form
    {
        public LapHopDong()
        {
            InitializeComponent();
        }
        string folderPDF = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "QLNTTT_PDF");
        Phong_BUS phong_BUS = new Phong_BUS();
        LoaiPhong_BUS loaiPhong_BUS = new LoaiPhong_BUS();
        List<CuDan_DTO> dsCuDan = new List<CuDan_DTO>();
        List<DanhMucDichVu_DTO> dsDichVu = new List<DanhMucDichVu_DTO>();
        TaiSan_BUS taiSan_BUS = new TaiSan_BUS();
        HopDong_DTO hd = new HopDong_DTO();
        HopDong_BUS hopDong_BUS = new HopDong_BUS();
        HopDong_CuDan_BUS hopDongCuDan_BUS = new HopDong_CuDan_BUS();

        HopDong_DichVu_BUS hopDongDichVu_BUS = new HopDong_DichVu_BUS();

        HopDong_TaiSan_BUS hopDongTaiSan_BUS = new HopDong_TaiSan_BUS();
        CuDan_BUS cuDan_BUS = new CuDan_BUS();
       
        void loadphong()
        {
            cbbPhong.DataSource = phong_BUS.GetAll();

            cbbPhong.DisplayMember = "TenPhong";
            cbbPhong.ValueMember = "MaPhong";
        }
        void loadtrangthai()
        {
            cbbTrangThai.Items.Clear();

            cbbTrangThai.Items.Add("Đang hiệu lực");
            cbbTrangThai.Items.Add("Đã kết thúc");

            cbbTrangThai.SelectedIndex = 0;
        }
        void LoadGridCuDan()
        {
            try
            {
                // xóa dữ liệu cũ
                dgvCuDan.Rows.Clear();

                // tạo cột nếu chưa có
                if (dgvCuDan.Columns.Count == 0)
                {
                    dgvCuDan.Columns.Add("MaCuDan", "Mã cư dân");
                    dgvCuDan.Columns.Add("TenCuDan", "Tên cư dân");
                    dgvCuDan.Columns.Add("VaiTro", "Vai trò");
                }

                // giao diện
                dgvCuDan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                dgvCuDan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvCuDan.ReadOnly = true;

                // tránh dòng trắng cuối grid
                dgvCuDan.AllowUserToAddRows = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load grid cư dân: " + ex.Message);
            }
        }
        void LoadGridDichVu()
        {
            dgvDichVu.Rows.Clear();

            if (dgvDichVu.Columns.Count == 0)
            {
                dgvDichVu.Columns.Add("MaDichVu", "Mã DV");
                dgvDichVu.Columns.Add("TenDichVu", "Tên dịch vụ");
                dgvDichVu.Columns.Add("DonGia", "Đơn giá");
                dgvDichVu.Columns.Add("HinhThucTinh", "Hình thức tính");
            }

            dgvDichVu.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;

            dgvDichVu.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgvDichVu.ReadOnly = true;

            dgvDichVu.AllowUserToAddRows = false;

            // format tiền
            dgvDichVu.Columns["DonGia"].DefaultCellStyle.Format = "N0";
        }
        void LoadGridTaiSan()
        {
            dgvTaiSan.Rows.Clear();

            if (dgvTaiSan.Columns.Count == 0)
            {
                dgvTaiSan.Columns.Add("MaTaiSan", "Mã TS");
                dgvTaiSan.Columns.Add("TenTaiSan", "Tên tài sản");
                dgvTaiSan.Columns.Add("SoLuong", "Số lượng");
                dgvTaiSan.Columns.Add("TinhTrangBanDau", "Tình Trạng Ban Đầu");
                dgvTaiSan.Columns.Add("TienDenBu", "Tiền Đền Bù");
                dgvTaiSan.Columns.Add("TinhTrangKhiTra", "Tình Trạng Khi Tra");
                dgvTaiSan.Columns.Add("TinhTrang", "Tình trạng");
                dgvTaiSan.Columns.Add("GiaTri", "Giá trị");
            }

            dgvTaiSan.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTaiSan.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTaiSan.ReadOnly = true;
            dgvTaiSan.AllowUserToAddRows = false;

            // format tiền
            dgvTaiSan.Columns["GiaTri"].DefaultCellStyle.Format = "N0";
        }
        void LoadTaiSanTheoPhong(string maPhong)
        {
            dgvTaiSan.Rows.Clear();

            if (string.IsNullOrEmpty(maPhong)) return;

            var ds = taiSan_BUS.GetByPhong(maPhong);

            foreach (var ts in ds)
            {
                dgvTaiSan.Rows.Add(
                    ts.MaTaiSan,
                    ts.TenTaiSan,
                    1,
                    "Tốt",
                    ts.GiaTri
                );
            }
        }
        void loadclear()
        {
            txtMaHopDong.Clear();

            dgvCuDan.Rows.Clear();

            dgvDichVu.Rows.Clear();

            LoadTaiSanTheoPhong(
                cbbPhong.SelectedValue.ToString());
        }
        void loaddata()
        {
            try
            {
                var ds = hopDong_BUS.GetAll();

                if (ds != null)
                {
                    dgvHopDong.DataSource = null;
                    dgvHopDong.DataSource = ds;

                    // =========================
                    // HEADER
                    // =========================

                    dgvHopDong.Columns["MaHopDong"].HeaderText = "Mã HĐ";

                    dgvHopDong.Columns["MaPhong"].HeaderText = "Phòng";

                    dgvHopDong.Columns["MaNguoiDaiDien"].HeaderText =
                        "Người đại diện";

                    dgvHopDong.Columns["NgayBatDau"].HeaderText =
                        "Ngày bắt đầu";

                    dgvHopDong.Columns["NgayKetThuc"].HeaderText =
                        "Ngày kết thúc";

                    dgvHopDong.Columns["TienCoc"].HeaderText =
                        "Tiền cọc";

                    dgvHopDong.Columns["GiaThue"].HeaderText =
                        "Giá thuê";

                    dgvHopDong.Columns["GiaDienChot"].HeaderText =
                        "Giá điện";

                    dgvHopDong.Columns["GiaNuocChot"].HeaderText =
                        "Giá nước";

                    dgvHopDong.Columns["NgayTao"].HeaderText =
                        "Ngày tạo";

                    dgvHopDong.Columns["GhiChu"].HeaderText =
                        "Ghi chú";

                    dgvHopDong.Columns["TrangThai"].HeaderText =
                        "Trạng thái";

                    // =========================
                    // FORMAT
                    // =========================

                    dgvHopDong.Columns["TienCoc"]
                        .DefaultCellStyle.Format = "N0";

                    dgvHopDong.Columns["GiaThue"]
                        .DefaultCellStyle.Format = "N0";

                    dgvHopDong.Columns["GiaDienChot"]
                        .DefaultCellStyle.Format = "N0";

                    dgvHopDong.Columns["GiaNuocChot"]
                        .DefaultCellStyle.Format = "N0";

                    dgvHopDong.Columns["NgayBatDau"]
                        .DefaultCellStyle.Format = "dd/MM/yyyy";

                    dgvHopDong.Columns["NgayKetThuc"]
                        .DefaultCellStyle.Format = "dd/MM/yyyy";

                    dgvHopDong.Columns["NgayTao"]
                        .DefaultCellStyle.Format = "dd/MM/yyyy";

                    // =========================
                    // GRID STYLE
                    // =========================

                    dgvHopDong.AutoSizeColumnsMode =
                        DataGridViewAutoSizeColumnsMode.Fill;

                    dgvHopDong.SelectionMode =
                        DataGridViewSelectionMode.FullRowSelect;

                    dgvHopDong.ReadOnly = true;

                    dgvHopDong.AllowUserToAddRows = false;

                    // =========================
                    // ẨN NAVIGATION PROPERTY
                    // =========================

                    foreach (DataGridViewColumn col in dgvHopDong.Columns)
                    {
                        if (col.ValueType != null)
                        {
                            if (col.ValueType.Name.Contains("Entity") ||
                                col.ValueType.Name.Contains("ICollection"))
                            {
                                col.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi load hợp đồng: " + ex.Message);
            }
        }
        private void LapHopDong_Load(object sender, EventArgs e)
        {
            loadphong();
            loadtrangthai();
            LoadGridCuDan();
            LoadGridDichVu();
            LoadGridTaiSan();
            loadclear();
            loaddata();
            Directory.CreateDirectory(folderPDF);
        }

        private void cbbPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbbPhong.SelectedValue == null)
                    return;

                string maPhong = cbbPhong.SelectedValue.ToString();

                // tránh lỗi lúc load combobox
                if (maPhong.Contains("System.Data"))
                    return;
                LoadTaiSanTheoPhong(maPhong);
                // lấy phòng
                var p = phong_BUS.GetById(maPhong);

                if (p == null)
                    return;

                // lấy loại phòng
                var lp = loaiPhong_BUS.GetById(p.MaLoaiPhong);

                if (lp == null)
                    return;

                // =========================
                // AUTO FILL
                // =========================
                numGiaThue.Value = (decimal)lp.GiaThueMacDinh;

                numTienCoc.Value = (decimal)p.TienCoc;

                numGiaDien.Value = (decimal)lp.DonGiaDien;

                numGiaNuoc.Value = (decimal)lp.DonGiaNuoc;

                // =========================
                // AUTO LOAD TÀI SẢN
                // =========================
                LoadTaiSanTheoPhong(maPhong);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // mở form cư dân
            CuDan f = new CuDan();

            if (f.ShowDialog() == DialogResult.OK)
            {
                CuDan_DTO cd = f.CuDanDuocChon;

                if (cd == null)
                    return;

                // =========================
                // CHECK TRÙNG
                // =========================
                foreach (DataGridViewRow row in dgvCuDan.Rows)
                {
                    if (row.Cells["MaCuDan"].Value != null)
                    {
                        if (row.Cells["MaCuDan"].Value.ToString() == cd.MaCuDan)
                        {
                            MessageBox.Show("Cư dân đã tồn tại!");
                            return;
                        }
                    }
                }

                // =========================
                // CHECK SỐ NGƯỜI TỐI ĐA
                // =========================
                string maPhong = cbbPhong.SelectedValue.ToString();

                var p = phong_BUS.GetById(maPhong);

                var lp = loaiPhong_BUS.GetById(p.MaLoaiPhong);

                if (lp == null)
                {
                    MessageBox.Show("Không tìm thấy loại phòng!");
                    return;
                }

                // DEBUG thử xem DB đang lưu bao nhiêu người
                MessageBox.Show(
                    "Số người tối đa: " + lp.SoNguoiToiDa);

                int soNguoiHienTai =
                    dgvCuDan.Rows
                    .Cast<DataGridViewRow>()
                    .Count(r => !r.IsNewRow);

                if (soNguoiHienTai >= lp.SoNguoiToiDa)
                {
                    MessageBox.Show(
                        $"Phòng tối đa {lp.SoNguoiToiDa} người!");
                    return;
                }

                // =========================
                // XÁC ĐỊNH VAI TRÒ
                // =========================
                string vaiTro = "Thành viên";

                // người đầu tiên = đại diện
                if (dgvCuDan.Rows.Count == 0)
                {
                    vaiTro = "Đại diện";

                    cbbNguoiDaiDien.Text = cd.TenCuDan;

                    // QUAN TRỌNG
                    cbbNguoiDaiDien.Tag = cd.MaCuDan;
                }

                // =========================
                // ADD GRID
                // =========================
                dgvCuDan.Rows.Add(
                    cd.MaCuDan,
                    cd.TenCuDan,
                    vaiTro
                );
            }
        }

        private void btnThemDichVu_Click(object sender, EventArgs e)
        {
            DanhMucDichVu f = new DanhMucDichVu();

            if (f.ShowDialog() == DialogResult.OK)
            {
                var dv = f.DichVuDuocChon;

                if (dv == null)
                    return;

                // =========================
                // CHECK TRÙNG
                // =========================
                foreach (DataGridViewRow row in dgvDichVu.Rows)
                {
                    if (row.Cells["MaDichVu"].Value != null)
                    {
                        if (row.Cells["MaDichVu"].Value.ToString() == dv.MaDichVu)
                        {
                            MessageBox.Show("Dịch vụ đã tồn tại!");
                            return;
                        }
                    }
                }

                // add list
                dsDichVu.Add(dv);

                // add grid
                dgvDichVu.Rows.Add(
                    dv.MaDichVu,
                    dv.TenDichVu,
                    dv.DonGia,
                    dv.HinhThucTinh
                );
            }
        }

        private void btnlaphoadon_Click(object sender, EventArgs e)
        {
            try
            {
                // =========================
                // VALIDATE
                // =========================

                if (txtMaHopDong.Text.Trim() == "")
                {
                    MessageBox.Show("Nhập mã hợp đồng");
                    return;
                }

                if (dgvCuDan.Rows.Count == 0)
                {
                    MessageBox.Show("Chưa có cư dân");
                    return;
                }
                
                if (cbbPhong.SelectedValue == null)
                {
                    MessageBox.Show("Chọn phòng");
                    return;
                }
                // =========================
                // CHECK NGÀY
                // =========================

                /*if (dtpNgayKetThuc.Value <= dtpNgayBatDau.Value)
                {
                    MessageBox.Show("Ngày kết thúc phải lớn hơn ngày bắt đầu");
                    return;
                }*/
                if (dtpNgayKetThuc.Value < dtpNgayBatDau.Value.AddMonths(3))
                {
                    MessageBox.Show("Thời hạn hợp đồng phải từ 3 tháng trở lên");
                    return;
                }

                // =========================
                // CHECK PHÒNG
                // =========================

                var p = phong_BUS.GetById(
                    cbbPhong.SelectedValue.ToString());

                if (p.TrangThai == "Đang thuê")
                {
                    MessageBox.Show("Phòng đang có hợp đồng!");
                    return;
                }

                // =========================
                // DTO HỢP ĐỒNG
                // =========================

                HopDong_DTO hd = new HopDong_DTO();

                hd.MaHopDong = txtMaHopDong.Text.Trim();

                hd.MaPhong = cbbPhong.SelectedValue.ToString();
                hd.MaNguoiDaiDien = cbbNguoiDaiDien.Tag.ToString();

                hd.NgayBatDau = dtpNgayBatDau.Value;

                hd.NgayKetThuc = dtpNgayKetThuc.Value;

                hd.TienCoc = numTienCoc.Value;

                hd.GiaThue = numGiaThue.Value;

                hd.GiaDienChot = numGiaDien.Value;

                hd.GiaNuocChot = numGiaNuoc.Value;
                hd.NgayTao = DateTime.Now;
                hd.GhiChu = txtGhiChu.Text;

                hd.TrangThai = "Đang hiệu lực";

                // =========================
                // GỌI BUS
                // =========================

                string kq = hopDong_BUS.LapHopDong(
                    hd,
                    dgvCuDan,
                    dgvDichVu,
                    dgvTaiSan
                );

                if (kq == "success")
                {
                    MessageBox.Show("Lập hợp đồng thành công!");

                    loaddata();
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

        private void btnpdf_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHopDong.CurrentRow == null)
                {
                    MessageBox.Show("Chọn hợp đồng");
                    return;
                }

                string maHD =
                    dgvHopDong.CurrentRow.Cells["MaHopDong"].Value.ToString();

                var hd = hopDong_BUS.GetById(maHD);
                var dsCuDan = hopDongCuDan_BUS.GetCuDan(maHD);
                var dsDV = hopDongDichVu_BUS.GetDV(maHD);
                var dsTS = hopDongTaiSan_BUS.GetTS(maHD);

                // ✔ FILE PDF CHUẨN
                string filePDF = Path.Combine(folderPDF, $"HD_{maHD}.pdf");

                // ✔ GỌI PDF CÓ PATH
                HopDongPDF.XuatPDF(hd, dsCuDan, dsDV, dsTS, filePDF);

                MessageBox.Show("Xuất PDF thành công");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGmail_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvHopDong.CurrentRow == null)
                {
                    MessageBox.Show("Vui lòng chọn hợp đồng!");
                    return;
                }

                string maHD =
                    dgvHopDong.CurrentRow.Cells["MaHopDong"].Value.ToString();

                var hd = hopDong_BUS.GetById(maHD);
                var cuDan = cuDan_BUS.GetById(hd.MaNguoiDaiDien);

                if (cuDan == null)
                {
                    MessageBox.Show("Không tìm thấy cư dân!");
                    return;
                }

                // ✔ FILE PDF ĐỒNG BỘ 100%
                string filePDF = Path.Combine(folderPDF, $"HD_{maHD}.pdf");

                if (!File.Exists(filePDF))
                {
                    MessageBox.Show("Chưa có file PDF! Hãy bấm Xuất PDF trước.");
                    return;
                }

                string kq = GuiMail.SendMail(
                    cuDan.Email,
                    cuDan.TenCuDan,
                    maHD,
                    filePDF
                );

                if (kq == "success")
                    MessageBox.Show("Gửi mail thành công!");
                else
                    MessageBox.Show("Gửi mail thất bại: " + kq);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
        }

        private void btnthoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btndahopdong_Click(object sender, EventArgs e)
        {
            DanhSachHopDong f = new DanhSachHopDong();
            f.Show();
        }
        void loadclearhopdong()
        {
            txtMaHopDong.Clear();
            cbbPhong.SelectedIndex = 0;
            cbbNguoiDaiDien.SelectedIndex = -1;
            dtpNgayBatDau.Value = DateTime.Now;
            dtpNgayKetThuc.Value = DateTime.Now.AddMonths(3);
            numTienCoc.Value = 0;
            numGiaThue.Value = 0;
            numGiaDien.Value = 0;
            numGiaNuoc.Value = 0;
            txtGhiChu.Clear();
            dgvCuDan.Rows.Clear();
            dgvDichVu.Rows.Clear();
            dgvTaiSan.Rows.Clear();
        }
        private void button4_Click(object sender, EventArgs e)
        {
           loadclearhopdong();
        }
    }
}
