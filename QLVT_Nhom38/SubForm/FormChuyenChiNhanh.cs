using DevExpress.XtraEditors;
using QLVT_Nhom38.SimpleForm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLVT_Nhom38
{
    public partial class FormChuyenChiNhanh : Form
    {
        public FormChuyenChiNhanh()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void FormChuyenChiNhanh_Load(object sender, EventArgs e)
        {
            /*Lấy dữ liệu từ form đăng nhập đổ vào nhưng chỉ lấy đúng danh sách
             phân mảnh mà thôi*/
            cmbChiNhanh1.DataSource = Program.bds_dspm.DataSource;
            cmbChiNhanh1.DisplayMember = "TENCN";
            cmbChiNhanh1.ValueMember = "TENSERVER";
            cmbChiNhanh1.SelectedIndex = Program.mChinhanh;
        }

        /************************************************************
         * delegate - một biến mà khi được gọi, nó sẽ thực hiện 1 hàm (function) khác có cùng kiểu tham
         * số và kiểu trả về
         * Ở class FormNhanVien, ta có hàm chuyenChiNhanh2, và ta đã khởi tạo biến chuyenChiNhanh1 với
         * tham số khởi tạo là hàm chuyenChiNhanh2
         * Vậy sau khi chọn chi nhánh và ấn nút Chuyển, ta gọi hàm chuyenChiNhanh1 thì nó sẽ gọi hàm
         * chuyenChiNhanh2 đã được định nghĩa bên class FormNhanVien
         *************************************************************/
        public delegate void MyDelegate(string chiNhanh);
        public MyDelegate chuyenChiNhanh1;

        private void btnChuyen_Click(object sender, EventArgs e)
        {
            if (cmbChiNhanh1.Text.Trim().Equals(""))
            {
                XtraMessageBox.Show("Vui lòng chọn chi nhánh", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FormNhanVien.serverMoi = cmbChiNhanh1.SelectedValue.ToString();
            // Chi nhánh được chọn là chi nhánh đang đăng nhập
            if (FormNhanVien.serverMoi.Equals(Program.serverName))
            {
                XtraMessageBox.Show("Hãy chọn chi nhánh khác chi nhánh bạn đang đăng nhập", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult dr = XtraMessageBox.Show("Hành động này không thế hoàn tác.\nBạn có chắc chắn muốn chuyển nhân viên này không?", "Thông báo",
                        MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            // người dùng chọn cancel thì return không làm gì cả
            if (dr != DialogResult.OK) return;

            this.Dispose();
        }
    }
}
