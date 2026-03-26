namespace CodeWalker.Project.Panels
{
    partial class EditProjectManifestPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditProjectManifestPanel));
            this.DescriptionLabel = new System.Windows.Forms.Label();
            this.ActionsGroupBox = new System.Windows.Forms.GroupBox();
            this.AutoPopulateButton = new System.Windows.Forms.Button();
            this.ProjectManifestGenerateButton = new System.Windows.Forms.Button();
            this.SaveManifestButton = new System.Windows.Forms.Button();
            this.StatusLabel = new System.Windows.Forms.Label();
            this.MainSplitContainer = new System.Windows.Forms.SplitContainer();
            this.DataGroupBox = new System.Windows.Forms.GroupBox();
            this.ManifestTabControl = new System.Windows.Forms.TabControl();
            this.TabMapDataGroups = new System.Windows.Forms.TabPage();
            this.TabHDTxdBindings = new System.Windows.Forms.TabPage();
            this.TabImapDeps = new System.Windows.Forms.TabPage();
            this.TabImapDeps2 = new System.Windows.Forms.TabPage();
            this.TabItypDeps2 = new System.Windows.Forms.TabPage();
            this.TabInteriors = new System.Windows.Forms.TabPage();
            this.GridMapDataGroups = new System.Windows.Forms.DataGridView();
            this.GridHDTxdBindings = new System.Windows.Forms.DataGridView();
            this.GridImapDeps = new System.Windows.Forms.DataGridView();
            this.GridImapDeps2 = new System.Windows.Forms.DataGridView();
            this.GridItypDeps2 = new System.Windows.Forms.DataGridView();
            this.GridInteriors = new System.Windows.Forms.DataGridView();
            this.XmlGroupBox = new System.Windows.Forms.GroupBox();
            this.ProjectManifestTextBox = new FastColoredTextBoxNS.FastColoredTextBox();
            this.SaveFileDialog = new System.Windows.Forms.SaveFileDialog();

            // Column definitions
            this.ColMDG_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMDG_Flags = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColMDG_HoursOnOff = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColMDG_Bounds = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMDG_WeatherTypes = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColHD_AssetType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColHD_TargetAsset = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColHD_HDTxd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColID_ImapName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColID_ItypName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColID_PackFileName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColID2_ImapName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColID2_ManifestFlags = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColID2_ItypDepArray = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIT2_ItypName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColIT2_ManifestFlags = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ColIT2_ItypDepArray = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColInt_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColInt_Bounds = new System.Windows.Forms.DataGridViewTextBoxColumn();

            this.ActionsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).BeginInit();
            this.MainSplitContainer.Panel1.SuspendLayout();
            this.MainSplitContainer.Panel2.SuspendLayout();
            this.MainSplitContainer.SuspendLayout();
            this.DataGroupBox.SuspendLayout();
            this.XmlGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GridMapDataGroups)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridHDTxdBindings)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridImapDeps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridImapDeps2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridItypDeps2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridInteriors)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectManifestTextBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DescriptionLabel
            // 
            this.DescriptionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DescriptionLabel.Location = new System.Drawing.Point(12, 9);
            this.DescriptionLabel.Name = "DescriptionLabel";
            this.DescriptionLabel.Size = new System.Drawing.Size(598, 30);
            this.DescriptionLabel.TabIndex = 0;
            this.DescriptionLabel.Text = "Generate and edit manifest (_manifest.ymf) data for the current project. Use Auto-Populate to fill tables from project files, or manually add/edit rows in each tab.";
            // 
            // ActionsGroupBox
            // 
            this.ActionsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ActionsGroupBox.Controls.Add(this.AutoPopulateButton);
            this.ActionsGroupBox.Controls.Add(this.ProjectManifestGenerateButton);
            this.ActionsGroupBox.Controls.Add(this.SaveManifestButton);
            this.ActionsGroupBox.Controls.Add(this.StatusLabel);
            this.ActionsGroupBox.Location = new System.Drawing.Point(12, 42);
            this.ActionsGroupBox.Name = "ActionsGroupBox";
            this.ActionsGroupBox.Size = new System.Drawing.Size(598, 55);
            this.ActionsGroupBox.TabIndex = 1;
            this.ActionsGroupBox.TabStop = false;
            this.ActionsGroupBox.Text = "Actions";
            // 
            // AutoPopulateButton
            // 
            this.AutoPopulateButton.Location = new System.Drawing.Point(10, 22);
            this.AutoPopulateButton.Name = "AutoPopulateButton";
            this.AutoPopulateButton.Size = new System.Drawing.Size(100, 24);
            this.AutoPopulateButton.TabIndex = 0;
            this.AutoPopulateButton.Text = "Auto-Populate";
            this.AutoPopulateButton.UseVisualStyleBackColor = true;
            this.AutoPopulateButton.Click += new System.EventHandler(this.AutoPopulateButton_Click);
            // 
            // ProjectManifestGenerateButton
            // 
            this.ProjectManifestGenerateButton.Location = new System.Drawing.Point(116, 22);
            this.ProjectManifestGenerateButton.Name = "ProjectManifestGenerateButton";
            this.ProjectManifestGenerateButton.Size = new System.Drawing.Size(100, 24);
            this.ProjectManifestGenerateButton.TabIndex = 1;
            this.ProjectManifestGenerateButton.Text = "Generate XML";
            this.ProjectManifestGenerateButton.UseVisualStyleBackColor = true;
            this.ProjectManifestGenerateButton.Click += new System.EventHandler(this.ProjectManifestGenerateButton_Click);
            // 
            // SaveManifestButton
            // 
            this.SaveManifestButton.Location = new System.Drawing.Point(222, 22);
            this.SaveManifestButton.Name = "SaveManifestButton";
            this.SaveManifestButton.Size = new System.Drawing.Size(120, 24);
            this.SaveManifestButton.TabIndex = 2;
            this.SaveManifestButton.Text = "Save _manifest.ymf";
            this.SaveManifestButton.UseVisualStyleBackColor = true;
            this.SaveManifestButton.Click += new System.EventHandler(this.SaveManifestButton_Click);
            // 
            // StatusLabel
            // 
            this.StatusLabel.AutoSize = true;
            this.StatusLabel.Location = new System.Drawing.Point(350, 27);
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(38, 13);
            this.StatusLabel.TabIndex = 3;
            this.StatusLabel.Text = "Ready";
            // 
            // MainSplitContainer
            // 
            this.MainSplitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MainSplitContainer.Location = new System.Drawing.Point(12, 103);
            this.MainSplitContainer.Name = "MainSplitContainer";
            this.MainSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.MainSplitContainer.Size = new System.Drawing.Size(598, 445);
            this.MainSplitContainer.SplitterDistance = 250;
            this.MainSplitContainer.TabIndex = 2;
            // 
            // DataGroupBox
            // 
            this.DataGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataGroupBox.Controls.Add(this.ManifestTabControl);
            this.DataGroupBox.Name = "DataGroupBox";
            this.DataGroupBox.TabIndex = 0;
            this.DataGroupBox.TabStop = false;
            this.DataGroupBox.Text = "Manifest Data";
            // 
            // ManifestTabControl
            // 
            this.ManifestTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManifestTabControl.Location = new System.Drawing.Point(3, 16);
            this.ManifestTabControl.Name = "ManifestTabControl";
            this.ManifestTabControl.TabIndex = 0;
            this.ManifestTabControl.TabPages.Add(this.TabMapDataGroups);
            this.ManifestTabControl.TabPages.Add(this.TabHDTxdBindings);
            this.ManifestTabControl.TabPages.Add(this.TabImapDeps);
            this.ManifestTabControl.TabPages.Add(this.TabImapDeps2);
            this.ManifestTabControl.TabPages.Add(this.TabItypDeps2);
            this.ManifestTabControl.TabPages.Add(this.TabInteriors);
            // 
            // Tab pages
            // 
            this.TabMapDataGroups.Text = "MapDataGroups";
            this.TabMapDataGroups.UseVisualStyleBackColor = true;
            this.TabMapDataGroups.Controls.Add(this.GridMapDataGroups);
            this.TabHDTxdBindings.Text = "HDTxdBindings";
            this.TabHDTxdBindings.UseVisualStyleBackColor = true;
            this.TabHDTxdBindings.Controls.Add(this.GridHDTxdBindings);
            this.TabImapDeps.Text = "imapDependencies";
            this.TabImapDeps.UseVisualStyleBackColor = true;
            this.TabImapDeps.Controls.Add(this.GridImapDeps);
            this.TabImapDeps2.Text = "imapDependencies_2";
            this.TabImapDeps2.UseVisualStyleBackColor = true;
            this.TabImapDeps2.Controls.Add(this.GridImapDeps2);
            this.TabItypDeps2.Text = "itypDependencies_2";
            this.TabItypDeps2.UseVisualStyleBackColor = true;
            this.TabItypDeps2.Controls.Add(this.GridItypDeps2);
            this.TabInteriors.Text = "Interiors";
            this.TabInteriors.UseVisualStyleBackColor = true;
            this.TabInteriors.Controls.Add(this.GridInteriors);
            // 
            // ColMDG columns
            // 
            this.ColMDG_Name.HeaderText = "Name";
            this.ColMDG_Name.Name = "ColMDG_Name";
            this.ColMDG_Flags.HeaderText = "Flags";
            this.ColMDG_Flags.Name = "ColMDG_Flags";
            this.ColMDG_Flags.Items.AddRange(new object[] { "", "INTERIOR_DATA" });
            this.ColMDG_HoursOnOff.HeaderText = "HoursOnOff";
            this.ColMDG_HoursOnOff.Name = "ColMDG_HoursOnOff";
            this.ColMDG_HoursOnOff.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" });
            this.ColMDG_Bounds.HeaderText = "Bounds (comma-separated)";
            this.ColMDG_Bounds.Name = "ColMDG_Bounds";
            this.ColMDG_WeatherTypes.HeaderText = "WeatherTypes";
            this.ColMDG_WeatherTypes.Name = "ColMDG_WeatherTypes";
            this.ColMDG_WeatherTypes.Items.AddRange(new object[] { "", "EXTRASUNNY", "CLEAR", "NEUTRAL", "SMOG", "FOGGY", "OVERCAST", "CLOUDS", "CLEARING", "RAIN", "THUNDER", "SNOW", "BLIZZARD", "SNOWLIGHT", "XMAS", "HALLOWEEN" });
            // 
            // GridMapDataGroups
            // 
            this.GridMapDataGroups.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridMapDataGroups.Name = "GridMapDataGroups";
            this.GridMapDataGroups.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridMapDataGroups.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridMapDataGroups.AllowUserToAddRows = true;
            this.GridMapDataGroups.AllowUserToDeleteRows = true;
            this.GridMapDataGroups.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.ColMDG_Name, this.ColMDG_Flags, this.ColMDG_HoursOnOff,
                this.ColMDG_Bounds, this.ColMDG_WeatherTypes
            });
            // 
            // ColHD columns
            // 
            this.ColHD_AssetType.HeaderText = "AssetType";
            this.ColHD_AssetType.Name = "ColHD_AssetType";
            this.ColHD_AssetType.Items.AddRange(new object[] { "AT_TXD", "AT_DRB", "AT_DWD", "AT_FRG" });
            this.ColHD_TargetAsset.HeaderText = "TargetAsset";
            this.ColHD_TargetAsset.Name = "ColHD_TargetAsset";
            this.ColHD_HDTxd.HeaderText = "HDTxd";
            this.ColHD_HDTxd.Name = "ColHD_HDTxd";
            // 
            // GridHDTxdBindings
            // 
            this.GridHDTxdBindings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridHDTxdBindings.Name = "GridHDTxdBindings";
            this.GridHDTxdBindings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridHDTxdBindings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridHDTxdBindings.AllowUserToAddRows = true;
            this.GridHDTxdBindings.AllowUserToDeleteRows = true;
            this.GridHDTxdBindings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.ColHD_AssetType, this.ColHD_TargetAsset, this.ColHD_HDTxd
            });
            // 
            // ColID columns
            // 
            this.ColID_ImapName.HeaderText = "imapName";
            this.ColID_ImapName.Name = "ColID_ImapName";
            this.ColID_ItypName.HeaderText = "itypName";
            this.ColID_ItypName.Name = "ColID_ItypName";
            this.ColID_PackFileName.HeaderText = "packFileName";
            this.ColID_PackFileName.Name = "ColID_PackFileName";
            // 
            // GridImapDeps
            // 
            this.GridImapDeps.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridImapDeps.Name = "GridImapDeps";
            this.GridImapDeps.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridImapDeps.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridImapDeps.AllowUserToAddRows = true;
            this.GridImapDeps.AllowUserToDeleteRows = true;
            this.GridImapDeps.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.ColID_ImapName, this.ColID_ItypName, this.ColID_PackFileName
            });
            // 
            // ColID2 columns
            // 
            this.ColID2_ImapName.HeaderText = "imapName";
            this.ColID2_ImapName.Name = "ColID2_ImapName";
            this.ColID2_ManifestFlags.HeaderText = "manifestFlags";
            this.ColID2_ManifestFlags.Name = "ColID2_ManifestFlags";
            this.ColID2_ManifestFlags.Items.AddRange(new object[] { "", "INTERIOR_DATA" });
            this.ColID2_ItypDepArray.HeaderText = "itypDepArray (comma-separated)";
            this.ColID2_ItypDepArray.Name = "ColID2_ItypDepArray";
            // 
            // GridImapDeps2
            // 
            this.GridImapDeps2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridImapDeps2.Name = "GridImapDeps2";
            this.GridImapDeps2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridImapDeps2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridImapDeps2.AllowUserToAddRows = true;
            this.GridImapDeps2.AllowUserToDeleteRows = true;
            this.GridImapDeps2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.ColID2_ImapName, this.ColID2_ManifestFlags, this.ColID2_ItypDepArray
            });
            // 
            // ColIT2 columns
            // 
            this.ColIT2_ItypName.HeaderText = "itypName";
            this.ColIT2_ItypName.Name = "ColIT2_ItypName";
            this.ColIT2_ManifestFlags.HeaderText = "manifestFlags";
            this.ColIT2_ManifestFlags.Name = "ColIT2_ManifestFlags";
            this.ColIT2_ManifestFlags.Items.AddRange(new object[] { "", "INTERIOR_DATA" });
            this.ColIT2_ItypDepArray.HeaderText = "itypDepArray (comma-separated)";
            this.ColIT2_ItypDepArray.Name = "ColIT2_ItypDepArray";
            // 
            // GridItypDeps2
            // 
            this.GridItypDeps2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridItypDeps2.Name = "GridItypDeps2";
            this.GridItypDeps2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridItypDeps2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridItypDeps2.AllowUserToAddRows = true;
            this.GridItypDeps2.AllowUserToDeleteRows = true;
            this.GridItypDeps2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.ColIT2_ItypName, this.ColIT2_ManifestFlags, this.ColIT2_ItypDepArray
            });
            // 
            // ColInt columns
            // 
            this.ColInt_Name.HeaderText = "Name";
            this.ColInt_Name.Name = "ColInt_Name";
            this.ColInt_Bounds.HeaderText = "Bounds (comma-separated)";
            this.ColInt_Bounds.Name = "ColInt_Bounds";
            // 
            // GridInteriors
            // 
            this.GridInteriors.Dock = System.Windows.Forms.DockStyle.Fill;
            this.GridInteriors.Name = "GridInteriors";
            this.GridInteriors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.GridInteriors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.GridInteriors.AllowUserToAddRows = true;
            this.GridInteriors.AllowUserToDeleteRows = true;
            this.GridInteriors.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.ColInt_Name, this.ColInt_Bounds
            });
            // 
            // XmlGroupBox
            // 
            this.XmlGroupBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.XmlGroupBox.Controls.Add(this.ProjectManifestTextBox);
            this.XmlGroupBox.Name = "XmlGroupBox";
            this.XmlGroupBox.TabIndex = 0;
            this.XmlGroupBox.TabStop = false;
            this.XmlGroupBox.Text = "XML Preview";
            // 
            // ProjectManifestTextBox
            // 
            this.ProjectManifestTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ProjectManifestTextBox.AutoCompleteBracketsList = new char[] {
                '(', ')', '{', '}', '[', ']', '\"', '\"', '\'', '\'' };
            this.ProjectManifestTextBox.AutoIndentCharsPatterns = "";
            this.ProjectManifestTextBox.AutoScrollMinSize = new System.Drawing.Size(27, 14);
            this.ProjectManifestTextBox.BackBrush = null;
            this.ProjectManifestTextBox.CharHeight = 14;
            this.ProjectManifestTextBox.CharWidth = 8;
            this.ProjectManifestTextBox.CommentPrefix = null;
            this.ProjectManifestTextBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.ProjectManifestTextBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.ProjectManifestTextBox.Font = new System.Drawing.Font("Courier New", 9.75F);
            this.ProjectManifestTextBox.IsReplaceMode = false;
            this.ProjectManifestTextBox.Language = FastColoredTextBoxNS.Language.XML;
            this.ProjectManifestTextBox.LeftBracket = '<';
            this.ProjectManifestTextBox.LeftBracket2 = '(';
            this.ProjectManifestTextBox.Location = new System.Drawing.Point(3, 16);
            this.ProjectManifestTextBox.Name = "ProjectManifestTextBox";
            this.ProjectManifestTextBox.Paddings = new System.Windows.Forms.Padding(0);
            this.ProjectManifestTextBox.RightBracket = '>';
            this.ProjectManifestTextBox.RightBracket2 = ')';
            this.ProjectManifestTextBox.SelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.ProjectManifestTextBox.ServiceColors = ((FastColoredTextBoxNS.ServiceColors)(resources.GetObject("ProjectManifestTextBox.ServiceColors")));
            this.ProjectManifestTextBox.TabIndex = 0;
            this.ProjectManifestTextBox.Zoom = 100;
            // 
            // SaveFileDialog
            // 
            this.SaveFileDialog.FileName = "_manifest.ymf";
            this.SaveFileDialog.Filter = "Manifest files|*.ymf";
            // 
            // Layout assembly
            // 
            this.MainSplitContainer.Panel1.Controls.Add(this.DataGroupBox);
            this.MainSplitContainer.Panel2.Controls.Add(this.XmlGroupBox);
            // 
            // EditProjectManifestPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 560);
            this.Controls.Add(this.MainSplitContainer);
            this.Controls.Add(this.ActionsGroupBox);
            this.Controls.Add(this.DescriptionLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditProjectManifestPanel";
            this.Text = "_manifest.ymf";
            this.ActionsGroupBox.ResumeLayout(false);
            this.ActionsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MainSplitContainer)).EndInit();
            this.MainSplitContainer.Panel1.ResumeLayout(false);
            this.MainSplitContainer.Panel2.ResumeLayout(false);
            this.MainSplitContainer.ResumeLayout(false);
            this.DataGroupBox.ResumeLayout(false);
            this.XmlGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GridMapDataGroups)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridHDTxdBindings)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridImapDeps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridImapDeps2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridItypDeps2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GridInteriors)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProjectManifestTextBox)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Label DescriptionLabel;
        private System.Windows.Forms.GroupBox ActionsGroupBox;
        private System.Windows.Forms.Button AutoPopulateButton;
        private System.Windows.Forms.Button ProjectManifestGenerateButton;
        private System.Windows.Forms.Button SaveManifestButton;
        private System.Windows.Forms.Label StatusLabel;
        private System.Windows.Forms.SplitContainer MainSplitContainer;
        private System.Windows.Forms.GroupBox DataGroupBox;
        private System.Windows.Forms.TabControl ManifestTabControl;
        private System.Windows.Forms.TabPage TabMapDataGroups;
        private System.Windows.Forms.TabPage TabHDTxdBindings;
        private System.Windows.Forms.TabPage TabImapDeps;
        private System.Windows.Forms.TabPage TabImapDeps2;
        private System.Windows.Forms.TabPage TabItypDeps2;
        private System.Windows.Forms.TabPage TabInteriors;
        private System.Windows.Forms.DataGridView GridMapDataGroups;
        private System.Windows.Forms.DataGridView GridHDTxdBindings;
        private System.Windows.Forms.DataGridView GridImapDeps;
        private System.Windows.Forms.DataGridView GridImapDeps2;
        private System.Windows.Forms.DataGridView GridItypDeps2;
        private System.Windows.Forms.DataGridView GridInteriors;
        private System.Windows.Forms.GroupBox XmlGroupBox;
        private FastColoredTextBoxNS.FastColoredTextBox ProjectManifestTextBox;
        private System.Windows.Forms.SaveFileDialog SaveFileDialog;

        // MapDataGroups columns
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMDG_Name;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColMDG_Flags;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColMDG_HoursOnOff;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMDG_Bounds;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColMDG_WeatherTypes;
        // HDTxdBindings columns
        private System.Windows.Forms.DataGridViewComboBoxColumn ColHD_AssetType;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColHD_TargetAsset;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColHD_HDTxd;
        // imapDependencies columns
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID_ImapName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID_ItypName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID_PackFileName;
        // imapDependencies_2 columns
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID2_ImapName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColID2_ManifestFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColID2_ItypDepArray;
        // itypDependencies_2 columns
        private System.Windows.Forms.DataGridViewTextBoxColumn ColIT2_ItypName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ColIT2_ManifestFlags;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColIT2_ItypDepArray;
        // Interiors columns
        private System.Windows.Forms.DataGridViewTextBoxColumn ColInt_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColInt_Bounds;
    }
}