using DevExpress.XtraReports.UI;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;

namespace QLVT_Nhom38
{
    public partial class Xrpt_Report_DSNhanVienTheoHoTen : DevExpress.XtraReports.UI.XtraReport
    {
        public Xrpt_Report_DSNhanVienTheoHoTen()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill();
        }

    }
}
