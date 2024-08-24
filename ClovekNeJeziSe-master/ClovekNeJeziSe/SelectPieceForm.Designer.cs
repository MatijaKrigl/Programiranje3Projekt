namespace ClovekNeJeziSe
{
    partial class IzberiFiguroForm
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

        private void SelectPieceForm_Load(object sender, EventArgs e)
        {
            // Your code here, if needed.
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            listBoxPieces = new ListBox();
            SuspendLayout();
            // 
            // listBoxPieces
            // 
            listBoxPieces.AccessibleName = "listBoxPieces";
            listBoxPieces.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            listBoxPieces.FormattingEnabled = true;
            listBoxPieces.ItemHeight = 28;
            listBoxPieces.Location = new Point(59, 59);
            listBoxPieces.Name = "listBoxPieces";
            listBoxPieces.Size = new Size(217, 172);
            listBoxPieces.TabIndex = 0;
            listBoxPieces.SelectedIndexChanged += listBoxPieces_SelectedIndexChanged_1;
            // 
            // IzberiFiguroForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(338, 324);
            Controls.Add(listBoxPieces);
            Name = "IzberiFiguroForm";
            ShowIcon = false;
            Text = "Metal/a si 6! Izberi figuro!";
            Load += SelectPieceForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListBox listBoxPieces;
    }
}