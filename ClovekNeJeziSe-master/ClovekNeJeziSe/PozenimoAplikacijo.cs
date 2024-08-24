using System;
using System.Windows.Forms;

namespace ClovekNeJeziSe
{
    static class PozenimoAplikacijo
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
