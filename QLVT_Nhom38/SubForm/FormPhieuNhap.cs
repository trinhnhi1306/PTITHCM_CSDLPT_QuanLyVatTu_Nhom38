using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT_Nhom38.SubForm
{
    public partial class FormPhieuNhap : Form
    {
        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bds_PhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.qLVTDataSet);

        }

        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bds_DatHang.EndEdit();
            this.tableAdapterManager.UpdateAll(this.qLVTDataSet);

        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {

            qLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.qLVTDataSet.DatHang);
            // TODO: This line of code loads data into the 'qLVTDataSet.CTDDH' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.qLVTDataSet.CTDDH);
            // TODO: This line of code loads data into the 'qLVTDataSet.PhieuNhap' table. You can move, or remove it, as needed.
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
            // TODO: This line of code loads data into the 'qLVTDataSet.CTPN' table. You can move, or remove it, as needed.
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
            
            


            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;

 
            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = btnChuyenChiNhanh.Enabled = btnReload.Enabled = true;  // bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;

            }
            else
            {
                cmbChiNhanh.Enabled = btnUndo.Enabled = btnGhi.Enabled = false;
                btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = true;
            }

        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi form load thì hàm này được gọi, mà bds chưa có dữ liệu nên sẽ gây lỗi
            // "System.Data.DataRowView" sẽ xuất hiện và tất nhiên hệ thống sẽ không thể
            // nhận diện được tên server "System.Data.DataRowView".
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;

            // Lấy tên server
            Program.serverName = cmbChiNhanh.SelectedValue.ToString();

            // Nếu tên server khác với tên server ngoài đăng nhập, thì ta phải sử dụng HTKN
            if (cmbChiNhanh.SelectedIndex != Program.mChinhanh)
            {
                Program.mlogin = Program.remoteLogin;
                Program.password = Program.remotePassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }

            if (Program.KetNoi() == 0)
                XtraMessageBox.Show("Lỗi kết nối về chi nhánh mới", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.qLVTDataSet.DatHang);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTDDHTableAdapter.Fill(this.qLVTDataSet.CTDDH);
                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
                this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
               
            }
        }

        private void datHangGridControl_EmbeddedNavigator_SizeChanged(object sender, EventArgs e)
        {
          
           
        }
    }
}
