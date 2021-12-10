using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace QLVT_Nhom38.SimpleForm
{
    public partial class FormTaoTaiKhoan : Form
    {
        public static String nhanVien = "";
        String role = "";
        public FormTaoTaiKhoan()
        {
            InitializeComponent();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void FormTaoTaiKhoan_Load(object sender, EventArgs e)
        {
            txtMatKhau.UseSystemPasswordChar = true;
            txtMatKhauXacNhan.UseSystemPasswordChar = true;

            if (Program.mGroup == "CONGTY")
            {
                role = "CONGTY";
                radioButton_ChiNhanh.Visible = false;
                radioButton_User.Visible = false;
            }    
        }

        private void btnTao_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNhanVien.Text))
            {
                XtraMessageBox.Show("Vui lòng chọn nhân viên!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                if (Program.mGroup != "CONGTY")
                    role = (radioButton_ChiNhanh.Checked == true) ? "CHINHANH" : "USER";

                String strLenh = string.Format("exec sp_TaoLogin '{0}', '{1}', '{2}', '{3}'",
                    txtLoginName.Text, txtMatKhau.Text, txtNhanVien.Text, role);

                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return;

                Program.myReader.Close();
                XtraMessageBox.Show("Tạo tài khoản cho nhân viên thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtLoginName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNhanVien.Text))
                return;
            if (string.IsNullOrWhiteSpace(txtLoginName.Text))
            {
                e.Cancel = true;
                txtLoginName.Focus();
                errorProvider1.SetError(txtLoginName, "Vui lòng điền Login name!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLoginName, "");
            }
        }

        private void txtMatKhau_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoginName.Text))
                return;

            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
            {
                e.Cancel = true;
                txtMatKhau.Focus();
                errorProvider1.SetError(txtMatKhau, "Vui lòng điền mật khẩu!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtMatKhau, "");
            }
        }

        private void txtMatKhauXacNhan_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMatKhau.Text))
                return;  
            if (string.IsNullOrWhiteSpace(txtMatKhauXacNhan.Text))
            {
                e.Cancel = true;
                txtMatKhauXacNhan.Focus();
                errorProvider1.SetError(txtMatKhauXacNhan, "Vui lòng điền mật khẩu xác nhận!");
            }
            else if (!txtMatKhauXacNhan.Text.Equals(txtMatKhau.Text))
            {
                e.Cancel = true;
                txtMatKhauXacNhan.Focus();
                errorProvider1.SetError(txtMatKhauXacNhan, "Mật khẩu xác nhận không đúng!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtMatKhauXacNhan, "");
            }
        }        

        private void btnChonNV_Click(object sender, EventArgs e)
        {            
            FormChonNhanVien form = new FormChonNhanVien();
            form.ShowDialog();

            txtNhanVien.Text = nhanVien;
        }
    }
}
