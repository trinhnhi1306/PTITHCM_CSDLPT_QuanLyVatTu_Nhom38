using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using DevExpress.XtraEditors;

namespace QLVT_Nhom38.Forms
{
    public partial class FormLogin : Form
    {
        private int isHided = 1;
        private SqlConnection conn_publisher = new SqlConnection();

        public FormLogin()
        {
            InitializeComponent();
        }

        private void Lay_DSPM(String cmd)
        {
            DataTable dt = new DataTable();
            if (conn_publisher.State == ConnectionState.Closed) conn_publisher.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd, conn_publisher);
            da.Fill(dt);
            conn_publisher.Close();
            Program.bds_dspm.DataSource = dt;
            cmbChiNhanh.DataSource = Program.bds_dspm;
            cmbChiNhanh.DisplayMember = "TENCN";
            cmbChiNhanh.ValueMember = "TENSERVER";
        }

        private int KetNoi_CSDLGOC()
        {
            if (conn_publisher != null && conn_publisher.State == ConnectionState.Open)
                conn_publisher.Close();
            try
            {
                conn_publisher.ConnectionString = Program.connstr_publisher;
                conn_publisher.Open();
                return 1;
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại Tên Server của Publisher và tên CSDL trong chuỗi kết nối.\n" + e.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        private void cmbChiNhanh_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Program.serverName = cmbChiNhanh.SelectedValue.ToString();
            }    
            catch(Exception)
            {

            }
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim() == "")
            {
                XtraMessageBox.Show("Vui lòng điền login name!", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLogin.Focus();
                return;
            }
            if (txtPass.Text.Trim() == "")
            {
                XtraMessageBox.Show("Vui lòng điền password!", "Error message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPass.Focus();
                return;
            }

            Program.mlogin = txtLogin.Text;
            Program.password = txtPass.Text;
            if (Program.KetNoi() == 0) return;

            Program.mChinhanh = cmbChiNhanh.SelectedIndex;
            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;
            Program.serverNameHT = Program.serverName;
            String strLenh = "EXEC SP_Lay_Thong_Tin_NV_Tu_Login '" + Program.mlogin + "'";

            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;

            try
            {
                Program.myReader.Read();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.username = Program.myReader.GetString(0); //lấy username
            if (Convert.IsDBNull(Program.username))
            {
                XtraMessageBox.Show("Login của bạn không có quyền truy cập dữ liệu.\nBạn xem lại username và password.", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Program.maNV = int.Parse(Program.username);
            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.conn.Close();

            Program.formMain.MANV.Text = "Mã NV: " + Program.maNV;
            Program.formMain.HOTEN.Text = "Họ tên NV: " + Program.mHoten;
            Program.formMain.NHOM.Text = "Nhóm: " + Program.mGroup;
            Program.formMain.HienThiMenu();
            XtraMessageBox.Show("Đăng nhập thành công!", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            if (isHided == 1)
            {
                txtPass.UseSystemPasswordChar = false;
                isHided = 0;
            }    
            else
            {
                txtPass.UseSystemPasswordChar = true;
                isHided = 1;
            }    
        }

        private void FormLogin_Load(object sender, EventArgs e)
        {
            if (KetNoi_CSDLGOC() == 0) return;
            Lay_DSPM("select * from Get_Subscribes");
            cmbChiNhanh.SelectedIndex = 0;
            Program.serverName = cmbChiNhanh.SelectedValue.ToString();
            txtPass.UseSystemPasswordChar = true;
        }
    }
}
