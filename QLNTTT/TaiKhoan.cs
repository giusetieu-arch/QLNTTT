using BUS;
using DocumentFormat.OpenXml.Drawing.Charts;
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

namespace QLNTTT
{
    public partial class TaiKhoan : Form
    {
        public TaiKhoan()
        {
            InitializeComponent();
        }
        TaiKhoan_BUS tkBUS = new TaiKhoan_BUS();
        CuDan_BUS cdBUS = new CuDan_BUS();
        PhanQuyen_BUS pqBUS = new PhanQuyen_BUS();

        // Load cư dân
        void LoadCuDan()
        {
            cbbCuDan.DataSource = cdBUS.GetAll();

            cbbCuDan.DisplayMember = "TenCuDan";

            cbbCuDan.ValueMember = "MaCuDan";
        }

        // Load quyền
        void LoadQuyen()
        {
            cbbQuyen.DataSource = pqBUS.GetAll();

            cbbQuyen.DisplayMember = "TenQuyen";

            cbbQuyen.ValueMember = "MaQuyen";
        }

        // Tạo mã tài khoản
        string TaoMaTK()
        {
            Random rd = new Random();

            return "TK" + rd.Next(100, 999);
        }
        void loaddata()
        {
            dataGridViewtaikhoan.DataSource = tkBUS.GetAll();

            dataGridViewtaikhoan.Columns[0].HeaderText = "Mã tài khoản";
            dataGridViewtaikhoan.Columns[1].HeaderText = "Tên đăng nhập";
            dataGridViewtaikhoan.Columns[2].HeaderText = "Mật khẩu";
            dataGridViewtaikhoan.Columns[3].HeaderText = "Mã quyền";
            dataGridViewtaikhoan.Columns[4].HeaderText = "Mã cư dân";
            dataGridViewtaikhoan.Columns[5].HeaderText = "Ngày tạo";
            dataGridViewtaikhoan.Columns[6].HeaderText = "Trạng thái";

            dataGridViewtaikhoan.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.Fill;
        }
        
         void LoadTrangThai()
        {
            cbtrangthai.Items.Clear();

            cbtrangthai.Items.Add("Hoạt động");

            cbtrangthai.Items.Add("Khóa");

            cbtrangthai.SelectedIndex = 0;
        }


        private void TaiKhoan_Load(object sender, EventArgs e)
        {
            LoadCuDan();
            LoadQuyen();
            loaddata();
                LoadTrangThai();
        }

        private void btnthem_Click(object sender, EventArgs e)
        {
          if(string.IsNullOrEmpty(txtmatkhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            TaiKhoan_DTO tk = new TaiKhoan_DTO
            {
                MaTaiKhoan = TaoMaTK(),
                MaCuDan = cbbCuDan.SelectedValue.ToString(),
                MaQuyen = cbbQuyen.SelectedValue.ToString(),
                Username=txttaikhoan.Text,
                Password = txtmatkhau.Text,
                TrangThai = cbtrangthai.SelectedItem.ToString(),
                NgayTao = DateTime.Now,
                
            };
            bool success = tkBUS.Insert(tk);
            if (success)
            {
                MessageBox.Show("Thêm tài khoản thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loaddata();
            }
            else
            {
                MessageBox.Show("Thêm tài khoản thất bại!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnsua_Click(object sender, EventArgs e)
        {
            if (dataGridViewtaikhoan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần sửa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtmatkhau.Text))
            {
                MessageBox.Show("Vui lòng nhập mật khẩu!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            TaiKhoan_DTO tk = new TaiKhoan_DTO
            {
                MaTaiKhoan = dataGridViewtaikhoan.CurrentRow.Cells[0].Value.ToString(),
                MaCuDan = cbbCuDan.SelectedValue.ToString(),
                MaQuyen = cbbQuyen.SelectedValue.ToString(),
                Username = txttaikhoan.Text,
                Password = txtmatkhau.Text,
                TrangThai = cbtrangthai.SelectedItem.ToString(),
                NgayTao = DateTime.Now
            };

            bool success = tkBUS.Update(tk);

            if (success)
            {
                MessageBox.Show("Sửa tài khoản thành công!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                loaddata();
            }
            else
            {
                MessageBox.Show("Sửa tài khoản thất bại!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void btnxoa_Click(object sender, EventArgs e)
        {
            if (dataGridViewtaikhoan.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn tài khoản cần xóa!",
                    "Thông báo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            string maTK = dataGridViewtaikhoan.CurrentRow.Cells[0].Value.ToString();

            DialogResult rs = MessageBox.Show(
                "Bạn có chắc muốn xóa tài khoản này?",
                "Xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (rs == DialogResult.Yes)
            {
                bool success = tkBUS.Delete(maTK);

                if (success)
                {
                    MessageBox.Show("Xóa tài khoản thành công!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    loaddata();
                }
                else
                {
                    MessageBox.Show("Xóa tài khoản thất bại!",
                        "Thông báo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }

        private void dataGridViewtaikhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row =
                dataGridViewtaikhoan.Rows[e.RowIndex];

            txttaikhoan.Text = row.Cells[1].Value?.ToString();
            txtmatkhau.Text = row.Cells[2].Value?.ToString();

            cbbQuyen.SelectedValue =
                row.Cells[3].Value?.ToString();

            cbbCuDan.SelectedValue =
                row.Cells[4].Value?.ToString();

            cbtrangthai.SelectedItem =
                row.Cells[6].Value?.ToString();
        }
    }
}
