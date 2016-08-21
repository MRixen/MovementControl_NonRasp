namespace WindowsFormsApplication6
{
    partial class DatabaseList
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
            this.listViewDatabaseContent = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.numericUpDown_tableSelector = new System.Windows.Forms.NumericUpDown();
            this.labelListEntries = new System.Windows.Forms.Label();
            this.labelSensorId = new System.Windows.Forms.Label();
            this.backgroundWorker_readDataset = new System.ComponentModel.BackgroundWorker();
            this.labelDatabaseId = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_tableSelector)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewDatabaseContent
            // 
            this.listViewDatabaseContent.BackColor = System.Drawing.SystemColors.Window;
            this.listViewDatabaseContent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.listViewDatabaseContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewDatabaseContent.Location = new System.Drawing.Point(3, 59);
            this.listViewDatabaseContent.Name = "listViewDatabaseContent";
            this.listViewDatabaseContent.Size = new System.Drawing.Size(328, 500);
            this.listViewDatabaseContent.TabIndex = 0;
            this.listViewDatabaseContent.UseCompatibleStateImageBehavior = false;
            this.listViewDatabaseContent.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "x";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "y";
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "z";
            this.columnHeader3.Width = 59;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "time";
            this.columnHeader4.Width = 73;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.listViewDatabaseContent, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(334, 562);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelDatabaseId);
            this.panel1.Controls.Add(this.numericUpDown_tableSelector);
            this.panel1.Controls.Add(this.labelListEntries);
            this.panel1.Controls.Add(this.labelSensorId);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.MinimumSize = new System.Drawing.Size(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(328, 50);
            this.panel1.TabIndex = 2;
            // 
            // numericUpDown_tableSelector
            // 
            this.numericUpDown_tableSelector.Location = new System.Drawing.Point(72, 5);
            this.numericUpDown_tableSelector.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numericUpDown_tableSelector.Name = "numericUpDown_tableSelector";
            this.numericUpDown_tableSelector.Size = new System.Drawing.Size(47, 20);
            this.numericUpDown_tableSelector.TabIndex = 5;
            this.numericUpDown_tableSelector.ValueChanged += new System.EventHandler(this.numericUpDown_valueChanged);
            // 
            // labelListEntries
            // 
            this.labelListEntries.AutoSize = true;
            this.labelListEntries.Location = new System.Drawing.Point(134, 8);
            this.labelListEntries.Name = "labelListEntries";
            this.labelListEntries.Size = new System.Drawing.Size(63, 13);
            this.labelListEntries.TabIndex = 4;
            this.labelListEntries.Text = "List entries: ";
            // 
            // labelSensorId
            // 
            this.labelSensorId.AutoSize = true;
            this.labelSensorId.Location = new System.Drawing.Point(9, 8);
            this.labelSensorId.Name = "labelSensorId";
            this.labelSensorId.Size = new System.Drawing.Size(57, 13);
            this.labelSensorId.TabIndex = 3;
            this.labelSensorId.Text = "Sensor ID:";
            // 
            // labelDatabaseId
            // 
            this.labelDatabaseId.AutoSize = true;
            this.labelDatabaseId.Location = new System.Drawing.Point(134, 28);
            this.labelDatabaseId.Name = "labelDatabaseId";
            this.labelDatabaseId.Size = new System.Drawing.Size(73, 13);
            this.labelDatabaseId.TabIndex = 6;
            this.labelDatabaseId.Text = "Database ID: ";
            // 
            // DatabaseList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 562);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MaximumSize = new System.Drawing.Size(350, 600);
            this.MinimumSize = new System.Drawing.Size(350, 222);
            this.Name = "DatabaseList";
            this.Text = "DatabaseList";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormDatabaseList_Closing);
            this.Load += new System.EventHandler(this.FormDatabase_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_tableSelector)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewDatabaseContent;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelSensorId;
        private System.Windows.Forms.Label labelListEntries;
        private System.Windows.Forms.NumericUpDown numericUpDown_tableSelector;
        private System.Windows.Forms.Label labelDatabaseId;
    }
}