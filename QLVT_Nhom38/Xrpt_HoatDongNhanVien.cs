﻿using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QLVT_Nhom38
{
    public partial class Xrpt_HoatDongNhanVien : DevExpress.XtraReports.UI.XtraReport
    {
        
        public Xrpt_HoatDongNhanVien(int maNV, DateTime dateStart, DateTime dateEnd)
        {
            InitializeComponent();
            this.sqlDataSource1.Connection.ConnectionString = Program.connstr;
            this.sqlDataSource1.Queries[0].Parameters[0].Value = maNV;
            this.sqlDataSource1.Queries[0].Parameters[1].Value = dateStart;
            this.sqlDataSource1.Queries[0].Parameters[2].Value = dateEnd;
            this.sqlDataSource1.Fill();
            
        }

    }
}
