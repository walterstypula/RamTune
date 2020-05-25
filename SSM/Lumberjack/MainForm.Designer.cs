namespace NateW.Ssm.ApplicationLogic
{
    partial class MainForm
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
            this.tabs = new System.Windows.Forms.TabControl();
            this.controlTab = new System.Windows.Forms.TabPage();
            this.loggingModeGroupBox = new System.Windows.Forms.GroupBox();
            this.logAlways = new System.Windows.Forms.RadioButton();
            this.logOff = new System.Windows.Forms.RadioButton();
            this.logFullThrottle = new System.Windows.Forms.RadioButton();
            this.logClosedLoop = new System.Windows.Forms.RadioButton();
            this.logOpenLoop = new System.Windows.Forms.RadioButton();
            this.logDefogger = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.folderButton = new System.Windows.Forms.Button();
            this.folderLabel = new System.Windows.Forms.Label();
            this.openLogFolderButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.plxSerialPorts = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ssmSerialPorts = new System.Windows.Forms.ListBox();
            this.ecuIdentifierLabel = new System.Windows.Forms.Label();
            this.connectButton = new System.Windows.Forms.Button();
            this.profileTab = new System.Windows.Forms.TabPage();
            this.removeButton = new System.Windows.Forms.Button();
            this.newButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.profiles = new System.Windows.Forms.ListBox();
            this.saveAsButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.parametersTab = new System.Windows.Forms.TabPage();
            this.parameterGrid = new System.Windows.Forms.DataGridView();
            this.ParamEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ParamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParamUnits = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dashboardTab = new System.Windows.Forms.TabPage();
            this.canvas = new System.Windows.Forms.Label();
            this.statusTab = new System.Windows.Forms.TabPage();
            this.statusText = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.liveTuningTab = new System.Windows.Forms.TabPage();
            this.liveTuningGrid = new System.Windows.Forms.DataGridView();
            this.liveTuningInitialize = new System.Windows.Forms.Button();
            this.tabs.SuspendLayout();
            this.controlTab.SuspendLayout();
            this.loggingModeGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.profileTab.SuspendLayout();
            this.parametersTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.parameterGrid)).BeginInit();
            this.dashboardTab.SuspendLayout();
            this.statusTab.SuspendLayout();
            this.liveTuningTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.liveTuningGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tabs
            // 
            this.tabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabs.Controls.Add(this.controlTab);
            this.tabs.Controls.Add(this.profileTab);
            this.tabs.Controls.Add(this.parametersTab);
            this.tabs.Controls.Add(this.dashboardTab);
            this.tabs.Controls.Add(this.statusTab);
            this.tabs.Controls.Add(this.liveTuningTab);
            this.tabs.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabs.Location = new System.Drawing.Point(3, 2);
            this.tabs.Margin = new System.Windows.Forms.Padding(4);
            this.tabs.Name = "tabs";
            this.tabs.SelectedIndex = 0;
            this.tabs.Size = new System.Drawing.Size(912, 534);
            this.tabs.TabIndex = 0;
            // 
            // controlTab
            // 
            this.controlTab.Controls.Add(this.loggingModeGroupBox);
            this.controlTab.Controls.Add(this.groupBox2);
            this.controlTab.Controls.Add(this.groupBox1);
            this.controlTab.Location = new System.Drawing.Point(4, 34);
            this.controlTab.Margin = new System.Windows.Forms.Padding(4);
            this.controlTab.Name = "controlTab";
            this.controlTab.Padding = new System.Windows.Forms.Padding(4);
            this.controlTab.Size = new System.Drawing.Size(904, 496);
            this.controlTab.TabIndex = 0;
            this.controlTab.Text = "Settings";
            this.controlTab.UseVisualStyleBackColor = true;
            // 
            // loggingModeGroupBox
            // 
            this.loggingModeGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.loggingModeGroupBox.Controls.Add(this.logAlways);
            this.loggingModeGroupBox.Controls.Add(this.logOff);
            this.loggingModeGroupBox.Controls.Add(this.logFullThrottle);
            this.loggingModeGroupBox.Controls.Add(this.logClosedLoop);
            this.loggingModeGroupBox.Controls.Add(this.logOpenLoop);
            this.loggingModeGroupBox.Controls.Add(this.logDefogger);
            this.loggingModeGroupBox.Location = new System.Drawing.Point(512, 7);
            this.loggingModeGroupBox.Margin = new System.Windows.Forms.Padding(4);
            this.loggingModeGroupBox.Name = "loggingModeGroupBox";
            this.loggingModeGroupBox.Padding = new System.Windows.Forms.Padding(4);
            this.loggingModeGroupBox.Size = new System.Drawing.Size(379, 337);
            this.loggingModeGroupBox.TabIndex = 15;
            this.loggingModeGroupBox.TabStop = false;
            this.loggingModeGroupBox.Text = "Logging Control";
            // 
            // logAlways
            // 
            this.logAlways.AutoSize = true;
            this.logAlways.Location = new System.Drawing.Point(23, 108);
            this.logAlways.Margin = new System.Windows.Forms.Padding(4);
            this.logAlways.Name = "logAlways";
            this.logAlways.Size = new System.Drawing.Size(171, 29);
            this.logAlways.TabIndex = 31;
            this.logAlways.Text = "&Always Logging";
            this.logAlways.UseVisualStyleBackColor = true;
            this.logAlways.CheckedChanged += new System.EventHandler(this.logAlways_CheckedChanged);
            // 
            // logOff
            // 
            this.logOff.AutoSize = true;
            this.logOff.Location = new System.Drawing.Point(23, 71);
            this.logOff.Margin = new System.Windows.Forms.Padding(4);
            this.logOff.Name = "logOff";
            this.logOff.Size = new System.Drawing.Size(122, 29);
            this.logOff.TabIndex = 30;
            this.logOff.Text = "&View Only";
            this.logOff.UseVisualStyleBackColor = true;
            this.logOff.CheckedChanged += new System.EventHandler(this.logOff_CheckedChanged);
            // 
            // logFullThrottle
            // 
            this.logFullThrottle.AutoSize = true;
            this.logFullThrottle.Enabled = false;
            this.logFullThrottle.Location = new System.Drawing.Point(23, 182);
            this.logFullThrottle.Margin = new System.Windows.Forms.Padding(4);
            this.logFullThrottle.Name = "logFullThrottle";
            this.logFullThrottle.Size = new System.Drawing.Size(135, 29);
            this.logFullThrottle.TabIndex = 35;
            this.logFullThrottle.Text = "Full &Throttle";
            this.logFullThrottle.UseVisualStyleBackColor = true;
            this.logFullThrottle.CheckedChanged += new System.EventHandler(this.logFullThrottle_CheckedChanged);
            // 
            // logClosedLoop
            // 
            this.logClosedLoop.AutoSize = true;
            this.logClosedLoop.Enabled = false;
            this.logClosedLoop.Location = new System.Drawing.Point(23, 265);
            this.logClosedLoop.Margin = new System.Windows.Forms.Padding(4);
            this.logClosedLoop.Name = "logClosedLoop";
            this.logClosedLoop.Size = new System.Drawing.Size(144, 29);
            this.logClosedLoop.TabIndex = 34;
            this.logClosedLoop.Text = "Closed &Loop";
            this.logClosedLoop.UseVisualStyleBackColor = true;
            this.logClosedLoop.Visible = false;
            this.logClosedLoop.CheckedChanged += new System.EventHandler(this.logClosedLoop_CheckedChanged);
            // 
            // logOpenLoop
            // 
            this.logOpenLoop.AutoSize = true;
            this.logOpenLoop.Enabled = false;
            this.logOpenLoop.Location = new System.Drawing.Point(23, 228);
            this.logOpenLoop.Margin = new System.Windows.Forms.Padding(4);
            this.logOpenLoop.Name = "logOpenLoop";
            this.logOpenLoop.Size = new System.Drawing.Size(131, 29);
            this.logOpenLoop.TabIndex = 33;
            this.logOpenLoop.Text = "Open &Loop";
            this.logOpenLoop.UseVisualStyleBackColor = true;
            this.logOpenLoop.Visible = false;
            this.logOpenLoop.CheckedChanged += new System.EventHandler(this.logOpenLoop_CheckedChanged);
            // 
            // logDefogger
            // 
            this.logDefogger.AutoSize = true;
            this.logDefogger.Checked = true;
            this.logDefogger.Enabled = false;
            this.logDefogger.Location = new System.Drawing.Point(23, 145);
            this.logDefogger.Margin = new System.Windows.Forms.Padding(4);
            this.logDefogger.Name = "logDefogger";
            this.logDefogger.Size = new System.Drawing.Size(113, 29);
            this.logDefogger.TabIndex = 32;
            this.logDefogger.TabStop = true;
            this.logDefogger.Text = "&Defogger";
            this.logDefogger.UseVisualStyleBackColor = true;
            this.logDefogger.CheckedChanged += new System.EventHandler(this.logDefogger_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.folderButton);
            this.groupBox2.Controls.Add(this.folderLabel);
            this.groupBox2.Controls.Add(this.openLogFolderButton);
            this.groupBox2.Location = new System.Drawing.Point(9, 352);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(881, 132);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Log Directory";
            // 
            // folderButton
            // 
            this.folderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.folderButton.Location = new System.Drawing.Point(8, 65);
            this.folderButton.Margin = new System.Windows.Forms.Padding(4);
            this.folderButton.Name = "folderButton";
            this.folderButton.Size = new System.Drawing.Size(224, 47);
            this.folderButton.TabIndex = 11;
            this.folderButton.Text = "Set &Folder";
            this.folderButton.UseVisualStyleBackColor = true;
            this.folderButton.Click += new System.EventHandler(this.folderButton_Click);
            // 
            // folderLabel
            // 
            this.folderLabel.AutoSize = true;
            this.folderLabel.Location = new System.Drawing.Point(8, 27);
            this.folderLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.folderLabel.Name = "folderLabel";
            this.folderLabel.Size = new System.Drawing.Size(197, 25);
            this.folderLabel.TabIndex = 10;
            this.folderLabel.Text = "Logs will be saved in:";
            // 
            // openLogFolderButton
            // 
            this.openLogFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openLogFolderButton.Location = new System.Drawing.Point(247, 65);
            this.openLogFolderButton.Margin = new System.Windows.Forms.Padding(4);
            this.openLogFolderButton.Name = "openLogFolderButton";
            this.openLogFolderButton.Size = new System.Drawing.Size(240, 47);
            this.openLogFolderButton.TabIndex = 12;
            this.openLogFolderButton.Text = "&Open Log Folder";
            this.openLogFolderButton.UseVisualStyleBackColor = true;
            this.openLogFolderButton.Click += new System.EventHandler(this.openLogFolderButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.plxSerialPorts);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.ssmSerialPorts);
            this.groupBox1.Controls.Add(this.ecuIdentifierLabel);
            this.groupBox1.Controls.Add(this.connectButton);
            this.groupBox1.Location = new System.Drawing.Point(9, 7);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(495, 337);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Connection";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 27);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 25);
            this.label4.TabIndex = 9;
            this.label4.Text = "&PLX Port";
            // 
            // plxSerialPorts
            // 
            this.plxSerialPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plxSerialPorts.FormattingEnabled = true;
            this.plxSerialPorts.ItemHeight = 25;
            this.plxSerialPorts.Location = new System.Drawing.Point(247, 59);
            this.plxSerialPorts.Margin = new System.Windows.Forms.Padding(4);
            this.plxSerialPorts.Name = "plxSerialPorts";
            this.plxSerialPorts.Size = new System.Drawing.Size(239, 104);
            this.plxSerialPorts.TabIndex = 8;
            this.plxSerialPorts.SelectedIndexChanged += new System.EventHandler(this.plxSerialPorts_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 25);
            this.label1.TabIndex = 2;
            this.label1.Text = "&SSM Port";
            // 
            // ssmSerialPorts
            // 
            this.ssmSerialPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.ssmSerialPorts.FormattingEnabled = true;
            this.ssmSerialPorts.ItemHeight = 25;
            this.ssmSerialPorts.Location = new System.Drawing.Point(8, 59);
            this.ssmSerialPorts.Margin = new System.Windows.Forms.Padding(4);
            this.ssmSerialPorts.Name = "ssmSerialPorts";
            this.ssmSerialPorts.Size = new System.Drawing.Size(223, 104);
            this.ssmSerialPorts.TabIndex = 3;
            this.ssmSerialPorts.SelectedIndexChanged += new System.EventHandler(this.ssmSerialPorts_SelectedIndexChanged);
            // 
            // ecuIdentifierLabel
            // 
            this.ecuIdentifierLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ecuIdentifierLabel.AutoSize = true;
            this.ecuIdentifierLabel.Location = new System.Drawing.Point(3, 255);
            this.ecuIdentifierLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ecuIdentifierLabel.Name = "ecuIdentifierLabel";
            this.ecuIdentifierLabel.Size = new System.Drawing.Size(143, 25);
            this.ecuIdentifierLabel.TabIndex = 6;
            this.ecuIdentifierLabel.Text = "Not connected.";
            // 
            // connectButton
            // 
            this.connectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.connectButton.Location = new System.Drawing.Point(8, 283);
            this.connectButton.Margin = new System.Windows.Forms.Padding(4);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(224, 47);
            this.connectButton.TabIndex = 7;
            this.connectButton.Text = "&Reconnect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // profileTab
            // 
            this.profileTab.Controls.Add(this.removeButton);
            this.profileTab.Controls.Add(this.newButton);
            this.profileTab.Controls.Add(this.saveButton);
            this.profileTab.Controls.Add(this.profiles);
            this.profileTab.Controls.Add(this.saveAsButton);
            this.profileTab.Controls.Add(this.openButton);
            this.profileTab.Controls.Add(this.label2);
            this.profileTab.Location = new System.Drawing.Point(4, 34);
            this.profileTab.Margin = new System.Windows.Forms.Padding(4);
            this.profileTab.Name = "profileTab";
            this.profileTab.Padding = new System.Windows.Forms.Padding(4);
            this.profileTab.Size = new System.Drawing.Size(904, 496);
            this.profileTab.TabIndex = 3;
            this.profileTab.Text = "Profile";
            this.profileTab.UseVisualStyleBackColor = true;
            // 
            // removeButton
            // 
            this.removeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.removeButton.Location = new System.Drawing.Point(256, 431);
            this.removeButton.Margin = new System.Windows.Forms.Padding(4);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(240, 47);
            this.removeButton.TabIndex = 16;
            this.removeButton.Text = "&Remove From List";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.removeButton_Click);
            // 
            // newButton
            // 
            this.newButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.newButton.Location = new System.Drawing.Point(8, 368);
            this.newButton.Margin = new System.Windows.Forms.Padding(4);
            this.newButton.Name = "newButton";
            this.newButton.Size = new System.Drawing.Size(240, 47);
            this.newButton.TabIndex = 12;
            this.newButton.Text = "&New Profile";
            this.newButton.UseVisualStyleBackColor = true;
            this.newButton.Click += new System.EventHandler(this.newButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveButton.Enabled = false;
            this.saveButton.Location = new System.Drawing.Point(504, 431);
            this.saveButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(240, 47);
            this.saveButton.TabIndex = 13;
            this.saveButton.Text = "&Save Profile";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // profiles
            // 
            this.profiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.profiles.FormattingEnabled = true;
            this.profiles.ItemHeight = 25;
            this.profiles.Location = new System.Drawing.Point(8, 57);
            this.profiles.Margin = new System.Windows.Forms.Padding(4);
            this.profiles.Name = "profiles";
            this.profiles.Size = new System.Drawing.Size(881, 254);
            this.profiles.TabIndex = 11;
            this.profiles.SelectedIndexChanged += new System.EventHandler(this.profiles_SelectedIndexChanged);
            // 
            // saveAsButton
            // 
            this.saveAsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.saveAsButton.Enabled = false;
            this.saveAsButton.Location = new System.Drawing.Point(504, 377);
            this.saveAsButton.Margin = new System.Windows.Forms.Padding(4);
            this.saveAsButton.Name = "saveAsButton";
            this.saveAsButton.Size = new System.Drawing.Size(240, 47);
            this.saveAsButton.TabIndex = 15;
            this.saveAsButton.Text = "Save Profile &As...";
            this.saveAsButton.UseVisualStyleBackColor = true;
            this.saveAsButton.Click += new System.EventHandler(this.saveAsButton_Click);
            // 
            // openButton
            // 
            this.openButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.openButton.Location = new System.Drawing.Point(8, 431);
            this.openButton.Margin = new System.Windows.Forms.Padding(4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(240, 47);
            this.openButton.TabIndex = 14;
            this.openButton.Text = "&Open Profile...";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.openButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 16);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Selected &Profile";
            // 
            // parametersTab
            // 
            this.parametersTab.Controls.Add(this.parameterGrid);
            this.parametersTab.Location = new System.Drawing.Point(4, 34);
            this.parametersTab.Margin = new System.Windows.Forms.Padding(4);
            this.parametersTab.Name = "parametersTab";
            this.parametersTab.Padding = new System.Windows.Forms.Padding(4);
            this.parametersTab.Size = new System.Drawing.Size(904, 496);
            this.parametersTab.TabIndex = 2;
            this.parametersTab.Text = "Parameters";
            this.parametersTab.UseVisualStyleBackColor = true;
            // 
            // parameterGrid
            // 
            this.parameterGrid.AllowUserToAddRows = false;
            this.parameterGrid.AllowUserToDeleteRows = false;
            this.parameterGrid.AllowUserToResizeColumns = false;
            this.parameterGrid.AllowUserToResizeRows = false;
            this.parameterGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.parameterGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.parameterGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ParamEnabled,
            this.ParamName,
            this.ParamUnits});
            this.parameterGrid.Location = new System.Drawing.Point(12, 7);
            this.parameterGrid.Margin = new System.Windows.Forms.Padding(4);
            this.parameterGrid.Name = "parameterGrid";
            this.parameterGrid.RowHeadersVisible = false;
            this.parameterGrid.RowTemplate.Height = 30;
            this.parameterGrid.ShowEditingIcon = false;
            this.parameterGrid.Size = new System.Drawing.Size(879, 476);
            this.parameterGrid.TabIndex = 16;
            this.parameterGrid.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.parameterGrid_CellValueChanged);
            // 
            // ParamEnabled
            // 
            this.ParamEnabled.FillWeight = 1F;
            this.ParamEnabled.Frozen = true;
            this.ParamEnabled.HeaderText = "Enabled";
            this.ParamEnabled.MinimumWidth = 20;
            this.ParamEnabled.Name = "ParamEnabled";
            this.ParamEnabled.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.ParamEnabled.Width = 75;
            // 
            // ParamName
            // 
            this.ParamName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ParamName.HeaderText = "Name";
            this.ParamName.Name = "ParamName";
            this.ParamName.ReadOnly = true;
            // 
            // ParamUnits
            // 
            this.ParamUnits.FillWeight = 1F;
            this.ParamUnits.HeaderText = "Units";
            this.ParamUnits.MinimumWidth = 50;
            this.ParamUnits.Name = "ParamUnits";
            this.ParamUnits.Width = 150;
            // 
            // dashboardTab
            // 
            this.dashboardTab.Controls.Add(this.canvas);
            this.dashboardTab.Location = new System.Drawing.Point(4, 34);
            this.dashboardTab.Margin = new System.Windows.Forms.Padding(4);
            this.dashboardTab.Name = "dashboardTab";
            this.dashboardTab.Padding = new System.Windows.Forms.Padding(4);
            this.dashboardTab.Size = new System.Drawing.Size(904, 496);
            this.dashboardTab.TabIndex = 1;
            this.dashboardTab.Text = "Display";
            this.dashboardTab.UseVisualStyleBackColor = true;
            // 
            // canvas
            // 
            this.canvas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.canvas.Location = new System.Drawing.Point(4, 4);
            this.canvas.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.canvas.Name = "canvas";
            this.canvas.Size = new System.Drawing.Size(896, 488);
            this.canvas.TabIndex = 0;
            // 
            // statusTab
            // 
            this.statusTab.Controls.Add(this.statusText);
            this.statusTab.Controls.Add(this.label3);
            this.statusTab.Location = new System.Drawing.Point(4, 34);
            this.statusTab.Margin = new System.Windows.Forms.Padding(4);
            this.statusTab.Name = "statusTab";
            this.statusTab.Size = new System.Drawing.Size(904, 496);
            this.statusTab.TabIndex = 4;
            this.statusTab.Text = "Debug";
            this.statusTab.UseVisualStyleBackColor = true;
            // 
            // statusText
            // 
            this.statusText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statusText.Location = new System.Drawing.Point(8, 42);
            this.statusText.Margin = new System.Windows.Forms.Padding(4);
            this.statusText.Multiline = true;
            this.statusText.Name = "statusText";
            this.statusText.ReadOnly = true;
            this.statusText.Size = new System.Drawing.Size(881, 441);
            this.statusText.TabIndex = 16;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 14);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(171, 25);
            this.label3.TabIndex = 15;
            this.label3.Text = "Debug messages:";
            // 
            // liveTuningTab
            // 
            this.liveTuningTab.Controls.Add(this.liveTuningGrid);
            this.liveTuningTab.Controls.Add(this.liveTuningInitialize);
            this.liveTuningTab.Location = new System.Drawing.Point(4, 34);
            this.liveTuningTab.Name = "liveTuningTab";
            this.liveTuningTab.Size = new System.Drawing.Size(904, 496);
            this.liveTuningTab.TabIndex = 5;
            this.liveTuningTab.Text = "Live Tuning (experimental)";
            this.liveTuningTab.UseVisualStyleBackColor = true;
            // 
            // liveTuningGrid
            // 
            this.liveTuningGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.liveTuningGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.liveTuningGrid.Location = new System.Drawing.Point(136, 4);
            this.liveTuningGrid.Name = "liveTuningGrid";
            this.liveTuningGrid.RowTemplate.Height = 24;
            this.liveTuningGrid.Size = new System.Drawing.Size(760, 485);
            this.liveTuningGrid.TabIndex = 1;
            // 
            // liveTuningInitialize
            // 
            this.liveTuningInitialize.Location = new System.Drawing.Point(6, 4);
            this.liveTuningInitialize.Name = "liveTuningInitialize";
            this.liveTuningInitialize.Size = new System.Drawing.Size(124, 36);
            this.liveTuningInitialize.TabIndex = 0;
            this.liveTuningInitialize.Text = "Initialize";
            this.liveTuningInitialize.UseVisualStyleBackColor = true;
            this.liveTuningInitialize.Click += new System.EventHandler(this.liveTuningInitialize_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(915, 537);
            this.Controls.Add(this.tabs);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Lumberjack";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabs.ResumeLayout(false);
            this.controlTab.ResumeLayout(false);
            this.loggingModeGroupBox.ResumeLayout(false);
            this.loggingModeGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.profileTab.ResumeLayout(false);
            this.profileTab.PerformLayout();
            this.parametersTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.parameterGrid)).EndInit();
            this.dashboardTab.ResumeLayout(false);
            this.statusTab.ResumeLayout(false);
            this.statusTab.PerformLayout();
            this.liveTuningTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.liveTuningGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabs;
        private System.Windows.Forms.TabPage controlTab;
        private System.Windows.Forms.TabPage dashboardTab;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ecuIdentifierLabel;
        private System.Windows.Forms.TabPage parametersTab;
        private System.Windows.Forms.DataGridView parameterGrid;
        private System.Windows.Forms.Label canvas;
        private System.Windows.Forms.ListBox ssmSerialPorts;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Button folderButton;
        private System.Windows.Forms.Label folderLabel;
        private System.Windows.Forms.Button openLogFolderButton;
        private System.Windows.Forms.TabPage profileTab;
        private System.Windows.Forms.Button newButton;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.ListBox profiles;
        private System.Windows.Forms.Button saveAsButton;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage statusTab;
        private System.Windows.Forms.TextBox statusText;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ParamEnabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParamName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ParamUnits;
        private System.Windows.Forms.GroupBox loggingModeGroupBox;
        private System.Windows.Forms.RadioButton logAlways;
        private System.Windows.Forms.RadioButton logOff;
        private System.Windows.Forms.RadioButton logFullThrottle;
        private System.Windows.Forms.RadioButton logClosedLoop;
        private System.Windows.Forms.RadioButton logOpenLoop;
        private System.Windows.Forms.RadioButton logDefogger;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox plxSerialPorts;
        private System.Windows.Forms.Button removeButton;
        private System.Windows.Forms.TabPage liveTuningTab;
        private System.Windows.Forms.Button liveTuningInitialize;
        private System.Windows.Forms.DataGridView liveTuningGrid;
    }
}

