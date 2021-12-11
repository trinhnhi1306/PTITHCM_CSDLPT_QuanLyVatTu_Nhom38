using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLVT_Nhom38
{
    public partial class Xrpt_Report_DS_VatTu : DevExpress.XtraReports.UI.XtraReport
    {
        public Xrpt_Report_DS_VatTu()
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Fill();
        }

    }
}
