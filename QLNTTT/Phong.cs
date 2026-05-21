using BUS;
using BUS;
using DTO;
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
using System.IO;
namespace QLNTTT
{
    public partial class Phong : Form
    {
        public Phong()
        {
            InitializeComponent();
        }
        Phong_BUS phong = new Phong_BUS();
        ToaNha_BUS toanha = new ToaNha_BUS();
        LoaiPhong_BUS loaiphong = new LoaiPhong_BUS();
        // Biến cờ để kiểm soát việc load dữ liệu ban đầu
        bool isFinishLoading = false;
        void loadtoanha()
        {
            try
            {
                var ds = toanha.GetAll();

                cbbToaNha.DataSource = ds;
                cbbToaNha.DisplayMember = "TenToaNha"; // hiển thị
                cbbToaNha.ValueMember = "MaToaNha";     // giá trị
                                                        
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load tòa nhà: " + ex.Message);
            }
        }
        
        void cleardata()
        {
            txtMaPhong.Clear();
            txtTenPhong.Clear();
            numDienTich.Value = 0;
            numericUpDown4.Value = 0;
            numericUpDown5.Value = 0;
            numTienCoc.Value = 0;
            numGiaThue.Value = 0;
            cbbToaNha.SelectedIndex = -1;
            cbbLoaiPhong.SelectedIndex = -1;
            cbbTrangThai.SelectedIndex = -1;
            // Xóa ảnh trong PictureBox
            picAnhDien.Image = null;
            picAnhNuoc.Image = null;
            // Reset đường dẫn ảnh tạm
            pathAnhDienTam = "";
            pathAnhNuocTam = "";
            txtMaPhong.Enabled = true; // Cho phép nhập mã mới
        }
        void loadloaiphong()
        {
            var ds = loaiphong.GetAll();

            cbbLoaiPhong.DataSource = ds;
            cbbLoaiPhong.DisplayMember = "TenLoaiPhong";
            cbbLoaiPhong.ValueMember = "MaLoaiPhong";

            isFinishLoading = true; // Đã load xong các ComboBox
        }
        void loaddata()
        {
            try
            {
                var ds = phong.GetAll();

                if (ds != null)
                {
                    dataGridViewPhong.DataSource = ds;

                    dataGridViewPhong.Columns["MaPhong"].HeaderText = "Mã phòng";
                    dataGridViewPhong.Columns["TenPhong"].HeaderText = "Tên phòng";
                    dataGridViewPhong.Columns["MaToaNha"].HeaderText = "Mã tòa";
                    dataGridViewPhong.Columns["MaLoaiPhong"].HeaderText = "Mã loại";
                    dataGridViewPhong.Columns["DienTich"].HeaderText = "Diện tích";
                    dataGridViewPhong.Columns["DienTich"].DefaultCellStyle.Format = "N1";
                    dataGridViewPhong.Columns["SoDienCu"].HeaderText = "Số điện";
                    dataGridViewPhong.Columns["SoNuocCu"].HeaderText = "Số nước";
                    dataGridViewPhong.Columns["TienCoc"].HeaderText = "Tiền cọc";
                    dataGridViewPhong.Columns["TienCoc"].DefaultCellStyle.Format = "N0";
                    dataGridViewPhong.Columns["GiaThue"].HeaderText = "Giá thuê";
                    dataGridViewPhong.Columns["GiaThue"].DefaultCellStyle.Format = "N0";
                    dataGridViewPhong.Columns["TrangThai"].HeaderText = "Trạng thái";

                    // format tiền
                    dataGridViewPhong.Columns["TienCoc"].DefaultCellStyle.Format = "N0";
                    dataGridViewPhong.Columns["GiaThue"].DefaultCellStyle.Format = "N0";

                    dataGridViewPhong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    dataGridViewPhong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                    dataGridViewPhong.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi load dữ liệu: " + ex.Message);
            }
        }
        void clearinput() 
        {

        }
        // load form
        private void Phong_Load(object sender, EventArgs e)
        {
            loadtoanha();
            loadloaiphong();
            loaddata();

            // trạng thái
            cbbTrangThai.Items.Clear();
            cbbTrangThai.Items.Add("Trống");
            cbbTrangThai.Items.Add("Đang thuê");
            cbbTrangThai.Items.Add("Bảo trì");
            cbbTrangThai.SelectedIndex = 0;
            cleardata();
            clearinput();
        }

        private void cbbLoaiPhong_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Chỉ chạy khi Form đã load xong và có giá trị được chọn
            if (!isFinishLoading || cbbLoaiPhong.SelectedValue == null) return;

            try
            {
                // Kiểm tra kỹ kiểu dữ liệu của SelectedValue
                string maLoai = cbbLoaiPhong.SelectedValue.ToString();

                // Tránh trường hợp SelectedValue trả về chính Object DTO thay vì String ID
                if (maLoai.Contains("DTO")) return;

                var lp = loaiphong.GetById(maLoai);
                if (lp != null)
                {
                    // Đổ giá thuê mặc định của loại phòng vào NumericUpDown
                    numGiaThue.Value = (decimal)lp.GiaThueMacDinh;
                }
            }
            catch { /* Tránh crash khi casting dữ liệu */ }
        }

        private void LoadImageSafe(PictureBox pic, string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                {
                    pic.Image = null;
                    return;
                }

                string path = Path.Combine(Application.StartupPath, "Images", fileName);

                if (File.Exists(path))
                {
                    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        pic.Image = Image.FromStream(fs);
                    }
                }
                else
                {
                    pic.Image = null; // hoặc ảnh mặc định
                }
            }
            catch
            {
                pic.Image = null;
            }
        }
        private void dataGridViewPhong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewPhong.Rows[e.RowIndex];

                txtMaPhong.Text = row.Cells["MaPhong"].Value.ToString();
                txtTenPhong.Text = row.Cells["TenPhong"].Value.ToString();
                cbbToaNha.SelectedValue = row.Cells["MaToaNha"].Value.ToString();
                cbbLoaiPhong.SelectedValue = row.Cells["MaLoaiPhong"].Value.ToString();
               numDienTich.Value = Convert.ToDecimal(row.Cells["DienTich"].Value);
                numTienCoc.Value = Convert.ToDecimal(row.Cells["TienCoc"].Value);
                numGiaThue.Value = Convert.ToDecimal(row.Cells["GiaThue"].Value);
                cbbTrangThai.Text = row.Cells["TrangThai"].Value.ToString();
                LoadImageSafe(picAnhDien, row.Cells["ACTD_Cu"].Value?.ToString());
                LoadImageSafe(picAnhNuoc, row.Cells["ACTN_Cu"].Value?.ToString());
                if (row.Cells["SoDienCu"].Value != null)
                    numericUpDown4.Value = Convert.ToDecimal(row.Cells["SoDienCu"].Value);

                if (row.Cells["SoNuocCu"].Value != null)
                    numericUpDown5.Value = Convert.ToDecimal(row.Cells["SoNuocCu"].Value);
                txtMaPhong.Enabled = false; // Không cho sửa mã khi đang chọn
            }
        }
        string pathAnhDienTam = "";
        string pathAnhNuocTam = "";
        private string LuuAnhVaoThuMuc(string pathGoc, string loaiAnh)
        {
            if (string.IsNullOrEmpty(pathGoc) || !File.Exists(pathGoc)) return "";
            try
            {
                string folderPath = Path.Combine(Application.StartupPath, "Images");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                // Tạo tên file: MaPhong_Loai_ThoiGian.jpg
                string tenFile = txtMaPhong.Text + "_" + loaiAnh + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + Path.GetExtension(pathGoc);
                string pathDich = Path.Combine(folderPath, tenFile);

                File.Copy(pathGoc, pathDich, true);
                return tenFile;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu file: " + ex.Message);
                return "";
            }
        }

        private void btndien_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            // Chỉ cho phép chọn các định dạng ảnh
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Gán đường dẫn file vào biến tạm để tí nữa nút "Thêm" sẽ dùng
                pathAnhDienTam = ofd.FileName;

                // Hiển thị ảnh lên PictureBox để Admin kiểm tra lại
                picAnhDien.Image = Image.FromFile(pathAnhDienTam);
            }
        }

        private void btnnuoc_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files(*.jpg; *.jpeg; *.png)|*.jpg; *.jpeg; *.png";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pathAnhNuocTam = ofd.FileName;
                picAnhNuoc.Image = Image.FromFile(pathAnhNuocTam);
            }
        }
        // Thêm
        private void button1_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(txtMaPhong.Text) ||
        string.IsNullOrWhiteSpace(txtTenPhong.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            try
            {
                // 🔥 Lưu ảnh trước
                string tenAnhDien = LuuAnhVaoThuMuc(pathAnhDienTam, "dien");
                string tenAnhNuoc = LuuAnhVaoThuMuc(pathAnhNuocTam, "nuoc");

                Phong_DTO p = new Phong_DTO()
                {
                    MaPhong = txtMaPhong.Text.Trim(),
                    TenPhong = txtTenPhong.Text.Trim(),
                    MaToaNha = cbbToaNha.SelectedValue.ToString(),
                    MaLoaiPhong = cbbLoaiPhong.SelectedValue.ToString(),
                    DienTich = (float)numDienTich.Value,
                    SoDienCu =(int)numericUpDown4.Value,
                    SoNuocCu=(int)numericUpDown5.Value,
                    TienCoc = (decimal)numTienCoc.Value,
                    GiaThue = (decimal)numGiaThue.Value,
                    TrangThai = cbbTrangThai.Text,

                    ACTD_Cu = tenAnhDien,
                    ACTN_Cu = tenAnhNuoc
                };

                string kq = phong.Insert_Phong(p);

                if (kq == "success")
                {
                    MessageBox.Show("Thêm thành công!");
                    loaddata();
                    clearinput();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            cleardata();
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text))
            {
                MessageBox.Show("Vui lòng chọn phòng cần sửa!");
                return;
            }

            try
            {
                string tenAnhDien = pathAnhDienTam != "" ? LuuAnhVaoThuMuc(pathAnhDienTam, "dien") : "";
                string tenAnhNuoc = pathAnhNuocTam != "" ? LuuAnhVaoThuMuc(pathAnhNuocTam, "nuoc") : "";

                Phong_DTO p = new Phong_DTO()
                {
                    MaPhong = txtMaPhong.Text,
                    TenPhong = txtTenPhong.Text,
                    MaToaNha = cbbToaNha.SelectedValue.ToString(),
                    MaLoaiPhong = cbbLoaiPhong.SelectedValue.ToString(),
                    DienTich = (float)numDienTich.Value,
                    SoDienCu = (int)numericUpDown4.Value,
                    SoNuocCu = (int)numericUpDown5.Value,
                    TienCoc = (decimal)numTienCoc.Value,
                    GiaThue = (decimal)numGiaThue.Value,
                    TrangThai = cbbTrangThai.Text,

                    ACTD_Cu = tenAnhDien,
                    ACTN_Cu = tenAnhNuoc
                };

                string kq = phong.Update_Phong(p);

                if (kq == "success")
                {
                    MessageBox.Show("Cập nhật thành công!");
                    loaddata();
                    clearinput();
                }
                else
                {
                    MessageBox.Show(kq);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            cleardata();
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMaPhong.Text))
            {
                MessageBox.Show("Chọn phòng cần xóa!");
                return;
            }

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa?",
                "Xác nhận",
                MessageBoxButtons.YesNo
            );

            if (rs == DialogResult.No) return;

            try
            {
                string kq = phong.Delete_Phong(txtMaPhong.Text);

                if (kq == "success")
                {
                    MessageBox.Show("Xóa thành công!");
                    loaddata();
                    clearinput();
                }
                else
                {
                    MessageBox.Show(kq);
                }
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

        private void dataGridViewPhong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridViewPhong.Columns[e.ColumnIndex].Name == "DienTich"
       && e.Value != null)
            {
                e.Value = e.Value + " m²";
                e.FormattingApplied = true;
            }
        }
    }
}
