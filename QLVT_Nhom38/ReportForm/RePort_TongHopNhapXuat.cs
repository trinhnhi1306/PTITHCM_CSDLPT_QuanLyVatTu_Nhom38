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
    public partial class RePort_TongHopNhapXuat : Form
    {
        String chiNhanh;
        public RePort_TongHopNhapXuat()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DateTime fromDate = (DateTime)dateStart.DateTime;
            DateTime toDate = (DateTime)dateEnd.DateTime;
            if (fromDate > toDate)
            {
                MessageBox.Show("ngày bắt đầu phải bé hơn ngày kết thúc",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
            DateTime today = DateTime.Today;
            if ((fromDate > today) || (toDate > today))
            {
                MessageBox.Show("ngày chọn không quá ngày hiện tại",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
            Xrpt_RePort_TongHopNhapXuat reportTHNX = new Xrpt_RePort_TongHopNhapXuat(fromDate, toDate);
            reportTHNX.lblStart.Text = fromDate.ToString();
            reportTHNX.lblEnd.Text = dateEnd.EditValue.ToString();
            reportTHNX.lbChiNhanh.Text = chiNhanh;
            ReportPrintTool rpt = new ReportPrintTool(reportTHNX);
            reportTHNX.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime fromDate = (DateTime)dateStart.DateTime;
            DateTime toDate = (DateTime)dateEnd.DateTime;
            if (fromDate > toDate)
            {
                MessageBox.Show("ngày bắt đầu phải bé hơn ngày kết thúc",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
            DateTime today = DateTime.Today;
            if ((fromDate > today) || (toDate > today))
            {
                MessageBox.Show("ngày chọn không quá ngày hiện tại",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
            Xrpt_RePort_TongHopNhapXuat reportTHNX = new Xrpt_RePort_TongHopNhapXuat(fromDate, toDate);
            reportTHNX.lblStart.Text = fromDate.ToString();
            reportTHNX.lblEnd.Text = dateEnd.EditValue.ToString();
            try
            {
                if (File.Exists(@"E:\FileReport\ReportTongHopNhapXuat.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportTongHopNhapXuat.pdf tại ổ E đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        reportTHNX.ExportToPdf(@"E:\FileReport\ReportTongHopNhapXuat.pdf");
                        MessageBox.Show("File ReportTongHopNhapXuat.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    reportTHNX.ExportToPdf(@"E:\FileReport\ReportTongHopNhapXuat.pdf");
                    MessageBox.Show("File ReportTongHopNhapXuat.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportTongHopNhapXuat.pdf",
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void RePort_TongHopNhapXuat_Load(object sender, EventArgs e)
        {
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "CONGTY")
                cmbChiNhanh.Enabled = true;  // bật tắt theo phân quyền
            else cmbChiNhanh.Enabled = false;
            chiNhanh = cmbChiNhanh.Text;
        }
    }
}
