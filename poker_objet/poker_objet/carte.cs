using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_objet
{

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
                case 4:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 5:
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
}
