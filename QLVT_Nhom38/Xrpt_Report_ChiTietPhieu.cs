using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLVT_Nhom38
{
    public partial class Xrpt_Report_ChiTietPhieu : DevExpress.XtraReports.UI.XtraReport
    {
        public Xrpt_Report_ChiTietPhieu(String role, String loai,DateTime dateStart, DateTime dateEnd)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = role;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = loai.Contains("N") ? "NHAP" : "XUAT";
            this.sqlDataSource1.Queries[0].Parameters[2].Value = dateStart.ToString("yyyy-MM-dd");
            this.sqlDataSource1.Queries[0].Parameters[3].Value = dateEnd.ToString("yyyy-MM-dd");
            this.sqlDataSource1.Fill();
        }

    }
}
