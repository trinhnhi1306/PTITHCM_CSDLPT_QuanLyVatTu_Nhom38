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
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace QLVT_Nhom38.SimpleForm
{
    public partial class FormKho : Form
    {
        int checkThem = 0;
        int position = 0; // vị trí trên grid view
        string maCN = "";
        Stack undoList = new Stack();

        public FormKho()
        {
            InitializeComponent();
        }

        private void khoBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsKho.EndEdit();
            this.tableAdapterManager.UpdateAll(this.QLVTDataSet);

        }

        private void FormKho_Load(object sender, EventArgs e)
        {
            QLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.Kho' table. You can move, or remove it, as needed.
            this.KhoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.KhoTableAdapter.Fill(this.QLVTDataSet.Kho);

            // TODO: This line of code loads data into the 'QLVTDataSet.DatHang' table. You can move, or remove it, as needed.
            this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);

            // TODO: This line of code loads data into the 'QLVTDataSet.PhieuNhap' table. You can move, or remove it, as needed.
            this.PhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.PhieuNhapTableAdapter.Fill(this.QLVTDataSet.PhieuNhap);

            // TODO: This line of code loads data into the 'QLVTDataSet.PhieuXuat' table. You can move, or remove it, as needed.
            this.PhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.PhieuXuatTableAdapter.Fill(this.QLVTDataSet.PhieuXuat);

            // giữ lại mã CN của nhân viên đầu tiên
            // tiềm ẩn lỗi, xác suất rất thấp, khi thi thầy sẽ thả lỗi này vô
            try
            {
                maCN = ((DataRowView)bdsKho[0])["MACN"].ToString();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Thông báo", MessageBoxButtons.OK);
            }


            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;

            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = btnReload.Enabled = true;  // bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;

            }
            else
            {
                cmbChiNhanh.Enabled = btnUndo.Enabled = btnGhi.Enabled = false;
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = true;
            }

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            checkThem = 1; // bật trạng thái đang thêm mới lên
            position = bdsKho.Position; // vị trí mà nhân viên đang đứng
            infoKho.Enabled = txtMaKho.Enabled = true;
            bdsKho.AddNew(); // thêm một record trống mới trong grid view

            // đặt các giá trị mặc định
            txtMaCN.Text = maCN;
            txtTenKho.Text = "";
            txtDiaChi.Text = "";
            txtMaKho.Text = "";

            // vô hiệu hóa các nút chức năng
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcKho.Enabled = false;
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
                this.KhoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.KhoTableAdapter.Fill(this.QLVTDataSet.Kho);
                this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);
                this.PhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.PhieuNhapTableAdapter.Fill(this.QLVTDataSet.PhieuNhap);
                this.PhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.PhieuXuatTableAdapter.Fill(this.QLVTDataSet.PhieuXuat);
                //maCN = ((DataRowView)bdsNV[0])["MACN"].ToString();
            }
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Đang ở chế độ thêm mới hoặc sửa, chưa ấn Ghi, ấn Hoàn tác sẽ thoát chế độ Thêm /Sửa
            if (btnThem.Enabled == false)
            {
                bdsKho.CancelEdit();
                if (checkThem == 1) // chế độ thêm
                {
                    bdsKho.RemoveCurrent(); // xóa record đang thêm dở dang đi
                    checkThem = 0;
                }
                bdsKho.Position = position;


                gcKho.Enabled = true;
                infoKho.Enabled = false;

                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
                btnGhi.Enabled = false;


                if (undoList.Count == 0)
                {
                    btnUndo.Enabled = false;
                }

                return;
            }

            // Danh sách undoList trống
            if (undoList.Count == 0)
            {
                XtraMessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnUndo.Enabled = false;
                return;
            }

            // Bắt đầu hoàn tác
            bdsKho.CancelEdit();
            String strLenhUndo = undoList.Pop().ToString();
            Console.WriteLine(strLenhUndo);

            if (Program.KetNoi() == 0)
                return;
            int n = Program.ExecSqlNonQuery(strLenhUndo);
            reload();
            bdsKho.Position = position;

            // Nếu sau khi undo mà danh sách undoList trống thì disable nút undo đi
            if (undoList.Count == 0)
            {
                btnUndo.Enabled = false;
                return;
            }
        }

        private void reload()
        {
            try
            {
                this.KhoTableAdapter.Fill(this.QLVTDataSet.Kho);
                bdsKho.Position = position;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Reload: " + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsKho.Position;
            gcKho.Enabled = txtMaKho.Enabled = false;
            infoKho.Enabled = true;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bdsKho.Count == 0)
                XtraMessageBox.Show("Danh sách trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            string maKho = ((DataRowView)bdsKho[bdsKho.Position])["MAKHO"].ToString();
            if (bdsDH.Count > 0)
            {
                XtraMessageBox.Show("Không thể xóa vì kho đã được lập đơn đặt hàng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (bdsPN.Count > 0)
            {
                XtraMessageBox.Show("Không thể xóa vì nhân viên đã được lập phiếu nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (bdsPX.Count > 0)
            {
                XtraMessageBox.Show("Không thể xóa vì nhân viên đã được lập phiếu xuất", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string strLenhUndo =
                string.Format("INSERT INTO DBO.KHO (MAKHO,TENKHO,DIACHI,MACN) " +
            "VALUES ('{0}', N'{1}', N'{2}', '{3}')", txtMaKho.Text, txtTenKho.Text, txtDiaChi.Text, txtMaCN.Text.Trim());

            undoList.Push(strLenhUndo);

            DialogResult dr = XtraMessageBox.Show("Bạn có thực sự muốn xóa kho này không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {

                try
                {
                    maKho = ((DataRowView)bdsKho[bdsKho.Position])["MAKHO"].ToString(); // giữ lại mã kho để phòng trường hợp xóa lỗi
                    bdsKho.RemoveCurrent(); // xóa trên máy hiện tại trước
                    this.KhoTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.KhoTableAdapter.Update(this.QLVTDataSet.Kho); // update về csdl sau
                    this.btnUndo.Enabled = true;
                    XtraMessageBox.Show("Xóa kho thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.KhoTableAdapter.Fill(this.QLVTDataSet.Kho); // trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                    // lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                    bdsKho.Position = bdsKho.Find("MAKHO", maKho); // sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo mã kho ban nãy muốn xóa
                    undoList.Pop();
                    return;
                }
            }
            else undoList.Pop();

            if (bdsKho.Count == 0) btnXoa.Enabled = false;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {

                String strLenhUndo = "";
                // Trường hợp đang thêm mới
                if (checkThem == 1)
                {
                    strLenhUndo = "DELETE DBO.KHO WHERE MAKHO = '" + txtMaKho.Text.Trim() + "'";
                }

                // Trường hợp sửa thì lưu lại dữ liệu cũ để đưa vào stack
                else
                {
                    String maKho = txtMaKho.Text.Trim();// Trim() de loai bo khoang trang thua
                    DataRowView drv = ((DataRowView)bdsKho[bdsKho.Position]);
                    String tenKho = drv["TENKHO"].ToString();

                    String diaChi = drv["DIACHI"].ToString();
                    String maCN = drv["MACN"].ToString();

                    strLenhUndo = "UPDATE DBO.KHO " +
                                    "SET " +
                                    "TENKHO = N'" + tenKho + "'," +
                                    "DIACHI = N'" + diaChi + "' " +
                                    "WHERE MAKHO = '" + maKho + "'";
                }

                try
                {
                    bdsKho.EndEdit(); // kết thúc quá trình hiệu chỉnh và ghi vào BindingSource
                    bdsKho.ResetCurrentItem(); // đưa thông tin vào grid
                    this.KhoTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                    this.KhoTableAdapter.Update(this.QLVTDataSet.Kho); // update xuống csdl

                    XtraMessageBox.Show("Ghi thông tin kho thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnUndo.Enabled = true;
                    undoList.Push(strLenhUndo);
                    checkThem = 0;
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi xảy ra trong quá trình ghi thông tin kho. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Khi Update database lỗi thì xóa record vừa thêm trong bds
                    //bdsNV.RemoveCurrent();
                    reload();
                    return;
                }

                gcKho.Enabled = true;
                infoKho.Enabled = false;

                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
                btnGhi.Enabled = false;
            }
            else
            {
                XtraMessageBox.Show("Lỗi xảy ra trong quá trình ghi thông tin kho. Vui lòng thử lại!\n", "Thông báo lỗi",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (infoKho.Enabled)
            {
                if (XtraMessageBox.Show("Dữ liệu Kho vẫn chưa lưu vào Database!\nBạn có chắn chắn muốn thoát?", "Thông báo",
                            MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void txtMaKho_Validating(object sender, CancelEventArgs e)
        {
            // Trường hợp đang thêm mới
            if (checkThem == 1)
            {
                var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
                if (string.IsNullOrWhiteSpace(txtMaKho.Text))
                {
                    e.Cancel = true;
                    txtMaKho.Focus();
                    errorProviderKho.SetError(txtMaKho, "Mã kho không được để trống!");
                }
                else if (txtMaKho.Text.Length > 4)
                {
                    e.Cancel = true;
                    txtMaKho.Focus();
                    errorProviderKho.SetError(txtMaKho, "Mã kho không được quá 4 kí tự!");

                }                
                else if (txtMaKho.Text.Trim().Contains(" "))
                {
                    e.Cancel = true;
                    txtMaKho.Focus();
                    errorProviderKho.SetError(txtMaKho, "Mã kho không được chứa khoảng trắng!");
                }
                else if (!regexItem.IsMatch(txtMaKho.Text.Trim()))
                {
                    e.Cancel = true;
                    txtMaKho.Focus();
                    errorProviderKho.SetError(txtMaKho, "Mã kho chỉ được gồm chữ cái và số!");
                }
                else
                {
                    /* kiểm tra mã nhân viên có trùng không 
                     * kiểm tra trên phân mảnh hiện tại trước, nếu không có mới lên server3 để tra cứu
                     * soạn sẵn câu lệnh để đưa vào hàm ExecSqlDataReader
                     * sau đó đọc kết quả trong myReader
                     */

                    int viTriMaKho = bdsKho.Find("MAKHO", txtMaKho.Text);
                    if (viTriMaKho != -1)
                    {
                        XtraMessageBox.Show("Mã kho này đã được sử dụng!", "Thông báo", MessageBoxButtons.OK);
                        txtMaKho.Focus();
                        errorProviderKho.SetError(txtMaKho, "Mã kho này đã được sử dụng!");
                    }
                    else
                    {
                        String strLenh = "DECLARE @return_value int " +
                                         "EXEC @return_value = [dbo].[sp_TraCuu_NhanVien_Kho] '" +
                                         txtMaKho.Text.Trim() + "', 'MAKHO' " +
                                         "SELECT 'Return Value' = @return_value";
                        Program.myReader = Program.ExecSqlDataReader(strLenh);
                        if (Program.myReader == null) return;

                        Program.myReader.Read();
                        int result = int.Parse(Program.myReader.GetValue(0).ToString());
                        Program.myReader.Close();

                        if (result == 1)
                        {
                            XtraMessageBox.Show("Mã kho này đã được sử dụng!", "Thông báo", MessageBoxButtons.OK);
                            txtMaKho.Focus();
                            errorProviderKho.SetError(txtMaKho, "Mã kho này đã được sử dụng!");
                        }
                    }
                }              
            }            
        }

        private void txtTenKho_Validating(object sender, CancelEventArgs e)
        {            
            /* kiểm tra mã nhân viên có trùng không 
            * kiểm tra trên phân mảnh hiện tại trước, nếu không có mới lên server3 để tra cứu
            * soạn sẵn câu lệnh để đưa vào hàm ExecSqlDataReader
            * sau đó đọc kết quả trong myReader
            */
            if (string.IsNullOrWhiteSpace(txtTenKho.Text))
            {
                e.Cancel = true;
                txtTenKho.Focus();
                errorProviderKho.SetError(txtTenKho, "Tên kho không được để trống!");
            }
            else
            {
                int viTriTenKho = bdsKho.Find("TENKHO", txtTenKho.Text);

                if (checkThem == 1 && viTriTenKho != -1)
                {
                    //XtraMessageBox.Show("Tên kho này đã được sử dụng ở chi nhánh này!", "Thông báo", MessageBoxButtons.OK);
                    txtTenKho.Focus();
                    errorProviderKho.SetError(txtTenKho, "Tên kho này đã được sử dụng!");
                    return;
                }

                if (checkThem == 0 && viTriTenKho != -1 && viTriTenKho != bdsKho.Position)
                {
                    //XtraMessageBox.Show("Tên kho này đã được sử dụng ở chi nhánh này!", "Thông báo", MessageBoxButtons.OK);
                    txtTenKho.Focus();
                    errorProviderKho.SetError(txtTenKho, "Tên kho này đã được sử dụng!");
                    return;
                }

                String strLenh = "DECLARE @return_value int " +
                                "EXEC @return_value = [dbo].[sp_TraCuu_NhanVien_Kho] '" +
                                txtTenKho.Text.Trim() + "', 'TENKHO' " +
                                "SELECT 'Return Value' = @return_value";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return;

                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    //XtraMessageBox.Show("Tên kho này đã được sử dụng ở chi nhánh khác!", "Thông báo", MessageBoxButtons.OK);
                    txtTenKho.Focus();
                    errorProviderKho.SetError(txtTenKho, "Tên kho này đã được sử dụng ở chi nhánh khác!");
                }
            }
        }

        private void txtDiaChi_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDiaChi.Text))
            {
                e.Cancel = true;
                txtDiaChi.Focus();
                errorProviderKho.SetError(txtDiaChi, "Địa chỉ không được để trống!");
            }
        }

        private void txtTenKho_KeyPress(object sender, KeyPressEventArgs e)
        {
            String maKho = txtMaKho.Text.Trim();// Trim() de loai bo khoang trang thua
            DataRowView drv = ((DataRowView)bdsKho[bdsKho.Position]);
            String tenKho = drv["TENKHO"].ToString();
            Console.WriteLine(tenKho);
            if (Char.IsLetter(e.KeyChar) || e.KeyChar == '\b' || char.IsDigit(e.KeyChar))
                e.Handled = false;
            else if (e.KeyChar == ' ')
            {
                if (txtTenKho.Text[txtTenKho.Text.Length - 1] == ' ')
                    e.Handled = true;
                else
                    e.Handled = false;
            }
            else
                e.Handled = true;
        }
    }
}
