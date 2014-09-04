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
        // Liste des combinaisons possibles
        public enum combinaison { RIEN, PAIRE, DOUBLE_PAIRE, BRELAN, QUINTE, FULL, COULEUR, CARRE, QUINTE_FLUSH };
        // Numéro des cartes a échanger
        private int[] echange = { 0, 0, 0, 0 };
        // Le Jeux
        Jeux MonJeux;
        // Resultat
        public string resultat = "";
        // Initialisation Class XML
        ClassXML connexionXML = new ClassXML();
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
                            tmpc = MonJeux.MonJeux[e[i] - 1].tirage();
                            // UnJeux.MonJeux[e[i] - 1] = tmpc;
                        } while (!MonJeux.carteUnique(tmpc, e[i] - 1));
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

            string res = "";

            // Test de la combinaison
            switch (cherche_combinaison(unJeux))
            {
                case combinaison.RIEN:
                    res = "rien du tout... desole!"; break;
                case combinaison.PAIRE:
                    res = "une simple paire..."; break;
                case combinaison.DOUBLE_PAIRE:
                    res = "une double paire; on peut esperer..."; break;
                case combinaison.BRELAN:
                    res = "un brelan; pas mal..."; break;
                case combinaison.QUINTE:
                    res = "une quinte; bien!"; break;
                case combinaison.FULL:
                    res = "un full; ouahh!"; break;
                case combinaison.COULEUR:
                    res = "une couleur; bravo!"; break;
                case combinaison.CARRE:
                    res = "un carre; champion!"; break;
                case combinaison.QUINTE_FLUSH:
                    res = "une quinte-flush; royal!"; break;
            };

            resultat = res;

            Console.WriteLine(res);

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
            Carte[] unJeu = unJeux.MonJeux;
            bool correct = false;
            string res = "";
            string nom = "";
           // StreamWriter f = null; //Anciennement BinaryWriter
          //  FileStream fs = null;
           // fs = new FileStream(nomFichier, FileMode.Append, FileAccess.Write);
          //  f = new StreamWriter(fs); //Anciennement BinaryWriter
            while(!correct)
            {
                Console.Write("Enregistrer le jeu ? ( O / N ) : ");
                string tmp = Console.ReadLine().ToUpper();
                if (tmp == "O" | tmp == "N")
                { correct = true; res = tmp; }
                else
                    correct = false;
            }
            if (res == "O")
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("Vous pouvez saisir votre nom ( ou pseudo ) : ");
                nom = Console.ReadLine().ToUpper();
                string[] liste = new string[12];

                liste[0] = nom;

                int j = 1;
                for (int i = 0; i < unJeu.Length; i++)
                {
                        liste[j] = unJeu[i].Famille.ToString();

                        liste[j+1] = unJeu[i].Valeur.ToString();
                    j += 2;
                }

                liste[11] = resultat;

                connexionXML.EcrireSurXML(liste[0], liste[1], liste[2], liste[3], liste[4], liste[5], liste[6], liste[7], liste[8], liste[9], liste[10], liste[11]);

            }
            //f.Close();
        }
        // Afficher score
        public void AfficherScores()
        {
            Carte[] c = {null, null, null, null, null};

            for(int ii = 0; ii < 5; ii++)
            {
                c[ii] = new Carte();
            }

            List<List<string>> liste = new List<List<string>>();

            liste = connexionXML.ExtraireXML();

            string nom = liste[0][0];

            int ligne = 0;

            int i = 0;
            int j = 0;
            int x = 1;
            while (j < 5)
            {

                    c[j].Famille = int.Parse(liste[x][0].ToString());
                    c[j].Valeur = char.Parse(liste[x+1][0]);
                j++;
                x += 2;
            }
            int position = 0;
            do{
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(nom + " :");
                for (j = 0; j < 5; j++)
                {
                    c[j].AffichageCarte(j, ligne);
                }

                Console.WriteLine(liste[11][0]);
                ligne += 14;
                position = i + 1;
                // 26
                // 51
                // 76
            } while (position < liste[0].Count - 2);

            Console.ReadKey();
        }
    }
}