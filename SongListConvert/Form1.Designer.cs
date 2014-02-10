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
            this.btnOpenfile = new System.Windows.Forms.Button();
            this.txtFilename = new System.Windows.Forms.TextBox();
            this.btnClearData = new System.Windows.Forms.Button();
            this.progressOfFile = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCurrentStatus = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblFIlesize = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnOpenfile
            // 
            this.btnOpenfile.Location = new System.Drawing.Point(13, 13);
            this.btnOpenfile.Name = "btnOpenfile";
            this.btnOpenfile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenfile.TabIndex = 0;
            this.btnOpenfile.Text = "指定來源檔";
            this.btnOpenfile.UseVisualStyleBackColor = true;
            this.btnOpenfile.Click += new System.EventHandler(this.btnOpenfile_Click);
            // 
            // txtFilename
            // 
            this.txtFilename.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilename.Location = new System.Drawing.Point(95, 13);
            this.txtFilename.Name = "txtFilename";
            this.txtFilename.ReadOnly = true;
            this.txtFilename.Size = new System.Drawing.Size(535, 22);
            this.txtFilename.TabIndex = 1;
            // 
            // btnClearData
            // 
            this.btnClearData.Location = new System.Drawing.Point(13, 101);
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
            this.progressOfFile.Location = new System.Drawing.Point(0, 331);
            this.progressOfFile.Name = "progressOfFile";
            this.progressOfFile.Size = new System.Drawing.Size(642, 23);
            this.progressOfFile.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressOfFile.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "執行進度:";
            // 
            // txtCurrentStatus
            // 
            this.txtCurrentStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCurrentStatus.Location = new System.Drawing.Point(95, 63);
            this.txtCurrentStatus.Name = "txtCurrentStatus";
            this.txtCurrentStatus.ReadOnly = true;
            this.txtCurrentStatus.Size = new System.Drawing.Size(535, 22);
            this.txtCurrentStatus.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "檔案大小:";
            // 
            // lblFIlesize
            // 
            this.lblFIlesize.AutoSize = true;
            this.lblFIlesize.Location = new System.Drawing.Point(93, 48);
            this.lblFIlesize.Name = "lblFIlesize";
            this.lblFIlesize.Size = new System.Drawing.Size(0, 12);
            this.lblFIlesize.TabIndex = 4;
            // 
            // txtResult
            // 
            this.txtResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResult.Location = new System.Drawing.Point(95, 101);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(535, 224);
            this.txtResult.TabIndex = 5;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(642, 354);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.lblFIlesize);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressOfFile);
            this.Controls.Add(this.btnClearData);
            this.Controls.Add(this.txtCurrentStatus);
            this.Controls.Add(this.txtFilename);
            this.Controls.Add(this.btnOpenfile);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "轉檔程式";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpenfile;
        private System.Windows.Forms.TextBox txtFilename;
        private System.Windows.Forms.Button btnClearData;
        private System.Windows.Forms.ProgressBar progressOfFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCurrentStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblFIlesize;
        private System.Windows.Forms.TextBox txtResult;
    }
}

