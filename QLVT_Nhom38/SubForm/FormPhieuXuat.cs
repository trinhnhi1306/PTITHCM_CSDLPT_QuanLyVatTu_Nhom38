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
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;

namespace QLVT_Nhom38.SubForm
{
    public partial class FormPhieuXuat : Form
    {
        bool cheDoThem = true; //1: Thêm ở phiếu xuất, 0: Thêm ở CTPX
        int checkThem = 0;
        int position = 0; //Vị trí trên grid view
        int cheDo = 1; //1: thao tác trên phiếu xuất, 2: thao tác trên chi tiết phiếu xuất
        Stack undoList = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        GroupControl info = null;

        public FormPhieuXuat()
        {
            InitializeComponent();
        }

        private void phieuXuatBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.phieuXuatBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.qLVTDataSet);

        }

        private void FormPhieuXuat_Load(object sender, EventArgs e)
        {
            qLVTDataSet.EnforceConstraints = false; //Không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.PhieuXuat' table. You can move, or remove it, as needed.
            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuXuatTableAdapter.Fill(this.qLVTDataSet.PhieuXuat);
            // TODO: This line of code loads data into the 'qLVTDataSet.CTPX' table. You can move, or remove it, as needed.
            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.qLVTDataSet.CTPX);
            // TODO: This line of code loads data into the 'qLVTDataSet.Kho' table. You can move, or remove it, as needed.
            this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.khoTableAdapter.Fill(this.qLVTDataSet.Kho);
            // TODO: This line of code loads data into the 'qLVTDataSet.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.qLVTDataSet.Vattu);
            // TODO: This line of code loads data into the 'qLVTDataSet.NhanVien' table. You can move, or remove it, as needed.
            this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
            this.nhanVienTableAdapter.Fill(this.qLVTDataSet.NhanVien);

            cmbChiNhanh.DataSource = Program.bds_dspm;  //Sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;

            Console.WriteLine("Phieu nhap");
            cheDo = 1;
            bds = phieuXuatBindingSource;
            gc = phieuXuatGridControl;
            info = groupControlPX;

            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = btnChuyenChiNhanh.Enabled = btnReload.Enabled = true;  //Bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;
            }
            else
            {
                cmbChiNhanh.Enabled = btnUndo.Enabled = btnGhi.Enabled = btnXoa.Enabled = false;
                btnThem.Enabled = btnReload.Enabled = true;
            }
        }

        private void reload()
        {
            try
            {
                this.phieuXuatTableAdapter.Fill(this.qLVTDataSet.PhieuXuat);
                this.cTPXTableAdapter.Fill(this.qLVTDataSet.CTPX);
                nhanVienBindingSource.Position = position;
                khoBindingSource.Position = position;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi Reload: " + ex.Message, "", MessageBoxButtons.OK);
                return;
            }
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            reload();
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Khi form load thì hàm này được gọi, mà bds chưa có dữ liệu nên sẽ gây lỗi
            //"System.Data.DataRowView" sẽ xuất hiện và tất nhiên hệ thống sẽ không thể nhận diện được tên server "System.Data.DataRowView".
            if (cmbChiNhanh.SelectedValue.ToString() == "System.Data.DataRowView") return;

            //Lấy tên server
            Program.serverName = cmbChiNhanh.SelectedValue.ToString();

            //Nếu tên server khác với tên server ngoài đăng nhập, thì ta phải sử dụng HTKN
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
            {
                XtraMessageBox.Show("Lỗi kết nối về chi nhánh mới", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuXuatTableAdapter.Fill(this.qLVTDataSet.PhieuXuat);

                this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPXTableAdapter.Fill(this.qLVTDataSet.CTPX);

                this.khoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.khoTableAdapter.Fill(this.qLVTDataSet.Kho);

                this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                this.vattuTableAdapter.Fill(this.qLVTDataSet.Vattu);

                this.nhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                this.nhanVienTableAdapter.Fill(this.qLVTDataSet.NhanVien);
                
                //maCN = ((DataRowView)bdsNV[0])["MACN"].ToString();
            }
        }

        private void cmbTenKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Khi form load thì hàm này được gọi, mà bds chưa có dữ liệu nên sẽ gây lỗi
            if (cmbTenKho.SelectedValue == null) return;

            mAKHOTextEdit.Text = cmbTenKho.SelectedValue.ToString();
        }

        private void cmbTenVatTu_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbVatTu.SelectedValue == null) return;

            mAVTTextEdit.Text = cmbVatTu.SelectedValue.ToString();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cheDo == 2)
            {
                if (mANVTextEdit.Text != Program.username)
                {
                    XtraMessageBox.Show("Không thể thêm chi tiết phiếu xuất trên phiếu người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            switchCheDo.Enabled = false;
            checkThem = 1; //Bật trạng thái đang thêm mới lên           
            position = bds.Position;
            info.Enabled = true;
            bds.AddNew(); //Thêm một record trống mới trong grid view

            //Đặt các giá trị mặc định
            if (cheDo == 1)
            {
                mAPXTextBox.Text = "";
                nGAYDateEdit.EditValue = DateTime.Now; //Ngày hiện hành
                nGAYDateEdit.Enabled = false;

                hOTENKHTextEdit.Text = "";
                mANVTextEdit.Text = Program.username;
                mANVTextEdit.Enabled = false;
                cheDoThem = true;
            }
            else if (cheDo == 2)
            {
                mAPXTextBoxCTPX.Text = mAPXTextBox.Text;
                sOLUONGNumericUpDown.Value = 0;
                dONGIATextEdit.EditValue = 0;
                cheDoThem = false;
            }

            //Vô hiệu hóa các nút chức năng
            btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gc.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            //Đang ở chế độ thêm mới hoặc sửa, chưa ấn Ghi, ấn Hoàn tác sẽ thoát chế độ Thêm /Sửa
            if (btnThem.Enabled == false)
            {
                bds.CancelEdit();
                if (checkThem == 1) // chế độ thêm
                {
                    bds.RemoveCurrent(); // xóa record đang thêm dở dang đi
                    checkThem = 0;
                }
                bds.Position = position;
                switchCheDo.Enabled = true;

                gc.Enabled = true;
                info.Enabled = false;

                btnThem.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
                btnGhi.Enabled = false;


                if (undoList.Count == 0)
                {
                    btnUndo.Enabled = false;
                }

                return;
            }

            //Danh sách undoList trống
            if (undoList.Count == 0)
            {
                XtraMessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnUndo.Enabled = false;
                return;
            }

            //Bắt đầu hoàn tác
            khoBindingSource.CancelEdit();
            String strLenhUndo = undoList.Pop().ToString();
            Console.WriteLine(strLenhUndo);

            if (Program.KetNoi() == 0)
                return;
            int n = Program.ExecSqlNonQuery(strLenhUndo);
            reload();
            bds.Position = position;

            // Nếu sau khi undo mà danh sách undoList trống thì disable nút undo đi
            if (undoList.Count == 0)
            {
                btnUndo.Enabled = false;
                return;
            }
        }

        private void switchCheDo_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (switchCheDo.Checked == true)
            {
                Console.WriteLine("Chi tiet phieu xuat");
                cheDo = 2;
                cTPXGridControl.Enabled = true; //Bật grid control CTPX
                phieuXuatGridControl.Enabled = false; //Tắt grid control PX
                bds = cTPXBindingSource;
                gc = cTPXGridControl;
                info = groupControlCTPX;
            }
            else
            {
                Console.WriteLine("Phieu xuat");
                cheDo = 1;
                phieuXuatGridControl.Enabled = true; //Bật grid control PX
                cTPXGridControl.Enabled = false; //Tắt grid control CTPX
                btnXoa.Enabled = false;
                bds = phieuXuatBindingSource;
                gc = phieuXuatGridControl;
                info = groupControlPX;
                if (cTPXBindingSource.Count == 0 && mANVTextEdit.Text == Program.username)
                {
                    DialogResult dr = XtraMessageBox.Show("Phiếu này chưa có chi tiết phiếu. Bạn có muốn xóa không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        int currentPosition = -1;
                        try
                        {
                            currentPosition = bds.Position; //Giữ lại vị trí grid để phòng trường hợp xóa lỗi
                            bds.RemoveCurrent(); //Xóa trên máy hiện tại trước

                            this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr; //Đường kết nối đã đăng nhập
                            this.phieuXuatTableAdapter.Update(this.qLVTDataSet.PhieuXuat); //Update xuống csdl

                            Console.WriteLine("Xóa phiếu thành công!");
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            this.phieuXuatTableAdapter.Fill(this.qLVTDataSet.PhieuXuat); //Trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu

                            //Lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                            bds.Position = currentPosition; //Sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo vị trí ban nãy muốn xóa

                            return;
                        }
                    }
                }
            }
        }

        private bool kiemTraCTPX()
        {
            //Console.WriteLine(vt);

            if (mAVTTextEdit.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn vật tư", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                String strLenh = "DECLARE @return_value int " +
                                "EXEC @return_value = [dbo].[sp_Kiem_Tra_CTDDH] '" +
                                mAPXTextBox.Text.Trim() + "', '" + mAVTTextEdit.Text.Trim() + "' " +
                                "SELECT 'Return Value' = @return_value";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return false;

                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    XtraMessageBox.Show("Vật tư này đã có trong phiếu xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (sOLUONGNumericUpDown.Value <= 0)
            {
                XtraMessageBox.Show("Số lượng phải lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            Console.WriteLine(dONGIATextEdit.EditValue.ToString());
            if (int.Parse(dONGIATextEdit.EditValue.ToString()) <= 0)
            {
                XtraMessageBox.Show("Đơn giá phải lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool kiemTraPX()
        {
            int vt = phieuXuatBindingSource.Find("MAPX", mAPXTextBox.Text);
            Console.WriteLine(vt);
            if (mAPXTextBox.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng điền mã phiếu xuất", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mAPXTextBox.Focus(); //Đặt con trỏ vào textbox MAPX
                return false;
            }
            else if (mAPXTextBox.Text.Length > 8)
            {
                XtraMessageBox.Show("Mã phiếu xuất không quá 8 kí tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                mAPXTextBox.Focus();
                return false;
            }
            else
            {
                String strLenh = "DECLARE @return_value int " +
                                "EXEC @return_value = [dbo].[sp_Kiem_Tra_MAPX] '" +
                                mAPXTextBox.Text.Trim() + "' " +
                                "SELECT 'Return Value' = @return_value";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return false;

                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    XtraMessageBox.Show("Mã phiếu xuất này đã được sử dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    mAPXTextBox.Focus();
                    return false;
                }
            }
            if (hOTENKHTextEdit.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng điền họ tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                hOTENKHTextEdit.Focus();
                return false;
            }
            if (mAKHOTextEdit.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cheDo == 1)
                if (!kiemTraPX()) return;
            if (cheDo == 2)
                if (!kiemTraCTPX()) return;

            String strLenhUndo = "";
            try
            {
                bds.EndEdit(); //Kết thúc quá trình hiệu chỉnh và ghi vào BindingSource
                bds.ResetCurrentItem(); //Đưa thông tin vào grid

                if (cheDo == 1)
                {
                    strLenhUndo = "DELETE DBO.PhieuXuat WHERE MAPX = '" + mAPXTextBox.Text.Trim() + "'";
                    this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr; //Đường kết nối đã đăng nhập
                    this.phieuXuatTableAdapter.Update(this.qLVTDataSet.PhieuXuat); //Update xuống csdl
                    Console.WriteLine("Thanh cong!");

                    DialogResult dr = XtraMessageBox.Show("Bạn có muốn lập chi tiết phiếu xuất không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (dr == DialogResult.OK)
                    {
                        switchCheDo.PerformClick();
                    }
                    else
                    {
                        DialogResult dr1 = XtraMessageBox.Show("Phiếu này chưa có chi tiết phiếu. Bạn có muốn xóa không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                        if (dr1 == DialogResult.OK)
                        {
                            String strLenhUndo1 = "";

                            DateTime ngay = (DateTime)((DataRowView)bds[bds.Position])["NGAY"];
                            strLenhUndo1 = string.Format("INSERT INTO DBO.PhieuXuat (MAPX, NGAY, HOTENKH, MANV, MAKHO) " +
                                "VALUES ('{0}', CAST('{1}' AS DATETIME), N'{2}', {3}, '{4}')", mAPXTextBox.Text, ngay.ToString("yyyy-MM-dd"),
                                hOTENKHTextEdit.Text, mANVTextEdit.Text, mAKHOTextEdit.Text);

                            int currentPosition = -1;
                            try
                            {
                                currentPosition = bds.Position; //Giữ lại vị trí grid để phòng trường hợp xóa lỗi
                                bds.RemoveCurrent(); //Xóa trên máy hiện tại trước

                                this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr; //Đường kết nối đã đăng nhập
                                this.phieuXuatTableAdapter.Update(this.qLVTDataSet.PhieuXuat); //Update xuống csdl

                                this.btnUndo.Enabled = true;
                                undoList.Push(strLenhUndo1);
                                XtraMessageBox.Show("Xóa thành công!", "Thông báo",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception ex)
                            {
                                XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                                if (cheDo == 1)
                                    this.phieuXuatTableAdapter.Fill(this.qLVTDataSet.PhieuXuat); //Trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                                if (cheDo == 1)
                                    this.cTPXTableAdapter.Fill(this.qLVTDataSet.CTPX);
                                //Lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                                bds.Position = currentPosition; //Sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo vị trí ban nãy muốn xóa

                                return;
                            }
                        }
                    }
                }
                else
                {
                    //Giảm số lượng vật tư
                    String str = "EXEC [dbo].sp_Giam_SL_VT '" +
                                    mAVTTextEdit.Text.Trim() + "', " +
                                    sOLUONGNumericUpDown.Text.Trim();
                    Console.WriteLine(str);
                    if (Program.KetNoi() == 0)
                        return;
                    int n = Program.ExecSqlNonQuery(str);

                    strLenhUndo = "DELETE DBO.CTPX WHERE MAPX = '" + mAPXTextBox.Text.Trim() + "' AND MAVT = '" + mAVTTextEdit.Text.Trim() + "' UPDATE DBO.Vattu SET SOLUONGTON = SOLUONGTON + '" + sOLUONGNumericUpDown.Text.Trim() + "' WHERE MAVT = '" + mAVTTextEdit.Text.Trim() + "'";
                    this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr; //Đường kết nối đã đăng nhập
                    this.cTPXTableAdapter.Update(this.qLVTDataSet.CTPX); //Update xuống csdl
                }

                if (cheDoThem == true)
                {
                    btnXoa.Enabled = btnUndo.Enabled = false;
                }
                else
                {
                    btnXoa.Enabled = btnUndo.Enabled = true;
                }

                Console.WriteLine("Ghi thông tin thành công!");
                undoList.Push(strLenhUndo);
                checkThem = 0;
                switchCheDo.Enabled = true;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi xảy ra trong quá trình ghi. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Khi Update database lỗi thì xóa record vừa thêm trong bds
                //bdsNV.RemoveCurrent();
                reload();
            }

            gc.Enabled = true;
            info.Enabled = false;

            btnThem.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = false;
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (bds.Count == 0)
                XtraMessageBox.Show("Danh sách trống!", "Thông báo lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

            String strLenhUndo = "";
            //if (cheDo == 1)
            //{
            //    if (mANVTextEdit.Text != Program.username)
            //    {
            //        XtraMessageBox.Show("Không thể xóa phiếu xuất người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    if (cTPXBindingSource.Count > 0)
            //    {
            //        XtraMessageBox.Show("Không thể xóa vì phiếu xuất đã được lập chi tiết phiếu xuất", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //        return;
            //    }
            //    DateTime ngay = (DateTime)((DataRowView)bds[bds.Position])["NGAY"];
            //    strLenhUndo = string.Format("INSERT INTO DBO.PhieuXuat (MAPX, NGAY, HOTENKH, MANV, MAKHO) " +
            //        "VALUES ('{0}', CAST('{1}' AS DATETIME), N'{2}', {3}, '{4}')", mAPXTextBox.Text, ngay.ToString("yyyy-MM-dd"),
            //        hOTENKHTextEdit.Text, mANVTextEdit.Text, mAKHOTextEdit.Text);
            //}
            if (cheDo == 2)
            {
                //if (mANVTextEdit.Text != Program.username)
                //{
                //    XtraMessageBox.Show("Không thể xóa chi tiết phiếu xuất trên phiếu người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //    return;
                //}

                strLenhUndo = string.Format("INSERT INTO DBO.CTPX (MAPX, MAVT, SOLUONG, DONGIA) " +
                    "VALUES ('{0}', '{1}', {2}, {3})", mAPXTextBoxCTPX.Text, mAVTTextEdit.Text, sOLUONGNumericUpDown.Value, int.Parse(dONGIATextEdit.Text)) + "UPDATE DBO.Vattu SET SOLUONGTON = SOLUONGTON - '" + sOLUONGNumericUpDown.Text.Trim() + "' WHERE MAVT = '" + mAVTTextEdit.Text.Trim() + "'"; ;
            }

            DialogResult dr = XtraMessageBox.Show("Bạn có thực sự muốn xóa không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                int currentPosition = -1;
                try
                {
                    currentPosition = bds.Position; //Giữ lại vị trí grid để phòng trường hợp xóa lỗi
                    bds.RemoveCurrent(); //Xóa trên máy hiện tại trước
                    if (cheDo == 1)
                    {
                        this.phieuXuatTableAdapter.Connection.ConnectionString = Program.connstr; //Đường kết nối đã đăng nhập
                        this.phieuXuatTableAdapter.Update(this.qLVTDataSet.PhieuXuat); //Update xuống csdl
                    }
                    else
                    {
                        this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr; //Đường kết nối đã đăng nhập
                        this.cTPXTableAdapter.Update(this.qLVTDataSet.CTPX); //Update xuống csdl

                        //Tăng số lượng vật tư
                        String str = "EXEC [dbo].sp_Tang_SL_VT '" +
                                        mAVTTextEdit.Text.Trim() + "', " +
                                        sOLUONGNumericUpDown.Text.Trim();
                        Console.WriteLine(str);
                        if (Program.KetNoi() == 0)
                            return;
                        int n = Program.ExecSqlNonQuery(str);
                    }
                    this.btnUndo.Enabled = true;
                    undoList.Push(strLenhUndo);
                    XtraMessageBox.Show("Xóa thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (cheDo == 1)
                        this.phieuXuatTableAdapter.Fill(this.qLVTDataSet.PhieuXuat); //Trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                    if (cheDo == 1)
                        this.cTPXTableAdapter.Fill(this.qLVTDataSet.CTPX);
                    //Lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                    bds.Position = currentPosition; //Sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo vị trí ban nãy muốn xóa

                    return;
                }
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (info.Enabled)
            {
                if (XtraMessageBox.Show("Dữ liệu vẫn chưa lưu vào Database!\nBạn có chắn chắn muốn thoát?", "Thông báo",
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
    }
}
