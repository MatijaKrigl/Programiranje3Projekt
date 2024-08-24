using System.Diagnostics;

namespace ClovekNeJeziSe
{
    public class Igra
    {
        public Igralec[] Igralci { get; private set; }
        private int trenutniIgralecIndeks;
        private Random nakljucno;
        private int stevecPotez = 0;
        public Dictionary<int, Color> obarvanaPolja;
        private int[,] polje;
        public event Action PoljeObarvano;
        private int prejsnjeObarvanoPolje;

        public static readonly int[][] ZacetnePozicije =
        {
            new int[] { 1 },    // Začetne pozicije za igralca 0
            new int[] { 13 }, // Začetne pozicije za igralca 1
            new int[] { 25 },
            new int[] { 37 }
        };

        public static readonly int[][] HisicaPozicije =
        {
            new int[] { -11, -12, -13, -14 }, // Hišica za igralca 0
            new int[] { -21, -22, -23, -24 }, // Hišica za igralca 1
            new int[] { -31, -32, -33, -34 }, // Hišica za igralca 2
            new int[] { -41, -42, -43, -44 }  // Hišica za igralca 3
        };

        public static readonly int[][] BazaPozicije =
        {
            new int[] { -111, -112, -113, -114 }, // Baza za igralca 0
            new int[] { -211, -212, -213, -214 },
            new int[] { -311, -312, -313, -314 },
            new int[] { -411, -412, -413, -414 }
        };

        /// <summary>
        /// Inicializira novo igro z določenim številom igralcev.
        /// Nastavi igralce, njihove figure in določi, kateri igralec je na vrsti.
        /// </summary>
        /// <param name="steviloIgralcev">Število igralcev v igri.</param>
        public Igra(int steviloIgralcev)
        {
            
            obarvanaPolja = new Dictionary<int, Color>();
            Igralci = new Igralec[steviloIgralcev];
            for (int i = 0; i < steviloIgralcev; i++)
            {
                Igralci[i] = new Igralec(i);
                for (int j = 0; j < 4; j++)
                {
                    Igralci[i].Figure[j] = BazaPozicije[i][j];
                }
            }
            trenutniIgralecIndeks = 0;
            nakljucno = new Random();
            prejsnjeObarvanoPolje = -1;
        }

        public void NastaviPolje(int[,] novoPolje)
        {
            polje = novoPolje;
        }

        public int VrziKocko()
        {
            return nakljucno.Next(1, 7);
        }

        /// <summary>
        /// Izvede naslednji korak v igri na podlagi rezultata metanja kocke.
        /// Poveča števec potez, preveri, ali je potrebno preiti na naslednjega igralca,
        /// in vsake 10. poteze ustvari novo magično polje.
        /// </summary>
        /// <param name="rezultatKocke">Rezultat metanja kocke, ki določa, koliko korakov bo igralec napredoval.</param>
        public void NaslednjiKorak(int rezultatKocke)
        {
            stevecPotez++;
            // Če igralec ni vrgel 6, premaknemo na naslednjega igralca
            if (rezultatKocke != 6)
            {
                trenutniIgralecIndeks = (trenutniIgralecIndeks + 1) % Igralci.Length;
            }
            if (stevecPotez % 10 == 0)  // Na 10 potez magično polje izgine in nastane novo
            {
                // Spremenimo prejšnje obarvano polje nazaj v navadno
                if (prejsnjeObarvanoPolje != -1 && obarvanaPolja.ContainsKey(prejsnjeObarvanoPolje))
                {
                    obarvanaPolja.Remove(prejsnjeObarvanoPolje);
                }
                ObarvajNakljucnoPolje();
            }
        }

        /// <summary>
        /// Obarva naključno izbrano polje na igralni plošči v rdečo ali zeleno barvo.
        /// Če polje uspešno obarvamo, se sproži dogodek `PoljeObarvano`.
        /// </summary>
        public void ObarvajNakljucnoPolje()
        {
            var nakljucnoPolje = VrniNakljucnoPolje();
            if (nakljucnoPolje.HasValue)
            {
                // Pretvori (x, y) koordinate v vrednost iz polja
                int pozicija = polje[nakljucnoPolje.Value.x, nakljucnoPolje.Value.y];

                // 50% verjetnost za rdečo ali zeleno barvo
                if (nakljucno.Next(0, 2) == 0)
                {
                    // Obarvaj v rdečo
                    obarvanaPolja[pozicija] = Color.Red;
                }
                else
                {
                    // Obarvaj v zeleno
                    obarvanaPolja[pozicija] = Color.Green;
                }

                prejsnjeObarvanoPolje = pozicija; // Shranimo trenutno obarvano polje
                PoljeObarvano?.Invoke();
            }
        }

        /// <summary>
        /// Vrne naključno izbrano prazno polje na igralni plošči, ki ni zasedeno z nobeno figuro.
        /// </summary>
        /// <returns>
        /// Tuple `(int x, int y)` z koordinatami naključno izbranega praznega polja na plošči.
        /// Če ni na voljo nobenih prostih polj, metoda vrne `null`.
        /// </returns>
        /// <remarks>
        /// Metoda najprej pregleda celotno igralno ploščo in identificira vsa polja, ki so prazna
        /// (tj. niso zasedena z nobeno figuro) in ki imajo vrednosti med 1 in 48.
        /// Nato naključno izbere eno izmed teh praznih polj in vrne njegove koordinate.
        /// Če ni na voljo nobenih prostih polj, metoda vrne `null`.
        /// </remarks>
        private (int x, int y)? VrniNakljucnoPolje()
        {
            List<(int x, int y)> dovoljenaPolja = new List<(int x, int y)>();

            // Poiščemo vsa dovoljena prazna polja (brez figur)
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 15; j++)
                {
                    if (polje[i, j] > 0 && polje[i, j] <= 48 && !Igralci.Any(igralec => igralec.Figure.Contains(polje[i, j])))
                    {
                        dovoljenaPolja.Add((i, j));
                    }
                }
            }

            if (dovoljenaPolja.Count > 0)
            {
                // Naključno izberemo eno izmed praznih polj
                var index = nakljucno.Next(dovoljenaPolja.Count);
                return dovoljenaPolja[index];
            }

            return null; // Ni prostih polj
        }

        public Igralec VrniTrenutnegaIgralca()
        {
            return Igralci[trenutniIgralecIndeks];
        }

        /// <summary>
        /// Premakne izbrano figuro določenega igralca glede na število vrženih korakov.
        /// Obdeluje različne scenarije, kot so premik figure na polje, vstop v hišico in odstranitev nasprotnikove figure.
        /// </summary>
        /// <param name="igralec">Igralec, katerega figuro premikamo.</param>
        /// <param name="indeksFigure">Indeks figure, ki jo želimo premakniti.</param>
        /// <param name="koraki">Število korakov, ki jih je potrebno izvesti.</param>
        public void PremakniFiguro(Igralec igralec, int indeksFigure, int koraki)
        {
            int trenutnaPozicija = igralec.Figure[indeksFigure];
            int prejsnaPozicija = trenutnaPozicija; // Shrani prejšnjo pozicijo
            int novaPozicija = trenutnaPozicija + koraki;

            if (obarvanaPolja.ContainsKey(novaPozicija))
            {
                var barva = obarvanaPolja[novaPozicija];

                if (barva == Color.Green)
                {
                    // Premaknemo za dodatnih 10 polj naprej
                    novaPozicija += 10;
                    obarvanaPolja.Remove(novaPozicija); // Odstranimo zeleno polje
                }
                else if (barva == Color.Red)
                {
                    // Premaknemo figuro na začetek
                    igralec.Figure[indeksFigure] = ZacetnePozicije[igralec.Id][0];
                    obarvanaPolja.Remove(novaPozicija); // Odstranimo rdeče polje
                    PoljeObarvano?.Invoke();
                    return;
                }

                obarvanaPolja.Remove(novaPozicija);
                PoljeObarvano?.Invoke();
            }

            // Preverimo če je figura na začetni poziciji (baza)
            if (BazaPozicije[igralec.Id].Contains(trenutnaPozicija))
            {
                if (koraki == 6)
                {
                    // Premik figure iz začetne pozicije na ploščo
                    igralec.Figure[indeksFigure] = ZacetnePozicije[igralec.Id][0];
                }
                else
                {
                    return;
                }
            }
            // Preverimo če je figura že v hišici
            else if (HisicaPozicije[igralec.Id].Contains(trenutnaPozicija))
            {
                // Določi indeks v hišici (npr. -11, -12, itd.)
                int trenutniIndeksPoljaHisice = Array.IndexOf(HisicaPozicije[igralec.Id], trenutnaPozicija);
                int noviIndeksVHisici = trenutniIndeksPoljaHisice + koraki;
                if (noviIndeksVHisici <= 4)
                {
                    // Premakni figuro globlje v hišico
                    igralec.Figure[indeksFigure] = HisicaPozicije[igralec.Id][noviIndeksVHisici];

                    // Preverimo, ali so vse figure v hišici
                    if (igralec.Figure.All(f => HisicaPozicije[igralec.Id].Contains(f)))
                    {
                        // Po zaprtju MessageBox začnemo novo igro
                        Form1 form = (Form1)Application.OpenForms["Form1"];
                        form.ZacniNovoIgro(); MessageBox.Show($"Igralec {igralec.Id + 1} je zmagal!", "Zmaga", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    // Če premik preseže meje hišice, je premik neveljaven
                    Debug.WriteLine($"Premik za figuro {indeksFigure + 1} ni mogoč, ker bi presegel meje hišice.");
                    return;
                }
            }
            else if (trenutnaPozicija >= 0) // Če je figura na glavni plošči
            {
                // Standardna logika za premikanje figur na glavni plošči
                if (novaPozicija > 48)
                {
                    novaPozicija = novaPozicija % 48;
                }

                // Preveri, če bi figura morala vstopiti v hišico
                if ((novaPozicija >= ZacetnePozicije[igralec.Id][0] && prejsnaPozicija < ZacetnePozicije[igralec.Id][0]) ||
         (igralec.Id == 0 && prejsnaPozicija > 42 && novaPozicija >= 1))
                {
                    int korakiVHisico = novaPozicija - ZacetnePozicije[igralec.Id][0];
                    if (korakiVHisico < HisicaPozicije[igralec.Id].Length)
                    {
                        igralec.Figure[indeksFigure] = HisicaPozicije[igralec.Id][korakiVHisico];
                    }
                    else
                    {
                        // Neveljaven premik, figura ne more vstopiti v hišico
                        Debug.WriteLine($"Figura {indeksFigure + 1} ne more vstopiti v hišico.");
                    }
                }
                else
                {
                    // Običajen premik na plošči
                    igralec.Figure[indeksFigure] = novaPozicija;
                }

                // Preverimo še druge figure
                foreach (var drugiIgralec in Igralci)
        {
                    if (drugiIgralec.Id != igralec.Id)
                    {
                        for (int i = 0; i < drugiIgralec.Figure.Length; i++)
                        {
                            if (drugiIgralec.Figure[i] == igralec.Figure[indeksFigure])
                            {
                                drugiIgralec.Figure[i] = BazaPozicije[drugiIgralec.Id][i];
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Pridobi trenutno stanje igre, vključno z informacijami o vseh igralcih, njihovih figurah in ali so računalniški nasprotniki.
        /// </summary>
        /// <returns>Vrnjen je niz, ki vsebuje podrobnosti o vsakem igralcu in njihovih figurah.</returns>
        public string PridobiStanjeIgre()
        {
            string stanjeIgre = "";

            foreach (var igralec in Igralci)
            {
                stanjeIgre += $"Igralec {igralec.Id}:\n";
                stanjeIgre += $"Figure: {string.Join(", ", igralec.Figure)}\n";
                stanjeIgre += $"Je računalnik: {igralec.JeRacunalnik}\n";
                stanjeIgre += "\n";
            }

            return stanjeIgre;
        }

        /// <summary>
        /// Naloži shranjeno stanje igre iz podanega niza in nastavi stanje igre na podlagi teh podatkov.
        /// </summary>
        /// <param name="stanjeIgre">Niz, ki vsebuje podatke o shranjenem stanju igre.</param>
        public void NaloziStanjeIgre(string stanjeIgre)
        {
            // Razdelimo stanje igre na vrstice
            var vrstice = stanjeIgre.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int i = 0;

            while (i < vrstice.Length)
            {
                if (vrstice[i].StartsWith("Igralec"))
                {
                    // Izvlečemo ID igralca
                    int id = int.Parse(vrstice[i].Split(' ')[1].Trim(':'));
                    var igralec = Igralci[id];

                    // Naslednja vrstica vsebuje figure
                    string[] figureStr = vrstice[++i].Replace("Figure: ", "").Split(',');
                    for (int j = 0; j < figureStr.Length; j++)
                    {
                        igralec.Figure[j] = int.Parse(figureStr[j].Trim());
                    }

                    // Naslednja vrstica pove, če je igralec računalnik
                    igralec.JeRacunalnik = vrstice[++i].Contains("true");
                }
                i++;
            }
        }
    }

    public class Igralec
    {
        public int Id { get; set; }
        public int[] Figure { get; set; }
        public bool JeRacunalnik { get; set; }

        public Igralec(int id)
        {
            Id = id;
            Figure = new int[4] { -1, -1, -1, -1 };
        }
    }
}