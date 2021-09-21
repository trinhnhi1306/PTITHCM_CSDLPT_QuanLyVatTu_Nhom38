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

namespace QLVT_Nhom38.Forms
{
    public partial class FormLogin : Form
    {
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
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại Tên Server của Publisher và tên CSDL trong chuỗi kết nối.\n" + e.Message, "", MessageBoxButtons.OK);
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
            Program.formMain.Close();
        }

        private void btnDangNhap_Click(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng điền login name!", "Error message", MessageBoxButtons.OK);
                txtLogin.Focus();
                return;
            }
            if (txtPass.Text.Trim() == "")
            {
                MessageBox.Show("Vui lòng điền password!", "Error message", MessageBoxButtons.OK);
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

            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0); //lấy username
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Login của bạn không có quyền truy cập dữ liệu.\nBạn xem lại username và password.", "", MessageBoxButtons.OK);
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
            MessageBox.Show("Đăng nhập thành công!", "", MessageBoxButtons.OK);
        }

        private void FormLogin_Load_1(object sender, EventArgs e)
        {
            if (KetNoi_CSDLGOC() == 0) return;
            Lay_DSPM("select * from Get_Subscribes");
            cmbChiNhanh.SelectedIndex = 0;
            Program.serverName = cmbChiNhanh.SelectedValue.ToString();
        }
    }
}
