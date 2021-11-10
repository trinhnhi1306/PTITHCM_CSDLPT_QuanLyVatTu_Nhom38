using DevExpress.XtraEditors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
        int position = 0; // vị trí trên grid view
        string maCN = "";
        string tenServerChuyenToi = "";
        Stack undoList = new Stack();

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
            try
            {
                maCN = ((DataRowView)bdsNV[0])["MACN"].ToString();
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
                cmbChiNhanh.Enabled = btnChuyenChiNhanh.Enabled = btnReload.Enabled = true;  // bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnGhi.Enabled = btnUndo.Enabled = btnChuyenChiNhanh.Enabled = false;
                
            }
            else
            {
                cmbChiNhanh.Enabled = btnUndo.Enabled = btnGhi.Enabled = false;
                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnChuyenChiNhanh.Enabled = btnReload.Enabled = true;
            }

        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            checkThem = 1; // bật trạng thái đang thêm mới lên
            position = bdsNV.Position; // vị trí mà nhân viên đang đứng
            infoNhanVien.Enabled = txtMaNV.Enabled = true;
            bdsNV.AddNew(); // thêm một record trống mới trong grid view

            // đặt các giá trị mặc định
            txtMaCN.Text = maCN;
            txtLuong.Value = 0;
            dteNgaySinh.EditValue = "";
            cbTrangThaiXoa.Checked = false;

            // vô hiệu hóa các nút chức năng
            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = btnChuyenChiNhanh.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;
            gcNhanVien.Enabled = false;
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // Đang ở chế độ thêm mới hoặc sửa, chưa ấn Ghi, ấn Hoàn tác sẽ thoát chế độ Thêm /Sửa
            if (btnThem.Enabled == false) 
            {
                bdsNV.CancelEdit();
                if (checkThem == 1) // chế độ thêm
                {
                    bdsNV.RemoveCurrent(); // xóa record đang thêm dở dang đi
                    checkThem = 0;
                }
                bdsNV.Position = position;
                

                gcNhanVien.Enabled = true;
                infoNhanVien.Enabled = false;

                btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = btnChuyenChiNhanh.Enabled = true;
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
            bdsNV.CancelEdit();
            String strLenhUndo = undoList.Pop().ToString();
            Console.WriteLine(strLenhUndo);

            // Hoàn tác chuyển chi chánh
            if (strLenhUndo.Contains("sp_ChuyenChiNhanh"))
            {
                /* Để chuyển chi nhánh thì phải đứng ở site phân mảnh lưu nhân viên cần chuyển,
                 * nên phải dùng tài khoản HTKN để kết nối tới site mà nhân viên đã chuyển tới,
                 * từ đó gọi sp_ChuyenChiNhanh để chuyển nhân viên về lại chi nhánh cũ.
                 * Sau khi chuyển về xong thì sửa lại sername, login, pass về lại như cũ.
                */
                Program.serverName = tenServerChuyenToi;
                Program.mlogin = Program.remoteLogin;
                Program.password = Program.remotePassword;

                if (Program.KetNoi() == 0)
                {
                    XtraMessageBox.Show("Lỗi kết nối về chi nhánh mới", "Error", MessageBoxButtons.OK);
                    return;
                }

                int n = Program.ExecSqlNonQuery(strLenhUndo);

                if (n == 0)
                {  
                    XtraMessageBox.Show("Chuyển nhân viên trở lại thành công", "Thông báo", MessageBoxButtons.OK);
                    this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
                }
                else
                    XtraMessageBox.Show("Chuyển nhân viên thất bại!", "Thông báo", MessageBoxButtons.OK);
                
                Program.serverName = Program.serverNameHT;
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
                if (Program.KetNoi() == 0)
                {
                    XtraMessageBox.Show("Lỗi kết nối về chi nhánh mới", "Error", MessageBoxButtons.OK);
                    return;
                }
            }
            else
            {
                if (Program.KetNoi() == 0)
                    return;
                int n = Program.ExecSqlNonQuery(strLenhUndo);
                reload();
                bdsNV.Position = position;
            }

            // Nếu sau khi undo mà danh sách undoList trống thì disable nút undo đi
            if (undoList.Count == 0)
            {                
                btnUndo.Enabled = false;
                return;
            }

        }

        private void btnSua_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            position = bdsNV.Position;
            gcNhanVien.Enabled = txtMaNV.Enabled = false;
            infoNhanVien.Enabled = true;

            btnChuyenChiNhanh.Enabled = btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = false;
            btnGhi.Enabled = btnUndo.Enabled = true;

            //txtMaNV.Enabled = false;

        }

        private void reload()
        {
            try
            {
                this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
                bdsNV.Position = position;
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

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            string maNV = ((DataRowView)bdsNV[bdsNV.Position])["MANV"].ToString();
            if (maNV == Program.username)
            {
                XtraMessageBox.Show("Không thể xóa chính tài khoản đang đăng nhập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
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

            // Lấy ngày sinh trong grid view
            DateTime ngaySinh = (DateTime)((DataRowView)bdsNV[bdsNV.Position])["NGAYSINH"];
            int trangThai = (cbTrangThaiXoa.Checked == true) ? 1 : 0;
            // tạo lệnh undo để bỏ vào undoList
            string strLenhUndo =
                string.Format("INSERT INTO DBO.NHANVIEN (MANV,HO,TEN,DIACHI,NGAYSINH,LUONG,MACN,TrangThaiXoa) " +
            "VALUES ({0}, N'{1}', N'{2}', N'{3}', CAST('{4}' AS DATETIME), {5}, '{6}', {7})", txtMaNV.Text, txtHo.Text, txtTen.Text, txtDiaChi.Text, ngaySinh.ToString("yyyy-MM-dd"), txtLuong.Value, txtMaCN.Text.Trim(), trangThai);

            undoList.Push(strLenhUndo);

            DialogResult dr = XtraMessageBox.Show("Bạn có thực sự muốn xóa nhân viên này không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                
                try
                {
                    maNV = ((DataRowView)bdsNV[bdsNV.Position])["MANV"].ToString(); // giữ lại mã nv để phòng trường hợp xóa lỗi
                    bdsNV.RemoveCurrent(); // xóa trên máy hiện tại trước
                    this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.NhanVienTableAdapter.Update(this.QLVTDataSet.NhanVien); // update về csdl sau
                    this.btnUndo.Enabled = true;
                    XtraMessageBox.Show("Xóa nhân viên thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show("Lỗi xảy ra trong quá trình xóa. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien); // trên màn hình đã xóa mà csdl chưa xóa nên phải tải lại dữ liệu
                    // lệnh Find trả về index của item trong danh sách với giá trị và tên cột được chỉ định
                    bdsNV.Position = bdsNV.Find("MANV", maNV); // sau khi fill xong thì con nháy đứng ở dòng đầu tiên nên mình đặt lại theo mã nv ban nãy muốn xóa
                    undoList.Pop();
                    return;
                }
            }
            else undoList.Pop();

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
            if (dteNgaySinh.Text.Trim() == "")
            {
                XtraMessageBox.Show("Ngày sinh nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                dteNgaySinh.Focus();
                return;
            }
            if (txtLuong.Text.Trim() == "")
            {
                XtraMessageBox.Show("Lương nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                dteNgaySinh.Focus();
                return;
            }
            if (txtDiaChi.Text.Trim() == "")
            {
                XtraMessageBox.Show("Địa chỉ nhân viên không được để trống!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }
            if (DateTime.Now.Year - dteNgaySinh.DateTime.Year < 18)
            {
                XtraMessageBox.Show("Nhân viên phải từ 18 tuổi trở lên!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                dteNgaySinh.Focus();
                return;
            }
            if (txtLuong.Value < 4000000)
            {
                XtraMessageBox.Show("Lương nhân viên phải từ 4,000,000 trở lên!", "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtDiaChi.Focus();
                return;
            }

            String strLenhUndo = "";
            // Trường hợp đang thêm mới
            if(checkThem == 1)
            {
                /* kiểm tra mã nhân viên có trùng không 
                 * kiểm tra trên phân mảnh hiện tại trước, nếu không có mới lên server3 để tra cứu
                 * soạn sẵn câu lệnh để đưa vào hàm ExecSqlDataReader
                 * sau đó đọc kết quả trong myReader
                 */

                int viTriMaNV = bdsNV.Find("MANV", txtMaNV.Text);
                if (viTriMaNV != -1)
                {
                    XtraMessageBox.Show("Mã nhân viên này đã được sử dụng!", "Thông báo", MessageBoxButtons.OK);
                    Console.WriteLine("Mã nhân viên này đã được sử dụng!");
                    return;
                }

                String strLenh = "DECLARE @return_value int " +
                                 "EXEC @return_value = [dbo].[sp_TraCuu_NhanVien_Kho] " +
                                 txtMaNV.Text.Trim() + ", 'MANV' " +
                                 "SELECT 'Return Value' = @return_value";
                Program.myReader = Program.ExecSqlDataReader(strLenh);
                if (Program.myReader == null) return;

                Program.myReader.Read();
                int result = int.Parse(Program.myReader.GetValue(0).ToString());
                Program.myReader.Close();

                if (result == 1)
                {
                    XtraMessageBox.Show("Mã nhân viên này đã được sử dụng!", "Thông báo", MessageBoxButtons.OK);
                    Console.WriteLine("Mã nhân viên này đã được sử dụng!");
                    return;
                }
                strLenhUndo = "DELETE DBO.NHANVIEN WHERE MANV = " + txtMaNV.Text.Trim();
                
            }

            // Trường hợp sửa thì lưu lại dữ liệu cũ để đưa vào stack
            else
            {                
                String maNV = txtMaNV.Text.Trim();// Trim() de loai bo khoang trang thua
                DataRowView drv = ((DataRowView)bdsNV[bdsNV.Position]);
                String ho = drv["HO"].ToString();
                String ten = drv["TEN"].ToString();

                String diaChi = drv["DIACHI"].ToString();

                DateTime ngaySinh = ((DateTime)drv["NGAYSINH"]);

                int luong = int.Parse(drv["LUONG"].ToString());
                String maCN = drv["MACN"].ToString();
                int trangThai = int.Parse(drv["TrangThaiXoa"].ToString());

                strLenhUndo = "UPDATE DBO.NhanVien " +
                                "SET " +
                                "HO = N'" + ho + "'," +
                                "TEN = N'" + ten + "'," +
                                "DIACHI = N'" + diaChi + "'," +
                                "NGAYSINH = CAST('" + ngaySinh.ToString("yyyy-MM-dd") + "' AS DATETIME)," +
                                "LUONG = '" + luong + "'," +
                                "TrangThaiXoa = " + trangThai + " " +
                                "WHERE MANV = " + maNV;
            }    

            try
            {
                bdsNV.EndEdit(); // kết thúc quá trình hiệu chỉnh và ghi vào BindingSource
                bdsNV.ResetCurrentItem(); // đưa thông tin vào grid
                this.NhanVienTableAdapter.Connection.ConnectionString = Program.connstr; // đường kết nối đã đăng nhập
                this.NhanVienTableAdapter.Update(this.QLVTDataSet.NhanVien); // update xuống csdl
                
                XtraMessageBox.Show("Ghi nhân viên thành công!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnUndo.Enabled = true;
                undoList.Push(strLenhUndo);
                checkThem = 0;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Lỗi xảy ra trong quá trình ghi nhân viên. Vui lòng thử lại!\n" + ex.Message, "Thông báo lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Khi Update database lỗi thì xóa record vừa thêm trong bds
                //bdsNV.RemoveCurrent();
                reload();
                return;
            }

            gcNhanVien.Enabled = btnChuyenChiNhanh.Enabled = true;
            infoNhanVien.Enabled = false;

            btnThem.Enabled = btnXoa.Enabled = btnSua.Enabled = btnReload.Enabled = btnThoat.Enabled = true;
            btnGhi.Enabled = false;
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
                this.PhieuXuatTableAdapter.Connection.ConnectionString = Program.connstr;
                this.PhieuXuatTableAdapter.Fill(this.QLVTDataSet.PhieuXuat);
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

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (infoNhanVien.Enabled)
            {
                if (XtraMessageBox.Show("Dữ liệu Nhân Viên vẫn chưa lưu vào Database!\nBạn có chắn chắn muốn thoát?", "Thông báo",
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

        private Form CheckExists(Type ftype)
        {
            foreach (Form f in this.MdiChildren)
                if (f.GetType() == ftype)
                    return f;
            return null;
        }

        public void chuyenChiNhanh2 (String server)
        {
            // Chi nhánh được chọn là chi nhánh đang đăng nhập
            if (Program.serverName == server)
            {
                XtraMessageBox.Show("Hãy chọn chi nhánh khác chi nhánh bạn đang đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            String maChiNhanhMoi = "";

            if (server.Contains("1"))
                maChiNhanhMoi = "CN1";

            else if (server.Contains("2"))
                maChiNhanhMoi = "CN2";

            else
            {
                XtraMessageBox.Show("Mã chi nhánh không hợp lệ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult dr = XtraMessageBox.Show("Hành động này không thế hoàn tác.\nBạn có chắc chắn muốn chuyển nhân viên này không?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            // người dùng chọn cancel thì return không làm gì cả
            if (dr != DialogResult.OK) return;

            int position = bdsNV.Position;
            String maNhanVien = ((DataRowView)bdsNV[position])["MANV"].ToString();

            // tạo chuỗi lệnh để chuyển nhân viên đến chi nhánh mới
            String strLenh = "EXEC sp_ChuyenChiNhanh " + maNhanVien + ",'" + maChiNhanhMoi + "'";
            int n = Program.ExecSqlNonQuery(strLenh);
            if (n == 0)
            {
                
                XtraMessageBox.Show("Chuyển chi nhánh thành công!", "Thông báo", MessageBoxButtons.OK);
                //btnUndo.Enabled = true;
                this.NhanVienTableAdapter.Fill(this.QLVTDataSet.NhanVien);
                return;
            }
            else
            {
                XtraMessageBox.Show("Chuyển chi nhánh thất bại!\n", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }            
        }

        private void btnChuyenChiNhanh_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int viTriHT = bdsNV.Position;
            int trangThaiXoa = int.Parse(((DataRowView)(bdsNV[viTriHT]))["TrangThaiXoa"].ToString());
            string maNV = ((DataRowView)(bdsNV[viTriHT]))["MANV"].ToString();

            if (maNV == Program.username)
            {
                XtraMessageBox.Show("Không thể chuyển chính người đang đăng nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Nhân viên đã bị xóa
            if (trangThaiXoa == 1)
            {
                XtraMessageBox.Show("Nhân viên này không có ở chi nhánh này", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Kiem tra xem FormChuyenChiNhanh da co trong bo nho chua
            Form f = this.CheckExists(typeof(FormChuyenChiNhanh));
            if (f != null)
            {
                f.Activate();
            }
            FormChuyenChiNhanh form = new FormChuyenChiNhanh();
            form.Show();

            // Khởi tạo delegate bên FormChuyenChiNhanh với tham số khởi tạo là phương thức
            // chuyenChiNhanh2 của FormNhanVien
            form.chuyenChiNhanh1 = new FormChuyenChiNhanh.MyDelegate(chuyenChiNhanh2);

            //this.btnUndo.Enabled = true;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
