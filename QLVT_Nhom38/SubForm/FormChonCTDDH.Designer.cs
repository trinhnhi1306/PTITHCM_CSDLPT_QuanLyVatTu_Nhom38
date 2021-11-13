namespace QLVT_Nhom38.SubForm
{
    partial class FormChonCTDDH
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTenVatTu = new DevExpress.XtraEditors.TextEdit();
            this.txtMaVT = new DevExpress.XtraEditors.TextEdit();
            this.label2 = new System.Windows.Forms.Label();
            this.txtmaDDH = new DevExpress.XtraEditors.TextEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.nbrSoLuong = new System.Windows.Forms.NumericUpDown();
            this.nbrDonGia = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.DONGIA = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SOLUONG = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TENVT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MAVT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MasoDDH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCTDDH = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnAll = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.txtTenVatTu.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaVT.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmaDDH.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrSoLuong)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrDonGia)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCTDDH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(597, 109);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tên Vật Tư";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // txtTenVatTu
            // 
            this.txtTenVatTu.Location = new System.Drawing.Point(700, 109);
            this.txtTenVatTu.Name = "txtTenVatTu";
            this.txtTenVatTu.Size = new System.Drawing.Size(168, 20);
            this.txtTenVatTu.TabIndex = 2;
            this.txtTenVatTu.Validating += new System.ComponentModel.CancelEventHandler(this.txtTenVatTu_Validating);
            // 
            // txtMaVT
            // 
            this.txtMaVT.Location = new System.Drawing.Point(700, 165);
            this.txtMaVT.Name = "txtMaVT";
            this.txtMaVT.Size = new System.Drawing.Size(168, 20);
            this.txtMaVT.TabIndex = 4;
            this.txtMaVT.Validating += new System.ComponentModel.CancelEventHandler(this.txtMaVT_Validating);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(597, 165);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Mã Vật Tư";
            // 
            // txtmaDDH
            // 
            this.txtmaDDH.Location = new System.Drawing.Point(700, 55);
            this.txtmaDDH.Name = "txtmaDDH";
            this.txtmaDDH.Size = new System.Drawing.Size(168, 20);
            this.txtmaDDH.TabIndex = 6;
            this.txtmaDDH.Validating += new System.ComponentModel.CancelEventHandler(this.txtmaDDH_Validating);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(597, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Mã Đơn Đặt Hàng";
            // 
            // nbrSoLuong
            // 
            this.nbrSoLuong.Location = new System.Drawing.Point(700, 218);
            this.nbrSoLuong.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.nbrSoLuong.Name = "nbrSoLuong";
            this.nbrSoLuong.Size = new System.Drawing.Size(120, 20);
            this.nbrSoLuong.TabIndex = 7;
            this.nbrSoLuong.Validating += new System.ComponentModel.CancelEventHandler(this.nbrSoLuong_Validating);
            // 
            // nbrDonGia
            // 
            this.nbrDonGia.Increment = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nbrDonGia.Location = new System.Drawing.Point(700, 280);
            this.nbrDonGia.Maximum = new decimal(new int[] {
            1410065407,
            2,
            0,
            0});
            this.nbrDonGia.Name = "nbrDonGia";
            this.nbrDonGia.Size = new System.Drawing.Size(120, 20);
            this.nbrDonGia.TabIndex = 8;
            this.nbrDonGia.Validating += new System.ComponentModel.CancelEventHandler(this.nbrDonGia_Validating);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(597, 225);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Số Lượng";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(597, 287);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Đơn giá";
            // 
            // DONGIA
            // 
            this.DONGIA.DataPropertyName = "DONGIA";
            this.DONGIA.HeaderText = "Đơn Gía";
            this.DONGIA.Name = "DONGIA";
            // 
            // SOLUONG
            // 
            this.SOLUONG.DataPropertyName = "SOLUONG";
            this.SOLUONG.HeaderText = "Số Lượng";
            this.SOLUONG.Name = "SOLUONG";
            // 
            // TENVT
            // 
            this.TENVT.DataPropertyName = "TENVT";
            this.TENVT.HeaderText = "Tên Vật Tư";
            this.TENVT.Name = "TENVT";
            // 
            // MAVT
            // 
            this.MAVT.DataPropertyName = "MAVT";
            this.MAVT.HeaderText = "Mã Vật Tư";
            this.MAVT.Name = "MAVT";
            // 
            // MasoDDH
            // 
            this.MasoDDH.DataPropertyName = "MasoDDH";
            this.MasoDDH.HeaderText = "Mã DDH";
            this.MasoDDH.Name = "MasoDDH";
            // 
            // dgvCTDDH
            // 
            this.dgvCTDDH.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCTDDH.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MasoDDH,
            this.MAVT,
            this.TENVT,
            this.SOLUONG,
            this.DONGIA});
            this.dgvCTDDH.Location = new System.Drawing.Point(12, 59);
            this.dgvCTDDH.Name = "dgvCTDDH";
            this.dgvCTDDH.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCTDDH.Size = new System.Drawing.Size(547, 296);
            this.dgvCTDDH.TabIndex = 0;
            this.dgvCTDDH.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvCTDDH_CellContentClick);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.Highlight;
            this.label6.Location = new System.Drawing.Point(110, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(266, 24);
            this.label6.TabIndex = 11;
            this.label6.Text = "CHI TIẾT ĐƠN ĐẶT HÀNG";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(644, 332);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "Chọn";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(761, 332);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(75, 23);
            this.btnAll.TabIndex = 13;
            this.btnAll.Text = "Lấy hết";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // FormChonCTDDH
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 404);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nbrDonGia);
            this.Controls.Add(this.nbrSoLuong);
            this.Controls.Add(this.txtmaDDH);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMaVT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtTenVatTu);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvCTDDH);
            this.Name = "FormChonCTDDH";
            this.Text = "FormChonCTDDH";
            ((System.ComponentModel.ISupportInitialize)(this.txtTenVatTu.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtMaVT.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtmaDDH.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrSoLuong)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nbrDonGia)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCTDDH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private DevExpress.XtraEditors.TextEdit txtTenVatTu;
        private DevExpress.XtraEditors.TextEdit txtMaVT;
        private System.Windows.Forms.Label label2;
        private DevExpress.XtraEditors.TextEdit txtmaDDH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nbrSoLuong;
        private System.Windows.Forms.NumericUpDown nbrDonGia;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn DONGIA;
        private System.Windows.Forms.DataGridViewTextBoxColumn SOLUONG;
        private System.Windows.Forms.DataGridViewTextBoxColumn TENVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn MAVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn MasoDDH;
        private System.Windows.Forms.DataGridView dgvCTDDH;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ErrorProvider errorProvider;
        private System.Windows.Forms.Button btnAll;
    }
}