namespace ClovekNeJeziSe
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            panelIgralnoPolje = new Panel();
            gumbZaMetKocke = new Button();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            timerHideDice = new System.Windows.Forms.Timer(components);
            zamikTimer = new System.Windows.Forms.Timer(components);
            barvaTimer = new System.Windows.Forms.Timer(components);
            panelBarve = new Panel();
            oznakaRezultatKocke = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panelBarve.SuspendLayout();
            SuspendLayout();
            // 
            // panelIgralnoPolje
            // 
            panelIgralnoPolje.Location = new Point(12, 15);
            panelIgralnoPolje.Margin = new Padding(3, 4, 3, 4);
            panelIgralnoPolje.Name = "panelIgralnoPolje";
            panelIgralnoPolje.Size = new Size(600, 750);
            panelIgralnoPolje.TabIndex = 0;
            panelIgralnoPolje.Paint += panelIgralnoPolje_Paint;
            
            // 
            // gumbZaMetKocke
            // 
            gumbZaMetKocke.Location = new Point(24, 32);
            gumbZaMetKocke.Margin = new Padding(3, 4, 3, 4);
            gumbZaMetKocke.Name = "gumbZaMetKocke";
            gumbZaMetKocke.Size = new Size(100, 62);
            gumbZaMetKocke.TabIndex = 1;
            gumbZaMetKocke.Text = "Vrzi kocko";
            gumbZaMetKocke.UseVisualStyleBackColor = true;
            gumbZaMetKocke.Click += gumbZaMetKocke_Click;
            // 
            // button1
            // 
            button1.Location = new Point(24, 667);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(100, 62);
            button1.TabIndex = 4;
            button1.Text = "Nova igra";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(30, 294);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(80, 80);
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // panelBarve
            // 
            panelBarve.Controls.Add(button1);
            panelBarve.Controls.Add(gumbZaMetKocke);
            panelBarve.Controls.Add(pictureBox1);
            panelBarve.Controls.Add(oznakaRezultatKocke);
            panelBarve.Location = new Point(632, 15);
            panelBarve.Margin = new Padding(2, 2, 2, 2);
            panelBarve.Name = "panelBarve";
            panelBarve.Size = new Size(145, 750);
            panelBarve.TabIndex = 6;
            // 
            // oznakaRezultatKocke
            // 
            oznakaRezultatKocke.AutoSize = true;
            oznakaRezultatKocke.Location = new Point(24, 294);
            oznakaRezultatKocke.Name = "oznakaRezultatKocke";
            oznakaRezultatKocke.Size = new Size(0, 20);
            oznakaRezultatKocke.TabIndex = 2;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 790);
            Controls.Add(panelBarve);
            Controls.Add(panelIgralnoPolje);
            Margin = new Padding(3, 4, 3, 4);
            Name = "Form1";
            Text = "Človek ne jezi se";
            FormClosing += Form1_Zapiranje;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panelBarve.ResumeLayout(false);
            panelBarve.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.Panel panelIgralnoPolje;
        private System.Windows.Forms.Button gumbZaMetKocke;
        private Button button1;
        private PictureBox pictureBox1;
        private System.Windows.Forms.Timer timerHideDice;
        private System.Windows.Forms.Timer zamikTimer;
        private System.Windows.Forms.Timer barvaTimer;
        private Panel panelBarve;
        private Label oznakaRezultatKocke;
    }
}
