using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Windows.Forms;

namespace TP3Galaga.Code
{
    //<JCOTE>
    /// <summary>
    /// Tout ce qui attrait au Leaderboard. Le paneau des scores de fin de partie.
    /// </summary>
    public class Leaderboard
    {
        
        public int count { get; private set; }
        //nombre maximum d'entrées dans le leaderboard.
        public const int MAX_LEADERBOARD_ENTRY = 5;
        //liste des entrées.
        private List<LeaderboardData> scoresAndNames = null;
        //tableau des entrées.
        private string[,] totalTab = new string[MAX_LEADERBOARD_ENTRY,2];
        //chamain pour accèder au fichier .txt
        private string path = "";

        /// <summary>
        /// Obtenir le score à la position voulue.
        /// </summary>
        /// <param name="position">position voulue</param>
        /// <param name="score">score du joueur</param>
        /// <param name="name">nom du joueur</param>
        public void GetScore(int position, ref int score, ref string name)
        {
            if (position<MAX_LEADERBOARD_ENTRY)
            {
                score = int.Parse(totalTab[position, 0]);
                name = totalTab[position, 1];
            }
            else
            {
                score = 0;
                name = "??????";
            }
        }
        /// <summary>
        /// insert le pointage du joueur dans le tableau des scores.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="name"></param>
        public void InsertScore(int score, string name)
        {

            for (int i = 0; i < totalTab.GetLength(0); i++)
            {
                if (IsBetterScore(score,i))
                {
                    Logger.GetInstance().Log(DateTime.Now.ToString() + " - " + "Nouvelle entrée au leaderboard: "+score+"-"+name);
                    for (int j = totalTab.GetLength(0)-1; j > i ; j--)
                    {
                        totalTab[j, 0] = totalTab[j - 1, 0];
                        totalTab[j, 1] = totalTab[j - 1, 1];

                    }
                    totalTab[i, 0] = score.ToString();
                    totalTab[i, 1] = name;
                    break;
                }
            }
            for (int i = 0; i < totalTab.GetLength(0); i++)
            {
                scoresAndNames.Add(new LeaderboardData(int.Parse(totalTab[i, 0]), totalTab[i, 1]));
            }

            //sauvegarde le score dans le fichier .txt
            Save(path);
        }
        /// <summary>
        /// vérifie si le score du joueur est meilleur que celui dans le tableau à un index donné.
        /// </summary>
        /// <param name="score">score du joueur</param>
        /// <param name="index">index dans le tableau totalTab</param>
        /// <returns>vrai si le score est meilleur faux si non</returns>
        public bool IsBetterScore(int score,int index)
        {
            if (int.Parse(totalTab[index,0]) < score)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// constructeur de la classe Leaderboard.
        /// </summary>
        public Leaderboard()
        {
            path = "Data\\lb.txt";
            scoresAndNames = new List<LeaderboardData>();
            //vérfie si l'ont peut accèder au fichier .txt
            try
            {
                if (File.Exists(path))
                {
                    //construit le tableau des pointages.
                    string[] tabRow = File.ReadAllLines(path);
                
                    for (int i = 0; i < tabRow.Length; i++)
                    {
                        count++;
                        string[] tempTab = tabRow[i].Split('=');
                        totalTab[i, 0] = tempTab[0];
                        totalTab[i, 1] = tempTab[1];
                    }
                    for (int i = 0; i < totalTab.GetLength(0); i++)
                    {
                        for (int j = 0; j < totalTab.GetLength(1); j++)
                        {
                            totalTab[i,j] = totalTab[i,j].Trim(' ');
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                DialogResult result = MessageBox.Show(exc.Message);
            }
            
        }

        /// <summary>
        /// Sauvegarde les pointage des le fichier .txt
        /// </summary>
        /// <param name="fileName"></param>
        public void Save(string fileName)
        {
            //vérfie si l'ont peut accèder au fichier .txt
            try
            {
                if (File.Exists(path))
                {
                    File.WriteAllText(path,string.Empty);
                    foreach (LeaderboardData data in scoresAndNames)
                    {
                        File.AppendAllText(path, data.Score + " = " + data.name + "\r\n");
                    }
                }
            }
            catch (Exception exc)
            {
                DialogResult result = MessageBox.Show(exc.Message);
            }
            
        }
        /// <summary>
        /// Dessine le leaderboard a l'écran.
        /// </summary>
        /// <param name="window">fenêtre d'affichage</param>
        /// <param name="gameFont">style de charactères</param>
        public void Draw(RenderWindow window, Font gameFont)
        {
            //Affichage texte titre.
            Text LeaderboardTitleText = new Text("", gameFont);
            LeaderboardTitleText.Color = Color.Blue;
            LeaderboardTitleText.CharacterSize = 24;
            LeaderboardTitleText.DisplayedString = "LEADERBOARD";
            LeaderboardTitleText.Position = new Vector2f((Game.GAME_WIDTH/2) - (LeaderboardTitleText.DisplayedString.Length/2*LeaderboardTitleText.CharacterSize), 32);

            //Affichage texte quitter.
            Text LeaderboardQuitText = new Text("", gameFont);
            LeaderboardQuitText.Color = Color.Red;
            LeaderboardQuitText.CharacterSize = 24;
            LeaderboardQuitText.Position = new Vector2f(32, Game.GAME_HEIGHT-32);
            LeaderboardQuitText.DisplayedString = StringTable.GetInstance().GetValue(Game.GetLanguage, "ID_PRESS_ESC") +" "+
                StringTable.GetInstance().GetValue(Game.GetLanguage, "ID_TO_QUIT");
            
            //Affichage des pointages.
            int padding = 0;
            foreach (LeaderboardData VARIABLE in scoresAndNames)
            {
                Text LeaderboardDataText = new Text("", gameFont);
                LeaderboardDataText.Color = Color.Green;
                LeaderboardDataText.CharacterSize = 24;
                LeaderboardDataText.Position = new Vector2f(LeaderboardTitleText.Position.X, LeaderboardTitleText.Position.Y + 90 + padding);
                LeaderboardDataText.DisplayedString = VARIABLE.Score + " : " + VARIABLE.name;
                padding += 30;
                window.Draw(LeaderboardDataText);
            }

            window.Draw(LeaderboardTitleText);
            window.Draw(LeaderboardQuitText);
        }
        //</JCOTE>
    }
}
