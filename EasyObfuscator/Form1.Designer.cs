namespace EasyObfuscator
{
    partial class Form1
    {
        /// <summary>
        /// 設計工具所需的變數。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清除任何使用中的資源。
        /// </summary>
        /// <param name="disposing">如果應該處置受控資源則為 true，否則為 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 設計工具產生的程式碼

        /// <summary>
        /// 此為設計工具支援所需的方法 - 請勿使用程式碼編輯器修改
        /// 這個方法的內容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.txtFolderPath = new System.Windows.Forms.TextBox();
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.labelSrc = new System.Windows.Forms.Label();
            this.btnObfuscator = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbDelete = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnSelectFolder.Location = new System.Drawing.Point(170, 90);
            this.btnSelectFolder.Margin = new System.Windows.Forms.Padding(6);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(260, 120);
            this.btnSelectFolder.TabIndex = 0;
            this.btnSelectFolder.Text = "Select Soure Folder";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // txtFolderPath
            // 
            this.txtFolderPath.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.txtFolderPath.Location = new System.Drawing.Point(169, 28);
            this.txtFolderPath.Margin = new System.Windows.Forms.Padding(6);
            this.txtFolderPath.Name = "txtFolderPath";
            this.txtFolderPath.Size = new System.Drawing.Size(785, 50);
            this.txtFolderPath.TabIndex = 1;
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnAnalyze.Location = new System.Drawing.Point(433, 90);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(6);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(260, 120);
            this.btnAnalyze.TabIndex = 2;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // labelSrc
            // 
            this.labelSrc.AutoSize = true;
            this.labelSrc.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.labelSrc.Location = new System.Drawing.Point(26, 36);
            this.labelSrc.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelSrc.Name = "labelSrc";
            this.labelSrc.Size = new System.Drawing.Size(117, 38);
            this.labelSrc.TabIndex = 3;
            this.labelSrc.Text = "Source";
            // 
            // btnObfuscator
            // 
            this.btnObfuscator.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.btnObfuscator.Location = new System.Drawing.Point(694, 90);
            this.btnObfuscator.Margin = new System.Windows.Forms.Padding(6);
            this.btnObfuscator.Name = "btnObfuscator";
            this.btnObfuscator.Size = new System.Drawing.Size(260, 120);
            this.btnObfuscator.TabIndex = 4;
            this.btnObfuscator.Text = "Obfuscator";
            this.btnObfuscator.UseVisualStyleBackColor = true;
            this.btnObfuscator.Click += new System.EventHandler(this.btnObfuscator_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微軟正黑體", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.label1.Location = new System.Drawing.Point(32, 234);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 38);
            this.label1.TabIndex = 5;
            this.label1.Text = "Delete";
            // 
            // tbDelete
            // 
            this.tbDelete.Font = new System.Drawing.Font("微軟正黑體", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            this.tbDelete.Location = new System.Drawing.Point(169, 229);
            this.tbDelete.Margin = new System.Windows.Forms.Padding(6);
            this.tbDelete.Multiline = true;
            this.tbDelete.Name = "tbDelete";
            this.tbDelete.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbDelete.Size = new System.Drawing.Size(785, 312);
            this.tbDelete.TabIndex = 6;
            this.tbDelete.Text = "Cybersoft\r\nCOLA\r\nAUSPCLF\r\nESVC";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 556);
            this.Controls.Add(this.tbDelete);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnObfuscator);
            this.Controls.Add(this.labelSrc);
            this.Controls.Add(this.btnAnalyze);
            this.Controls.Add(this.txtFolderPath);
            this.Controls.Add(this.btnSelectFolder);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "EasyObfuscator";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TextBox txtFolderPath;
        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Label labelSrc;
        private System.Windows.Forms.Button btnObfuscator;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDelete;
    }
}

