using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ClovekNeJeziSe
{
    public partial class FormStart : Form
    {
        public bool ContinueGame { get; private set; }
      

        private void btnContinue_Click(object sender, EventArgs e)
        {
            ContinueGame = true;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnNewGame_Click(object sender, EventArgs e)
        {
            ContinueGame = false;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
