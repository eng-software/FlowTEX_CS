namespace FlowTEX
{
    partial class frmFlowTex
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFlowTex));
            this.lblFlow = new System.Windows.Forms.Label();
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.icoFlowTEX = new System.Windows.Forms.PictureBox();
            this.comboSerialFlowTex = new System.Windows.Forms.ComboBox();
            this.btnAbrirFlowTEX = new System.Windows.Forms.Button();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.imgStatus = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.icoFlowTEX)).BeginInit();
            this.SuspendLayout();
            // 
            // lblFlow
            // 
            this.lblFlow.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblFlow.Font = new System.Drawing.Font("Courier New", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFlow.Location = new System.Drawing.Point(28, 68);
            this.lblFlow.Name = "lblFlow";
            this.lblFlow.Size = new System.Drawing.Size(238, 30);
            this.lblFlow.TabIndex = 0;
            this.lblFlow.Text = "lblFlow";
            this.lblFlow.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // icoFlowTEX
            // 
            this.icoFlowTEX.Location = new System.Drawing.Point(249, 13);
            this.icoFlowTEX.Name = "icoFlowTEX";
            this.icoFlowTEX.Size = new System.Drawing.Size(20, 20);
            this.icoFlowTEX.TabIndex = 39;
            this.icoFlowTEX.TabStop = false;
            // 
            // comboSerialFlowTex
            // 
            this.comboSerialFlowTex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboSerialFlowTex.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.comboSerialFlowTex.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboSerialFlowTex.FormattingEnabled = true;
            this.comboSerialFlowTex.Location = new System.Drawing.Point(67, 11);
            this.comboSerialFlowTex.Name = "comboSerialFlowTex";
            this.comboSerialFlowTex.Size = new System.Drawing.Size(95, 24);
            this.comboSerialFlowTex.TabIndex = 37;
            // 
            // btnAbrirFlowTEX
            // 
            this.btnAbrirFlowTEX.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbrirFlowTEX.Location = new System.Drawing.Point(168, 12);
            this.btnAbrirFlowTEX.Name = "btnAbrirFlowTEX";
            this.btnAbrirFlowTEX.Size = new System.Drawing.Size(75, 23);
            this.btnAbrirFlowTEX.TabIndex = 38;
            this.btnAbrirFlowTEX.Text = "Abrir";
            this.btnAbrirFlowTEX.UseVisualStyleBackColor = true;
            this.btnAbrirFlowTEX.Click += new System.EventHandler(this.btnAbrirFlowTEX_Click);
            // 
            // lblTemperature
            // 
            this.lblTemperature.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.lblTemperature.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemperature.Location = new System.Drawing.Point(34, 118);
            this.lblTemperature.Name = "lblTemperature";
            this.lblTemperature.Size = new System.Drawing.Size(232, 32);
            this.lblTemperature.TabIndex = 40;
            this.lblTemperature.Text = "lblTemperature";
            this.lblTemperature.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // imgStatus
            // 
            this.imgStatus.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgStatus.ImageStream")));
            this.imgStatus.TransparentColor = System.Drawing.Color.White;
            this.imgStatus.Images.SetKeyName(0, "OK.png");
            this.imgStatus.Images.SetKeyName(1, "Alert.png");
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 41;
            this.label1.Text = "Serial :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 16);
            this.label2.TabIndex = 42;
            this.label2.Text = "Vazão:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 16);
            this.label3.TabIndex = 43;
            this.label3.Text = "Temperatura:";
            // 
            // frmFlowTex
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(280, 167);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTemperature);
            this.Controls.Add(this.icoFlowTEX);
            this.Controls.Add(this.comboSerialFlowTex);
            this.Controls.Add(this.btnAbrirFlowTEX);
            this.Controls.Add(this.lblFlow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFlowTex";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TEX - Sensor FlowTEX";
            ((System.ComponentModel.ISupportInitialize)(this.icoFlowTEX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblFlow;
        private System.Windows.Forms.PictureBox icoFlowTEX;
        private System.Windows.Forms.ComboBox comboSerialFlowTex;
        private System.Windows.Forms.Button btnAbrirFlowTEX;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.ImageList imgStatus;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}

