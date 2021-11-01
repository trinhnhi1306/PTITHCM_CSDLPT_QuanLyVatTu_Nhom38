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
    public partial class FormChiTietPhieu : Form
    {
        public FormChiTietPhieu()
        {
            InitializeComponent();
            dateFrom.Properties.Mask.EditMask = "MM-yyyy";

            dateFrom.Properties.Mask.UseMaskAsDisplayFormat = true;
            dateFrom.DateTime = DateTime.Today.AddYears(-5);
            dateFrom.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
            dateTo.Properties.Mask.EditMask = "MM-yyyy";

            dateTo.Properties.Mask.UseMaskAsDisplayFormat = true;
            dateTo.DateTime = DateTime.Today.AddYears(0);
            dateTo.Properties.VistaCalendarViewStyle = DevExpress.XtraEditors.VistaCalendarViewStyle.YearView;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            String role = Program.mGroup;
            String loai = cmbLoai.SelectedItem.ToString();
            DateTime fromDate = (DateTime)dateFrom.DateTime;
            DateTime toDate = (DateTime)dateTo.DateTime;
            Xrpt_Report_ChiTietPhieu reportCTP = new Xrpt_Report_ChiTietPhieu(role, loai, fromDate, toDate);

            reportCTP.lblLoai.Text = loai;
            ReportPrintTool rpt = new ReportPrintTool(reportCTP);
            reportCTP.ShowPreviewDialog();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                  
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String role = Program.mGroup;
            String loai = cmbLoai.SelectedItem.ToString();
            DateTime fromDate = (DateTime)dateFrom.DateTime;
            DateTime toDate = (DateTime)dateTo.DateTime;
            Xrpt_Report_ChiTietPhieu reportCTP = new Xrpt_Report_ChiTietPhieu(role, loai, fromDate, toDate);

            reportCTP.lblLoai.Text = loai;
            try
            {
                if (File.Exists(@"E:\FileReport\ReportChiTietPhieu.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportChiTietPhieu.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        reportCTP.ExportToPdf(@"E:\FileReport\ReportChiTietPhieu.pdf");
                        MessageBox.Show("File ReportChiTietPhieu.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    reportCTP.ExportToPdf(@"E:\FileReport\ReportChiTietPhieu.pdf");
                    MessageBox.Show("File ReportChiTietPhieu.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportChiTietPhieu.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }
    }
}
