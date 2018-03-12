using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3Galaga.Code
{
    //<JCOTE>
    /// <summary>
    /// une entrée dans le leaderboard.
    /// </summary>
    public class LeaderboardData
    {

        public string name;
        public int Score 
        {
            get 
            { return score; }
            private set 
            { score=value; }
        }

        private int score;
        /// <summary>
        /// Constructeur de la classe LeaderboardData contient le score d'un joueur et son nom.
        /// </summary>
        /// <param name="score"></param>
        /// <param name="name"></param>
        public LeaderboardData(int score, string name)
        {
            Score = score;
            this.name = name;
        }
    }
    //</JCOTE>
}
