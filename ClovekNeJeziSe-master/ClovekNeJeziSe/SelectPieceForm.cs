using System;
using System.Windows.Forms;

namespace ClovekNeJeziSe
{
    public partial class IzberiFiguroForm : Form
    {
        public int SelectedIndex { get; private set; }
        private Form1 form1DostopDoMetode; //  VrniBarvoZaIgralca

        public IzberiFiguroForm(string[] options, Form1 form1, int igralecId)
        {
            InitializeComponent();
            listBoxPieces.Items.AddRange(options);
            this.form1DostopDoMetode = form1;

            // Spremenenimo ozadje za prikaz
            this.BackColor = form1.VrniBarvoZaIgralca(igralecId);
        }

        private void listBoxPieces_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBoxPieces.SelectedIndex >= 0)
            {
                SelectedIndex = listBoxPieces.SelectedIndex;
                // zapremo form
                DialogResult = DialogResult.OK;
                Close();
            }
        }
    }
}
