namespace WikiSearcher
{
    partial class Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.LoadBtn = new System.Windows.Forms.Button();
            this.Status = new System.Windows.Forms.Label();
            this.Search = new System.Windows.Forms.TextBox();
            this.SearchBtn = new System.Windows.Forms.Button();
            this.FileSelection = new System.Windows.Forms.ComboBox();
            this.Browser = new System.Windows.Forms.WebBrowser();
            this.WikiBtn = new System.Windows.Forms.Button();
            this.performanceCounter1 = new System.Diagnostics.PerformanceCounter();
            this.RamUsed = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer1.Start();
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).BeginInit();
            this.SuspendLayout();
            // 
            // LoadBtn
            // 
            this.LoadBtn.Location = new System.Drawing.Point(12, 47);
            this.LoadBtn.Name = "LoadBtn";
            this.LoadBtn.Size = new System.Drawing.Size(118, 21);
            this.LoadBtn.TabIndex = 0;
            this.LoadBtn.Text = "Load Database";
            this.LoadBtn.UseVisualStyleBackColor = true;
            this.LoadBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // Status
            // 
            this.Status.AutoSize = true;
            this.Status.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F);
            this.Status.Location = new System.Drawing.Point(13, 13);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(321, 31);
            this.Status.TabIndex = 1;
            this.Status.Text = "no status, app not started";
            this.Status.Click += new System.EventHandler(this.label1_Click);
            // 
            // Search
            // 
            this.Search.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Search.Location = new System.Drawing.Point(12, 74);
            this.Search.Name = "Search";
            this.Search.Size = new System.Drawing.Size(879, 20);
            this.Search.TabIndex = 2;
            this.Search.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // SearchBtn
            // 
            this.SearchBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SearchBtn.Location = new System.Drawing.Point(897, 73);
            this.SearchBtn.Name = "SearchBtn";
            this.SearchBtn.Size = new System.Drawing.Size(75, 21);
            this.SearchBtn.TabIndex = 3;
            this.SearchBtn.Text = "Search";
            this.SearchBtn.UseVisualStyleBackColor = true;
            this.SearchBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // FileSelection
            // 
            this.FileSelection.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.FileSelection.FormattingEnabled = true;
            this.FileSelection.Location = new System.Drawing.Point(12, 100);
            this.FileSelection.Name = "FileSelection";
            this.FileSelection.Size = new System.Drawing.Size(879, 21);
            this.FileSelection.TabIndex = 4;
            this.FileSelection.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // Browser
            // 
            this.Browser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Browser.Location = new System.Drawing.Point(12, 127);
            this.Browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Browser.Name = "Browser";
            this.Browser.ScriptErrorsSuppressed = true;
            this.Browser.Size = new System.Drawing.Size(960, 422);
            this.Browser.TabIndex = 5;
            // 
            // WikiBtn
            // 
            this.WikiBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.WikiBtn.Location = new System.Drawing.Point(897, 99);
            this.WikiBtn.Name = "WikiBtn";
            this.WikiBtn.Size = new System.Drawing.Size(75, 21);
            this.WikiBtn.TabIndex = 6;
            this.WikiBtn.Text = "Open Wiki";
            this.WikiBtn.UseVisualStyleBackColor = true;
            this.WikiBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // performanceCounter1
            // 
            this.performanceCounter1.CategoryName = "Memory";
            this.performanceCounter1.CounterName = "Available MBytes";
            // 
            // RamUsed
            // 
            this.RamUsed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.RamUsed.AutoSize = true;
            this.RamUsed.Location = new System.Drawing.Point(914, 9);
            this.RamUsed.Name = "RamUsed";
            this.RamUsed.Size = new System.Drawing.Size(58, 13);
            this.RamUsed.TabIndex = 7;
            this.RamUsed.Text = "no memory";
            this.RamUsed.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.RamUsed);
            this.Controls.Add(this.WikiBtn);
            this.Controls.Add(this.Browser);
            this.Controls.Add(this.FileSelection);
            this.Controls.Add(this.SearchBtn);
            this.Controls.Add(this.Search);
            this.Controls.Add(this.Status);
            this.Controls.Add(this.LoadBtn);
            this.Name = "Form";
            this.Text = "WikiSearch";
            ((System.ComponentModel.ISupportInitialize)(this.performanceCounter1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button LoadBtn;
        public System.Windows.Forms.Label Status;
        public System.Windows.Forms.TextBox Search;
        public System.Windows.Forms.Button SearchBtn;
        public System.Windows.Forms.ComboBox FileSelection;
        public System.Windows.Forms.WebBrowser Browser;
        public System.Windows.Forms.Button WikiBtn;
        public System.Diagnostics.PerformanceCounter performanceCounter1;
        public System.Windows.Forms.Label RamUsed;
        public System.Windows.Forms.Timer timer1;
    }
}

