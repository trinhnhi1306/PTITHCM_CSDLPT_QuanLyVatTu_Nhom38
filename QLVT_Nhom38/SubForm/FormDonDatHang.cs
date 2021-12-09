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
    public partial class FormDonDatHang : Form
    {
        int checkThem = 0;
        int position = 0; // vị trí trên grid view
        int cheDo = 1; // 1: thao tác trên đơn hàng, 2: thao tác trên chi tiết đơn hàng
        Stack undoList = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        GroupControl info = null;

        public FormDonDatHang()
        {
            InitializeComponent();
        }

        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsDDH.EndEdit();
            this.tableAdapterManager.UpdateAll(this.QLVTDataSet);

        }

        private void FormDonDatHang_Load(object sender, EventArgs e)
        {
            QLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'QLVTDataSet.Vattu' table. You can move, or remove it, as needed.
            this.VatTuTableAdapter.Connection.ConnectionString = Program.connstr;
            this.VatTuTableAdapter.Fill(this.QLVTDataSet.Vattu);

            // TODO: This line of code loads data into the 'QLVTDataSet.Kho' table. You can move, or remove it, as needed.
            this.KhoTableAdapter.Connection.ConnectionString = Program.connstr;
            this.KhoTableAdapter.Fill(this.QLVTDataSet.Kho);
            
            // TODO: This line of code loads data into the 'qLVTDataSet.DatHang' table. You can move, or remove it, as needed.
            this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);

            // TODO: This line of code loads data into the 'QLVTDataSet.CTDDH' table. You can move, or remove it, as needed.
            this.CTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.CTDDHTableAdapter.Fill(this.QLVTDataSet.CTDDH);

            // TODO: This line of code loads data into the 'QLVTDataSet.PhieuNhap' table. You can move, or remove it, as needed.
            this.PhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.PhieuNhapTableAdapter.Fill(this.QLVTDataSet.PhieuNhap);

            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;

            Console.WriteLine("Don dat hang");
            cheDo = 1;
            bds = bdsDDH;
            gc = gcDDH;
            info = infoDDH;

            if (Program.mGroup == "CONGTY")
            {
                cmbChiNhanh.Enabled = btnChuyenChiNhanh.Enabled = btnReload.Enabled = true;  // bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnUndo.Enabled = false;

            }
            else
            {
                cmbChiNhanh.Enabled = btnUndo.Enabled = btnGhi.Enabled = false;
                btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = true;
            }

        }

        private void reload()
        {
            try
            {
                this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);
                this.CTDDHTableAdapter.Fill(this.QLVTDataSet.CTDDH);
                bdsKho.Position = position;
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
                this.VatTuTableAdapter.Connection.ConnectionString = Program.connstr;
                this.VatTuTableAdapter.Fill(this.QLVTDataSet.Vattu);
                this.KhoTableAdapter.Connection.ConnectionString = Program.connstr;
                this.KhoTableAdapter.Fill(this.QLVTDataSet.Kho);
                this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang);
                this.CTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                this.CTDDHTableAdapter.Fill(this.QLVTDataSet.CTDDH);
                this.PhieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.PhieuNhapTableAdapter.Fill(this.QLVTDataSet.PhieuNhap);
                
                //maCN = ((DataRowView)bdsNV[0])["MACN"].ToString();
            }
        }

        private void cmbTenKho_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi form load thì hàm này được gọi, mà bds chưa có dữ liệu nên sẽ gây lỗi
            if (cmbTenKho.SelectedValue == null) return;

            txtMaKho.Text = cmbTenKho.SelectedValue.ToString();
        }

        private void cmbVatTu_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Khi form load thì hàm này được gọi, mà bds chưa có dữ liệu nên sẽ gây lỗi
            if (cmbVatTu.SelectedValue == null) return;

            txtMaVT.Text = cmbVatTu.SelectedValue.ToString();
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cheDo == 2)
            {
                if (txtMaNV.Text != Program.username)
                {
                    XtraMessageBox.Show("Không thể thêm chi tiết đơn hàng trên phiếu người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (bdsPN.Count > 0)
                {
                    XtraMessageBox.Show("Không thể sửa vì đơn hàng đã được lập phiếu nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            switchCheDo.Enabled = false;
            checkThem = 1; // bật trạng thái đang thêm mới lên           
            position = bds.Position;
            info.Enabled = true;
            bds.AddNew(); // thêm một record trống mới trong grid view

            // đặt các giá trị mặc định
            if (cheDo == 1)
            {
                txtMaSoDDH.Text = "";
                dteNgay.EditValue = DateTime.Now; // ngày hiện hành
                dteNgay.Enabled = false;

                txtNhaCC.Text = "";
                txtMaNV.Text = Program.username;
                txtMaNV.Enabled = false;
            }
            else if (cheDo == 2)
            {             
                txtMaDDHCuaCTDDH.Text = txtMaSoDDH.Text;
                txtSoLuong.Value = 0;
                txtDonGia.Value = 0;
            }

            // vô hiệu hóa các nút chức năng
            btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gc.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Đang ở chế độ thêm mới hoặc sửa, chưa ấn Ghi, ấn Hoàn tác sẽ thoát chế độ Thêm /Sửa
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

                btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
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
            if(switchCheDo.Checked == true)
            {
                Console.WriteLine("Chi tiet don dat hang");
                cheDo = 2;
                gcCTDDH.Enabled = true; // bật grid control CTDDH
                gcDDH.Enabled = false; // tắt grid control DDH
                bds = bdsCTDDH;
                gc = gcCTDDH;
                info = infoCTDDH;
            }  
            else
            {
                Console.WriteLine("Don dat hang");
                cheDo = 1;
                gcDDH.Enabled = true; // bật grid control DDH
                gcCTDDH.Enabled = false; // tắt grid control CTDDH
                bds = bdsDDH;
                gc = gcDDH;
                info = infoDDH;
                if (bdsCTDDH.Count == 0 && txtMaNV.Text == Program.username)
                {
                    XtraMessageBox.Show("Phiếu này chưa có chi tiết phiếu nên sẽ bị xóa!", "Xác nhận", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    //if (dr == DialogResult.OK)
                    //{
                    int currentPosition = -1;
                        try
                        {
                            currentPosition = bds.Position; // giữ lại vị trí grid để phòng trường hợp xóa lỗi
                            bds.RemoveCurrent(); // xóa trên máy hiện tại trước
                            
                            this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                            this.DatHangTableAdapter.Update(this.QLVTDataSet.DatHang); // update xuống csdl
                            
                            Console.WriteLine("Xóa phiếu thành công!");
                        }
                        catch (Exception ex)
                        {
                            XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                           
                            this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang); // trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                            
                            // lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                            bds.Position = currentPosition; // sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo vị trí ban nãy muốn xóa

                            return;
                        }
                    //}
                }
            }    
        }

        private bool kiemTraCTDDH()
        {
          
           
           
            
            //Console.WriteLine(vt);

            if (txtMaVT.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn vật tư", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                String strLenh = "DECLARE @return_value int " +
                                "EXEC @return_value = [dbo].[sp_Kiem_Tra_CTDDH] '" +
                                txtMaSoDDH.Text.Trim() + "', '" + txtMaVT.Text.Trim() + "' " +
                                "SELECT 'Return Value' = @return_value";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return false;

                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    XtraMessageBox.Show("Vật tư này đã có trong đơn đặt hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
            }
            if (txtSoLuong.Value <= 0)
            {
                XtraMessageBox.Show("Số lượng phải lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (txtDonGia.Value <= 0)
            {
                XtraMessageBox.Show("Đơn giá phải lớn hơn 0", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private bool kiemTraDDH()
        {
            int vt = bdsDDH.Find("MasoDDH", txtMaSoDDH.Text);
            Console.WriteLine(vt);
            if (txtMaSoDDH.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng điền mã đơn đặt hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSoDDH.Focus();
                return false;
            }
            else if (txtMaSoDDH.Text.Length > 8)
            {
                XtraMessageBox.Show("Mã đơn đặt hàng không quá 8 kí tự", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtMaSoDDH.Focus();
                return false;
            }
            else
            {
                String strLenh = "DECLARE @return_value int " +
                                "EXEC @return_value = [dbo].[sp_Kiem_Tra_MasoDDH] '" +
                                txtMaSoDDH.Text.Trim() + "' " +
                                "SELECT 'Return Value' = @return_value";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return false;

                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    XtraMessageBox.Show("Mã đơn hàng này đã được sử dụng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtMaSoDDH.Focus();
                    return false;
                }                
            }
            if (txtNhaCC.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng điền nhà cung cấp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNhaCC.Focus();
                return false;
            }
            if (txtMaKho.Text.Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (cheDo == 1)
                if (!kiemTraDDH()) return;
            if (cheDo == 2)
                if (!kiemTraCTDDH()) return;

            String strLenhUndo = "";
            try
            {
                bds.EndEdit(); // kết thúc quá trình hiệu chỉnh và ghi vào BindingSource
                bds.ResetCurrentItem(); // đưa thông tin vào grid
                if (cheDo == 1)
                {
                    strLenhUndo = "DELETE DBO.DatHang WHERE MasoDDH = '" + txtMaSoDDH.Text.Trim() + "'";
                    this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                    this.DatHangTableAdapter.Update(this.QLVTDataSet.DatHang); // update xuống csdl
                }
                else
                {
                    strLenhUndo = "DELETE DBO.CTDDH WHERE MasoDDH = '" + txtMaSoDDH.Text.Trim() + "' AND MAVT = '" + txtMaVT.Text.Trim() + "'";
                    this.CTDDHTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                    this.CTDDHTableAdapter.Update(this.QLVTDataSet.CTDDH); // update xuống csdl   
                }

                Console.WriteLine("Ghi thông tin thành công!");
                btnUndo.Enabled = true;
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

            btnThem.Enabled = btnXoa.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = false;
            if (cheDo == 1)
            {
                switchCheDo.Checked = true;
                bds = bdsCTDDH;
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(bds.Count == 0)
                XtraMessageBox.Show("Danh sách trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            String strLenhUndo = "";
            if (cheDo == 1)
            {
                if (txtMaNV.Text != Program.username)
                {
                    XtraMessageBox.Show("Không thể xóa đơn hàng người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (bdsCTDDH.Count > 0)
                {
                    XtraMessageBox.Show("Không thể xóa vì đơn hàng đã được lập chi tiết đơn đặt hàng", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (bdsPN.Count > 0)
                {
                    XtraMessageBox.Show("Không thể xóa vì đơn hàng đã được lập phiếu nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DateTime ngay = (DateTime)((DataRowView)bds[bds.Position])["NGAY"];
                strLenhUndo = string.Format("INSERT INTO DBO.DatHang (MasoDDH, NGAY, NhaCC, MANV, MAKHO) " +
                    "VALUES ('{0}', CAST('{1}' AS DATETIME), N'{2}', {3}, '{4}')", txtMaSoDDH.Text, ngay.ToString("yyyy-MM-dd"),
                    txtNhaCC.Text, txtMaNV.Text, txtMaKho.Text);
            }
            if (cheDo == 2)
            {
                if (txtMaNV.Text != Program.username)
                {
                    XtraMessageBox.Show("Không thể xóa chi tiết đơn hàng trên phiếu người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (bdsPN.Count > 0)
                {
                    XtraMessageBox.Show("Không thể sửa vì đơn hàng đã được lập phiếu nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                strLenhUndo = string.Format("INSERT INTO DBO.CTDDH (MasoDDH, MAVT, SOLUONG, DONGIA) " +
                    "VALUES ('{0}', '{1}', {2}, {3})", txtMaDDHCuaCTDDH.Text, txtMaVT.Text, txtSoLuong.Value, txtDonGia.Value);
            }

            DialogResult dr = XtraMessageBox.Show("Bạn có thực sự muốn xóa không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                int currentPosition = -1;
                try
                {
                    currentPosition = bds.Position; // giữ lại vị trí grid để phòng trường hợp xóa lỗi
                    bds.RemoveCurrent(); // xóa trên máy hiện tại trước
                    if (cheDo == 1)
                    {
                        this.DatHangTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                        this.DatHangTableAdapter.Update(this.QLVTDataSet.DatHang); // update xuống csdl
                    }
                    else
                    {
                        this.CTDDHTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                        this.CTDDHTableAdapter.Update(this.QLVTDataSet.CTDDH); // update xuống csdl   
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
                        this.DatHangTableAdapter.Fill(this.QLVTDataSet.DatHang); // trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                    if (cheDo == 1)
                        this.CTDDHTableAdapter.Fill(this.QLVTDataSet.CTDDH);
                    // lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                    bds.Position = currentPosition; // sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo vị trí ban nãy muốn xóa
                    
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
