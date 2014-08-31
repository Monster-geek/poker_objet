using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace poker_objet
{
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
