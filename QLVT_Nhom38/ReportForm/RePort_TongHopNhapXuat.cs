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
        public RePort_TongHopNhapXuat()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime fromDate = (DateTime)dateStart.DateTime;
            DateTime toDate = (DateTime)dateEnd.DateTime;
            Xrpt_RePort_TongHopNhapXuat reportTHNX = new Xrpt_RePort_TongHopNhapXuat(fromDate, toDate);
            reportTHNX.lblStart.Text = fromDate.ToString();
            reportTHNX.lblEnd.Text = dateEnd.EditValue.ToString();
            
            ReportPrintTool rpt = new ReportPrintTool(reportTHNX);
            reportTHNX.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime fromDate = (DateTime)dateStart.DateTime;
            DateTime toDate = (DateTime)dateEnd.DateTime;
            Xrpt_RePort_TongHopNhapXuat reportTHNX = new Xrpt_RePort_TongHopNhapXuat(fromDate, toDate);
            reportTHNX.lblStart.Text = fromDate.ToString();
            reportTHNX.lblEnd.Text = dateEnd.EditValue.ToString();
            try
            {
                if (File.Exists(@"E:\FileReport\ReportTongHopNhapXuat.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportTongHopNhapXuat.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        reportTHNX.ExportToPdf(@"E:\FileReport\ReportTongHopNhapXuat.pdf");
                        MessageBox.Show("File ReportTongHopNhapXuat.pdf đã được ghi thành công tại ổ D",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    reportTHNX.ExportToPdf(@"E:\FileReport\ReportTongHopNhapXuat.pdf");
                    MessageBox.Show("File ReportTongHopNhapXuat.pdf đã được ghi thành công tại ổ D",
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
    }
}
