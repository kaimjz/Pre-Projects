namespace BenTi
{
    partial class ReadKeyword
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.TbxPath = new System.Windows.Forms.TextBox();
            this.LblPath = new System.Windows.Forms.Label();
            this.LblPercent = new System.Windows.Forms.Label();
            this.BtnContent = new System.Windows.Forms.Button();
            this.BtnCatalog = new System.Windows.Forms.Button();
            this.BtnCommit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TbxPath
            // 
            this.TbxPath.Location = new System.Drawing.Point(129, 50);
            this.TbxPath.Name = "TbxPath";
            this.TbxPath.Size = new System.Drawing.Size(303, 21);
            this.TbxPath.TabIndex = 1;
            this.TbxPath.Click += new System.EventHandler(this.TbxPath_Click);
            // 
            // LblPath
            // 
            this.LblPath.AutoSize = true;
            this.LblPath.Location = new System.Drawing.Point(64, 53);
            this.LblPath.Name = "LblPath";
            this.LblPath.Size = new System.Drawing.Size(59, 12);
            this.LblPath.TabIndex = 2;
            this.LblPath.Text = "文件路径:";
            // 
            // LblPercent
            // 
            this.LblPercent.AutoSize = true;
            this.LblPercent.Location = new System.Drawing.Point(127, 99);
            this.LblPercent.Name = "LblPercent";
            this.LblPercent.Size = new System.Drawing.Size(0, 12);
            this.LblPercent.TabIndex = 3;
            // 
            // BtnContent
            // 
            this.BtnContent.Location = new System.Drawing.Point(244, 125);
            this.BtnContent.Name = "BtnContent";
            this.BtnContent.Size = new System.Drawing.Size(75, 23);
            this.BtnContent.TabIndex = 0;
            this.BtnContent.Text = "上册内容";
            this.BtnContent.UseVisualStyleBackColor = true;
            this.BtnContent.Click += new System.EventHandler(this.BtnContent_Click);
            // 
            // BtnCatalog
            // 
            this.BtnCatalog.Location = new System.Drawing.Point(129, 125);
            this.BtnCatalog.Name = "BtnCatalog";
            this.BtnCatalog.Size = new System.Drawing.Size(75, 23);
            this.BtnCatalog.TabIndex = 0;
            this.BtnCatalog.Text = "下册目录";
            this.BtnCatalog.UseVisualStyleBackColor = true;
            this.BtnCatalog.Click += new System.EventHandler(this.BtnCatalog_Click);
            // 
            // BtnCommit
            // 
            this.BtnCommit.Location = new System.Drawing.Point(357, 125);
            this.BtnCommit.Name = "BtnCommit";
            this.BtnCommit.Size = new System.Drawing.Size(75, 23);
            this.BtnCommit.TabIndex = 4;
            this.BtnCommit.Text = "提交数据库";
            this.BtnCommit.UseVisualStyleBackColor = true;
            this.BtnCommit.Click += new System.EventHandler(this.BtnCommit_Click);
            // 
            // ReadKeyword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(509, 192);
            this.Controls.Add(this.BtnCommit);
            this.Controls.Add(this.LblPercent);
            this.Controls.Add(this.LblPath);
            this.Controls.Add(this.TbxPath);
            this.Controls.Add(this.BtnCatalog);
            this.Controls.Add(this.BtnContent);
            this.Name = "ReadKeyword";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国药学主题词表";
            this.Load += new System.EventHandler(this.ReadKeyword_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox TbxPath;
        private System.Windows.Forms.Label LblPath;
        private System.Windows.Forms.Label LblPercent;
        private System.Windows.Forms.Button BtnContent;
        private System.Windows.Forms.Button BtnCatalog;
        private System.Windows.Forms.Button BtnCommit;
    }
}

