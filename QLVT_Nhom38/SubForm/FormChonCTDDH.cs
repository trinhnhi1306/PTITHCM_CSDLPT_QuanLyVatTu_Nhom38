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

namespace QLVT_Nhom38.SubForm
{
    public partial class FormChonCTDDH : Form
    {
        int soLuongMax = 0;
        public FormChonCTDDH()
        {
            InitializeComponent();
            using (SqlConnection sqlCon = new SqlConnection(Program.connstr))
            {
                sqlCon.Open();
                SqlDataAdapter sqlData = new SqlDataAdapter("[dbo].[SP_getCTDDH] '"+ Program.maDDH + "'", sqlCon);
                DataTable dtb1 = new DataTable();
                sqlData.Fill(dtb1);
                
                dgvCTDDH.DataSource = dtb1;
                dgvCTDDH.AllowUserToAddRows = false;
            }
            txtMaVT.Enabled = false;
            txtmaDDH.Enabled = false;
            txtTenVatTu.Enabled = false;
            DataGridViewRow row = dgvCTDDH.Rows[0];
            txtmaDDH.Text = row.Cells[0].Value.ToString();
            txtMaVT.Text = row.Cells[1].Value.ToString();
            txtTenVatTu.Text = row.Cells[2].Value.ToString();
            nbrSoLuong.Value = int.Parse(row.Cells[3].Value.ToString());
            nbrDonGia.Value = int.Parse(row.Cells[4].Value.ToString());
            soLuongMax = int.Parse(row.Cells[3].Value.ToString());
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dgvCTDDH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           int index = e.RowIndex;
           
            if(index != -1)
            {
                DataGridViewRow row = dgvCTDDH.Rows[index];
                txtmaDDH.Text = row.Cells[0].Value.ToString();
                txtMaVT.Text = row.Cells[1].Value.ToString();
                txtTenVatTu.Text = row.Cells[2].Value.ToString();
                nbrSoLuong.Value = int.Parse(row.Cells[3].Value.ToString());
                nbrDonGia.Value = int.Parse(row.Cells[4].Value.ToString());
                soLuongMax = int.Parse(row.Cells[3].Value.ToString());
            }

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                if (nbrSoLuong.Value > soLuongMax)
                {      
                    nbrSoLuong.Focus();
                    MessageBox.Show("không được nhập quá số lượng chi tiết đơn hàng", "Thông báo", MessageBoxButtons.OK);
                    return;
                }
                Console.WriteLine(txtMaVT.Text.Length);
                Program.maVatTuCTDDH = txtMaVT.Text;
                Program.SoLuongCTDDH = Convert.ToInt32(Math.Round(nbrSoLuong.Value, 0));
                Program.DonGiaCTDDH = Convert.ToInt32(Math.Round(nbrDonGia.Value, 0));
                this.Close();
            }
            
        }

        private void nbrSoLuong_Validating(object sender, CancelEventArgs e)
        {
            if(nbrSoLuong.Value > soLuongMax)
            {
                e.Cancel = true;
                nbrSoLuong.Focus();
                errorProvider.SetError(nbrSoLuong, "Số lượng không được quá số lượng trong chi tiết đơn hàng !");
            }
            
            if (nbrSoLuong.Value == 0)
            {
                e.Cancel = true;
                nbrSoLuong.Focus();
                errorProvider.SetError(nbrSoLuong, "Vui lòng chọn số lượng !");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(nbrSoLuong, "");
            }
        }

        private void nbrDonGia_Validating(object sender, CancelEventArgs e)
        {
            
            if (nbrDonGia.Value < 1)
            {
                e.Cancel = true;
                nbrSoLuong.Focus();
                errorProvider.SetError(nbrDonGia, "Đơn giá phải lớn hơn 0 !");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(nbrDonGia, "");
            }
        }

        private void txtmaDDH_Validating(object sender, CancelEventArgs e)
        {
            
            if (string.IsNullOrWhiteSpace(txtmaDDH.Text))
            {
                e.Cancel = true;
                txtmaDDH.Focus();
                errorProvider.SetError(txtmaDDH, "vui lòng chọn chi tiết đơn hàng !");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtmaDDH, "");
            }
        }

        private void txtTenVatTu_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTenVatTu.Text))
            {
                e.Cancel = true;
                txtTenVatTu.Focus();
                errorProvider.SetError(txtTenVatTu, "vui lòng chọn chi tiết đơn hàng !");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtTenVatTu, "");
            }
        }

        private void txtMaVT_Validating(object sender, CancelEventArgs e)
        {
            
            if (txtMaVT.Text.Length < 0)
            {
                e.Cancel = true;
                errorProvider.SetError(txtMaVT, "chưa chọn chi tiết đơn hàng !");
            }
            if (txtMaVT.Text.Trim().Contains(" "))
            {
                e.Cancel = true;

                errorProvider.SetError(txtMaVT, "vui lòng chọn chi tiết đơn hàng !");
            }
            if (string.IsNullOrWhiteSpace(txtMaVT.Text))
            {
                e.Cancel = true;
           
                errorProvider.SetError(txtMaVT, "vui lòng chọn chi tiết đơn hàng !");
            }
            else
            {
                e.Cancel = false;
                errorProvider.SetError(txtMaVT, "");
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            Program.getAll = true;
            this.Close();
        }
    }
}
