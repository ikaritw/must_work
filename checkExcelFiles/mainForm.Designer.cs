namespace checkExcelFiles {
    partial class mainForm {
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
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器
        /// 修改這個方法的內容。
        /// </summary>
        private void InitializeComponent() {
            this.btnOpen = new System.Windows.Forms.Button();
            this.lstFileNames = new System.Windows.Forms.ListBox();
            this.btnExeute = new System.Windows.Forms.Button();
            this.lblCheckmark = new System.Windows.Forms.Label();
            this.cmbCheckmark = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(13, 13);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(75, 23);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.Text = "讀取檔案";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // lstFileNames
            // 
            this.lstFileNames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFileNames.FormattingEnabled = true;
            this.lstFileNames.ItemHeight = 12;
            this.lstFileNames.Location = new System.Drawing.Point(12, 54);
            this.lstFileNames.Name = "lstFileNames";
            this.lstFileNames.Size = new System.Drawing.Size(665, 436);
            this.lstFileNames.TabIndex = 1;
            // 
            // btnExeute
            // 
            this.btnExeute.Location = new System.Drawing.Point(303, 13);
            this.btnExeute.Name = "btnExeute";
            this.btnExeute.Size = new System.Drawing.Size(75, 23);
            this.btnExeute.TabIndex = 2;
            this.btnExeute.Text = "開始做它";
            this.btnExeute.UseVisualStyleBackColor = true;
            this.btnExeute.Click += new System.EventHandler(this.btnExeute_Click);
            // 
            // lblCheckmark
            // 
            this.lblCheckmark.AutoSize = true;
            this.lblCheckmark.Location = new System.Drawing.Point(109, 18);
            this.lblCheckmark.Name = "lblCheckmark";
            this.lblCheckmark.Size = new System.Drawing.Size(53, 12);
            this.lblCheckmark.TabIndex = 3;
            this.lblCheckmark.Text = "填入內容";
            // 
            // cmbCheckmark
            // 
            this.cmbCheckmark.FormattingEnabled = true;
            this.cmbCheckmark.Items.AddRange(new object[] {
            "v",
            "✓",
            "✔",
            "●",
            "○",
            "■",
            "□",
            "▲",
            "△",
            "▼",
            "▽",
            "★",
            "☆"});
            this.cmbCheckmark.Location = new System.Drawing.Point(169, 13);
            this.cmbCheckmark.Name = "cmbCheckmark";
            this.cmbCheckmark.Size = new System.Drawing.Size(121, 20);
            this.cmbCheckmark.TabIndex = 4;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 509);
            this.Controls.Add(this.cmbCheckmark);
            this.Controls.Add(this.lblCheckmark);
            this.Controls.Add(this.btnExeute);
            this.Controls.Add(this.lstFileNames);
            this.Controls.Add(this.btnOpen);
            this.Name = "mainForm";
            this.Text = "幫您打勾";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ListBox lstFileNames;
        private System.Windows.Forms.Button btnExeute;
        private System.Windows.Forms.Label lblCheckmark;
        private System.Windows.Forms.ComboBox cmbCheckmark;
    }
}

