using DevExpress.XtraEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT_Nhom38.SimpleForm
{
    public partial class FormNhanVien : Form
    {
        int checkThem = 0;
        int position = 0; // dùng cho nút phục hồi và thêm
        string maCN = "";
        Stack undolist = new Stack();

        public FormNhanVien()
        {
            InitializeComponent();
        }

        private void nhanVienBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsNV.EndEdit();
            this.tableAdapterManager.UpdateAll(this.QLVTDataSet);

        }

        private void FormNhanVien_Load(object sender, EventArgs e)
        {
            QLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.NhanVien' table. You can move, or remove it, as needed.
            this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
            
            // TODO: This line of code loads data into the 'QLVTDataSet.PhieuXuat' table. You can move, or remove it, as needed.
            this.PhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.PhieuXuatTableAdapter.Fill(this.QLVTDataSet.PhieuXuat);
            
            // TODO: This line of code loads data into the 'QLVTDataSet.DatHang' table. You can move, or remove it, as needed.
            this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);
            
            // TODO: This line of code loads data into the 'QLVTDataSet.PhieuNhap' table. You can move, or remove it, as needed.
            this.PhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.PhieuNhapTableAdapter.Fill(this.QLVTDataSet.PhieuNhap);

            // giữ lại mã CN của nhân viên đầu tiên
            // tiềm ẩn lỗi, xác suất rất thấp, khi thi thầy sẽ thả lỗi này vô
            maCN = ((DataRowView)bdsNV[0])["MACN"].ToString();
            
            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;

            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = btnChuyenChiNhanh.Enabled = true;  // bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
                
            }
            else
            {
                cmbChiNhanh.Enabled = false;
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnGhi.Enabled = btnUndo.Enabled = true;
            }

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            checkThem = 1;
            position = bdsNV.Position; // vị trí mà nhân viên đang đứng
            infoNhanVien.Enabled = txtMaNV.Enabled = true;
            bdsNV.AddNew();
            txtMaCN.Text = maCN;
            dteNgaySinh.EditValue = "";
            cbTrangThaiXoa.Checked = false;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcNhanVien.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            bdsNV.CancelEdit();
            if (btnThem.Enabled == false && checkThem == 1) // kiểm tra xem có đang ở chế độ thêm mới không
            {
                bdsNV.RemoveCurrent(); // xóa record đang thêm dở dang đi
                bdsNV.Position = position;
                checkThem = 0;
            }
            gcNhanVien.Enabled = true;
            infoNhanVien.Enabled = false;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsNV.Position;
            gcNhanVien.Enabled = txtMaNV.Enabled = false;
            infoNhanVien.Enabled = true;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;

        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Reload :" + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maNV = "";
            if (bdsDH.Count > 0)
            {
                XtraMessageBox.Show("Không thể xóa vì nhân viên đã lập đơn đặt hàng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (bdsPN.Count > 0)
            {
                XtraMessageBox.Show("Không thể xóa vì nhân viên đã lập phiếu nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (bdsPX.Count > 0)
            {
                XtraMessageBox.Show("Không thể xóa vì nhân viên đã lập phiếu xuất", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = XtraMessageBox.Show("Bạn có thực sự muốn xóa nhân viên này không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    maNV = ((DataRowView)bdsNV[bdsNV.Position])["MANV"].ToString(); // giữ lại mã nv để phòng trường hợp xóa lỗi
                    bdsNV.RemoveCurrent(); // xóa trên máy hiện tại trước
                    this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.NhanVienTableAdapter.Update(this.QLVTDataSet.NhanVien); // update về csdl sau
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien); // trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                    // lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                    bdsNV.Position = bdsNV.Find("MANV", maNV); // sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo mã nv ban nãy muốn xóa
                    return;
                }
            }

            if (bdsNV.Count == 0) btnXoa.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (txtMaNV.Text.Trim() == "")
            {
                XtraMessageBox.Show("Mã nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMaNV.Focus();
                return;
            }
            if (txtHo.Text.Trim() == "")
            {
                XtraMessageBox.Show("Họ nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtHo.Focus();
                return;
            }
            if (txtTen.Text.Trim() == "")
            {
                XtraMessageBox.Show("Tên nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtTen.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim() == "")
            {
                XtraMessageBox.Show("Địa chỉ nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }

            try
            {
                bdsNV.EndEdit(); // kết thúc quá trình hiệu chỉnh và ghi vào BindingSource
                bdsNV.ResetCurrentItem(); // đưa thông tin vào grid
                this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                this.NhanVienTableAdapter.Update(this.QLVTDataSet.NhanVien); // update xuống csdl
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi xảy ra trong quá trình ghi nhân viên. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Khi Update database lỗi thì xóa record vừa thêm trong bds
                //bdsNV.RemoveCurrent();
                return;
            }

            gcNhanVien.Enabled = true;
            infoNhanVien.Enabled = false;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = btnUndo.Enabled = false;
        }

        private void txtMaNV_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Xác thực rằng phím vừa nhấn không phải CTRL hoặc không phải dạng số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // nếu phải thì không làm gì cả
            }
        }

        private void txtHo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || e.KeyChar == '\b')
                e.Handled = false;
            else if (e.KeyChar == ' ')
            {
                if (txtHo.Text[txtHo.Text.Length - 1] == ' ')
                    e.Handled = true;
                else
                    e.Handled = false;
            }
            else
                e.Handled = true;
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi form load thì hàm này được gọi, mà bds chưa có dữ liệu nên sẽ gây lỗi
            // "System.Data.DataRowView" sẽ xuất hiện và tất nhiên hệ thống sẽ không thể
            // nhận diện được tên server "System.Data.DataRowView".
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;

            // Lấy tên server
            Program.serverName = cmbChiNhanh.SelectedValue.ToString();

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
                this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
                this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);
                this.PhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.PhieuNhapTableAdapter.Fill(this.QLVTDataSet.PhieuNhap);
                //maCN = ((DataRowView)bdsNV[0])["MACN"].ToString();
            }
        }

        private void txtTen_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || e.KeyChar == '\b')
                e.Handled = false;
            else if (e.KeyChar == ' ')
            {
                if (txtTen.Text[txtTen.Text.Length - 1] == ' ')
                    e.Handled = true;
                else
                    e.Handled = false;
            }    
            else
                e.Handled = true;
        }
    }
}
