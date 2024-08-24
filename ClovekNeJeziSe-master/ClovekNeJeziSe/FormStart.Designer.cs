using Nest;
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
        private Button btnContinue;
        private Button btnNewGame;
        private Label label1;

        public FormStart()
        {
            InitializeComponent();
        }
        private void InitializeComponent()
        {

            btnContinue = new Button();
            btnNewGame = new Button();
            label1 = new Label();
            SuspendLayout();
            // 
            // btnContinue
            // 
            btnContinue.Location = new Point(84, 191);
            btnContinue.Name = "btnContinue";
            btnContinue.Size = new Size(111, 56);
            btnContinue.TabIndex = 0;
            btnContinue.Text = "Nadaljuj z igro";
            btnContinue.UseVisualStyleBackColor = true;
            btnContinue.Click += btnContinue_Click;
            // 
            // btnNewGame
            // 
            btnNewGame.Location = new Point(274, 191);
            btnNewGame.Name = "btnNewGame";
            btnNewGame.Size = new Size(111, 56);
            btnNewGame.TabIndex = 1;
            btnNewGame.Text = "Nova igra";
            btnNewGame.UseVisualStyleBackColor = true;
            btnNewGame.Click += btnNewGame_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 238);
            label1.Location = new Point(168, 88);
            label1.Name = "label1";
            label1.Size = new Size(144, 31);
            label1.TabIndex = 2;
            label1.Text = "Pozdravljeni!";
            // 
            // FormStart
            // 
            ClientSize = new Size(469, 350);
            Controls.Add(label1);
            Controls.Add(btnNewGame);
            Controls.Add(btnContinue);
            Name = "FormStart";
            ResumeLayout(false);
            PerformLayout();
        }
        
    }
}
