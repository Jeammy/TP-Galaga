using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;

namespace TP3Galaga.Code
{
    //<JCOTE>
    /// <summary>
    /// Représente une étoile dans le jeu.
    /// </summary>
    public class Particle
    {
        //position de l'étoile.
        public Single positionX { get; private set; }
        public Single positionY { get; private set; }
        //Dimensions de l'étoile.
        private const float PARTICLE_HEIGHT = 5;
        private const float PARTICLE_WIDTH = 1;
        //vitesse de l'étoile.
        private  Single Speed = 5;

        private Random rnd = new Random();
        
        /// <summary>
        /// Constructeur de la classe particle.
        /// </summary>
        /// <param name="speed">vitesse aleatoire de l'étoile</param>
        /// <param name="rndPositionX">position X aléatoire de départ de l'étoile</param>
        /// <param name="rndPositionY">position Y aléatoire de départ de l'étoile</param>
        public Particle(int speed, int rndPositionX, int rndPositionY)
        {
            Speed = speed;
            positionX = rndPositionX;
            positionY = rndPositionY;
        }

        /// <summary>
        /// Dessine l'étoile dans la partie.
        /// </summary>
        /// <param name="window"></param>
        public void Draw(RenderWindow window)
        {
            RectangleShape projectileShape = new RectangleShape(new Vector2f(PARTICLE_WIDTH, PARTICLE_HEIGHT));
            projectileShape.Position = (new Vector2f(positionX, positionY));
            projectileShape.FillColor = Color.White;
            window.Draw(projectileShape);
        }

        /// <summary>
        /// Met à jour la position de l'étoile dans la partie et détermine si elle est a l'extérieur de la zone d'affichage.
        /// </summary>
        /// <param name="deltaT"></param>
        /// <returns>vrai si l'étoile est hors champ, faux si elle peut continuer d'avancer.</returns>
        public bool Update(Single deltaT)
        {
            if (positionY < Game.GAME_HEIGHT && positionY > 0)
            {
                positionY += Speed;
                return false;
            }
            return true;
        }
        /// <summary>
        /// retourne l'étoile à l'intérieur de la partie.
        /// </summary>
        public void RestartParticle()
        {
            positionY = 1;
        }
    }
    //</JCOTE>
}
