namespace WindowsFormsApplication6
{
    partial class FormMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.button_startControlAlgorithm = new System.Windows.Forms.Button();
            this.checkBox_showDatabase = new System.Windows.Forms.CheckBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.backgroundWorker_CreateLocalDb = new System.ComponentModel.BackgroundWorker();
            this.backgroundWorker_InitComPort = new System.ComponentModel.BackgroundWorker(); 
            this.backgroundWorker_DeleteDb = new System.ComponentModel.BackgroundWorker(); 
            this.textBox_Info = new System.Windows.Forms.TextBox();
            this.button_Ok = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button_startControlAlgorithm
            // 
            this.button_startControlAlgorithm.Enabled = false;
            this.button_startControlAlgorithm.Location = new System.Drawing.Point(13, 17);
            this.button_startControlAlgorithm.Margin = new System.Windows.Forms.Padding(4);
            this.button_startControlAlgorithm.Name = "button_startControlAlgorithm";
            this.button_startControlAlgorithm.Size = new System.Drawing.Size(181, 28);
            this.button_startControlAlgorithm.TabIndex = 9;
            this.button_startControlAlgorithm.Text = "Start Control Algorithm";
            this.button_startControlAlgorithm.UseVisualStyleBackColor = true;
            this.button_startControlAlgorithm.Click += new System.EventHandler(this.startControlAlgorithm_Click);
            // 
            // checkBox_showDatabase
            // 
            this.checkBox_showDatabase.AutoSize = true;
            this.checkBox_showDatabase.Enabled = false;
            this.checkBox_showDatabase.Location = new System.Drawing.Point(30, 92);
            this.checkBox_showDatabase.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox_showDatabase.Name = "checkBox_showDatabase";
            this.checkBox_showDatabase.Size = new System.Drawing.Size(127, 21);
            this.checkBox_showDatabase.TabIndex = 12;
            this.checkBox_showDatabase.Text = "Show database";
            this.checkBox_showDatabase.UseVisualStyleBackColor = true;
            this.checkBox_showDatabase.CheckedChanged += new System.EventHandler(this.checkBox_showDatabase_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(67, 4);
            // 
            // textBox_Info
            // 
            this.textBox_Info.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox_Info.ForeColor = System.Drawing.Color.Red;
            this.textBox_Info.Location = new System.Drawing.Point(7, 26);
            this.textBox_Info.Multiline = true;
            this.textBox_Info.Name = "textBox_Info";
            this.textBox_Info.Size = new System.Drawing.Size(539, 50);
            this.textBox_Info.TabIndex = 40;
            // 
            // button_Ok
            // 
            this.button_Ok.Location = new System.Drawing.Point(553, 32);
            this.button_Ok.Margin = new System.Windows.Forms.Padding(4);
            this.button_Ok.Name = "button_Ok";
            this.button_Ok.Size = new System.Drawing.Size(53, 34);
            this.button_Ok.TabIndex = 42;
            this.button_Ok.Text = "Clear";
            this.button_Ok.UseVisualStyleBackColor = true;
            this.button_Ok.Click += new System.EventHandler(this.button_clearInfo_Clicked);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_Info);
            this.groupBox2.Controls.Add(this.button_Ok);
            this.groupBox2.Location = new System.Drawing.Point(13, 348);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(617, 92);
            this.groupBox2.TabIndex = 40;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Information";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 453);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button_startControlAlgorithm);
            this.Controls.Add(this.checkBox_showDatabase);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximumSize = new System.Drawing.Size(661, 500);
            this.MinimumSize = new System.Drawing.Size(661, 500);
            this.Name = "FormMain";
            this.Text = "Movement Diagnose";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_Closed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private System.Windows.Forms.Button button_startControlAlgorithm;
        private System.Windows.Forms.CheckBox checkBox_showDatabase;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox textBox_Info;
        private System.Windows.Forms.Button button_Ok;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}