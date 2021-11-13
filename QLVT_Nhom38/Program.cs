using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Data;
using DevExpress.XtraEditors;
using QLVT_Nhom38.SubForm;

namespace QLVT_Nhom38
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public static SqlConnection conn = new SqlConnection();
        public static String connstr;
        public static String connstr_publisher = "Data Source=.;Initial Catalog=QLVT;Integrated Security=True"; // kết nối csdl với mode Windows
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

        /* - Phần này giúp load thông tin đơn đặt hàng và lấy thông tin chi tiết đơn đặt hàng vào chi tiết phiếu nhập
         * + maDDH lấy mã đơn đặt hàng để load thông tin tương ứng với đơn đặt hàng đó
         * + maVatTuCTDDH là mã vật tư của vật tư trong chi tiết đơn hàng
         * + SoLuongCTDDH là số lượng khi lập chi tiết phiếu nhập
         * + DonGiaCTDDH là giá tiền khi lập chi tiết phiếu nhập
         */
        public static String maDDH = "";
        public static String maVatTuCTDDH = "";
        public static int SoLuongCTDDH = 0;
        public static int DonGiaCTDDH = 0;
        public static bool getAll = false;

        public static BindingSource bds_dspm = new BindingSource();  // binding source danh sách phân mảnh, giữ bdsPM khi đăng nhập

        // -- FORMS --
        public static FormMain formMain;
        public static FormChonCTDDH formChonCTDDH;
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
                XtraMessageBox.Show("Lỗi kết nối cơ sở dữ liệu.\nBạn xem lại user name và password.\n" + e.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return 0;
            }
        }

        // ExecSqlDataReader thực hiện câu lệnh mà dữ liệu trả về chỉ dùng
        // để xem & không thao tác với nó.
        public static SqlDataReader ExecSqlDataReader(String strLenh)
        {
            SqlDataReader myreader;
            SqlCommand sqlcmd = new SqlCommand(strLenh, Program.conn);
            sqlcmd.CommandType = CommandType.Text;
            sqlcmd.CommandTimeout = 600;// 10 phut
            if (Program.conn.State == ConnectionState.Closed) Program.conn.Open();
            try
            {
                myreader = sqlcmd.ExecuteReader();
                return myreader;
            }
            catch (SqlException ex)
            {
                Program.conn.Close();
                XtraMessageBox.Show(ex.Message, "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        //Cập nhật trên một stored procedure và không trả về giá trị
        public static int ExecSqlNonQuery(String strlenh)
        {
            SqlCommand Sqlcmd = new SqlCommand(strlenh, conn);
            Sqlcmd.CommandType = CommandType.Text;
            Sqlcmd.CommandTimeout = 600;// 10 phut
            if (conn.State == ConnectionState.Closed) conn.Open();
            try
            {
                Sqlcmd.ExecuteNonQuery();
                conn.Close();
                return 0;
            }
            catch (SqlException ex)
            {
                XtraMessageBox.Show(ex.Message);
                conn.Close();
                return ex.State;

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
