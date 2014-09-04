using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Net;

namespace poker_objet
{
    public class ClassXML
    {
        private XmlTextWriter Writer;   // Pour écrire le XML.
        private XmlTextReader Reader;   // Pour lire en tant que XML.
        private StreamReader sr;        // Pour lire un fichier texte.
        private XmlDocument cache;      // Zone mémoire en local avec des données en XML.

        public void EcrireSurXML(string Nom, string carte1Famille, string carte1Valeur, string carte2Famille, string carte2Valeur, string carte3Famille, string carte3Valeur, string carte4Famille, string carte4Valeur, string carte5Famille, string carte5Valeur, string resultat)
        {
            List<List<string>> listeDeListe = new List<List<string>>();
            listeDeListe = ExtraireXML();

            List<string> listeNom = new List<string>();
            listeNom.Add(Nom);
            List<string> listecarte1Famille = new List<string>();
            listecarte1Famille.Add(carte1Famille);
            List<string> listecarte1Valeur = new List<string>();
            listecarte1Valeur.Add(carte1Valeur);
            List<string> listecarte2Famille = new List<string>();
            listecarte2Famille.Add(carte2Famille);
            List<string> listecarte2Valeur = new List<string>();
            listecarte2Valeur.Add(carte2Valeur);
            List<string> listecarte3Famille = new List<string>();
            listecarte3Famille.Add(carte3Famille);
            List<string> listecarte3Valeur = new List<string>();
            listecarte3Valeur.Add(carte3Valeur);
            List<string> listecarte4Famille = new List<string>();
            listecarte4Famille.Add(carte4Famille);
            List<string> listecarte4Valeur = new List<string>();
            listecarte4Valeur.Add(carte4Valeur);
            List<string> listecarte5Famille = new List<string>();
            listecarte5Famille.Add(carte5Famille);
            List<string> listecarte5Valeur = new List<string>();
            listecarte5Valeur.Add(carte5Valeur);
            List<string> listeResultat = new List<string>();
            listeResultat.Add(resultat);
            
            listeDeListe.Add(listeNom);
            listeDeListe.Add(listecarte1Famille);
            listeDeListe.Add(listecarte1Valeur);
            listeDeListe.Add(listecarte2Famille);
            listeDeListe.Add(listecarte2Valeur);
            listeDeListe.Add(listecarte3Famille);
            listeDeListe.Add(listecarte3Valeur);
            listeDeListe.Add(listecarte4Famille);
            listeDeListe.Add(listecarte5Valeur);
            listeDeListe.Add(listecarte5Famille);
            listeDeListe.Add(listecarte5Valeur);
            listeDeListe.Add(listeResultat);

            try
            {
                //Suppression du fichier si existant
                if (File.Exists("score.xml"))
                {       
                    File.Delete("score.xml");
                }

                Writer = new XmlTextWriter("score.xml", null); // Utilisation du fichier
                Writer.Formatting = Formatting.Indented;

                Writer.WriteStartDocument(true);
                Writer.WriteStartElement("scores");

                for (int i = 0; i < listeDeListe[0].Count; i++) // Réécriture du fichier XML précédent
                {
                    Writer.WriteStartElement("score");

                    Writer.WriteStartElement("nom");
                    Writer.WriteString(listeDeListe[0][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte1Famille");
                    Writer.WriteString(listeDeListe[1][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte1Valeur");
                    Writer.WriteString(listeDeListe[2][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte2Famille");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte2Valeur");
                    Writer.WriteString(listeDeListe[4][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte3Famille");
                    Writer.WriteString(listeDeListe[5][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte3Valeur");
                    Writer.WriteString(listeDeListe[6][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte4Famille");
                    Writer.WriteString(listeDeListe[7][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte4Valeur");
                    Writer.WriteString(listeDeListe[8][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte5Famille");
                    Writer.WriteString(listeDeListe[9][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte5Valeur");
                    Writer.WriteString(listeDeListe[10][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("resultat");
                    Writer.WriteString(listeDeListe[11][i]);
                    Writer.WriteEndElement();


                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();

                Writer.WriteEndDocument();
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                if (Writer != null)
                    this.Writer.Close();
            }
        }



        // Lire le fichier XML
        public List<List<string>> ExtraireXML()
        {
            List<string> listeNom = new List<string>();
            List<string> listecarte1Famille = new List<string>();
            List<string> listecarte1Valeur = new List<string>();
            List<string> listecarte2Famille = new List<string>();
            List<string> listecarte2Valeur = new List<string>();
            List<string> listecarte3Famille = new List<string>();
            List<string> listecarte3Valeur = new List<string>();
            List<string> listecarte4Famille = new List<string>();
            List<string> listecarte4Valeur = new List<string>();
            List<string> listecarte5Famille = new List<string>();
            List<string> listecarte5Valeur = new List<string>();
            List<string> listeResultat = new List<string>();
            List<List<string>> listeUltime = new List<List<string>>();

            try
            {
                // a. Instancier les attributs de lecture
                sr = new StreamReader("score.xml");
                this.Reader = new XmlTextReader(sr);

                // b. Instancier le "cache" XML
                cache = new XmlDocument();
                cache.Load(this.Reader);

                // c. Récupérer une collection de balise <Logiciel>...
                XmlNodeList lesLogiciels = cache.SelectNodes("scores/score");   // xmlNode = Balise XML

                // d. Parcours de la collection de balises
                foreach (XmlNode balise in lesLogiciels)
                {
                    listeNom.Add(balise.SelectSingleNode("nom").InnerText);
                    listecarte1Famille.Add(balise.SelectSingleNode("carte1Famille").InnerText);
                    listecarte1Valeur.Add(balise.SelectSingleNode("carte1Valeur").InnerText);
                    listecarte2Famille.Add(balise.SelectSingleNode("carte2Famille").InnerText);
                    listecarte2Valeur.Add(balise.SelectSingleNode("carte2Valeur").InnerText);
                    listecarte3Famille.Add(balise.SelectSingleNode("carte3Famille").InnerText);
                    listecarte3Valeur.Add(balise.SelectSingleNode("carte3Valeur").InnerText);
                    listecarte4Famille.Add(balise.SelectSingleNode("carte4Famille").InnerText);
                    listecarte4Valeur.Add(balise.SelectSingleNode("carte4Valeur").InnerText);
                    listecarte5Famille.Add(balise.SelectSingleNode("carte5Famille").InnerText);
                    listecarte5Valeur.Add(balise.SelectSingleNode("carte5Valeur").InnerText);
                    listeResultat.Add(balise.SelectSingleNode("resultat").InnerText);

                }

                listeUltime.Add(listeNom);
                listeUltime.Add(listecarte1Famille);
                listeUltime.Add(listecarte1Valeur);
                listeUltime.Add(listecarte2Famille);
                listeUltime.Add(listecarte2Valeur);
                listeUltime.Add(listecarte3Famille);
                listeUltime.Add(listecarte3Valeur);
                listeUltime.Add(listecarte4Famille);
                listeUltime.Add(listecarte4Valeur);
                listeUltime.Add(listecarte5Famille);
                listeUltime.Add(listecarte5Valeur);
                listeUltime.Add(listeResultat);

            }

            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
            finally
            {
                if (Reader != null)
                    this.Reader.Close();
            }

            return listeUltime;
        }
    }
}
