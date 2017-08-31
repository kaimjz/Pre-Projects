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
            this.BtnStart = new System.Windows.Forms.Button();
            this.TbxPath = new System.Windows.Forms.TextBox();
            this.LblPath = new System.Windows.Forms.Label();
            this.LblPercent = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnStart
            // 
            this.BtnStart.Location = new System.Drawing.Point(196, 139);
            this.BtnStart.Name = "BtnStart";
            this.BtnStart.Size = new System.Drawing.Size(75, 23);
            this.BtnStart.TabIndex = 0;
            this.BtnStart.Text = "开始查找";
            this.BtnStart.UseVisualStyleBackColor = true;
            this.BtnStart.Click += new System.EventHandler(this.BtnStart_Click);
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
            // FindTxt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(509, 192);
            this.Controls.Add(this.LblPercent);
            this.Controls.Add(this.LblPath);
            this.Controls.Add(this.TbxPath);
            this.Controls.Add(this.BtnStart);
            this.Name = "FindTxt";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "中国药学主题词表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStart;
        private System.Windows.Forms.TextBox TbxPath;
        private System.Windows.Forms.Label LblPath;
        private System.Windows.Forms.Label LblPercent;
    }
}

