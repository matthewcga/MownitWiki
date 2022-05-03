using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiSearcher
{
    internal static class Program
    {
        public static Form VM;
        public static string Dir;
        public static string CurrentFile;


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            VM = new Form();

            SetSearchElements(false);
            SetFileElements(false);
            ChangeStatus("no database");
            Application.Run(VM);
        }

        public static void ChangeStatus(string status)
        {
            VM.Status.Text = status;
        }

        public static void SetSearchElements(bool setTo)
        {
            VM.SearchBtn.Enabled = setTo;
            VM.Search.Enabled = setTo;
        }

        public static void SetFileElements(bool setTo)
        {
            VM.WikiBtn.Enabled = setTo;
            VM.FileSelection.Enabled = setTo;
        }
    }
}
