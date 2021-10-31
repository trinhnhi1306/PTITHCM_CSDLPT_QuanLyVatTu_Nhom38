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
        public FormDSDH_Chua_Co_PN()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Xrpt_DonhangkhongcoPhieuNhap reportDSDHKCPN = new Xrpt_DonhangkhongcoPhieuNhap();
            ReportPrintTool rpt = new ReportPrintTool(reportDSDHKCPN);
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
    }
}
