using System;
using System.Dynamic;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Policy;
using System.Globalization;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using TP3Galaga.Code;

namespace UnitTestProjectGalaga
{
    [TestClass]
    public class UnitTest1
    {
        /// <summary>
        /// vérifie si le constructeur positionne le hero correctement
        /// </summary>
        [TestMethod]
        public void TestMethodHero1()
        {
            Hero hero =new Hero();
            Assert.AreEqual((Game.GAME_WIDTH/2)-(32/2),hero.XPosition);
        }
        /// <summary>
        /// vérifie si le constructeur génère adéquatement le nombre de vies du hero.
        /// </summary>
        [TestMethod]
        public void TestMethodHero2()
        {
            Hero hero =new Hero();
            Assert.AreEqual(3,hero.GetnbLifeRemain);
        }
        /// <summary>
        /// vérifie que le fichier se charge.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard1()
        {
            Leaderboard lb =new Leaderboard();
            Assert.AreEqual(5,lb.count);
        }
        /// <summary>
        /// ajout au debut de valeur présentes.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard2()
        {
            Leaderboard lb = new Leaderboard();
            lb.InsertScore(23000,"T.Riddle");
            string name = "";
            int score = 0;
            lb.GetScore(0,ref score,ref name);
            Assert.AreEqual(score,23000);
            Assert.AreEqual(name,"T.Riddle");
        }

        /// <summary>
        /// ajout à la fin de valeur présentes.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard5()
        {
            Leaderboard lb = new Leaderboard();
            lb.InsertScore(3000, "S.Rayleigh");
            string name = "";
            int score = 0;
            lb.GetScore(4, ref score, ref name);
            Assert.AreEqual(score, 3000);
            Assert.AreEqual(name, "S.Rayleigh");
        }

        /// <summary>
        /// ajout au milieu de valeur présentes.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard6()
        {
            Leaderboard lb = new Leaderboard();
            lb.InsertScore(6666, "Mugiwara");
            string name = "";
            int score = 0;
            lb.GetScore(2, ref score, ref name);
            Assert.AreEqual(score, 6666);
            Assert.AreEqual(name, "Mugiwara");
        }

        /// <summary>
        /// ajout au debut de valeur présentes.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard7()
        {
            Leaderboard lb = new Leaderboard();
            lb.InsertScore(10000, "Gol.DRoger");
            string name = "";
            int score = 0;
            lb.GetScore(1, ref score, ref name);
            Assert.AreEqual(score, 10000);
            Assert.AreEqual(name, "Gol.DRoger");
        }
        /// <summary>
        /// si un pointage fait partie des 5 meilleurs
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard3()
        {
            Leaderboard lb = new Leaderboard();
            for (int i = 0; i < 5; i++)
            {
                Assert.IsTrue(lb.IsBetterScore(100000, i));
            }
        }
        /// <summary>
        /// si un pointage ne fait pas partie des 5 meilleurs.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard4()
        {
            Leaderboard lb = new Leaderboard();
            for (int i = 0; i < 5; i++)
            {
                Assert.IsFalse(lb.IsBetterScore(1,i));
            }
        }
        /// <summary>
        /// mise à jour du missile pour l'ennemi.
        /// </summary>
        [TestMethod]
        public void TestMethodProjectile1()
        {
            Projectile pewpew = new Projectile(42,42,CharacterType.Enemy);
            pewpew.Update(42);
            Assert.IsTrue(pewpew.PositionY==42+pewpew.Speed);
        }
        /// <summary>
        /// mise à jour de missile pour le hero.
        /// </summary>
        [TestMethod]
        public void TestMethodProjectile2()
        {
            Projectile pewpew = new Projectile(42, 42, CharacterType.Hero);
            pewpew.Update(42);
            Assert.IsTrue(pewpew.PositionY == 42 - pewpew.Speed);
        }

        /// <summary>
        /// Obtient un valeur a l'extérieur des 5 valeur max.
        /// </summary>
        [TestMethod]
        public void TestMethodLeaderBoard8()
        {
            Leaderboard lb = new Leaderboard();
            int score = 42;
            string name = "42";
            lb.GetScore(8,ref score,ref name);
            Assert.AreEqual(0,score);
            Assert.AreEqual("??????",name);
        }
    }
}
