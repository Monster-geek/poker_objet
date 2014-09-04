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

        public void EcrireSurXML(string NomLogiciel, string URL_DL, string URL_version, string version, string nbLigne)
        {
            List<List<string>> listeDeListe = ExtraireXML();

            try
            {

                if (File.Exists("score.xml")) // Suppression du fichier si existant
                    File.Delete("score.xml");

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

                    Writer.WriteStartElement("carte1");
                    Writer.WriteString(listeDeListe[1][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte2");
                    Writer.WriteString(listeDeListe[2][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte3");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte4");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte5");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("Resultat");
                    Writer.WriteString(listeDeListe[4][i]);
                    Writer.WriteEndElement();

                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("score");

                Writer.WriteStartElement("nom");
                Writer.WriteString(listeDeListe[0][i]);
                Writer.WriteEndElement();

                Writer.WriteStartElement("carte1");
                Writer.WriteString(listeDeListe[1][i]);
                Writer.WriteEndElement();

                Writer.WriteStartElement("carte2");
                Writer.WriteString(listeDeListe[2][i]);
                Writer.WriteEndElement();

                Writer.WriteStartElement("carte3");
                Writer.WriteString(listeDeListe[3][i]);
                Writer.WriteEndElement();

                Writer.WriteStartElement("carte4");
                Writer.WriteString(listeDeListe[3][i]);
                Writer.WriteEndElement();

                Writer.WriteStartElement("carte5");
                Writer.WriteString(listeDeListe[3][i]);
                Writer.WriteEndElement();

                Writer.WriteStartElement("Resultat");
                Writer.WriteString(listeDeListe[4][i]);
                Writer.WriteEndElement();

                Writer.WriteEndElement();

                Writer.WriteEndElement();

                Writer.WriteEndDocument();
                MessageBox.Show("Saisi enregistrée.", "Saisi enregistrée", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); }
            finally
            {
                if (Writer != null)
                    this.Writer.Close();
            }
        } // Ecrire dans le XMl + 1 surcharge

        public void EcrireSurXML(List<List<string>> listeDeListe)
        {
            try
            {
                if (File.Exists("score.xml")) // Suppression du fichier si existant
                    File.Delete("score.xml");

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

                    Writer.WriteStartElement("carte1");
                    Writer.WriteString(listeDeListe[1][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte2");
                    Writer.WriteString(listeDeListe[2][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte3");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte4");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("carte5");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("Resultat");
                    Writer.WriteString(listeDeListe[4][i]);
                    Writer.WriteEndElement();

                    Writer.WriteEndElement();
                }
                Writer.WriteEndElement();

                Writer.WriteEndDocument();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); }
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
            List<string> listecarte1 = new List<string>();
            List<string> listecarte2 = new List<string>();
            List<string> listecarte3 = new List<string>();
            List<string> listecarte4 = new List<string>();
            List<string> listecarte5 = new List<string>();
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
                    listecarte1.Add(balise.SelectSingleNode("URL_DL").InnerText);
                    listecarte2.Add(balise.SelectSingleNode("URL_DL").InnerText);
                    listecarte3.Add(balise.SelectSingleNode("URL_DL").InnerText);
                    listecarte4.Add(balise.SelectSingleNode("URL_DL").InnerText);
                    listecarte5.Add(balise.SelectSingleNode("URL_DL").InnerText);
                    listeResultat.Add(balise.SelectSingleNode("nbLigne").InnerText);
                }

                listeUltime.Add(listeNom);
                listeUltime.Add(listecarte1);
                listeUltime.Add(listecarte2);
                listeUltime.Add(listecarte3);
                listeUltime.Add(listecarte4);
                listeUltime.Add(listecarte5);
                listeUltime.Add(listeResultat);

            }

            catch (Exception ex)
            { MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning); }
            finally
            {
                if (Reader != null)
                    this.Reader.Close();
            }

            return listeUltime;
        }
    }
}
