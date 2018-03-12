using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Drawing;



using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;



namespace TP3Galaga.Code
{
    //<SSPEICHERT>
    /// <summary>
    /// Représente tout ce qui est relié à la gestion externe d'un ennemi,
    /// ainsi qu'au chargement d'un niveau de jeu.
    /// </summary>
    public class Level
    {

        //La liste d'ennemis.
        private List<Enemy> listOfEnemies = new List<Enemy>();

        //La propriété C# permettant d'accéder à la liste d'ennemis.
        public List<Enemy> ListOfEnemies { get { return listOfEnemies; } }

        //La propriété C# permettant d'obtenir le nombre d'ennemis présents dans le niveau.
        public int EnemyCount { get { return listOfEnemies.Count; }}

        //La liste d'ennemis à supprimer.
        private List<Enemy> listOfEnemiesToRemove = new List<Enemy>();

        //La propriété C# permettant d'accéder à la liste d'ennemis à supprimer.
        public List<Enemy> ListOfEnemiesToRemove { get { return listOfEnemiesToRemove; } }

        //La liste de projectiles ennemis.
        private List<Projectile> listOfEnemyProjectiles = new List<Projectile>();

        //La propriété C# permettant d'accéder à la liste de projectiles.
        public List<Projectile> ListOfEnemyProjectiles { get { return listOfEnemyProjectiles; } }

        //La liste de projectiles ennemis à supprimer.
        private List<Projectile> listOfDeletedProjectiles = new List<Projectile>();

        //Création d'un générateur de nombres aléatoires.
        Random rnd = new Random();


        /// <summary>
        /// Constructeur de la classe Level.
        /// </summary>
        /// <returns>Aucune valeur de retour</returns>
        public Level()
        {
            ReadFile(); 
        }

        /// <summary>
        /// La lecture du fichier niveau se fait là-dedans, c'est-à-dire que les ennemis 
        /// vont être crées selon leurs paramètres écrits dans le fichier texte, puis 
        /// placés dans la liste d'ennemis.
        /// </summary>
        /// <returns>Aucune valeur de retour</returns>
        public void ReadFile()
        {
           //Fichier à lire.
           string file = "Data//Level1(sam).txt";

           //On teste le bout de code ci-dessous, c'est-à-dire celui qui va lire le fichier et le séparer ligne-par-ligne et mot-par-mots.
           try
           {                                           
               if (File.Exists(file))
               {
                   //Cet itérateur sera incrémenté à chaque fois que l'on parcours un mot de chaque ligne.
                   int i = 0;

                   //Le numéro du type d'ennemi qui sera crée.
                   int assignedEnemyType = 0;
                   //La position en X de l'ennemi qui sera crée.
                   int assignedX = 0;
                   //La position en Y de l'ennemi qui sera crée.
                   int assignedY = 0;
                   //La fréquence d'attaque de l'ennemi qui sera crée.
                   int assignedFrequency = 0;

                   //On lit le fichier en le plaçant dans un tableau 1D en lisant toutes les lignes.
                   string[] fileLines = File.ReadAllLines(file);

                   //Pour chaque ligne du fichier.
                   foreach (string line in fileLines)
                   {
                       //On sépare la ligne selon la position d'un espace, qui sépare l'id des mots à traduire.
                       string[] splittedLine = line.Split(' ');

                       //Pour chaque mot du fichier, on assigne la valeur selon sa position dans le fichier.
                       foreach (string word in splittedLine)
                       {
                           if (i == 0)
                           {
                               assignedEnemyType = int.Parse(word);
                           }
                           if (i == 1)
                           {
                               assignedX = int.Parse(word);
                           }
                           if (i == 2)
                           {
                               assignedY = int.Parse(word);
                           }
                           if (i == 3)
                           {
                               assignedFrequency = int.Parse(word);
                           }
                           //On incrémente l'itérateur, car on va changer de mot.
                           i++;
                       }
                       //On crée un ennemi avec les paramètres indiqués dans le fichier texte tout en le plaçcant dans une liste d'ennemis.
                       listOfEnemies.Add(new Enemy(assignedEnemyType, Convert.ToSingle(assignedX), Convert.ToSingle(assignedY), assignedFrequency));

                       //Et on remet l'itérateur à 0, parce que l'on va changer de ligne.
                       i = 0;
                   }
               }
           }
           //Si le bout de code ci-dessus ne fonctionne pas, on affiche un message d'erreur.
           catch(Exception exc)
           {
               DialogResult result = MessageBox.Show(exc.Message);
           }
        }

        /// <summary>
        /// La fonction Update de Level permet de mettre à jour les projectiles ennemis, ainsi que
        /// les ennemis eux-mêmes.
        /// </summary>
        /// <param name="heroPositionX">La position en X du héros.</param>
        /// <returns>Aucune valeur de retour.</returns>
        public void Update(int heroPositionX)
        {

            //On met à jour chaque ennemi de la liste d'ennemis.
            foreach(Enemy enemy in listOfEnemies)
            {
                enemy.Update(heroPositionX, rnd.Next(0, 8), rnd.Next(0, 20000));

                // Si le héros est face à l'ennemi, un projectile est crée (si la fonction Update de l'ennemi retourne vrai).
                if (enemy.Update(heroPositionX, rnd.Next(0, 8), rnd.Next(0, 20000)) == true)
                {
                    ListOfEnemyProjectiles.Add(new Projectile(enemy.PositionX, enemy.PositionY + 16, CharacterType.Enemy));
                }
            }

            //On met à jour chaque projectile de la liste des projectiles ennemis.
            foreach (Projectile projectile in ListOfEnemyProjectiles)
            {
                projectile.Update(0.0f);
            }  
        }

        /// <summary>
        /// La fonction Draw de level permet d'appeler les fonctions Draw des projectiles
        /// et des ennemis de leurs listes respectives.
        /// </summary>
        /// <param name="window">Le rendu en fenêtre de la fenêtre.</param>
        /// <returns>Aucun retour.</returns>
        public void Draw(RenderWindow window)
        {
            //On dessine à l'écran chaque projectile de la liste de projectiles des ennemis.
            foreach (Projectile projectile in ListOfEnemyProjectiles)
            {
                projectile.Draw(window);
            }
            //On dessine à l'écran chaque ennmi de la liste des ennemis.
            foreach (Enemy enemy in listOfEnemies)
            {
                enemy.Draw(window);
            }
            
          
        }

        //</SSPEICHERT>
    }
}
