using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLVT_Nhom38
{
    public partial class Xrpt_DonhangkhongcoPhieuNhap : DevExpress.XtraReports.UI.XtraReport
    {
        public Xrpt_DonhangkhongcoPhieuNhap()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill();
        }

    }
}
