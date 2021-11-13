using QLVT_Nhom38.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using QLVT_Nhom38.SimpleForm;
using QLVT_Nhom38.ReportForm;
using QLVT_Nhom38.SubForm;

namespace QLVT_Nhom38
{
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public FormMain()
        {
            InitializeComponent();
        }

        public void skins()
        {
            DevExpress.LookAndFeel.DefaultLookAndFeel themes = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            themes.LookAndFeel.SkinName = "Summer 2008";
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
            btnDangNhap.Enabled = false;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            skins();
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr;
            dr = XtraMessageBox.Show("Bạn có muốn thoát không?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
        }

        private void btnNhanVien_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                FormNhanVien f = new FormNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDangXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            ribDanhMuc.Visible = ribNghiepVu.Visible = ribBaoCao.Visible = false;
            //if trên Program.mGroup để bật tắt menu
            if (Program.mGroup.Equals("CONGTY") || Program.mGroup.Equals("CHINHANH"))
            {
                btnTaoTaiKhoan.Enabled = false;
            }
            foreach (Form f in this.MdiChildren)
                f.Dispose();
            btnDangNhap.Enabled = true;
            btnDangXuat.Enabled = false;

            Program.formMain.MANV.Text = "MANV";
            Program.formMain.HOTEN.Text = "HOTEN";
            Program.formMain.NHOM.Text = "NHOM";
        }

        private void btnVatTu_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormVatTu));
            if (frm != null) frm.Activate();
            else
            {
                FormVatTu f = new FormVatTu();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnReportVT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(Report_DS_VatTu));
            if (frm != null) frm.Activate();
            else
            {
                Report_DS_VatTu f = new Report_DS_VatTu();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(RePort_TongHopNhapXuat));
            if (frm != null) frm.Activate();
            else
            {
                RePort_TongHopNhapXuat f = new RePort_TongHopNhapXuat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(HoatDongNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                HoatDongNhanVien f = new HoatDongNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormInDSNhanVien));
            if (frm != null) frm.Activate();
            else
            {
                FormInDSNhanVien f = new FormInDSNhanVien();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormChiTietPhieu));
            if (frm != null) frm.Activate();
            else
            {
                FormChiTietPhieu f = new FormChiTietPhieu();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormDSDH_Chua_Co_PN));
            if (frm != null) frm.Activate();
            else
            {
                FormDSDH_Chua_Co_PN f = new FormDSDH_Chua_Co_PN();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnKho_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormKho));
            if (frm != null) frm.Activate();
            else
            {
                FormKho f = new FormKho();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnDonDatHang_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormDonDatHang));
            if (frm != null) frm.Activate();
            else
            {
                FormDonDatHang f = new FormDonDatHang();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPhieuNhap_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormPhieuNhap));
            if (frm != null) frm.Activate();
            else
            {
                FormPhieuNhap f = new FormPhieuNhap();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnPhieuXuat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormPhieuXuat));
            if (frm != null) frm.Activate();
            else
            {
                FormPhieuXuat f = new FormPhieuXuat();
                f.MdiParent = this;
                f.Show();
            }
        }

        private void btnTaoTaiKhoan_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Form frm = this.CheckExists(typeof(FormTaoTaiKhoan));
            if (frm != null) frm.Activate();
            else
            {
                FormTaoTaiKhoan f = new FormTaoTaiKhoan();
                f.MdiParent = this;
                f.Show();
            }
        }
    }
}
