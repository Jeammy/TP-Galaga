using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TP3Galaga.Code
{
    //<JCOTE>
    /// <summary>
    /// Représent le joueur dans la partie, le vaisseau.
    /// </summary>
    public class Hero
    {
        //Positionnement du hero.
        Vector2i position = new Vector2i();
        public int XPosition { get { return position.X; } }
        public int YPosition { get { return position.Y; } }
        //Dimension du hero.
        public int HeroHeight{ get { return 32; }}
        public int HeroWidth{get { return 32; }}
        //sprite du hero.
        private Texture heroTexture = null;
        private Texture invisibleTexture = null;
        private Sprite heroSprite = null;
        //vies du hero.
        private int nbLifeRemain = 3;
        public int GetnbLifeRemain { get { return nbLifeRemain; } }
        List<Sprite> heroLifes = new List<Sprite>(); 

        /// <summary>
        /// Constructeur de la classe hero.
        /// </summary>
        public Hero()
        {
            //position de départ
            position.X = (Game.GAME_WIDTH - HeroWidth)/2;
            position.Y = Game.GAME_HEIGHT - HeroHeight*2;
            //sprite du vaisseau.
            heroTexture = new Texture("Data\\Arts\\Spaceship2.tga");
            heroSprite = new Sprite(heroTexture);
            //sprite des vies du joueur.
            for (int i = 0; i < nbLifeRemain; i++)
            {
                Sprite lifeSprite = new Sprite(heroTexture);
                lifeSprite.Position = new Vector2f(32*i,Game.GAME_HEIGHT-32);
                heroLifes.Add(lifeSprite);
            }
        }

        /// <summary>
        /// Met à jour la position du hero.
        /// </summary>
        /// <param name="key"></param>
        public void Update(Keyboard.Key key)
        {
            if (key == Keyboard.Key.Left)
            {
                position.X = Math.Max(0, position.X - 20);
            }
            if (key == Keyboard.Key.Right)
            {
                position.X = Math.Min(Game.GAME_WIDTH - HeroWidth, position.X + 20);
            }
        }

        /// <summary>
        /// Dessine le hero et ses vies.
        /// </summary>
        /// <param name="window"></param>
        public void Draw(RenderWindow window)
        {
            heroSprite.Position = new Vector2f(XPosition,YPosition);
            window.Draw(heroSprite);
            foreach (var life in heroLifes)
            {
                window.Draw(life);
            }
        }
        /// <summary>
        /// Enlève une vie au joueur.
        /// </summary>
        public void RetrivePlayerLife()
        {
            heroLifes.Remove(heroLifes[heroLifes.Count - 1]);
            nbLifeRemain -= 1;
        }
    }
    //</JCOTE>
}
