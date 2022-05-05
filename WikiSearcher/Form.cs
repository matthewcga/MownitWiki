using System;
using System.Windows.Forms;
using WikiSearcher.Database;

namespace WikiSearcher
{
    public partial class Form : System.Windows.Forms.Form
    {
        private readonly FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();

        public Form()
        { InitializeComponent(); }


        private void SelectDbAction(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                Program.Dir = folderBrowserDialog.SelectedPath;
                DbManager.GetDbFromFile(Program.Dir);
            }
        }


        private void SearchAction(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Program.VM.Search.Text)) return;
            Program.VM.FileSelection.Items.Clear();
            Program.VM.FileSelection.Items.AddRange(DbManager.AskDb(Program.VM.Search.Text).ToArray());
            Program.SetFileElements(true);
        }


        private void ComboBoxSelectAction(object sender, EventArgs e)
        {
            Program.CurrentFile = $"{Program.Dir}\\downloads\\{Program.VM.FileSelection.SelectedItem}";
            Program.VM.Browser.Navigate($"file:///{Program.CurrentFile}");
            Program.ChangeStatus($"displaying '{Program.VM.FileSelection.SelectedItem}'");
        }


        private void OpenWikiAction(object sender, EventArgs e)
        { System.Diagnostics.Process.Start(DbManager.LinksDataBase[Program.VM.FileSelection.SelectedItem.ToString()]); }


        private void TimerTick(object sender, EventArgs e)
        { Program.VM.RamUsed.Text = $"RAM: {Convert.ToInt32(Program.VM.performanceCounter1.NextValue())} Mb"; }
    }
}
