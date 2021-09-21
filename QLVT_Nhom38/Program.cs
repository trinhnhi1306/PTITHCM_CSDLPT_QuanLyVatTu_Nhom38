using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;

namespace QLVT_Nhom38
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static SqlConnection conn = new SqlConnection();
        public static String connstr;
        public static String connstr_publisher = "Data Source=DESKTOP-PQNFTKR;Initial Catalog=QLVT;Integrated Security=True";
        //public static String connstr_publisher = "Data Source=DESKTOP-PQNFTKR;Initial Catalog=QLVT;Integrated Security=True";
        //public static String connstr_publisher = "Data Source=DESKTOP-PQNFTKR;Initial Catalog=QLVT;Integrated Security=True";

        public static SqlDataReader myReader;
        public static String serverName = "";
        public static String username = "";
        public static String mlogin = "";
        public static String password = "";

        public static String tenChiNhanh = "";
        public static String serverNameHT = "";

        public static String database = "QLVT";
        public static String remoteLogin = "HTKN";
        public static String remotePassword = "123456";
        public static String mloginDN = "";
        public static String passwordDN = "";
        public static String mGroup = "";
        public static String mHoten = "";
        public static int maNV = 0;
        public static int mChinhanh = 0;

        public static BindingSource bds_dspm = new BindingSource();  // binding source danh sách phân mảnh, giữ bdsPM khi đăng nhập

        // -- FORMS --
        public static FormMain formMain;

        public static int KetNoi()
        {
            if (Program.conn != null && Program.conn.State == ConnectionState.Open)
                Program.conn.Close();
            try
            {
                Program.connstr = "Data Source=" + Program.serverName + ";Initial Catalog=" +
                      Program.database + ";User ID=" +
                      Program.mlogin + ";password=" + Program.password;
                Program.conn.ConnectionString = Program.connstr;
                Program.conn.Open();
                return 1;
            }

            catch (Exception e)
            {
                MessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại chi nhánh, user name và password.\n" + e.Message, "", MessageBoxButtons.OK);
                return 0;
            }
        }

        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.conn);
            sqlcmd.CommandType = CommandType.Text;
            if (Program.conn.State == ConnectionState.Closed) Program.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader();
                return myreader;
            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            formMain = new FormMain();
            Application.Run(formMain);
        }
    }
}
