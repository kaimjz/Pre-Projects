using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace BenTi
{
    public partial class ReadKeyword : Form
    {
        public delegate void LoadPercent(string str);

        public ReadKeyword()
        {
            InitializeComponent();
        }

        private void TbxPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog()
            {
                DefaultExt = ".txt",
                InitialDirectory = Directory.GetParent(Application.ExecutablePath).FullName
            };
            if (BtnStart.Enabled && openDialog.ShowDialog() == DialogResult.OK)
            {
                TbxPath.Text = openDialog.FileName;
            }
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            if (!File.Exists(TbxPath.Text))
            {
                MessageBox.Show("路径为空或文件不存在", "提示");
                return;
            }
            BtnStart.Enabled = false;
            var th = new Thread(new ThreadStart(() =>
            {
                var rt = new ReadTxt(TbxPath.Text, new LoadPercent(Load_Per));
                rt.Execute();
                Invoke(new Action(() =>
                {
                    BtnStart.Enabled = true;
                }));
            }))
            {
                IsBackground = true
            };
            th.Start();
        }

        public void Load_Per(string str)
        {
            Invoke(new Action(() =>
            {
                LblPercent.Text = str;
            }));
        }
    }
}
