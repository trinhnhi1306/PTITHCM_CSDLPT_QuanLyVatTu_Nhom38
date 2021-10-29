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
    public partial class Report_DS_VatTu : Form
    {
        public Report_DS_VatTu()
        {
            InitializeComponent();
        }

        private void btnReview_Click(object sender, EventArgs e)
        {
            Xrpt_Report_DS_VatTu reportDSVT = new Xrpt_Report_DS_VatTu();
            ReportPrintTool rpt = new ReportPrintTool(reportDSVT);
            reportDSVT.ShowPreviewDialog();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            Xrpt_Report_DS_VatTu report = new Xrpt_Report_DS_VatTu();
            try
            {
                if (File.Exists(@"E:\FileReport\ReportDanhSachVatTu.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportDanhSachVatTu.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        report.ExportToPdf(@"E:\FileReport\ReportDanhSachVatTu.pdf");
                        MessageBox.Show("File ReportDanhSachVatTu.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    report.ExportToPdf(@"E:\FileReport\ReportDanhSachVatTu.pdf");
                    MessageBox.Show("File ReportDanhSachVatTu.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportDanhSachVatTu.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
