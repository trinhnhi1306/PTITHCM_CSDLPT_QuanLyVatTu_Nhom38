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
    public partial class HoatDongNhanVien : Form
    {
        int maNV ;
        String chiNhanh;
        public HoatDongNhanVien()
        {
          
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.hOTEN.Connection.ConnectionString = Program.connstr;
            // TODO: This line of code loads data into the 'qLVTDataSet.NhanVien1' table. You can move, or remove it, as needed.
            this.hOTEN.Fill(this.qLVTDataSet.NhanVien1);
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;
            if (Program.mGroup == "CONGTY")
                cmbChiNhanh.Enabled = true;  // bật tắt theo phân quyền
            else cmbChiNhanh.Enabled = false;
            chiNhanh = cmbChiNhanh.Text;


            dateEnd.Properties.Mask.UseMaskAsDisplayFormat = true;
            dateEnd.DateTime = DateTime.Today.AddYears(0);



            dateStart.Properties.Mask.UseMaskAsDisplayFormat = true;
            dateStart.DateTime = DateTime.Today.AddYears(-5);
           
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;

            // Lấy tên server
            Program.serverName = cmbChiNhanh.SelectedValue.ToString();
            chiNhanh = cmbChiNhanh.Text;

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
                this.hOTEN.Connection.ConnectionString = Program.connstr;
                // TODO: This line of code loads data into the 'qLVTDataSet.NhanVien1' table. 
                //You can move, or remove it, as needed.
                this.hOTEN.Fill(this.qLVTDataSet.NhanVien1);
                cmbHoTen.SelectedIndex = 1;
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void hOTENComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {




            String strLenh = "[dbo].[sp_ThongTinNhanVien] " +
                                 int.Parse(cmbHoTen.SelectedValue.ToString())  ;
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;

            Program.myReader.Read();
            String HOTEN = Program.myReader.GetValue(0).ToString();
            String DAICHI = Program.myReader.GetValue(1).ToString();
            DateTime NGAYSINH = (DateTime)Program.myReader.GetValue(2);
            float LUONG = float.Parse(Program.myReader.GetValue(3).ToString());
            Program.myReader.Close();

            DateTime fromDate = (DateTime)dateStart.DateTime;
            DateTime toDate = (DateTime)dateEnd.DateTime;

            if (fromDate > toDate)
            {
                MessageBox.Show("ngày bắt đầu phải bé hơn ngày kết thúc",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
            if ((fromDate > DateTime.Today.AddYears(0)) || (toDate > DateTime.Today.AddYears(0)))
            {
                MessageBox.Show("ngày chọn không quá ngày hiện tại",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }

            Xrpt_HoatDongNhanVien reportHDNV = new Xrpt_HoatDongNhanVien(int.Parse(cmbHoTen.SelectedValue.ToString()), fromDate, toDate);
            reportHDNV.lblMaNV.Text = cmbHoTen.SelectedValue.ToString();
            reportHDNV.lblDiaChi.Text = DAICHI;
            reportHDNV.lblNgaySinh.Text = NGAYSINH.ToString();
            reportHDNV.lblLuong.Text = LUONG.ToString();
            reportHDNV.lblChiNhanh.Text = chiNhanh;
            reportHDNV.lblTenNV.Text = HOTEN;

            ReportPrintTool rpt = new ReportPrintTool(reportHDNV);
            reportHDNV.ShowPreviewDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String strLenh = "[dbo].[sp_ThongTinNhanVien] " +
                                int.Parse(cmbHoTen.SelectedValue.ToString());
            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;

            Program.myReader.Read();
            String HOTEN = Program.myReader.GetValue(0).ToString();
            String DAICHI = Program.myReader.GetValue(1).ToString();
            DateTime NGAYSINH = (DateTime)Program.myReader.GetValue(2);
            float LUONG = float.Parse(Program.myReader.GetValue(3).ToString());
            Program.myReader.Close();

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
            Xrpt_HoatDongNhanVien reportHDNV = new Xrpt_HoatDongNhanVien(int.Parse(cmbHoTen.SelectedValue.ToString()), fromDate, toDate);
            reportHDNV.lblMaNV.Text = cmbHoTen.SelectedValue.ToString();
            reportHDNV.lblDiaChi.Text = DAICHI;
            reportHDNV.lblNgaySinh.Text = NGAYSINH.ToString();
            reportHDNV.lblLuong.Text = LUONG.ToString();
            reportHDNV.lblChiNhanh.Text = chiNhanh;
            reportHDNV.lblTenNV.Text = HOTEN;

            try
            {
                if (File.Exists(@"E:\FileReport\ReportHoatDongNhanVien.pdf"))
                {
                    DialogResult dr = MessageBox.Show("File ReportHoatDongNhanVien.pdf tại ổ D đã có!\nBạn có muốn tạo lại?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dr == DialogResult.Yes)
                    {
                        reportHDNV.ExportToPdf(@"E:\FileReport\ReportHoatDongNhanVien.pdf");
                        MessageBox.Show("File ReportHoatDongNhanVien.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                }
                else
                {
                    reportHDNV.ExportToPdf(@"E:\FileReport\ReportHoatDongNhanVien.pdf");
                    MessageBox.Show("File ReportHoatDongNhanVien.pdf đã được ghi thành công tại folder FileReport trong ổ E",
                "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show("Vui lòng đóng file ReportHoatDongNhanVien.pdf",
                    "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
