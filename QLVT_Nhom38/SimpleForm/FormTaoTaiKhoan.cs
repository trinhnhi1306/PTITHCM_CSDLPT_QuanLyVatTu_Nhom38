﻿using System;
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
            // TODO: This line of code loads data into the 'qLVTDataSet.NhanVien1' table. You can move, or remove it, as needed.
            this.HOTEN.Connection.ConnectionString = Program.connstr;
            this.HOTEN.Fill(this.QLVTDataSet.NhanVien1);

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
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                if (Program.mGroup != "CONGTY")
                    role = (radioButton_ChiNhanh.Checked == true) ? "CHINHANH" : "USER";

                String strLenh = string.Format("exec sp_TaoLogin '{0}', '{1}', '{2}', '{3}'",
                    txtLoginName.Text, txtMatKhau.Text, cmbNhanVien.SelectedValue.ToString(), role);

                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return;

                XtraMessageBox.Show("Tạo tài khoản cho nhân viên thành công!", "Thông báo", MessageBoxButtons.OK);
            }
        }

        private void txtLoginName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLoginName.Text))
            {
                e.Cancel = true;
                txtLoginName.Focus();
                errorProvider1.SetError(txtLoginName, "Vui lòng điền Login name!");
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
        }


    }
}
