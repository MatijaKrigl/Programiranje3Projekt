using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace ClovekNeJeziSe
{
    public partial class Form1 : Form
    {
        private Igra igra;
        private Random nakljucno;
        public Brush brush;
        private bool kockaJePrikazana = false;

        public Form1()
        {
            InitializeComponent();
            // nastavimo velikost forma na zacetku
            this.Height = 630;
            this.Width = 700;
            PrikaziNavodila();
            igra = new Igra(4); // Začetek igre s 4 igralci
            nakljucno = new Random();
            timerHideDice.Interval = 1800;
            timerHideDice.Tick += new EventHandler(timerSkrijKocko);
            zamikTimer.Interval = 1800;
            zamikTimer.Tick += new EventHandler(ZamikTimer_Tick);
            igra.PoljeObarvano += () => panelIgralnoPolje.Invalidate();

            using (FormStart startForm = new FormStart())
            {
                var result = startForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (startForm.ContinueGame && System.IO.File.Exists("stanjeIgre.txt"))
                    {
                        // Naložimo shranjeno stanje igre
                        string shranjenoStanje = System.IO.File.ReadAllText("stanjeIgre.txt");
                        igra = new Igra(4); // Potrebno za inicializacijo
                        igra.NaloziStanjeIgre(shranjenoStanje);
                    }
                    else
                    {
                        // Začnemo novo igro
                        igra = new Igra(4);
                    }
                }
                else
                {
                    // če uporabnik zapre formo brez izbire, zapremo aplikacijo
                    this.Close();
                }
            }

            PosodobiPrikazTrenutnegaIgralca();

            this.FormClosing += new FormClosingEventHandler(Form1_Zapiranje);
        }

        private void PrikaziNavodila()
        {
            string sporocilo = "Dobrodošli v igro! \n\n" +
                               "Če igralec pride na zeleno polje, ga bo premaknilo za 10 polj naprej.\n" +
                               "Če igralec pride na rdeče polje, ga bo vrnilo na začetek.\n" +
                               "Ta polja se pojavljajo naključno vsakih nekaj potez.\n\n" +
                               "Uživajte v igri!";
            MessageBox.Show(sporocilo, "Navodila za igro", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ShraniStanjeIgre()
        {
            try
            {
                string stanjeIgre = igra.PridobiStanjeIgre();
                System.IO.File.WriteAllText("stanjeIgre.txt", stanjeIgre);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Prišlo je do napake pri shranjevanju igre: " + ex.Message);
            }
        }

        private void ZamikTimer_Tick(object sender, EventArgs e)
        {
            zamikTimer.Stop(); // Ustavimo Timer, da se ne ponavlja
            PosodobiPrikazTrenutnegaIgralca(); // Pokličemo metodo po preteku časa
            gumbZaMetKocke.Enabled = true;
        }

        /// <summary>
        /// Nariše igralno polje na podlagi določenih koordinat in barv. 
        /// Polje je sestavljeno iz celic, ki predstavljajo igralne ploščice.
        /// Barve celic se prilagajajo glede na trenutnega igralca.
        /// </summary>
        /// <param name="g">Grafični objekt, ki omogoča risanje na površino.</param>
        private void NarišiPolje(Graphics g)
        {
            int velikostCelice = 40;
            Pen pero = new Pen(Color.Black, 2);
            Brush sivaCopic = new SolidBrush(Color.DarkGray);
            Brush rumenaCopic = new SolidBrush(Color.Yellow);

            // Vzorec kroženja po igralnem polju znotraj križa
            int[,] polje = new int[15, 15]
            {
        { -111, -112, -1, -1, -1, -1, -1, -1, -1, -1, -1, -211, -212, -1, -1 },
        { -113, -114, -1, -1, -1, 11, 12, 13, -1, -1, -1, -213, -214, -1, -1 },
        { -1, -1, -1, -1, -1, 10,-21, 14, -1, -1, -1, -1, -1, -1, -1 },
        { -1, -1, -1, -1, -1,  9,-22, 15, -1, -1, -1, -1, -1, -1, -1 },
        { -1, -1, -1, -1, -1,  8,-23, 16, -1, -1, -1, -1, -1, -1, -1 },
        { -1, -1, -1, -1, -1,  7,-24, 17, -1, -1, -1, -1, -1, -1, -1 },
        {  1,  2,  3,  4,  5,  6, -1, 18, 19, 20, 21, 22, 23, -1, -1 },
        { 48,-11,-12,-13,-14, -1, -1, -1,-34,-33,-32,-31, 24, -1, -1 },
        { 47, 46, 45, 44, 43, 42, -1, 30, 29, 28, 27, 26, 25, -1, -1 },
        { -1, -1, -1, -1, -1, 41,-44, 31, -1, -1, -1, -1, -1, -1, -1 },
        { -1, -1, -1, -1, -1, 40,-43, 32, -1, -1, -1, -1, -1, -1, -1 },
        { -1, -1, -1, -1, -1, 39,-42, 33, -1, -1, -1, -1, -1, -1, -1 },
        { -411, -412,  -1, -1, -1, 38,-41, 34, -1, -1, -1, -311, -312, -1, -1},
        { -413, -414, -1, -1, -1, 37, 36, 35, -1, -1, -1, -313, -314, -1, -1},
        {  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
            };

            // Naročimo igri, da uporabi to polje
            igra.NastaviPolje(polje);

            var trenutniIgralec = igra.VrniTrenutnegaIgralca();
            

            // Nariši zunanji okvir in zapolni polja, ki niso enaka -1
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    int pozicija = polje[i, j];
                    if (pozicija > -1)
                    {
                        // Preverimo, ali je to polje posebej obarvano
                        if (igra.obarvanaPolja.ContainsKey(pozicija))
                        {
                            // Uporabimo obarvano polje
                            g.FillRectangle(new SolidBrush(igra.obarvanaPolja[pozicija]), j * velikostCelice, i * velikostCelice, velikostCelice, velikostCelice);
                        }
                        else
                        {
                            // Pobarvaj določena polja, če je na vrsti rumeni igralec
                            if (trenutniIgralec.Id == 3 && (polje[i, j] == -411 || polje[i, j] == -412 || polje[i, j] == -413 || polje[i, j] == -414))
                            {
                                g.FillRectangle(rumenaCopic, j * velikostCelice, i * velikostCelice, velikostCelice, velikostCelice);
                            }
                            else
                            {
                                g.FillRectangle(sivaCopic, j * velikostCelice, i * velikostCelice, velikostCelice, velikostCelice);
                            }
                        }
                    }
                    g.DrawRectangle(Pens.Black, j * velikostCelice, i * velikostCelice, velikostCelice, velikostCelice);
                }
            }
        }

        /// <summary>
        /// Obdeluje dogodek klika na gumb za met kocke. 
        /// Po metanju kocke prikaže rezultat, omogoči izbiro in premik figur glede na rezultat metanja.
        /// </summary>
        /// <param name="sender">Pošiljatelj dogodka.</param>
        /// <param name="e">Podatki o dogodku.</param>
        private void gumbZaMetKocke_Click(object sender, EventArgs e)
        {
            gumbZaMetKocke.Enabled = false;
            kockaJePrikazana = true;
            int rezultatKocke = igra.VrziKocko();
            oznakaRezultatKocke.Text = $"Rezultat: {rezultatKocke}";
            oznakaRezultatKocke.Visible = true;
            var trenutniIgralec = igra.VrniTrenutnegaIgralca();
            string imagePath = System.IO.Path.Combine(Application.StartupPath, "Images", $"dice_{rezultatKocke}.png");
            pictureBox1.Image = Image.FromFile(imagePath);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            var figuraNaPolju = trenutniIgralec.Figure.Where(f => f >= 0).ToList();
            var figuraNaZacetku = trenutniIgralec.Figure.Select((value, index) => new { value, index })
                                    .Where(x => Igra.BazaPozicije[trenutniIgralec.Id].Contains(x.value))
                                    .Select(x => x.index).ToList();
            var figuraVHisici = trenutniIgralec.Figure.Select((value, index) => new { value, index })
                                    .Where(x => Igra.HisicaPozicije[trenutniIgralec.Id].Contains(x.value) &&
                                                x.value + rezultatKocke <= Igra.HisicaPozicije[trenutniIgralec.Id].Last())
                                    .Select(x => x.index).ToList();
            timerHideDice.Start();

            if (rezultatKocke == 6)
            {
                if (figuraNaPolju.Count > 0 && figuraNaZacetku.Count > 0)
                {
                    // Igralec lahko izbere med premikom figure na plošči ali figuro iz začetne pozicije
                    int indeksIzbraneFigure = IzberiFiguro(trenutniIgralec, rezultatKocke, true);
                    if (figuraNaZacetku.Contains(indeksIzbraneFigure))
                    {
                        igra.PremakniFiguro(trenutniIgralec, indeksIzbraneFigure, rezultatKocke); // Aktiviraj figuro iz začetne pozicije
                    }
                    else
                    {
                        igra.PremakniFiguro(trenutniIgralec, indeksIzbraneFigure, rezultatKocke); // Premakni figuro na plošči
                    }
                    gumbZaMetKocke.Enabled = true;
                }
                else if (figuraNaZacetku.Count > 0)
                {
                    // če so samo figure v začetnih pozicijah
                    int selectedFigureIndex = figuraNaZacetku.First();
                    igra.PremakniFiguro(trenutniIgralec, selectedFigureIndex, rezultatKocke);
                }
                else if (figuraNaPolju.Count > 0 || figuraVHisici.Count > 0)
                {
                    // če so figure na plošči ali v hišici
                    int selectedFigureIndex = IzberiFiguro(trenutniIgralec, rezultatKocke);
                    igra.PremakniFiguro(trenutniIgralec, selectedFigureIndex, rezultatKocke);
                }
            }
            else if (figuraNaPolju.Count > 0 || figuraVHisici.Count > 0)
            {
                if (figuraNaPolju.Count > 1 || figuraVHisici.Count > 0)
                {
                    int selectedFigureIndex = IzberiFiguro(trenutniIgralec, rezultatKocke);
                    igra.PremakniFiguro(trenutniIgralec, selectedFigureIndex, rezultatKocke);
                }
                else
                {
                    int indexFigure = trenutniIgralec.Figure.ToList().IndexOf(figuraNaPolju.First());
                    igra.PremakniFiguro(trenutniIgralec, indexFigure, rezultatKocke);
                }
            }

            panelIgralnoPolje.Invalidate();
            igra.NaslednjiKorak(rezultatKocke);
            zamikTimer.Start();
        }

        /// <summary>
        /// Prikaže uporabniku dialog za izbiro figure, ki jo želi premakniti. 
        /// Vključuje figure, ki so na plošči ali v začetnih pozicijah, glede na parameter.
        /// </summary>
        /// <param name="trenutniIgralec">Trenutni igralec, ki je na potezi.</param>
        /// <param name="includeStart">Določa, ali naj bo vključena tudi izbira figur v začetnih pozicijah.</param>
        /// <returns>Vrnjen je indeks izbrane figure. Če uporabnik prekliče izbiro, se vrne indeks prve možnosti.</returns>
        public int IzberiFiguro(Igralec trenutniIgralec, int koraki, bool vkljuciZacetek = false)
        {
            // Poišči indekse figur, ki so bodisi na plošči, v začetnih pozicijah, ali v hišici (če izpolnjujejo pogoje)
            var opcije = trenutniIgralec.Figure
                .Select((value, index) => new { value, index })
                .Where(x =>
                    (vkljuciZacetek && Igra.BazaPozicije[trenutniIgralec.Id].Contains(x.value)) || // Figure v začetnih pozicijah
                    (x.value >= 0 && !Igra.HisicaPozicije[trenutniIgralec.Id].Contains(x.value)) || // Figure na plošči
                    (Igra.HisicaPozicije[trenutniIgralec.Id].Contains(x.value) && // Figure v hišici, ki izpolnjujejo pogoj
                    x.value - koraki >= Igra.HisicaPozicije[trenutniIgralec.Id].Last()) // Pogoj za hišico
                )
                .Select(x => x.index)
                .ToArray();

            // Pretvorimo opcije v besedilo za prikaz
            string[] opcijeBesedilo = opcije.Select(index => $"Figura {index + 1}").ToArray();

            // Prikažimo formo za izbiro figure
            using (var selectPieceForm = new IzberiFiguroForm(opcijeBesedilo, this, trenutniIgralec.Id))
            {
                if (selectPieceForm.ShowDialog() == DialogResult.OK)
                {
                    return opcije[selectPieceForm.SelectedIndex];
                }
            }

            // Privzeto vrnemo prvo opcijo, če uporabnik prekliče izbiro
            return opcije[0];
        }


        private void timerSkrijKocko(object sender, EventArgs e)
        {
            // Timer je končan, skrijemo sliko kocke
            pictureBox1.Image = null;
            oznakaRezultatKocke.Visible = false;
            kockaJePrikazana = false;

            PosodobiPrikazTrenutnegaIgralca();

            // Ustavimo Timer
            timerHideDice.Stop();
        }

        private void panelIgralnoPolje_Paint(object sender, PaintEventArgs e)
        {
            NarišiPolje(e.Graphics);
            NarišiFigure(e.Graphics);
        }

        /// <summary>
        /// metoda narise posamezno figuro (krogci) in stevilke na figuri 
        /// </summary>
        /// <param name="g"></param>
        private void NarišiFigure(Graphics g)
        {
            int velikostCelice = 40;
            foreach (var igralec in igra.Igralci)
            {
                for (int i = 0; i < igralec.Figure.Length; i++)
                {
                    var figura = igralec.Figure[i];
                    if (figura != -1)
                    {
                        var (x, y) = VrniKoordinatePolja(figura);
                        g.FillEllipse(new SolidBrush(VrniBarvoZaIgralca(igralec.Id)), x * velikostCelice + 5, y * velikostCelice + 5, 30, 30);
                        using (Font font = new Font("Arial", 16))
                        {
                            string stevilka = (i + 1).ToString();  
                            var velikostStevilke = g.MeasureString(stevilka, font);
                            float textX = x * velikostCelice + 5 + (30 - velikostStevilke.Width) / 2;
                            float textY = y * velikostCelice + 5 + (30 - velikostStevilke.Height) / 2;
                            g.DrawString(stevilka, font, Brushes.Black, textX, textY);
                        }
                    }
                }
            }
            this.Invalidate();
        }

        /// <summary>
        /// Vrne koordinate določene pozicije na igralni plošči.
        /// </summary>
        /// <param name="pozicija">Pozicija na plošči, za katero so potrebne koordinate.</param>
        /// <returns>
        /// Točka, ki predstavlja (X, Y) koordinate na igralni plošči. 
        /// Če pozicija ni najdena, vrne (0, 0).
        /// </returns>
        private (int, int) VrniKoordinatePolja(int pozicija)
        {
            // Vzorec kroženja po igralnem polju znotraj križa
            int[,] polje = new int[15, 15]
                 {
                { -111, -112, -1, -1, -1, -1, -1, -1, -1, -1, -1, -211, -212, -1, -1 },
                { -113, -114, -1, -1, -1, 11, 12, 13, -1, -1, -1, -213, -214, -1, -1 },
                { -1, -1, -1, -1, -1, 10,-21, 14, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1,  9,-22, 15, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1,  8,-23, 16, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1,  7,-24, 17, -1, -1, -1, -1, -1, -1, -1 },
                {  1,  2,  3,  4,  5,  6, -1, 18, 19, 20, 21, 22, 23, -1, -1 },
                { 48,-11,-12,-13,-14, -1, -1, -1,-34,-33,-32,-31, 24, -1, -1 },
                { 47, 46, 45, 44, 43, 42, -1, 30, 29, 28, 27, 26, 25, -1, -1 },
                { -1, -1, -1, -1, -1, 41,-44, 31, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1, 40,-43, 32, -1, -1, -1, -1, -1, -1, -1 },
                { -1, -1, -1, -1, -1, 39,-42, 33, -1, -1, -1, -1, -1, -1, -1 },
                { -411, -412,  -1, -1, -1, 38,-41, 34, -1, -1, -1, -311, -312, -1, -1},
                { -413, -414, -1, -1, -1, 37, 36, 35, -1, -1, -1, -313, -314, -1, -1},
                {  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 }
                };

            for (int vrstica = 0; vrstica < 15; vrstica++)
            {
                for (int stolpec = 0; stolpec < 15; stolpec++)
                {
                    if (polje[vrstica, stolpec] == pozicija)
                    {
                        return (stolpec, vrstica);
                    }
                }
            }
            return (0, 0); // Privzeta vrednost, če ni pozicije
        }

        /// <summary>
        /// Vrne barvo, ki ustreza določenemu igralcu glede na njegov ID.
        /// </summary>
        /// <param name="igralecId">ID igralca, za katerega želimo pridobiti barvo.</param>
        /// <returns>Barva, ki ustreza igralčevemu ID-ju. Privzeto vrne črno barvo, če ID ni med pričakovanimi vrednostmi.</returns>
        public Color VrniBarvoZaIgralca(int igralecId)
        {
            switch (igralecId)
            {
                case 0: return Color.Red;
                case 1: return Color.Blue;
                case 2: return Color.Green;
                case 3: return Color.Yellow;
                default: return Color.Black;
            }
        }

        /// <summary>
        /// Posodobi prikaz trenutnega igralca na uporabniškem vmesniku. 
        /// Pridobi trenutnega igralca iz igre in nastavi ustrezno ime in barvo.
        /// </summary>
        private void PosodobiPrikazTrenutnegaIgralca()
        {
            if (kockaJePrikazana) return;  // Če je kocka prikazana, ne spremeni barve

            var trenutniIgralec = igra.VrniTrenutnegaIgralca();
            string imeIgralca = trenutniIgralec.Id switch
            {
                0 => "Rdeč",
                1 => "Moder",
                2 => "Zelen",
                3 => "Rumen",
                _ => "Neznan"
            };

            panelBarve.BackColor = VrniBarvoZaIgralca(trenutniIgralec.Id);

        }
        public void ZacniNovoIgro()
        {
            igra = new Igra(4);
            PosodobiPrikazTrenutnegaIgralca();
            oznakaRezultatKocke.Text = "Rezultat: 0";
            pictureBox1.Image = null;
            panelIgralnoPolje.Invalidate();
        }

        private void Form1_Zapiranje(object sender, FormClosingEventArgs e)
        {
            ShraniStanjeIgre();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            igra = new Igra(4);

            PosodobiPrikazTrenutnegaIgralca();
            oznakaRezultatKocke.Text = "Rezultat: 0";

            // Ponastavimo sliko kocke v PictureBox
            pictureBox1.Image = null;

            //Osvežitev igralnega polja
            panelIgralnoPolje.Invalidate();
        }
    }
}
