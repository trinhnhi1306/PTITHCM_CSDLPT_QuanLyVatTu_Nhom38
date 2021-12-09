using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
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

namespace QLVT_Nhom38.SubForm
{
    public partial class FormPhieuNhap : Form
    {
        int checkThem = 0;
        int cheDo = 1; // 1: thao tác trên đơn hàng, 2: thao tác trên chi tiết đơn hàng
        Stack undoList = new Stack();

        BindingSource bds = null;
        GridControl gc = null;
        GroupControl info = null;
        public FormPhieuNhap()
        {
            InitializeComponent();
        }

        private void phieuNhapBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bds_PhieuNhap.EndEdit();
            this.tableAdapterManager.UpdateAll(this.qLVTDataSet);

        }

        private void datHangBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bds_DatHang.EndEdit();
            this.tableAdapterManager.UpdateAll(this.qLVTDataSet);

        }

        private void FormPhieuNhap_Load(object sender, EventArgs e)
        {            
            qLVTDataSet.EnforceConstraints = false; //không kiểm tra khóa ngoại trên dataset này

            // TODO: This line of code loads data into the 'qLVTDataSet.DatHang' table. You can move, or remove it, as needed.
            this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
            this.datHangTableAdapter.Fill(this.qLVTDataSet.DatHang);
            // TODO: This line of code loads data into the 'qLVTDataSet.CTDDH' table. You can move, or remove it, as needed.
            this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTDDHTableAdapter.Fill(this.qLVTDataSet.CTDDH);
            // TODO: This line of code loads data into the 'qLVTDataSet.PhieuNhap' table. You can move, or remove it, as needed.
            this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
            this.phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
            // TODO: This line of code loads data into the 'qLVTDataSet.CTPN' table. You can move, or remove it, as needed.
            this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
            this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
            
            cheDo = 1;
            bds = bds_PhieuNhap;
            gc = gcPhieuNhap;
            gcCTPN.Enabled = false;


         

            cmbChiNhanh.DataSource = Program.bds_dspm;  // sao chép bds_dspm đã load ở form đăng nhập qua
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
            cmbChiNhanh.SelectedIndex = Program.mChinhanh;

            
            if (Program.mGroup == "CONGTY")
            {
               
                cmbChiNhanh.Enabled = btnChuyenChiNhanh.Enabled = btnReload.Enabled = true;  // bật tắt theo phân quyền
                btnThem.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnUndo.Enabled= switchCheDo.Enabled = false;
                gcCTPN.Enabled = true;
            }
            else
            {
                cmbChiNhanh.Enabled = btnUndo.Enabled  = false;
                btnThem.Enabled = btnXoa.Enabled = btnGhi.Enabled = btnReload.Enabled = true;
            }

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
                this.datHangTableAdapter.Connection.ConnectionString = Program.connstr;
                this.datHangTableAdapter.Fill(this.qLVTDataSet.DatHang);
                this.cTDDHTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTDDHTableAdapter.Fill(this.qLVTDataSet.CTDDH);
                this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                this.phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
                this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
               
            }
        }

        private void datHangGridControl_EmbeddedNavigator_SizeChanged(object sender, EventArgs e)
        {
          
           
        }

        private void tENVTLabel_Click(object sender, EventArgs e)
        {

        }

        private void switchCheDo_CheckedChanged(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (switchCheDo.Checked == true)
            {
                Console.WriteLine("Chi tiet don dat hang");
                cheDo = 2;
                gcCTPN.Enabled = true; // bật grid control CTDDH
                gcPhieuNhap.Enabled = false; // tắt grid control DDH
                bds = bds_CTPN;
                gc = gcCTPN;
                gcDatHang.Enabled = false;
                btnThem.Enabled = true;
                btnGhi.Enabled = false;

            }
            else
            {
                Console.WriteLine("Don dat hang");
                cheDo = 1;
                btnGhi.Enabled = false;
                gcPhieuNhap.Enabled = true; // bật grid control DDH
                gcDatHang.Enabled = true;
                gcCTPN.Enabled = false; // tắt grid control CTDDH
                bds = bds_PhieuNhap;
                gc = gcPhieuNhap;
                
            }
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            int maNhanVien = int.Parse(((DataRowView)bds_DatHang[bds_DatHang.Position])["MANV"].ToString());
            if (cheDo == 1)
            {
                this.btnXoa.Enabled = this.btnThoat.Enabled = this.btnReload.Enabled = false;
                this.btnUndo.Enabled = this.btnGhi.Enabled = true;
                if (bds_PhieuNhap.Count == 1)
                {
                    MessageBox.Show("Đơn hàng này đã có phiếu nhập, vui lòng vào hiệu chỉnh chi tiết phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                checkThem = 1;
                if (Program.maNV != maNhanVien)
                {
                    XtraMessageBox.Show("Không thể thêm phiếu nhập trên phiếu người khác lập", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                this.bds_PhieuNhap.AddNew();
                gcPhieuNhap.Enabled =  true;
                gcDatHang.Enabled = gcCTPN.Enabled = false;
                this.btnGhi.Enabled = true;

               
                string maKho = ((DataRowView)bds_DatHang[bds_DatHang.Position])["MAKHO"].ToString().Trim();
                ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAKHO"] = maKho;
                ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MANV"] = Program.maNV;
                ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["NGAY"] = DateTime.Today;
               

            }
            else
            {

                if (bds_PhieuNhap.Count == 0)
                {
                    MessageBox.Show("chưa có phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                if (bds_CTDDH.Count == 0)
                {
                    MessageBox.Show("Đơn đặt hàng chưa lập chi tiết đơn đặt hàng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (bds_CTPN.Count == bds_CTDDH.Count)
                {
                    MessageBox.Show("chi tiết đơn hàng đã được nhập hết", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                if (Program.maNV != maNhanVien)
                {
                    MessageBox.Show("Bạn không thêm chi tiết phiếu nhập trên phiếu không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                
                Program.maDDH = ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MasoDDH"].ToString();

                FormChonCTDDH formChonCTDDH = new FormChonCTDDH();
                formChonCTDDH.ShowDialog();

                /*this.bds_CTPN.AddNew();
                ((DataRowView)bds_CTPN[bds_CTPN.Position])["MAPN"] = ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString();
                ((DataRowView)bds_CTPN[bds_CTPN.Position])["MAVT"] = Program.maVatTuCTDDH;
                ((DataRowView)bds_CTPN[bds_CTPN.Position])["SOLUONG"] = Program.SoLuongCTDDH;
                ((DataRowView)bds_CTPN[bds_CTPN.Position])["DONGIA"] = Program.DonGiaCTDDH;
                btnGhi.Enabled = true;*/
                String query = "";
                String queryUndo = "";
                if (Program.getAll == true)
                {
                    String strLenh = "[dbo].[SP_getCTDDH] '" + Program.maDDH + "'";
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
                    while (Program.myReader.Read())
                    {
                       String i = Program.myReader["MAVT"].ToString();
                        String c = Program.myReader["SOLUONG"].ToString();
                        String v = Program.myReader["DONGIA"].ToString();

                         query += "INSERT INTO DBO.CTPN(MAPN, MAVT, SOLUONG, DONGIA) " +
                                       "VALUES('" + ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString() + "', '" +
                                       Program.myReader["MAVT"].ToString() + "', " +
                                       Program.myReader["SOLUONG"].ToString() + ", " +
                                       Program.myReader["DONGIA"].ToString() + ");";
                        query += "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON +"+ Program.myReader["SOLUONG"].ToString() + " WHERE MAVT= '"+ Program.myReader["MAVT"].ToString() + "';";

                        queryUndo += "DELETE FROM DBO.CTPN " +
                                "WHERE MAPN = '" + ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString() + "' " +
                                "AND MAVT = '" + Program.myReader["MAVT"].ToString() + "';";
                        queryUndo += "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON -" + Program.myReader["SOLUONG"].ToString() + " WHERE MAVT= '" + Program.myReader["MAVT"].ToString() + "';";

                    }
                    Program.myReader.Close();
                    Program.ExecSqlNonQuery(query);
                    cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
                    phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
                }
                else
                {
                    if (Program.SoLuongCTDDH > 0)
                    {
                         query = "INSERT INTO DBO.CTPN(MAPN, MAVT, SOLUONG, DONGIA) " +
                        "VALUES('" + ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString() + "', '" +
                        Program.maVatTuCTDDH + "', " +
                         Program.SoLuongCTDDH + ", " +
                        Program.DonGiaCTDDH + ")";
                        query += "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON +" + Program.SoLuongCTDDH + " WHERE MAVT= '" + Program.maVatTuCTDDH + "';";

                        queryUndo = "DELETE FROM DBO.CTPN " +
                               "WHERE MAPN = '" + ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString() + "' " +
                               "AND MAVT = '" + Program.maVatTuCTDDH + "';";
                        queryUndo += "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON -" + Program.SoLuongCTDDH + " WHERE MAVT= '" + Program.maVatTuCTDDH + "';";

                        Program.ExecSqlNonQuery(query);
                        cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
                        phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
                        Program.SoLuongCTDDH = 0;
                        Program.DonGiaCTDDH = 0;
                        Program.maDDH = "";
                        Program.maVatTuCTDDH = "";
                        Program.getAll = false;

                    }
                    
                }
                this.btnUndo.Enabled = this.btnXoa.Enabled = this.btnThoat.Enabled = this.btnReload.Enabled = this.btnThem.Enabled = true;
                this.btnGhi.Enabled = false;
                undoList.Push(queryUndo );
               
            }
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {

            /*if (cheDo == 2)
            {

                for (int index = 0; index < bds_CTPN.Count; index++)
                {
                    String maVTCTPN = ((DataRowView)bds_CTPN[index])["MAVT"].ToString();
                    int soLuongCTPN = int.Parse(((DataRowView)bds_CTPN[index])["SOLUONG"].ToString());
                    for (int j = 0; j < bds_CTPN.Count; j++)
                    {

                        int soLuongCTDDH = int.Parse(((DataRowView)bds_CTDDH[j])["SOLUONG"].ToString());
                        if (maVTCTPN.Equals(((DataRowView)bds_CTDDH[j])["MAVT"].ToString()))
                        {
                            if (soLuongCTPN > soLuongCTDDH)
                            {
                                XtraMessageBox.Show("vật tư có mã " + maVTCTPN + " đã vượt qua số lượng ở chi tiết đơn đặt hàng, Vui lòng hiệu chỉnh lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (soLuongCTPN < 0)
                            {
                                XtraMessageBox.Show("vật tư có mã " + maVTCTPN + " có số lượng bé hơn 0, Vui lòng hiệu chỉnh lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                            if (int.Parse(((DataRowView)bds_CTDDH[j])["DONGIA"].ToString()) < 1)
                            {
                                XtraMessageBox.Show("vật tư có mã " + maVTCTPN + " có đơn giá bé hơn 1, Vui lòng hiệu chỉnh lại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }

                        }
                    }

                }


            }*/
            if(cheDo == 1 )
            {
                if (((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString().Trim().Equals(""))
                {
                    MessageBox.Show("Không bỏ trống mã phiếu nhập !", "Thông báo", MessageBoxButtons.OK);
                   return;
                }
                if(((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString().Trim().Length > 8)
                {
                    MessageBox.Show("độ dài mã phiếu nhập không quá 8 số !", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (checkThem == 0 && bds_CTPN.Count < 1)
                {
                    MessageBox.Show("Phiếu này đã có chi tiết, vui lòng không được sửa", "Thông báo", MessageBoxButtons.OK);
                }
            }
            string cauTruyVanHoanTac = "";
            String maPhieuNhap = ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString();
           
            DialogResult dr = MessageBox.Show("Bạn có chắc muốn ghi dữ liệu vào cơ sở dữ liệu ?", "Thông báo",
                         MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == DialogResult.OK)
            {
                try
                {
                    if (cheDo == 1 )
                    {
                       if(checkThem == 1)
                        {
                            cauTruyVanHoanTac =
                           "DELETE FROM DBO.PHIEUNHAP " +
                           "WHERE MAPN = '" + maPhieuNhap + "'";
                        }
                        else
                        {
                            cauTruyVanHoanTac =
                           "DELETE FROM DBO.PHIEUNHAP " +
                           "WHERE MAPN = '" + maPhieuNhap + "'";
                        }
                    }

                   
                    /*if (cheDo == 2)
                    {
                        int soLuong = int.Parse(((DataRowView)bds_CTPN[bds_CTPN.Position])["SOLUONG"].ToString());
                        int donGia = int.Parse(((DataRowView)bds_CTPN[bds_CTPN.Position])["DONGIA"].ToString());
                        string maVatTu = Program.maVatTuCTDDH;
                        cauTruyVanHoanTac =
                            "DELETE FROM DBO.CTPN " +
                            "WHERE MAPN = '" + maPhieuNhap + "' " +
                            "AND MAVT = '" + maVatTu + "'";

                        cauTruyVanHoanTac += "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON -" + Program.myReader["SOLUONG"].ToString() + " WHERE MAVT= " + Program.myReader["MAVT"].ToString() + ";";
                      
                       
                    }*/

                    
                    undoList.Push(cauTruyVanHoanTac);

                    this.bds_PhieuNhap.EndEdit();
                    this.bds_CTPN.EndEdit();
                    this.phieuNhapTableAdapter.Update(this.qLVTDataSet.PhieuNhap);
                    checkThem = 0;
                    this.cTPNTableAdapter.Update(this.qLVTDataSet.CTPN);

                    this.btnThem.Enabled = true;
                    this.btnXoa.Enabled = true;
                    this.btnGhi.Enabled = true;

                    this.btnUndo.Enabled = true;
                    this.btnReload.Enabled = true;
                    this.btnThoat.Enabled = true;
                    gcDatHang.Enabled = true;

                    
                    MessageBox.Show("Ghi thành công", "Thông báo", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    bds.RemoveCurrent();
                    MessageBox.Show("Da xay ra loi !\n\n" + ex.Message, "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            string cauTruyVanHoanTac = "";
           
            if (cheDo == 1)
            {
                
                String maNhanVien = ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MANV"].ToString();
                if (Program.username != maNhanVien)
                {
                    MessageBox.Show("Không xóa chi tiết phiếu xuất không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }

                if (bds_CTPN.Count > 0)
                {
                    MessageBox.Show("Không thể xóa phiếu nhập vì có chi tiết phiếu nhập", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                DateTime ngay = ((DateTime)((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["NGAY"]);

                cauTruyVanHoanTac = "INSERT INTO DBO.PHIEUNHAP(MAPN, NGAY, MasoDDH, MANV, MAKHO) " +
                    "VALUES( '" + ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAPN"].ToString().Trim() + "', '" +
                    ngay.ToString("yyyy-MM-dd") + "', '" +
                    ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MasoDDH"].ToString() + "', '" +
                    ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MANV"].ToString() + "', '" +
                   ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MAKHO"].ToString() + "')";

            }

            if (cheDo == 2)
            {
                
                String maNhanVien = ((DataRowView)bds_PhieuNhap[bds_PhieuNhap.Position])["MANV"].ToString(); ;
                if (Program.username != maNhanVien)
                {
                    MessageBox.Show("Bạn không xóa chi tiết phiếu nhập không phải do mình tạo", "Thông báo", MessageBoxButtons.OK);
                    return;
                }


                
                cauTruyVanHoanTac = "INSERT INTO DBO.CTPN(MAPN, MAVT, SOLUONG, DONGIA) " +
                    "VALUES('" + ((DataRowView)bds_CTPN[bds_CTPN.Position])["MAPN"].ToString().Trim() + "', '" +
                     ((DataRowView)bds_CTPN[bds_CTPN.Position])["MAVT"].ToString().Trim() + "', " +
                     ((DataRowView)bds_CTPN[bds_CTPN.Position])["SOLUONG"].ToString().Trim() + ", " +
                     ((DataRowView)bds_CTPN[bds_CTPN.Position])["DONGIA"].ToString().Trim() + ")";
                cauTruyVanHoanTac += "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON +" + ((DataRowView)bds_CTPN[bds_CTPN.Position])["SOLUONG"].ToString().Trim() + " WHERE MAVT= '" + ((DataRowView)bds_CTPN[bds_CTPN.Position])["MAVT"].ToString().Trim() + "';";

                
            }

            undoList.Push(cauTruyVanHoanTac);
           

            
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không ?", "Thông báo",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    
                    
                    if (cheDo == 1)
                    {
                        bds_PhieuNhap.RemoveCurrent();
                    }
                    if (cheDo == 2)
                    {
                        String query = "UPDATE dbo.Vattu SET SOLUONGTON = SOLUONGTON -" + ((DataRowView)bds_CTPN[bds_CTPN.Position])["SOLUONG"].ToString().Trim() + " WHERE MAVT= '" + ((DataRowView)bds_CTPN[bds_CTPN.Position])["MAVT"].ToString().Trim() + "';";
                        Program.ExecSqlNonQuery(query);
                        bds.RemoveCurrent(); 
                        

                    }


                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.qLVTDataSet.PhieuNhap);

                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPNTableAdapter.Update(this.qLVTDataSet.CTPN);

                    
                    MessageBox.Show("Xóa thành công ", "Thông báo", MessageBoxButtons.OK);
                    this.btnUndo.Enabled = true;
                }
                catch (Exception ex)
                {
                    
                    MessageBox.Show("Lỗi xóa. Hãy thử lại\n" + ex.Message, "Thông báo", MessageBoxButtons.OK);
                    this.phieuNhapTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.phieuNhapTableAdapter.Update(this.qLVTDataSet.PhieuNhap);

                    this.cTPNTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.cTPNTableAdapter.Update(this.qLVTDataSet.CTPN);
                    
                    
                    return;
                }
            }
            else
            {
                undoList.Pop();
            }
        }

        private void btnUndo_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (checkThem == 1 && this.btnThem.Enabled == false)
            {
                checkThem = 0;

                this.btnThem.Enabled = true;
                this.btnXoa.Enabled = true;
                this.btnGhi.Enabled = true;

                this.btnReload.Enabled = true;
                this.btnThoat.Enabled = true;

                this.gcPhieuNhap.Enabled = true;
                this.gcDatHang.Enabled = true;

                bds_PhieuNhap.CancelEdit();
              
                
                

                return;
            }

            
            if (undoList.Count == 0)
            {
                MessageBox.Show("Không còn thao tác nào để khôi phục", "Thông báo", MessageBoxButtons.OK);
                btnUndo.Enabled = false;
                return;
            }

           
            bds.CancelEdit();
            String cauTruyVanHoanTac = undoList.Pop().ToString();

            Console.WriteLine(cauTruyVanHoanTac);
            int n = Program.ExecSqlNonQuery(cauTruyVanHoanTac);

            this.phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
            this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
        }

        private void btnReload_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            try
            {
                this.datHangTableAdapter.Fill(this.qLVTDataSet.DatHang);
                this.phieuNhapTableAdapter.Fill(this.qLVTDataSet.PhieuNhap);
                this.cTPNTableAdapter.Fill(this.qLVTDataSet.CTPN);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi làm mời dữ liệu\n\n" + ex.Message, "Thông Báo", MessageBoxButtons.OK);
                Console.WriteLine(ex.Message);
                return;
            }
        }

        private void btnThoat_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.Dispose();

        }

        private void gcCTPN_Click(object sender, EventArgs e)
        {

        }
    }
}
