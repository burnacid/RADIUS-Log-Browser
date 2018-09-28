namespace Radius_Log_Browser
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.dataSet1 = new System.Data.DataSet();
            this.lvLogTable = new System.Windows.Forms.ListView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.cmRemoveSearch = new System.Windows.Forms.ToolStripMenuItem();
            this.cmSearchSep = new System.Windows.Forms.ToolStripSeparator();
            this.cmCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.loadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToUseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataSet1
            // 
            this.dataSet1.DataSetName = "NewDataSet";
            // 
            // lvLogTable
            // 
            this.lvLogTable.AllowColumnReorder = true;
            this.lvLogTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLogTable.ContextMenuStrip = this.contextMenuStrip1;
            this.lvLogTable.FullRowSelect = true;
            this.lvLogTable.Location = new System.Drawing.Point(12, 27);
            this.lvLogTable.Name = "lvLogTable";
            this.lvLogTable.Size = new System.Drawing.Size(1039, 551);
            this.lvLogTable.SmallImageList = this.imageList1;
            this.lvLogTable.TabIndex = 0;
            this.lvLogTable.UseCompatibleStateImageBehavior = false;
            this.lvLogTable.View = System.Windows.Forms.View.Details;
            this.lvLogTable.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvLogTable_ColumnClick);
            this.lvLogTable.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvLogTable_ItemSelectionChanged);
            this.lvLogTable.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvLogTable_KeyDown);
            this.lvLogTable.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lvLogTable_MouseDown);
            this.lvLogTable.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lvLogTable_MouseMove);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmSearch,
            this.cmRemoveSearch,
            this.cmSearchSep,
            this.cmCopy,
            this.clearToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(155, 98);
            // 
            // cmSearch
            // 
            this.cmSearch.Name = "cmSearch";
            this.cmSearch.Size = new System.Drawing.Size(154, 22);
            this.cmSearch.Text = "Search on";
            this.cmSearch.Visible = false;
            this.cmSearch.Click += new System.EventHandler(this.cmSearch_Click);
            // 
            // cmRemoveSearch
            // 
            this.cmRemoveSearch.Name = "cmRemoveSearch";
            this.cmRemoveSearch.Size = new System.Drawing.Size(154, 22);
            this.cmRemoveSearch.Text = "Remove search";
            this.cmRemoveSearch.Visible = false;
            this.cmRemoveSearch.Click += new System.EventHandler(this.cmRemoveSearch_Click);
            // 
            // cmSearchSep
            // 
            this.cmSearchSep.Name = "cmSearchSep";
            this.cmSearchSep.Size = new System.Drawing.Size(151, 6);
            // 
            // cmCopy
            // 
            this.cmCopy.Enabled = false;
            this.cmCopy.Name = "cmCopy";
            this.cmCopy.ShortcutKeyDisplayString = "Ctrl+C";
            this.cmCopy.Size = new System.Drawing.Size(154, 22);
            this.cmCopy.Text = "Copy";
            this.cmCopy.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // clearToolStripMenuItem
            // 
            this.clearToolStripMenuItem.Name = "clearToolStripMenuItem";
            this.clearToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.clearToolStripMenuItem.Text = "Clear";
            this.clearToolStripMenuItem.Click += new System.EventHandler(this.clearToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "1476368477_BT_sort_az.png");
            this.imageList1.Images.SetKeyName(1, "1476368485_BT_sort_za.png");
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadFileToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.howToUseToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1063, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // loadFileToolStripMenuItem
            // 
            this.loadFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectFileToolStripMenuItem});
            this.loadFileToolStripMenuItem.Name = "loadFileToolStripMenuItem";
            this.loadFileToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.loadFileToolStripMenuItem.Text = "Load";
            // 
            // selectFileToolStripMenuItem
            // 
            this.selectFileToolStripMenuItem.Name = "selectFileToolStripMenuItem";
            this.selectFileToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.selectFileToolStripMenuItem.Text = "Select File";
            this.selectFileToolStripMenuItem.Click += new System.EventHandler(this.selectFileToolStripMenuItem_Click);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(95, 20);
            this.exportToolStripMenuItem.Text = "Export to Excel";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // howToUseToolStripMenuItem
            // 
            this.howToUseToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.howToUseToolStripMenuItem.Image = global::Radius_Log_Browser.Properties.Resources.help_icon;
            this.howToUseToolStripMenuItem.Name = "howToUseToolStripMenuItem";
            this.howToUseToolStripMenuItem.Size = new System.Drawing.Size(28, 20);
            this.howToUseToolStripMenuItem.Click += new System.EventHandler(this.howToUseToolStripMenuItem_Click);
            // 
            // ofdFile
            // 
            this.ofdFile.Filter = "Log files|*.log";
            this.ofdFile.Title = "Select a Radius Log";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1063, 590);
            this.Controls.Add(this.lvLogTable);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "RADIUS Log Browser";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.dataSet1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Data.DataSet dataSet1;
        private System.Windows.Forms.ListView lvLogTable;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem loadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToUseToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem cmCopy;
        private System.Windows.Forms.ToolStripMenuItem cmSearch;
        private System.Windows.Forms.ToolStripSeparator cmSearchSep;
        private System.Windows.Forms.ToolStripMenuItem clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cmRemoveSearch;
        private System.Windows.Forms.OpenFileDialog ofdFile;
    }
}

