using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace TP3Galaga.Code
{
    public class StringTable
    {
        //<SSPEICHERT>
        /// <summary>
        /// Représente tout ce qui est relié à la gestion d'un ennemi.
        /// </summary>

        //La langue par défaut
        public const Language DEFAULT_LANGUAGE = Language.English;

        //Fichier à lire
        private string fileToRead = "Data//st.txt";

        //Propriété C# permettant d'accéder au fichier
        public string FileToRead { get { return fileToRead; } set { FileToRead = value; } }

        //La déclaration de l'instance de StringTable en tant que singleton.
        private static StringTable instance = null;

        //Dictionnaire contenant tous les mots pouvant être traduits.
        Dictionary<string, string> languagesAndWords = new Dictionary<string, string>();


        /// <summary>
        /// Constructeur de la classe Enemy.
        /// </summary>
        /// <returns>Aucune valeur de retour</returns>
        private StringTable()
        {
            ReadFile();
        }


        /// <summary>
        /// Fonction qui va lire le fichier des mots un par un, ainsi que son 
        /// identifiant, pour le placer par la suite dans le dictionnaire languagesAndWords.
        /// </summary>
        /// <returns>Aucune valeur de retour</returns>
        public void ReadFile()
        {
            
            

            //On déclare l'identifiant, sans lui assigner de texte.
            string id = "";

            //On déclare le mot à traduire, sans lui assigner de texte.
            string wordToTranslate = "";
            
            //On teste le bout de code ci-dessous, c'est-à-dire celui qui va lire le fichier et le séparer ligne-par-ligne et mot-par-mots.
            try
            {
                if (File.Exists(fileToRead))
                {
                    //Cet itérateur sera incrémenté à chaque fois que l'on parcours un mot de chaque ligne.
                    int i = 0;
                    
                    //On lit le fichier en le plaçant dans un tableau 1D en lisant toutes les lignes.
                    string[] fileLines = File.ReadAllLines(fileToRead);

                    //Pour chaque ligne du fichier.
                    foreach (string line in fileLines)
                    {
                        //On sépare la ligne selon la position du "==>", qui sépare l'id des mots à traduire.
                        //Lieu ou j'ai trouvé comment séparer des chaînes de caractères sur plusieurs caractères au lieu d'un
                        //https://msdn.microsoft.com/en-us/library/ms131448(v=vs.110).aspx
                        string[] splittedLine = line.Split(new[] { "==>" }, StringSplitOptions.None);
                        
                        //Pour chaque mot du fichier, on séparer la ligne.
                        foreach (string word in splittedLine)
                        {
                            
                            if (i == 0)
                            {
                                id = word;
                            }
                            if (i == 1)
                            {
                                wordToTranslate = word;
                            }
                            //On incrémente l'itérateur, car on va changer de mot.
                            i++;
                        }
                        //On ajoute le mot dans le dictionnaire.
                        languagesAndWords.Add(id, wordToTranslate);

                        //Et on remet l'itérateur à 0, parce que l'on va changer de ligne.
                        i = 0;
                    }
                }
            }
            //Si le bout de code ci-dessus ne fonctionne pas, on affiche un message d'erreur.
            catch (Exception exc)
            {
                DialogResult result = MessageBox.Show(exc.Message);
            }
        }


        /// <summary>
        /// Cette fonction sert à obtenir un mot selon la langue sélectionnée par l'utilisateur et
        /// l'id du mot.
        /// </summary>
        /// <param name="lang">La langue sélectionnée par l'utilisateur.</param>
        /// <param name="id">L'id du mot que l'on veux retourner.</param>
        /// <returns>On retourne le mot selon la langue sélectionnée, et sinon, une "string" indiquant que la langue n'a pas pu être trouvée.</returns>
        public string GetValue(Language lang, string id)
        {
            //C'est le mot que l'on veux accéder selon son iD dans le dictionnaire languagesAndWords.
            string accessedWord = languagesAndWords[id];

            //On places dans un tableau 1D les traductions possibles d'un mot, en séparant chaque mot selon la position d'un "---".
            string[] splittedLine = accessedWord.Split(new[] { "---" }, StringSplitOptions.None);

            //Si la langue est égale à français.
            if (lang == Language.Francais)
            {
                return splittedLine[0];
            }
            //Si la langue est égale à anglais.
            else if (lang == Language.English)
            {
                return splittedLine[1];
            }
            //Sinon, on retourne cet chaîne de caractères.
            return "Language can't be found.";
        }

        /// <summary>
        /// Cet fonction sert à obtenir l'instance de StringTable, pour son utilisation
        /// en tant que Singleton.
        /// </summary>
        /// <returns>On retourne l'instance de la classe, lors de son utilisation.</returns>
        public static StringTable GetInstance()
        {
            if( instance == null)
                instance = new StringTable();
            return StringTable.instance;
        }
        //</SSPEICHERT>
    }
}
