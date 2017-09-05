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
        private ReadTxt Reader = null;
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
            if (BtnCatalog.Enabled && openDialog.ShowDialog() == DialogResult.OK)
            {
                TbxPath.Text = openDialog.FileName;
                Reader = new ReadTxt(TbxPath.Text, new LoadPercent(Load_Per));
            }
        }


        public void Load_Per(string str)
        {
            Invoke(new Action(() =>
            {
                LblPercent.Text = str;
            }));
        }

        private void BtnCatalog_Click(object sender, EventArgs e)
        {
            if (!File.Exists(TbxPath.Text))
            {
                MessageBox.Show("路径为空或文件不存在", "提示");
                return;
            }
            BtnCatalog.Enabled = false;
            var th = new Thread(new ThreadStart(() =>
            {
                Reader.ReadCatalog();
                Invoke(new Action(() =>
                {
                    BtnContent.Enabled = true;
                }));
            }))
            {
                IsBackground = true
            };
            th.Start();
        }
        private void BtnContent_Click(object sender, EventArgs e)
        {
            BtnContent.Enabled = false;
            var th = new Thread(new ThreadStart(() =>
            {
                Reader.ReadContent();
                Invoke(new Action(() =>
                {
                    BtnCommit.Enabled = true;
                }));
            }))
            {
                IsBackground = true
            };
            th.Start();
        }

        private void BtnCommit_Click(object sender, EventArgs e)
        {
            BtnCommit.Enabled = false;
            var th = new Thread(new ThreadStart(() =>
            {
                Reader.InsertDB();
                Invoke(new Action(() =>
                {
                    BtnCatalog.Enabled = true;
                }));
            }))
            {
                IsBackground = true
            };
            th.Start();
        }

        private void ReadKeyword_Load(object sender, EventArgs e)
        {
            BtnContent.Enabled = false;
            BtnCommit.Enabled = false;
        }
    }
}
