using QLVT_Nhom38.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QLVT_Nhom38
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        private void btnDangNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormLogin));
            if (frm != null) frm.Activate();
            else
            {
                FormLogin f = new FormLogin();
                f.MdiParent = this;
                f.Show();
            }

        }

        public void HienThiMenu ()
        {
            ribDanhMuc.Visible = ribNghiepVu.Visible = ribBaoCao.Visible = true;
            //if trên Program.mGroup để bật tắt menu
            if(Program.mGroup.Equals("CONGTY") || Program.mGroup.Equals("CHINHANH"))
            {
                btnTaoTaiKhoan.Enabled = true;
            }
            btnDangXuat.Enabled = true;
        }    
    }
}
