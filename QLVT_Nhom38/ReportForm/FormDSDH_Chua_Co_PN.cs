using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
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

namespace QLVT_Nhom38.ReportForm
{
    public partial class FormDSDH_Chua_Co_PN : Form
    {
        String chiNhanh;
        public FormDSDH_Chua_Co_PN()
        {
            InitializeComponent();
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            
            if (Program.mGroup == "CONGTY")
                cmbChiNhanh.Enabled = true;  // bật tắt theo phân quyền
            else cmbChiNhanh.Enabled = false;
            chiNhanh = cmbChiNhanh.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Xrpt_DonhangkhongcoPhieuNhap reportDSDHKCPN = new Xrpt_DonhangkhongcoPhieuNhap();
            ReportPrintTool rpt = new ReportPrintTool(reportDSDHKCPN);
            reportDSDHKCPN.lbChiNhanh.Text = chiNhanh;
            reportDSDHKCPN.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Xrpt_DonhangkhongcoPhieuNhap reportDSDHKCPN = new Xrpt_DonhangkhongcoPhieuNhap();
       
            try
            {
                if (File.Exists(@"E:\FileReport\ReportDonHangKhongCoPhieu.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportDonHangKhongCoPhieu.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        reportDSDHKCPN.ExportToPdf(@"E:\FileReport\ReportDonHangKhongCoPhieu.pdf");
                        MessageBox.Show("File ReportDonHangKhongCoPhieu.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    reportDSDHKCPN.ExportToPdf(@"E:\FileReport\ReportDonHangKhongCoPhieu.pdf");
                    MessageBox.Show("File ReportDonHangKhongCoPhieu.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportDonHangKhongCoPhieu.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                chiNhanh = cmbChiNhanh.Text;
            }
        }
    }
}
