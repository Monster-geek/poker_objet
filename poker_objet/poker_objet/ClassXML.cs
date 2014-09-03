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

                if (File.Exists("Logiciels.xml")) // Suppression du fichier si existant
                    File.Delete("Logiciels.xml");

                Writer = new XmlTextWriter("Logiciels.xml", null); // Utilisation du fichier
                Writer.Formatting = Formatting.Indented;

                Writer.WriteStartDocument(true);
                Writer.WriteStartElement("Logiciels");

                for (int i = 0; i < listeDeListe[0].Count; i++) // Réécriture du fichier XML précédent
                {
                    Writer.WriteStartElement("Logiciel");

                    Writer.WriteStartElement("nom");
                    Writer.WriteString(listeDeListe[0][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("URL_DL");
                    Writer.WriteString(listeDeListe[1][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("URL_Version");
                    Writer.WriteString(listeDeListe[2][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("version");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("nbLigne");
                    Writer.WriteString(listeDeListe[4][i]);
                    Writer.WriteEndElement();

                    Writer.WriteEndElement();
                }

                Writer.WriteStartElement("Logiciel");

                Writer.WriteStartElement("nom");
                Writer.WriteString(NomLogiciel);
                Writer.WriteEndElement();

                Writer.WriteStartElement("URL_DL");
                Writer.WriteString(URL_DL);
                Writer.WriteEndElement();

                Writer.WriteStartElement("URL_Version");
                Writer.WriteString(URL_version);
                Writer.WriteEndElement();

                Writer.WriteStartElement("version");
                Writer.WriteString(version);
                Writer.WriteEndElement();

                Writer.WriteStartElement("nbLigne");
                Writer.WriteString(nbLigne);
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
                if (File.Exists("Logiciels.xml")) // Suppression du fichier si existant
                    File.Delete("Logiciels.xml");

                Writer = new XmlTextWriter("Logiciels.xml", null); // Utilisation du fichier
                Writer.Formatting = Formatting.Indented;

                Writer.WriteStartDocument(true);
                Writer.WriteStartElement("Logiciels");

                for (int i = 0; i < listeDeListe[0].Count; i++) // Réécriture du fichier XML précédent
                {
                    Writer.WriteStartElement("Logiciel");

                    Writer.WriteStartElement("nom");
                    Writer.WriteString(listeDeListe[0][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("URL_DL");
                    Writer.WriteString(listeDeListe[1][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("URL_Version");
                    Writer.WriteString(listeDeListe[2][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("version");
                    Writer.WriteString(listeDeListe[3][i]);
                    Writer.WriteEndElement();

                    Writer.WriteStartElement("nbLigne");
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
            List<string> listeURLDL = new List<string>();
            List<string> listeURLversion = new List<string>();
            List<string> listeVersion = new List<string>();
            List<string> listeNbLigne = new List<string>();
            List<List<string>> listeUltime = new List<List<string>>();

            try
            {
                // a. Instancier les attributs de lecture
                sr = new StreamReader("Logiciels.xml");
                this.Reader = new XmlTextReader(sr);

                // b. Instancier le "cache" XML
                cache = new XmlDocument();
                cache.Load(this.Reader);

                // c. Récupérer une collection de balise <Logiciel>...
                XmlNodeList lesLogiciels = cache.SelectNodes("Logiciels/Logiciel");   // xmlNode = Balise XML

                // d. Parcours de la collection de balises
                foreach (XmlNode balise in lesLogiciels)
                {
                    listeNom.Add(balise.SelectSingleNode("nom").InnerText);
                    listeURLDL.Add(balise.SelectSingleNode("URL_DL").InnerText);
                    listeURLversion.Add(balise.SelectSingleNode("URL_Version").InnerText);
                    listeVersion.Add(balise.SelectSingleNode("version").InnerText);
                    listeNbLigne.Add(balise.SelectSingleNode("nbLigne").InnerText);
                }

                listeUltime.Add(listeNom);
                listeUltime.Add(listeURLDL);
                listeUltime.Add(listeURLversion);
                listeUltime.Add(listeVersion);
                listeUltime.Add(listeNbLigne);
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
