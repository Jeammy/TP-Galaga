using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Xml;

namespace TP3Galaga.Code
{
    //<JCOTE>
    /// <summary>
    /// Enregistre dans un fichier .txt les information important de la partie.
    /// </summary>
    public class Logger
    {
        //instance du singleton
        private static Logger instance = null;
        //pour écrire dans le fichier texte.
        private TextWriter tw = null;

        /// <summary>
        /// constructeur de la classe Logger
        /// </summary>
        private Logger()
        {
            
        }
        /// <summary>
        /// Ferme le fichier
        /// </summary>
        public void Close()
        {
            tw.Close();
        }
        /// <summary>
        /// pour obtenir l'instance ou en créer une si il n'y en a pas..
        /// </summary>
        /// <returns></returns>
        public static Logger GetInstance()
        {
            if (instance==null)
            {
                instance=new Logger();
            }
            return instance;
        }
        /// <summary>
        /// Enregistre la chaine de texte dans le fichier log.txt
        /// </summary>
        /// <param name="text">chaine de texte a enregistrer</param>
        public void Log(string text)
        {
            try
            {
                //si le fichier n'existe pas déja en créé un nouveau
                if (!Open("Data\\log.txt"))
                {
                    File.Create("Data\\log.txt");
                    
                }
                tw = new StreamWriter("Data\\log.txt");
                tw.WriteLine(text);

            }
            catch (Exception exc)
            {
                DialogResult result = MessageBox.Show(exc.Message);  
                throw;
            }
            Close();
            
        }
        /// <summary>
        /// vérifie si le fichier existe déja.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool Open(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    return true;
                }
                
            }
            catch (Exception exc)
            {
                DialogResult result = MessageBox.Show(exc.Message);
                return false;
                throw;
            }
            return false;
        }
    }
    //</JCOTE>
}
