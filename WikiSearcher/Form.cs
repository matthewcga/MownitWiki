using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WikiSearcher.Database;

namespace WikiSearcher
{
    public partial class Form : System.Windows.Forms.Form
    {
        private FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        public Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Program.Dir = folderBrowserDialog.SelectedPath;
                DbManager.GetDbFromFile(Program.Dir);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Program.VM.Search.Text)) return;
            Program.VM.FileSelection.Items.Clear();
            Program.VM.FileSelection.Items.AddRange(DbManager.FileQuery(Program.VM.Search.Text).ToArray());
            Program.SetFileElements(true);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Program.CurrentFile = $"{Program.Dir}\\downloads\\{Program.VM.FileSelection.SelectedItem}";
            Program.VM.Browser.Navigate($"file:///{Program.CurrentFile}");
            Program.ChangeStatus($"displaying '{Program.VM.FileSelection.SelectedItem}'");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(DbManager.GetLink(Program.VM.FileSelection.SelectedItem.ToString()));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Program.VM.RamUsed.Text = $"{Convert.ToInt32(Program.VM.performanceCounter1.NextValue())} Mb";
        }
    }
}
