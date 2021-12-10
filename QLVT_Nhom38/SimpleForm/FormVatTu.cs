using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;

namespace QLVT_Nhom38.SimpleForm
{
    /* chú thích biến
     * --- biến checkThem: 
     * + kiểm tra cập nhật hay thêm mới ở btnGhi
     * + validate tên vật tư và mã vật tư tránh bị trùng ( trường hợp thêm mới kiểm tra bị trùng) 
     * --- viTriList: 
     * + lưu lại vị trí thao tác để khi undo sẽ tự động chọn dòng thao tác trước
     * --- undoList:
     * + lưu lại những câu lệnh hoàn tác 
     * --- viTri: 
     * + lưu lại vị trí xóa nếu xảy ra vấn đề nếu tí nữa chạy lỗi
     */
    public partial class FormVatTu : Form
    {
        
        int checkThem = 0;
        /* vị trí của con trỏ trên grid view*/
        int viTri = 0;
       
        int position = 0; // vị trí trên grid view
        // lưu vị trí và lệnh hoàn tác
        Stack viTriList = new Stack();
        Stack undoList = new Stack();
        public FormVatTu()
        {
            InitializeComponent();
        }

        private void vattuBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsVatTu.EndEdit();
            this.tableAdapterManager.UpdateAll(this.qLVTDataSet);

        }

        private void FormVatTu_Load(object sender, EventArgs e)
        {
            qLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.Vattu' table. You can move, or remove it, as needed.
            this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.vattuTableAdapter.Fill(this.qLVTDataSet.Vattu);

            // TODO: This line of code loads data into the 'qLVTDataSet.CTDDH' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.qLVTDataSet.CTDDH);
            // TODO: This line of code loads data into the 'qLVTDataSet.CTPN' table. You can move, or remove it, as needed.
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);

            // TODO: This line of code loads data into the 'qLVTDataSet.CTPX' table. You can move, or remove it, as needed.
            this.cTPXTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPXTableAdapter.Fill(this.qLVTDataSet.CTPX);

            
            if (Program.mGroup == "CONGTY")
            {

                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnGhi.Enabled = btnUndo.Enabled  = false;
            }
            else
            {
                gcInfoVatTu.Enabled = btnUndo.Enabled = btnGhi.Enabled = false;
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = true;
            }

            

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void mAVTLabel_Click(object sender, EventArgs e)
        {

        }

        private void sOLUONGTONLabel_Click(object sender, EventArgs e)
        {

        }

        private void mAVTLabel_Click_1(object sender, EventArgs e)
        {

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
  
            checkThem = 1; // bật trạng thái đang thêm mới lên
            
            bdsVatTu.AddNew(); // thêm một record trống mới trong grid view

            position = bdsVatTu.Position; // lấy vị trị hiện tại của vật tư
            txtMaVT.Enabled = true;
 
            // vô hiệu hóa một số nút cho đúng logic
            btnSua.Enabled = btnThem.Enabled = btnXoa.Enabled = gridVatTu.Enabled = btnReload.Enabled =  false;
            btnGhi.Enabled = gcInfoVatTu.Enabled = btnUndo.Enabled = true;
            txtSLT.Value = 0;

            
        }
        /* tóm tắt nút ghi
         * - kiểm tra xem vật tư đã được sử dụng ở các phiếu hay chưa
         * - validating đã kiểm tra tính đúng sai về dữ liệu
         * - chạy thành công :
         * + chuẩn bị câu lệnh hoàn tác nếu chạy thành công ( thêm vị trí hoàn tác)
         * - chạy thất bại : 
         * + thông báo thôi bro
         */
        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            DataRowView drv = ((DataRowView)bdsVatTu[bdsVatTu.Position]);
            String maVatTu = drv["MAVT"].ToString();
            String tenVatTuCu = drv["TENVT"].ToString();
            String donViTinhCu = drv["DVT"].ToString();
            String soLuongTonCu = (drv["SOLUONGTON"].ToString());
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                // câu lệnh kiểm tra vật tư đã được sử dụng hay chưa
                String strLenh = "DECLARE @return_value int " +
                               "EXEC @return_value = [dbo].[sp_Kiem_Tra_TT_VatTu] " +
                               "'" + txtMaVT.Text.Trim() + "' " +
                               "SELECT 'Return Value' = @return_value";

                
                try
                {
                    Program.myReader = Program.ExecSqlDataReader(strLenh);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Thực thi database thất bại!\n" + ex.Message, "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (Program.myReader == null) return;
                /*1: đã được sử dụng ở chi nhánh khác
                 * 0: chưa sử dụng
                 */
                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();
                if (result == 1)
                {
                    MessageBox.Show("Vật tư đang được sử dụng ở chi nhánh khác!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào Database không?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (dr == DialogResult.OK)
                {
                    try
                    {

                        /*lưu 1 câu truy vấn để hoàn tác yêu cầu*/
                        String cauTruyVanHoanTac = "";
                        /*trước khi ấn btnGHI là btnTHEM*/
                        if (checkThem == 1)
                        {
                            cauTruyVanHoanTac = "" +
                                "DELETE DBO.VATTU " +
                                "WHERE MAVT = '" + txtMaVT.Text.Trim() + "'";
                        }
                        /*trước khi ấn btnGHI là sửa thông tin nhân viên*/
                        else
                        {
                            // lấy dữ liệu phục vụ hoàn tác
                            
                            cauTruyVanHoanTac =
                                "UPDATE DBO.VATTU " +
                                "SET " +
                                "TENVT = '" + tenVatTuCu + "'," +
                                "DVT = '" + donViTinhCu + "'," +
                                "SOLUONGTON = " + soLuongTonCu + " " +
                                "WHERE MAVT = '" + maVatTu + "'";
                        }

                        /*Đưa câu truy vấn hoàn tác vào undoList*/
                        undoList.Push(cauTruyVanHoanTac);
                        viTriList.Push(bdsVatTu.Position +"");
                        Console.WriteLine("btnThem line 176: " + bdsVatTu.Position);
                        this.bdsVatTu.EndEdit(); // kết thúc edit
                        this.bdsVatTu.ResetCurrentItem(); // đưa thông tin vào grid
                        this.vattuTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                        // sử dụng vattuTableAdapter cập nhật thông tin với dữ liệu ở qLVTDataSet
                        this.vattuTableAdapter.Update(this.qLVTDataSet.Vattu); 

                        // cập nhật trạng thái thêm mới
                        checkThem = 0;
                        btnThoat.Enabled = btnUndo.Enabled =btnThem.Enabled = btnXoa.Enabled = gridVatTu.Enabled = btnReload.Enabled = btnSua.Enabled = gcInfoVatTu.Enabled = true;
                        btnGhi.Enabled = false;
                        
                        
                        bdsVatTu.Position = position;
                        this.txtMaVT.Enabled = false;
                        this.gcInfoVatTu.Enabled = false;
                        
                      
                        MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);

                    }
                    catch (Exception ex)
                    {
                        // Khi Update database lỗi thì xóa record vừa thêm trong bds
                        bdsVatTu.RemoveCurrent();
                        MessageBox.Show("Thất bại. Vui lòng kiểm tra lại!\n" + ex.Message, "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            else
            {
                XtraMessageBox.Show("Lỗi xảy ra trong quá trình ghi vật tư. Vui lòng thử lại!\n" , "Thông báo lỗi",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void txtMaVT_Validating(object sender, CancelEventArgs e)
        {


            
            int viTriMaVatTu = bdsVatTu.Find("MAVT", txtMaVT.Text);

            if (viTriMaVatTu != -1 && checkThem == 1)
            {
                errorProviderVT.SetError(txtMaVT, "Mã vật tư này đã được sử dụng !");
                MessageBox.Show("Mã vật tư này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);
                txtMaVT.Focus();
            }
            var regexItem = new Regex("^[a-zA-Z0-9 ]*$");
            if (txtMaVT.Text.Length > 4)
            {
                e.Cancel = true;
                txtMaVT.Focus();
                errorProviderVT.SetError(txtMaVT, "Mã vật tư không được quá 4 kí tự!");

            }
            if (string.IsNullOrWhiteSpace(txtMaVT.Text))
            {
                e.Cancel = true;
                txtMaVT.Focus();
                errorProviderVT.SetError(txtMaVT, "Mã vật tư không được để trống!");
            }
            else if (txtMaVT.Text.Trim().Contains(" "))
            {
                e.Cancel = true;
                txtMaVT.Focus();
                errorProviderVT.SetError(txtMaVT, "Mã vật tư không được chứa khoảng trắng!");
            }
            else if (!regexItem.IsMatch(txtMaVT.Text.Trim()))
            {
                e.Cancel = true;
                txtMaVT.Focus();
                errorProviderVT.SetError(txtMaVT, "Mã vật tư không được chứa ký tự đặc biệt!");
            }
            else
            {
                e.Cancel = false;
                errorProviderVT.SetError(txtMaVT, null);
            }
        }

        private void txtDVT_Validating(object sender, CancelEventArgs e)
        {
            //var regexItem = new Regex("^[~!@#$%^&*()_+>?<:';0-9]*$");
            var regexItem = new Regex("^\\p{L}+$");
            if (string.IsNullOrWhiteSpace(txtDVT.Text))
            {
                e.Cancel = true;
                txtDVT.Focus();
                errorProviderVT.SetError(txtDVT, "Đơn vị tính không được để trống!");
            }
            else if (!regexItem.IsMatch(txtDVT.Text.Trim()))
            {
                e.Cancel = true;
                txtDVT.Focus();
                errorProviderVT.SetError(txtDVT, "Đơn vị tính không được chứa ký tự đặc biệt!");
            }
            else
            {
                e.Cancel = false;
                errorProviderVT.SetError(txtDVT, null);
            }
        }

        private void txtTenVT_Validating(object sender, CancelEventArgs e)
        {
            
            int viTriTENVT = bdsVatTu.Find("TENVT", txtMaVT.Text);
            if (viTriTENVT != -1 && checkThem == 1)
            {
                
                MessageBox.Show("Tên vật tư này đã được sử dụng !", "Thông báo", MessageBoxButtons.OK);

                txtMaVT.Focus();
            }
            
            if (txtTenVT.Text.Length > 30)
            {
                e.Cancel = true;
                txtTenVT.Focus();
                errorProviderVT.SetError(txtTenVT, "Đơn vị tính không được chứa ký tự đặc biệt!");
            }else if (string.IsNullOrWhiteSpace(txtTenVT.Text))
            {
                e.Cancel = true;
                txtTenVT.Focus();
                errorProviderVT.SetError(txtTenVT, "Tên vật tư không được để trống!");
            }
            else
            {
                e.Cancel = false;
                errorProviderVT.SetError(txtTenVT, null);
            }
        }

        private void txtSLT_Validating(object sender, CancelEventArgs e)
        {
            if (txtSLT.Value == 0)
            {
                e.Cancel = true;
                txtSLT.Focus();
                errorProviderVT.SetError(txtSLT, "Vui lòng chọn số lượng tồn!");
            }
            else
            {
                e.Cancel = false;
                errorProviderVT.SetError(txtSLT, "");
            }
        }

        /* tóm tắt nút xóa
         * - kiểm tra xem vật tư đã được sử dụng ở các phiếu hay chưa
         * + chi nhánh hiện tại thì dùng bds để kiểm tra
         * + chi nhánh khác dùng sp kiểm tra
         * - chạy thành công :
         * + chuẩn bị câu lệnh hoàn tác nếu chạy thành công ( thêm vị trí hoàn tác)
         * - chạy thất bại : 
         * + thông báo thôi bro
         */
        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // kiểm tra thông tin vật tư có ở phiếu hay chưa
            /*
             * có : không cho xóa
             * không : xóa
             */
             // xóa hết vô hiệu hóa nút xóa
            if (bdsVatTu.Count == 0)
            {
                btnXoa.Enabled = false;
            }
            // đã lập đơn đặt hàng
            if (bdsCTDDH.Count > 0)
            {
                MessageBox.Show("Không thể xóa vật tư này vì đã lập đơn đặt hàng", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            // đã có trong phiếu nhập
            if (bdsCTPN.Count > 0)
            {
                MessageBox.Show("Không thể xóa vật tư này vì đã lập phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            // đã có ở phiếu xuất
            if (bdsCTPX.Count > 0)
            {
                MessageBox.Show("Không thể xóa vật tư này vì đã lập phiếu xuất", "Thông báo", MessageBoxButtons.OK);
                return;
            }

            // kiểm tra đã xài ở chi nhánh khác chưa
            String strLenh = "DECLARE @return_value int " +
                               "EXEC @return_value = [dbo].[sp_Kiem_Tra_TT_VatTu] " +
                               "'" + txtMaVT.Text.Trim() + "' " +
                               "SELECT 'Return Value' = @return_value";

            try
            {
                Program.myReader = Program.ExecSqlDataReader(strLenh);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Thực thi database thất bại!\n" + ex.Message, "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Program.myReader == null) return;
            /*1: đã được sử dụng ở chi nhánh khác
             * 0: chưa sử dụng
             */
            Program.myReader.Read();
            int result = int.Parse(Program.myReader.GetValue(0).ToString());
            Program.myReader.Close();
            if (result == 1)
            {
                MessageBox.Show("Vật tư đang được sử dụng ở chi nhánh khác!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // câu truy vấn hoàn tác
            string cauTruyVanHoanTac =
            "INSERT INTO DBO.VATTU( MAVT,TENVT,DVT,SOLUONGTON) " +
            " VALUES( '" + txtMaVT.Text.Trim() + "','" +
                        txtTenVT.Text.Trim() + "','" +
                        txtDVT.Text.Trim() + "', " +
                        txtSLT.Value + " ) ";
          
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    // lưu lại vị trí xóa nếu xảy ra vấn đề nếu tí nữa chạy lỗi
                    viTri = bdsVatTu.Position;
                    // xóa trên view
                    bdsVatTu.RemoveCurrent();
                    // bước cập nhật thông tin
                    this.vattuTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.vattuTableAdapter.Update(this.qLVTDataSet.Vattu);
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnUndo.Enabled = true;
                    // lưu vị trí và câu lệnh hoàn tác
                    undoList.Push(cauTruyVanHoanTac);
                    viTriList.Push(bdsVatTu.Position+"");
                    Console.WriteLine("line 414:" +bdsVatTu.Position);

                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Lỗi xóa vật tư. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.vattuTableAdapter.Fill(this.qLVTDataSet.Vattu);
                    // trở về vị trí nãy vừa bị lỗi
                    bdsVatTu.Position = viTri;
                    return;
                }
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();
        }

        private void gridVatTu_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int vitriCu = 0;
            
            if ((checkThem == 1 && this.btnThem.Enabled == false)||(checkThem == 2 && this.btnThem.Enabled == false))
            {
                checkThem = 0;

                this.txtMaVT.Enabled = false;
                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = false;

                this.btnUndo.Enabled = true;
                this.btnThem.Enabled = true;
                this.btnThoat.Enabled = true;
                this.btnSua.Enabled = true;
                this.btnReload.Enabled = true;

                this.gridVatTu.Enabled = true;
                this.gcInfoVatTu.Enabled = false;

                bdsVatTu.CancelEdit();
                errorProviderVT.SetError(txtDVT, null);
                errorProviderVT.SetError(txtMaVT, null);
                errorProviderVT.SetError(txtSLT, null);
                errorProviderVT.SetError(txtTenVT, null);

                return;
            }


           
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnUndo.Enabled = false;
                return;
            }

    
            bdsVatTu.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();
           
            Program.ExecSqlNonQuery(cauTruyVanHoanTac);
            
            this.vattuTableAdapter.Fill(this.qLVTDataSet.Vattu);
            bdsVatTu.Position = int.Parse(viTriList.Pop().ToString());
            Console.WriteLine("line 486:" + bdsVatTu.Position);
        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            checkThem = 2;

            position = bdsVatTu.Position;
            gridVatTu.Enabled = txtMaVT.Enabled = false;
            gcInfoVatTu.Enabled = true;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.vattuTableAdapter.Fill(this.qLVTDataSet.Vattu);
                this.gcInfoVatTu.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Làm mới" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                return;
            }
        }

        private void txtTenVT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar) || e.KeyChar == '\b')
                e.Handled = false;
            else if (e.KeyChar == ' ')
            {
                if (txtTenVT.Text[txtTenVT.Text.Length - 1] == ' ')
                    e.Handled = true;
                else
                    e.Handled = false;
            }
            else
                e.Handled = true;
        }
    }
}
