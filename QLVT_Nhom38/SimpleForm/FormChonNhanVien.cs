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
    public partial class FormChonNhanVien : Form
    {
        public FormChonNhanVien()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNV.EndEdit();
            this.tableAdapterManager.UpdateAll(this.QLVTDataSet);

        }

        private void FormChonNhanVien_Load(object sender, EventArgs e)
        {
            QLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.NhanVien' table. You can move, or remove it, as needed.
            this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
        }

        private void btnChon_Click(object sender, EventArgs e)
        {
            DataRowView drv = ((DataRowView)bdsNV[bdsNV.Position]);
            if(drv["TrangThaiXoa"].ToString().Equals("1"))
            {
                XtraMessageBox.Show("Nhân viên này đã bị xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            FormTaoTaiKhoan.nhanVien = drv["MANV"].ToString();
            this.Dispose();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
