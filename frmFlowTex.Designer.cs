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
            this.tmrRefresh = new System.Windows.Forms.Timer(this.components);
            this.comboSerialFlowTex = new System.Windows.Forms.ComboBox();
            this.btnAbrirFlowTEX = new System.Windows.Forms.Button();
            this.lblTemperature = new System.Windows.Forms.Label();
            this.imgStatus = new System.Windows.Forms.ImageList(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblModel = new System.Windows.Forms.Label();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.lblVersion = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFlow = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.comboFlowUnit = new System.Windows.Forms.ComboBox();
            this.btnZero = new System.Windows.Forms.Button();
            this.lblUnit = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.timerZero = new System.Windows.Forms.Timer(this.components);
            this.btnClrZero = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.lblStandarization = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnChangeI2CAddress = new System.Windows.Forms.Button();
            this.edtI2CAddress = new System.Windows.Forms.TextBox();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.icoFlowTEX = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.icoFlowTEX)).BeginInit();
            this.SuspendLayout();
            // 
            // tmrRefresh
            // 
            this.tmrRefresh.Enabled = true;
            this.tmrRefresh.Tick += new System.EventHandler(this.tmrRefresh_Tick);
            // 
            // comboSerialFlowTex
            // 
            this.comboSerialFlowTex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboSerialFlowTex, "comboSerialFlowTex");
            this.comboSerialFlowTex.FormattingEnabled = true;
            this.comboSerialFlowTex.Name = "comboSerialFlowTex";
            // 
            // btnAbrirFlowTEX
            // 
            resources.ApplyResources(this.btnAbrirFlowTEX, "btnAbrirFlowTEX");
            this.btnAbrirFlowTEX.Name = "btnAbrirFlowTEX";
            this.btnAbrirFlowTEX.UseVisualStyleBackColor = true;
            this.btnAbrirFlowTEX.Click += new System.EventHandler(this.btnAbrirFlowTEX_Click);
            // 
            // lblTemperature
            // 
            this.lblTemperature.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.lblTemperature, "lblTemperature");
            this.lblTemperature.Name = "lblTemperature";
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
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // lblModel
            // 
            this.lblModel.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.lblModel, "lblModel");
            this.lblModel.Name = "lblModel";
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.lblSerialNumber, "lblSerialNumber");
            this.lblSerialNumber.Name = "lblSerialNumber";
            // 
            // lblVersion
            // 
            this.lblVersion.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.lblVersion, "lblVersion");
            this.lblVersion.Name = "lblVersion";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // lblFlow
            // 
            this.lblFlow.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.lblFlow, "lblFlow");
            this.lblFlow.Name = "lblFlow";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // comboFlowUnit
            // 
            this.comboFlowUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboFlowUnit, "comboFlowUnit");
            this.comboFlowUnit.FormattingEnabled = true;
            this.comboFlowUnit.Name = "comboFlowUnit";
            this.comboFlowUnit.SelectedIndexChanged += new System.EventHandler(this.comboFlowUnit_SelectedIndexChanged);
            // 
            // btnZero
            // 
            resources.ApplyResources(this.btnZero, "btnZero");
            this.btnZero.Name = "btnZero";
            this.btnZero.UseVisualStyleBackColor = true;
            this.btnZero.Click += new System.EventHandler(this.btnZero_Click);
            this.btnZero.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnZero_MouseDown);
            this.btnZero.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnZero_MouseUp);
            // 
            // lblUnit
            // 
            this.lblUnit.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            resources.ApplyResources(this.lblUnit, "lblUnit");
            this.lblUnit.Name = "lblUnit";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.lblFlow);
            this.panel1.Controls.Add(this.lblUnit);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // progressBar1
            // 
            resources.ApplyResources(this.progressBar1, "progressBar1");
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.Value = 50;
            // 
            // timerZero
            // 
            this.timerZero.Tick += new System.EventHandler(this.timerZero_Tick);
            // 
            // btnClrZero
            // 
            resources.ApplyResources(this.btnClrZero, "btnClrZero");
            this.btnClrZero.Name = "btnClrZero";
            this.btnClrZero.UseVisualStyleBackColor = true;
            this.btnClrZero.Click += new System.EventHandler(this.btnClrZero_Click);
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // lblStandarization
            // 
            resources.ApplyResources(this.lblStandarization, "lblStandarization");
            this.lblStandarization.Name = "lblStandarization";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // btnChangeI2CAddress
            // 
            resources.ApplyResources(this.btnChangeI2CAddress, "btnChangeI2CAddress");
            this.btnChangeI2CAddress.Name = "btnChangeI2CAddress";
            this.btnChangeI2CAddress.UseVisualStyleBackColor = true;
            this.btnChangeI2CAddress.Click += new System.EventHandler(this.btnChangeI2CAddress_Click);
            // 
            // edtI2CAddress
            // 
            resources.ApplyResources(this.edtI2CAddress, "edtI2CAddress");
            this.edtI2CAddress.Name = "edtI2CAddress";
            this.edtI2CAddress.KeyDown += new System.Windows.Forms.KeyEventHandler(this.edtI2CAddress_KeyDown);
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button3.BackgroundImage = global::FlowTEXMonitor.Properties.Resources.usaflag;
            resources.ApplyResources(this.button3, "button3");
            this.button3.Name = "button3";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button2.BackgroundImage = global::FlowTEXMonitor.Properties.Resources.jpflag;
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.button1.BackgroundImage = global::FlowTEXMonitor.Properties.Resources.brflag;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // icoFlowTEX
            // 
            resources.ApplyResources(this.icoFlowTEX, "icoFlowTEX");
            this.icoFlowTEX.Name = "icoFlowTEX";
            this.icoFlowTEX.TabStop = false;
            // 
            // frmFlowTex
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnChangeI2CAddress);
            this.Controls.Add(this.edtI2CAddress);
            this.Controls.Add(this.lblStandarization);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnClrZero);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnZero);
            this.Controls.Add(this.comboFlowUnit);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblVersion);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblSerialNumber);
            this.Controls.Add(this.lblModel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblTemperature);
            this.Controls.Add(this.icoFlowTEX);
            this.Controls.Add(this.comboSerialFlowTex);
            this.Controls.Add(this.btnAbrirFlowTEX);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFlowTex";
            this.Load += new System.EventHandler(this.frmFlowTex_Load);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.icoFlowTEX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox icoFlowTEX;
        private System.Windows.Forms.ComboBox comboSerialFlowTex;
        private System.Windows.Forms.Button btnAbrirFlowTEX;
        private System.Windows.Forms.Label lblTemperature;
        private System.Windows.Forms.ImageList imgStatus;
        private System.Windows.Forms.Timer tmrRefresh;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblModel;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label lblVersion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFlow;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboFlowUnit;
        private System.Windows.Forms.Button btnZero;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timerZero;
        private System.Windows.Forms.Button btnClrZero;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblStandarization;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnChangeI2CAddress;
        private System.Windows.Forms.TextBox edtI2CAddress;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

