
namespace QLVT_Nhom38.SimpleForm
{
    partial class FormTaoTaiKhoan
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtMatKhauXacNhan = new System.Windows.Forms.TextBox();
            this.txtMatKhau = new System.Windows.Forms.TextBox();
            this.separatorControl1 = new DevExpress.XtraEditors.SeparatorControl();
            this.radioButton_User = new System.Windows.Forms.RadioButton();
            this.radioButton_ChiNhanh = new System.Windows.Forms.RadioButton();
            this.btnThoat = new System.Windows.Forms.Button();
            this.btnTao = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLoginName = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbNhanVien = new System.Windows.Forms.ComboBox();
            this.bdsNV1 = new System.Windows.Forms.BindingSource(this.components);
            this.QLVTDataSet = new QLVT_Nhom38.QLVTDataSet();
            this.HOTEN = new QLVT_Nhom38.QLVTDataSetTableAdapters.HOTEN();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginName.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsNV1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.QLVTDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Info;
            this.panel1.Controls.Add(this.txtMatKhauXacNhan);
            this.panel1.Controls.Add(this.txtMatKhau);
            this.panel1.Controls.Add(this.separatorControl1);
            this.panel1.Controls.Add(this.radioButton_User);
            this.panel1.Controls.Add(this.radioButton_ChiNhanh);
            this.panel1.Controls.Add(this.btnThoat);
            this.panel1.Controls.Add(this.btnTao);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtLoginName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cmbNhanVien);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(929, 432);
            this.panel1.TabIndex = 0;
            // 
            // txtMatKhauXacNhan
            // 
            this.txtMatKhauXacNhan.Location = new System.Drawing.Point(628, 202);
            this.txtMatKhauXacNhan.Name = "txtMatKhauXacNhan";
            this.txtMatKhauXacNhan.Size = new System.Drawing.Size(252, 29);
            this.txtMatKhauXacNhan.TabIndex = 16;
            this.txtMatKhauXacNhan.Validating += new System.ComponentModel.CancelEventHandler(this.txtMatKhauXacNhan_Validating);
            // 
            // txtMatKhau
            // 
            this.txtMatKhau.Location = new System.Drawing.Point(628, 151);
            this.txtMatKhau.Name = "txtMatKhau";
            this.txtMatKhau.Size = new System.Drawing.Size(252, 29);
            this.txtMatKhau.TabIndex = 15;
            this.txtMatKhau.Validating += new System.ComponentModel.CancelEventHandler(this.txtMatKhau_Validating);
            // 
            // separatorControl1
            // 
            this.separatorControl1.LineOrientation = System.Windows.Forms.Orientation.Vertical;
            this.separatorControl1.Location = new System.Drawing.Point(315, 34);
            this.separatorControl1.Name = "separatorControl1";
            this.separatorControl1.Size = new System.Drawing.Size(87, 397);
            this.separatorControl1.TabIndex = 14;
            // 
            // radioButton_User
            // 
            this.radioButton_User.AutoSize = true;
            this.radioButton_User.Location = new System.Drawing.Point(705, 271);
            this.radioButton_User.Name = "radioButton_User";
            this.radioButton_User.Size = new System.Drawing.Size(63, 25);
            this.radioButton_User.TabIndex = 13;
            this.radioButton_User.Text = "User";
            this.radioButton_User.UseVisualStyleBackColor = true;
            // 
            // radioButton_ChiNhanh
            // 
            this.radioButton_ChiNhanh.AutoSize = true;
            this.radioButton_ChiNhanh.Checked = true;
            this.radioButton_ChiNhanh.Location = new System.Drawing.Point(536, 271);
            this.radioButton_ChiNhanh.Name = "radioButton_ChiNhanh";
            this.radioButton_ChiNhanh.Size = new System.Drawing.Size(103, 25);
            this.radioButton_ChiNhanh.TabIndex = 12;
            this.radioButton_ChiNhanh.TabStop = true;
            this.radioButton_ChiNhanh.Text = "Chi nhánh";
            this.radioButton_ChiNhanh.UseVisualStyleBackColor = true;
            // 
            // btnThoat
            // 
            this.btnThoat.Location = new System.Drawing.Point(686, 337);
            this.btnThoat.Name = "btnThoat";
            this.btnThoat.Size = new System.Drawing.Size(111, 44);
            this.btnThoat.TabIndex = 11;
            this.btnThoat.Text = "Thoát";
            this.btnThoat.UseVisualStyleBackColor = true;
            this.btnThoat.Click += new System.EventHandler(this.btnThoat_Click);
            // 
            // btnTao
            // 
            this.btnTao.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.btnTao.Location = new System.Drawing.Point(523, 337);
            this.btnTao.Name = "btnTao";
            this.btnTao.Size = new System.Drawing.Size(111, 44);
            this.btnTao.TabIndex = 10;
            this.btnTao.Text = "Tạo";
            this.btnTao.UseVisualStyleBackColor = false;
            this.btnTao.Click += new System.EventHandler(this.btnTao_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Image = global::QLVT_Nhom38.Properties.Resources.create_account;
            this.pictureBox1.Location = new System.Drawing.Point(57, 65);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(213, 227);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(63, 337);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(207, 26);
            this.label5.TabIndex = 8;
            this.label5.Text = "TẠO TÀI KHOẢN";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(442, 210);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 21);
            this.label4.TabIndex = 7;
            this.label4.Text = "Xác nhận mật khẩu";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(442, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 21);
            this.label3.TabIndex = 6;
            this.label3.Text = "Mật khẩu";
            // 
            // txtLoginName
            // 
            this.txtLoginName.Location = new System.Drawing.Point(628, 108);
            this.txtLoginName.Name = "txtLoginName";
            this.txtLoginName.Properties.Appearance.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLoginName.Properties.Appearance.Options.UseFont = true;
            this.txtLoginName.Size = new System.Drawing.Size(252, 28);
            this.txtLoginName.TabIndex = 3;
            this.txtLoginName.Validating += new System.ComponentModel.CancelEventHandler(this.txtLoginName_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(442, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Nhân viên";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(442, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Login name";
            // 
            // cmbNhanVien
            // 
            this.cmbNhanVien.DataSource = this.bdsNV1;
            this.cmbNhanVien.DisplayMember = "HOTEN";
            this.cmbNhanVien.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbNhanVien.FormattingEnabled = true;
            this.cmbNhanVien.Location = new System.Drawing.Point(628, 64);
            this.cmbNhanVien.Name = "cmbNhanVien";
            this.cmbNhanVien.Size = new System.Drawing.Size(252, 29);
            this.cmbNhanVien.TabIndex = 0;
            this.cmbNhanVien.ValueMember = "MANV";
            // 
            // bdsNV1
            // 
            this.bdsNV1.DataMember = "NhanVien1";
            this.bdsNV1.DataSource = this.QLVTDataSet;
            // 
            // QLVTDataSet
            // 
            this.QLVTDataSet.DataSetName = "QLVTDataSet";
            this.QLVTDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // HOTEN
            // 
            this.HOTEN.ClearBeforeFill = true;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // FormTaoTaiKhoan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(929, 432);
            this.Controls.Add(this.panel1);
            this.Name = "FormTaoTaiKhoan";
            this.Text = "FormTaoTaiKhoan";
            this.Load += new System.EventHandler(this.FormTaoTaiKhoan_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.separatorControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLoginName.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bdsNV1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.QLVTDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private DevExpress.XtraEditors.TextEdit txtLoginName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbNhanVien;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnThoat;
        private System.Windows.Forms.Button btnTao;
        private System.Windows.Forms.RadioButton radioButton_User;
        private System.Windows.Forms.RadioButton radioButton_ChiNhanh;
        private DevExpress.XtraEditors.SeparatorControl separatorControl1;
        private QLVTDataSet QLVTDataSet;
        private System.Windows.Forms.BindingSource bdsNV1;
        private QLVTDataSetTableAdapters.HOTEN HOTEN;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtMatKhauXacNhan;
        private System.Windows.Forms.TextBox txtMatKhau;
    }
}