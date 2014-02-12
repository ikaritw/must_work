namespace WindowsFormsApplication1 {
    partial class Form1 {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置 Managed 資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改這個方法的內容。
        ///
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnClearData = new System.Windows.Forms.Button();
            this.progressOfFile = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurrentStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFIlesize = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.說明LToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.btnGetExistFile = new System.Windows.Forms.Button();
            this.isCheckExist = new System.Windows.Forms.CheckBox();
            this.txtExistFile = new System.Windows.Forms.TextBox();
            this.btnLoadUnreconizeFile = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(121, 30);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(451, 22);
            this.txtFilename.TabIndex = 1;
            // 
            // btnClearData
            // 
            this.btnClearData.Location = new System.Drawing.Point(14, 182);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(75, 23);
            this.btnClearData.TabIndex = 2;
            this.btnClearData.Text = "清理資料";
            this.btnClearData.UseVisualStyleBackColor = true;
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
            // 
            // progressOfFile
            // 
            this.progressOfFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressOfFile.Location = new System.Drawing.Point(0, 283);
            this.progressOfFile.Name = "progressOfFile";
            this.progressOfFile.Size = new System.Drawing.Size(584, 23);
            this.progressOfFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressOfFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 157);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "執行進度:";
            // 
            // txtCurrentStatus
            // 
            this.txtCurrentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentStatus.Location = new System.Drawing.Point(95, 154);
            this.txtCurrentStatus.Name = "txtCurrentStatus";
            this.txtCurrentStatus.ReadOnly = true;
            this.txtCurrentStatus.Size = new System.Drawing.Size(477, 22);
            this.txtCurrentStatus.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(120, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "檔案大小:";
            // 
            // lblFIlesize
            // 
            this.lblFIlesize.AutoSize = true;
            this.lblFIlesize.Location = new System.Drawing.Point(182, 55);
            this.lblFIlesize.Name = "lblFIlesize";
            this.lblFIlesize.Size = new System.Drawing.Size(0, 12);
            this.lblFIlesize.TabIndex = 4;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(95, 182);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(477, 95);
            this.txtResult.TabIndex = 5;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.說明LToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(584, 25);
            this.toolStrip1.TabIndex = 6;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 說明LToolStripButton
            // 
            this.說明LToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.說明LToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("說明LToolStripButton.Image")));
            this.說明LToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.說明LToolStripButton.Name = "說明LToolStripButton";
            this.說明LToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.說明LToolStripButton.Text = "說明(&L)";
            this.說明LToolStripButton.Click += new System.EventHandler(this.說明LToolStripButton_Click);
            // 
            // btnGetExistFile
            // 
            this.btnGetExistFile.Enabled = false;
            this.btnGetExistFile.Location = new System.Drawing.Point(14, 114);
            this.btnGetExistFile.Name = "btnGetExistFile";
            this.btnGetExistFile.Size = new System.Drawing.Size(101, 23);
            this.btnGetExistFile.TabIndex = 7;
            this.btnGetExistFile.Text = "載入已存在檔案";
            this.btnGetExistFile.UseVisualStyleBackColor = true;
            this.btnGetExistFile.Click += new System.EventHandler(this.btnGetExistFile_Click);
            // 
            // isCheckExist
            // 
            this.isCheckExist.AutoSize = true;
            this.isCheckExist.Location = new System.Drawing.Point(14, 92);
            this.isCheckExist.Name = "isCheckExist";
            this.isCheckExist.Size = new System.Drawing.Size(144, 16);
            this.isCheckExist.TabIndex = 8;
            this.isCheckExist.Text = "是否截入已存在的資料";
            this.isCheckExist.UseVisualStyleBackColor = true;
            this.isCheckExist.CheckedChanged += new System.EventHandler(this.isCheckExist_CheckedChanged);
            // 
            // txtExistFile
            // 
            this.txtExistFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtExistFile.Location = new System.Drawing.Point(122, 114);
            this.txtExistFile.Name = "txtExistFile";
            this.txtExistFile.ReadOnly = true;
            this.txtExistFile.Size = new System.Drawing.Size(450, 22);
            this.txtExistFile.TabIndex = 9;
            // 
            // btnLoadUnreconizeFile
            // 
            this.btnLoadUnreconizeFile.Location = new System.Drawing.Point(14, 28);
            this.btnLoadUnreconizeFile.Name = "btnLoadUnreconizeFile";
            this.btnLoadUnreconizeFile.Size = new System.Drawing.Size(101, 23);
            this.btnLoadUnreconizeFile.TabIndex = 10;
            this.btnLoadUnreconizeFile.Text = "待比對的檔案";
            this.btnLoadUnreconizeFile.UseVisualStyleBackColor = true;
            this.btnLoadUnreconizeFile.Click += new System.EventHandler(this.btnLoadUnreconizeFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 306);
            this.Controls.Add(this.btnLoadUnreconizeFile);
            this.Controls.Add(this.txtExistFile);
            this.Controls.Add(this.isCheckExist);
            this.Controls.Add(this.btnGetExistFile);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblFIlesize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressOfFile);
            this.Controls.Add(this.btnClearData);
            this.Controls.Add(this.txtCurrentStatus);
            this.Controls.Add(this.txtFilename);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 250);
            this.Name = "Form1";
            this.Text = "轉檔程式";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.ProgressBar progressOfFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurrentStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFIlesize;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 說明LToolStripButton;
        private System.Windows.Forms.Button btnGetExistFile;
        private System.Windows.Forms.CheckBox isCheckExist;
        private System.Windows.Forms.TextBox txtExistFile;
        private System.Windows.Forms.Button btnLoadUnreconizeFile;
    }
}

