namespace VisualFileFormat
{
    partial class frmMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsert = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataType = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeByte = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeSByte = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeShort = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeUShort = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeInt = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeUInt = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeLong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeULong = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeSingle = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeDouble = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeDecimal = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeChar = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeString = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuInsertBasicDataTypeBoolean = new System.Windows.Forms.ToolStripMenuItem();
            this.compoundTypeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabErrors = new System.Windows.Forms.TabPage();
            this.tabWarnings = new System.Windows.Forms.TabPage();
            this.lstErrors = new System.Windows.Forms.ListBox();
            this.lstWarnings = new System.Windows.Forms.ListBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pgProperties = new System.Windows.Forms.PropertyGrid();
            this.tvTree = new System.Windows.Forms.TreeView();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabErrors.SuspendLayout();
            this.tabWarnings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.mnuInsert});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(942, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // mnuInsert
            // 
            this.mnuInsert.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInsertBasicDataType,
            this.compoundTypeToolStripMenuItem});
            this.mnuInsert.Name = "mnuInsert";
            this.mnuInsert.Size = new System.Drawing.Size(48, 20);
            this.mnuInsert.Text = "Insert";
            // 
            // mnuInsertBasicDataType
            // 
            this.mnuInsertBasicDataType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuInsertBasicDataTypeByte,
            this.mnuInsertBasicDataTypeSByte,
            this.mnuInsertBasicDataTypeShort,
            this.mnuInsertBasicDataTypeUShort,
            this.mnuInsertBasicDataTypeInt,
            this.mnuInsertBasicDataTypeUInt,
            this.mnuInsertBasicDataTypeLong,
            this.mnuInsertBasicDataTypeULong,
            this.mnuInsertBasicDataTypeSingle,
            this.mnuInsertBasicDataTypeDouble,
            this.mnuInsertBasicDataTypeDecimal,
            this.mnuInsertBasicDataTypeChar,
            this.mnuInsertBasicDataTypeString,
            this.mnuInsertBasicDataTypeBoolean});
            this.mnuInsertBasicDataType.Name = "mnuInsertBasicDataType";
            this.mnuInsertBasicDataType.Size = new System.Drawing.Size(173, 22);
            this.mnuInsertBasicDataType.Text = "Basic Data Type";
            // 
            // mnuInsertBasicDataTypeByte
            // 
            this.mnuInsertBasicDataTypeByte.Name = "mnuInsertBasicDataTypeByte";
            this.mnuInsertBasicDataTypeByte.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeByte.Text = "Byte";
            this.mnuInsertBasicDataTypeByte.Click += new System.EventHandler(this.mnuInsertBasicDataTypeByte_Click);
            // 
            // mnuInsertBasicDataTypeSByte
            // 
            this.mnuInsertBasicDataTypeSByte.Name = "mnuInsertBasicDataTypeSByte";
            this.mnuInsertBasicDataTypeSByte.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeSByte.Text = "Signed Byte";
            // 
            // mnuInsertBasicDataTypeShort
            // 
            this.mnuInsertBasicDataTypeShort.Name = "mnuInsertBasicDataTypeShort";
            this.mnuInsertBasicDataTypeShort.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeShort.Text = "Signed Short";
            // 
            // mnuInsertBasicDataTypeUShort
            // 
            this.mnuInsertBasicDataTypeUShort.Name = "mnuInsertBasicDataTypeUShort";
            this.mnuInsertBasicDataTypeUShort.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeUShort.Text = "Unsigned Short";
            // 
            // mnuInsertBasicDataTypeInt
            // 
            this.mnuInsertBasicDataTypeInt.Name = "mnuInsertBasicDataTypeInt";
            this.mnuInsertBasicDataTypeInt.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeInt.Text = "Signed Integer";
            // 
            // mnuInsertBasicDataTypeUInt
            // 
            this.mnuInsertBasicDataTypeUInt.Name = "mnuInsertBasicDataTypeUInt";
            this.mnuInsertBasicDataTypeUInt.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeUInt.Text = "Unsigned Integer";
            // 
            // mnuInsertBasicDataTypeLong
            // 
            this.mnuInsertBasicDataTypeLong.Name = "mnuInsertBasicDataTypeLong";
            this.mnuInsertBasicDataTypeLong.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeLong.Text = "Signed Long";
            // 
            // mnuInsertBasicDataTypeULong
            // 
            this.mnuInsertBasicDataTypeULong.Name = "mnuInsertBasicDataTypeULong";
            this.mnuInsertBasicDataTypeULong.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeULong.Text = "Unsigned Long";
            // 
            // mnuInsertBasicDataTypeSingle
            // 
            this.mnuInsertBasicDataTypeSingle.Name = "mnuInsertBasicDataTypeSingle";
            this.mnuInsertBasicDataTypeSingle.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeSingle.Text = "Single";
            // 
            // mnuInsertBasicDataTypeDouble
            // 
            this.mnuInsertBasicDataTypeDouble.Name = "mnuInsertBasicDataTypeDouble";
            this.mnuInsertBasicDataTypeDouble.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeDouble.Text = "Double";
            // 
            // mnuInsertBasicDataTypeDecimal
            // 
            this.mnuInsertBasicDataTypeDecimal.Name = "mnuInsertBasicDataTypeDecimal";
            this.mnuInsertBasicDataTypeDecimal.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeDecimal.Text = "Decimal";
            // 
            // mnuInsertBasicDataTypeChar
            // 
            this.mnuInsertBasicDataTypeChar.Name = "mnuInsertBasicDataTypeChar";
            this.mnuInsertBasicDataTypeChar.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeChar.Text = "Char";
            // 
            // mnuInsertBasicDataTypeString
            // 
            this.mnuInsertBasicDataTypeString.Name = "mnuInsertBasicDataTypeString";
            this.mnuInsertBasicDataTypeString.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeString.Text = "String";
            // 
            // mnuInsertBasicDataTypeBoolean
            // 
            this.mnuInsertBasicDataTypeBoolean.Name = "mnuInsertBasicDataTypeBoolean";
            this.mnuInsertBasicDataTypeBoolean.Size = new System.Drawing.Size(164, 22);
            this.mnuInsertBasicDataTypeBoolean.Text = "Boolean";
            // 
            // compoundTypeToolStripMenuItem
            // 
            this.compoundTypeToolStripMenuItem.Name = "compoundTypeToolStripMenuItem";
            this.compoundTypeToolStripMenuItem.Size = new System.Drawing.Size(173, 22);
            this.compoundTypeToolStripMenuItem.Text = "Compound Type...";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(942, 502);
            this.splitContainer1.SplitterDistance = 300;
            this.splitContainer1.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabErrors);
            this.tabControl1.Controls.Add(this.tabWarnings);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(942, 198);
            this.tabControl1.TabIndex = 0;
            // 
            // tabErrors
            // 
            this.tabErrors.Controls.Add(this.lstErrors);
            this.tabErrors.Location = new System.Drawing.Point(4, 22);
            this.tabErrors.Name = "tabErrors";
            this.tabErrors.Padding = new System.Windows.Forms.Padding(3);
            this.tabErrors.Size = new System.Drawing.Size(934, 172);
            this.tabErrors.TabIndex = 0;
            this.tabErrors.Text = "Errors";
            this.tabErrors.UseVisualStyleBackColor = true;
            // 
            // tabWarnings
            // 
            this.tabWarnings.Controls.Add(this.lstWarnings);
            this.tabWarnings.Location = new System.Drawing.Point(4, 22);
            this.tabWarnings.Name = "tabWarnings";
            this.tabWarnings.Padding = new System.Windows.Forms.Padding(3);
            this.tabWarnings.Size = new System.Drawing.Size(524, 126);
            this.tabWarnings.TabIndex = 1;
            this.tabWarnings.Text = "Warnings";
            this.tabWarnings.UseVisualStyleBackColor = true;
            // 
            // lstErrors
            // 
            this.lstErrors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstErrors.FormattingEnabled = true;
            this.lstErrors.Location = new System.Drawing.Point(3, 3);
            this.lstErrors.Name = "lstErrors";
            this.lstErrors.Size = new System.Drawing.Size(928, 166);
            this.lstErrors.TabIndex = 2;
            // 
            // lstWarnings
            // 
            this.lstWarnings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstWarnings.FormattingEnabled = true;
            this.lstWarnings.Location = new System.Drawing.Point(3, 3);
            this.lstWarnings.Name = "lstWarnings";
            this.lstWarnings.Size = new System.Drawing.Size(518, 120);
            this.lstWarnings.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tvTree);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer2.Size = new System.Drawing.Size(942, 300);
            this.splitContainer2.SplitterDistance = 584;
            this.splitContainer2.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pgProperties);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(354, 300);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Properties";
            // 
            // pgProperties
            // 
            this.pgProperties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgProperties.Location = new System.Drawing.Point(3, 16);
            this.pgProperties.Name = "pgProperties";
            this.pgProperties.Size = new System.Drawing.Size(348, 281);
            this.pgProperties.TabIndex = 1;
            // 
            // tvTree
            // 
            this.tvTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvTree.Location = new System.Drawing.Point(0, 0);
            this.tvTree.Name = "tvTree";
            this.tvTree.Size = new System.Drawing.Size(584, 300);
            this.tvTree.TabIndex = 0;
            this.tvTree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvTree_AfterSelect);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 526);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabErrors.ResumeLayout(false);
            this.tabWarnings.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mnuInsert;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataType;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeByte;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeSByte;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeShort;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeUShort;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeInt;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeUInt;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeLong;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeULong;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeSingle;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeDouble;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeDecimal;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeChar;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeString;
        private System.Windows.Forms.ToolStripMenuItem mnuInsertBasicDataTypeBoolean;
        private System.Windows.Forms.ToolStripMenuItem compoundTypeToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabErrors;
        private System.Windows.Forms.ListBox lstErrors;
        private System.Windows.Forms.TabPage tabWarnings;
        private System.Windows.Forms.ListBox lstWarnings;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView tvTree;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PropertyGrid pgProperties;
    }
}

