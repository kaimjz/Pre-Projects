using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace BenTi
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Form f = null;
            string formName = ConfigurationManager.AppSettings["InitForm"] ?? "";
            switch (formName)
            {
                default:
                case "ReadKeyword":
                    f = new ReadKeyword();
                    break;
            }
            Application.Run(f);
        }
    }
}
