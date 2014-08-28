using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace poker_objet
{
    // fonction principale
    class Poker
    {
        // Attributs
        // Nom du fichier de scores
        private string nomFichier = "scores.txt";
        // Liste des combinaisons possibles
        public enum combinaison { RIEN, PAIRE, DOUBLE_PAIRE, BRELAN, QUINTE, FULL, COULEUR, CARRE, QUINTE_FLUSH };
        // Numéro des cartes a échanger
        private int[] echange = { 0, 0, 0, 0 };
        // Le Jeux
        Jeux MonJeux;
        // Constructeur
        public Poker()
        {
            try
            {
                MonJeux = new Jeux();
                string reponse = "";
                while (true)
                {
                    reponse = menu();
                    if (reponse == "1")
                    {
                        MonJeux.LancerJeux();
                        EchangeDeCarte(MonJeux, echange);
                        AfficheResultat(MonJeux);
                        enregistrer(MonJeux);
                    }
                    else if (reponse == "2")
                    {
                        AfficherScores();
                    }
                    else if (reponse == "3")
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                Console.Clear();
                Poker p = new Poker();
            }
        }
        // Methodes
        //Echange de cartes
        public void EchangeDeCarte(Jeux UnJeux, int[] e)
        {
            int aEchanger = 0;
            bool correct = false;
            bool fin = false;
            for (int i = 0; i < e.Length; i++)
            {
                e[i] = 0;
            }
            do
            {
                string tmp = "";
                Console.Write("Nombre de cartes a echanger ( 0 - 4 ) ? : ");
                tmp = Console.ReadLine().ToString();
                if (tmp == "0")
                {
                    correct = true;
                    fin = true;
                }
                if (tmp == "1" | tmp == "2" | tmp == "3" | tmp == "4")
                {
                    aEchanger = int.Parse(tmp);
                    correct = true;
                }
                else
                    if (fin == false)
                        correct = false;
            } while (!correct);
            if (fin == false)
            {
                Console.WriteLine("\n");
                correct = false;
                bool is_ok = false;
                bool existe = false;
                for (int i = 0; i < aEchanger; i++)
                {
                    do
                    {
                        Console.Write("\t{0} Carte ( 1 - 5 ) : \t", i + 1);
                        string tmps = Console.ReadLine().ToString();
                        int tmp = 0;
                        if (tmps == "1" | tmps == "2" | tmps == "3" | tmps == "4" | tmps == "5")
                        {
                            tmp = int.Parse(tmps);
                            is_ok = true;
                        }
                        else
                        {
                            is_ok = false;
                        }
                        if (is_ok == true)
                        {
                            for (int j = 0; j < e.Length; j++)
                            {
                                if (e[j] == tmp)
                                { existe = true; break; }
                                else
                                    existe = false;
                            }
                            if (tmp >= 1 & tmp <= 5 & existe == false)
                            {
                                e[i] = tmp;
                                correct = true;
                            }
                            else
                            {
                                correct = false;
                            }
                        }
                    } while (!correct);
                }
                Carte tmpc = new Carte();
                for (int i = 0; i < e.Length; i++)
                {
                    if (e[i] != 0)
                    {
                        do
                        {
                            tmpc.tirage();
                            UnJeux.MonJeux[e[i] - 1] = tmpc;
                        } while (!UnJeux.carteUnique(tmpc, e[i] - 1));
                    }
                }
                Console.Clear();
            }
        }
        // Calcule et retourne la COMBINAISON (paire, double-paire... , quinte-flush)
        // pour un jeu complet de 5 cartes.
        // La valeur retournée est un élement de l'énumération 'combinaison' (=constante)
        public combinaison cherche_combinaison(Jeux unJeux)
        {
            unJeux = new Jeux();
            Carte[] unJeu = unJeux.MonJeux;
            int i, j, nbpaires = 0, nb;
            // Nombre de valeurs similaires dans le jeu pour chaque carte
            int[] similaire = { 0, 0, 0, 0, 0 };
            // Booléens : si paire ET brelan alors on a un FULL
            bool paire = false;
            bool brelan = false;
            // Possibilités de quinte. Tableau 4*5
            char[,] quintes = { {'X','V','D','R','A'},
                                {'9','X','V','D','R'},
                                {'8','9','X','V','D'},
                                {'7','8','9','X','V'}
                                };
            // Résultat à renvoyer
            combinaison resultat;
            // Par défaut : aucun jeu
            resultat = combinaison.RIEN;
            // Compte, pour chaque carte, le nombre
            // d'autres cartes ayant la même valeur
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    if (unJeu[i].Valeur == unJeu[j].Valeur)
                        similaire[i]++;
                }
            }
            //---------------------------
            // Recherche des combinasions
            //---------------------------
            // Carré, brelan ou paire ?
            for (i = 0; i < 5; i++)
            {
                // CARRE (4 cartes de même valeur)
                if (similaire[i] == 4)
                {
                    resultat = combinaison.CARRE;
                    return resultat;
                }
                else
                {
                    if (similaire[i] == 3)
                    {
                        // BRELAN (3 cartes de même valeur)
                        // Pas de retour car possibilité de FULL
                        resultat = combinaison.BRELAN;
                        brelan = true;
                    }
                    else
                    {
                        if (similaire[i] == 2)
                        {
                            // PAIRE (2 cartes de même valeur)
                            // Pas de retour car possibilité de DOUBLE PAIRE ou de FULL
                            resultat = combinaison.PAIRE;
                            paire = true; // VRAI
                            // Il peut y avoir plusieurs (cf. DOUBLE PAIRE)
                            nbpaires++;
                        }
                    }
                }
            }
            // Double-paire ?
            // Si double-paire, le compteur 'nbpaires'= 4
            nbpaires = nbpaires / 2;
            if (nbpaires == 2)
            {
                resultat = combinaison.DOUBLE_PAIRE;
                return resultat;
            }
            // Full ?
            if (paire && brelan)
            {
                resultat = combinaison.FULL;
                return resultat;
            }
            // Quinte ou quinte flush ?
            // Les 5 cartes doivent être uniques
            if ((similaire[0] + similaire[1] + similaire[2] + similaire[3] + similaire[4]) == 5)
            {
                // Quinte ?
                for (i = 0; i < 4; i++) // Test de chaque possibilité de quinte (cf. tableau 'quintes')
                {
                    nb = 0; // Deviendra = 5 si les 5 cartes se suivent
                    //(correspondent à une combinaison de quinte)
                    // Parcours de chaque carte du jeu
                    for (j = 0; j < 5; j++)
                    {
                        if (unJeu[j].Valeur == quintes[i, 0] || unJeu[j].Valeur == quintes[i, 1] ||
                        unJeu[j].Valeur == quintes[i, 2] || unJeu[j].Valeur == quintes[i, 3] ||
                        unJeu[j].Valeur == quintes[i, 4])
                            nb++;
                    }
                    if (nb == 5)
                    {
                        resultat = combinaison.QUINTE; // Pas de retour car possibilité de Quinte flush
                        // Quinte flush ?
                        // Une quinte avec des cartes de même famille
                        if (unJeu[0].Famille == unJeu[1].Famille &&
                        unJeu[1].Famille == unJeu[2].Famille &&
                        unJeu[2].Famille == unJeu[3].Famille &&
                        unJeu[3].Famille == unJeu[4].Famille)
                        {
                            resultat = combinaison.QUINTE_FLUSH;
                            return resultat;
                        }
                        break; // Sortie du for
                    }
                }
            }
            // Couleur ?
            // Les cartes de même famille mais qui ne constituent pas une quinte
            if (unJeu[0].Famille == unJeu[1].Famille &&
            unJeu[1].Famille == unJeu[2].Famille &&
            unJeu[2].Famille == unJeu[3].Famille &&
            unJeu[3].Famille == unJeu[4].Famille)
                resultat = combinaison.COULEUR;
            return resultat;
        }
        //Calcul et affichage du résultat
        public void AfficheResultat(Jeux unJeux)
        {
            Console.Clear();
            unJeux.AfficherMonJeu();
            Console.Write("RESULTAT - Vous avez : ");
            Console.ForegroundColor = ConsoleColor.Red;
            // Test de la combinaison
            switch (cherche_combinaison(unJeux))
            {
                case combinaison.RIEN:
                    Console.WriteLine("rien du tout... desole!"); break;
                case combinaison.PAIRE:
                    Console.WriteLine("une simple paire..."); break;
                case combinaison.DOUBLE_PAIRE:
                    Console.WriteLine("une double paire; on peut esperer..."); break;
                case combinaison.BRELAN:
                    Console.WriteLine("un brelan; pas mal..."); break;
                case combinaison.QUINTE:
                    Console.WriteLine("une quinte; bien!"); break;
                case combinaison.FULL:
                    Console.WriteLine("un full; ouahh!"); break;
                case combinaison.COULEUR:
                    Console.WriteLine("une couleur; bravo!"); break;
                case combinaison.CARRE:
                    Console.WriteLine("un carre; champion!"); break;
                case combinaison.QUINTE_FLUSH:
                    Console.WriteLine("une quinte-flush; royal!"); break;
            };
            Console.ForegroundColor = ConsoleColor.White;
        }
        // Affichage du menu
        public string menu()
        {
            string reponse;
            Console.ForegroundColor = ConsoleColor.Red;
            // Positionnement et affichage
            Console.Clear();
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', ' ', 'P', 'O', 'K', 'E', 'R', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', '1', ' ', 'J', 'o', 'u', 'e', 'r', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', '2', ' ', 'S', 'c', 'o', 'r', 'e', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', '3', ' ', 'F', 'i', 'n', ' ', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            Console.WriteLine("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
            Console.ForegroundColor = ConsoleColor.Yellow;
            // Lecture du choix
            do
            {
                Console.Write("Votre choix : ");
                reponse = Console.ReadLine();
            }
            while (reponse != "1" && reponse != "2" && reponse != "3");
            Console.Clear();
            return reponse;
        }
        // Demander si enregistrer ou non
        public void enregistrer(Jeux unJeux)
        {
            unJeux = new Jeux();
            Carte[] unJeu = unJeux.MonJeux;
            bool correct = false;
            string res = "";
            string nom, ligne = "";
            BinaryWriter f = null;
            FileStream fs = null;
            fs = new FileStream(nomFichier, FileMode.Append, FileAccess.Write);
            f = new BinaryWriter(fs);
            do
            {
                Console.Write("Enregistrer le jeu ? ( O / N ) : ");
                string tmp = Console.ReadLine().ToUpper();
                if (tmp == "O" | tmp == "N")
                { correct = true; res = tmp; }
                else
                    correct = false;
            } while (!correct);
            if (res == "O")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Vous pouvez saisir votre nom ( ou pseudo ) : ");
                nom = Console.ReadLine().ToUpper();
                for (int i = 0; i < unJeu.Length; i++)
                {
                    ligne += ";" + unJeu[i].Famille.ToString() + ";" + unJeu[i].Valeur;
                }
                f.Write(nom + ligne);
            }
            f.Close();
        }
        // Afficher score
        public void AfficherScores()
        {
            BinaryReader br = null;
            FileStream fs = null;
            fs = new FileStream(nomFichier, FileMode.Open, FileAccess.Read);
            if (fs == null)
            {
                throw new Exception("Ouverture impossible de " + nomFichier);
            }
            br = new BinaryReader(fs);
            int ligne = 0;
            fs.Seek(1, SeekOrigin.Begin);
            char[] tab = new char[fs.Length];
            int i = 0;
            while (fs.Position < fs.Length & i < fs.Length)
            {
                tab[i] = br.ReadChar();
                i++;
            }
            i = 0;
            int position = i;
            do
            {
                Carte[] c = new Carte[5];
                string nom = "";
                int nbpoint = 11;
                for (i = position; i < tab.Length; i++)
                {
                    if (tab[i] == ';')
                    { nbpoint--; break; }
                    nom += tab[i];
                }
                int a = 0;
                int j = 0;
                while (j < 5)
                {
                    if (tab[i] != ';' & a == 0)
                    {
                        c[j].Famille = int.Parse(tab[i].ToString());
                        a = 1;
                    }
                    else if (tab[i] != ';' & a == 1)
                    {
                        c[j].Valeur = tab[i];
                        a = 0;
                    }
                    else if (tab[i] == ';')
                        nbpoint--;
                    if (c[j].Famille != '\0' && c[j].Valeur != '\0')
                        j++;
                    i++;
                }
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(nom + " :");
                for (j = 0; j < 5; j++)
                {
                    Jeux unJeux = new Jeux();
                    Carte[] unJeu = unJeux.MonJeux;
                    unJeu[i].AffichageCarte(j, ligne);
                }
                if (nbpoint == 0)
                {
                    ligne++;
                }
                ligne += 14;
                position = i + 1;
                // 26
                // 51
                // 76
            } while (position < tab.Length);
            br.Close();
            Console.ReadKey();
        }
    }
    // Classe de l'objet carte
    class Carte
    {
        // Attributs
        private char valeur;
        private int famille;
        //Coordonnées
        private int LigneDebut;
        private int ColoneDebut;
        // Propriété
        public char Valeur
        {
            get { return valeur; }
            set { valeur = value; }
        }
        public int Famille
        {
            get { return famille; }
            set { famille = value; }
        }
        // Valeurs des cartes : As, Roi,...
        private char[] valeurs = { 'A', 'R', 'D', 'V', 'X', '9', '8', '7' };
        // Codes ASCII (3 : coeur, 4 : carreau, 5 : trèfle, 6 : pique)
        private int[] familles = { 3, 4, 5, 6 };
        // Constructeur
        public Carte()
        {
            valeur = '7';
            famille = 0;
            LigneDebut = 0;
            ColoneDebut = 0;
        }
        public Carte(char val, int fam)
        {
            valeur = val;
            famille = fam;
        }
        // Methodes
        // Génère une carte
        public Carte tirage()
        {
            Random r = new Random();
            // Valeur aléatoire (1 sur les 8)
            Valeur = valeurs[r.Next(0, 7)];
            // Famille aléatoire (1 sur les 4)
            Famille = familles[r.Next(0, 3)];
            Carte ca = new Carte(Valeur, Famille);
            return ca;
        }
        //placer curseur X Y
        public void AllerA(int x, int y)
        {
            Console.SetCursorPosition(x + ColoneDebut, y + LigneDebut);
        }
        //Afficher carte
        public void AffichageCarte(int colone, int ligne)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.BackgroundColor = ConsoleColor.White;
            switch (famille)
            {
                case 3:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 5:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                case 6:
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
            }
            // Affichage de la carte
            AllerA(colone * 15, 1 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
            AllerA(colone * 15, 2 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)famille, ' ', (char)famille, ' ', (char)famille, ' ', (char)famille, ' ', (char)famille, '|');
            AllerA(colone * 15, 3 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            AllerA(colone * 15, 4 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)famille, ' ', ' ', ' ', ' ', ' ', ' ', ' ', (char)famille, '|');
            AllerA(colone * 15, 5 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', (char)valeur, (char)valeur, (char)valeur, ' ', ' ', ' ', '|');
            AllerA(colone * 15, 6 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)famille, ' ', ' ', (char)valeur, (char)valeur, (char)valeur, ' ', ' ', (char)famille, '|');
            AllerA(colone * 15, 7 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', (char)valeur, (char)valeur, (char)valeur, ' ', ' ', ' ', '|');
            AllerA(colone * 15, 8 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)famille, ' ', ' ', ' ', ' ', ' ', ' ', ' ', (char)famille, '|');
            AllerA(colone * 15, 9 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', '|');
            AllerA(colone * 15, 10 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '|', (char)famille, ' ', (char)famille, ' ', (char)famille, ' ', (char)famille, ' ', (char)famille, '|');
            AllerA(colone * 15, 11 + ligne);
            Console.Write("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}\n", '*', '-', '-', '-', '-', '-', '-', '-', '-', '-', '*');
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            AllerA((colone * 15) + 5, 12 + ligne);
            Console.Write("{0}", colone + 1);
            Console.WriteLine("\n");
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
    // Classe du jeux contenant les 5 cartes
    class Jeux
    {
        // Attributs
        private Carte[] monJeux;
        // Propriété
        internal Carte[] MonJeux
        {
            get { return monJeux; }
            set { monJeux = value; }
        }
        // Constructeur
        public Jeux()
        {
            monJeux = new Carte[5];
            for (int i = 0; i < 5; i++)
            {
                monJeux[i] = new Carte('7', 0);
            }
        }
        // Methodes
        // Indique si une carte est déjà présente dans le jeu
        // Paramètres : une carte, le jeu 5 cartes, le numéro de la carte dans le jeu
        // Retourne un entier (booléen)
        public bool carteUnique(Carte uneCarte, int numero)
        {
            int i = 0;
            bool unique = true; // Vrai
            // Parcours du Jeu à la recherche de la carte
            do
            {
                if (i != numero) // La carte ne doit pas se comparer à elle même !
                {
                    if ((uneCarte.Valeur == monJeux[i].Valeur) && (uneCarte.Famille == monJeux[i].Famille))
                        unique = false;
                    else
                        i++;
                }
                else
                    i++;
            } while (unique && i < 5);
            return unique;
        }
        //Tirage du jeu
        public void tirageDuJeu()
        {
            Carte tmpc = new Carte();
            for (int i = 0; i < 5; i++)
            {
                do
                {
                    tmpc = monJeux[i].tirage();
                } while (!carteUnique(tmpc, i));
                monJeux[i] = tmpc;
            }
        }
        //Affichage des 5 cartes
        public void AfficherMonJeu()
        {
            for (int i = 0; i < 5; i++)
            {
                monJeux[i].AffichageCarte(i, 0);
            }
        }
        // Fonction principqle du jeux
        public void LancerJeux()
        {
            tirageDuJeu();
            AfficherMonJeu();
        }
    }
}