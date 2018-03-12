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
    public class Projectile
    {
        public Single PositionX { get; private set; }
        public Single PositionY { get; private set; }

        public const float PROJECTILE_HEIGHT = 15;
        public const float PROJECTILE_WIDTH = 2;

        private bool canShoot = true;
        private int AttackFrequency = 0;

        public Single Speed
        {
            get
            {
                if (Type == CharacterType.Hero)
                {
                    speed = 5;
                }
                else if (Type == CharacterType.Enemy)
                {
                    speed = 3;
                }
                return speed;
            } 
            private set { speed = value; } 
        }

        public CharacterType Type;

        private Vector2f position = new Vector2f();

        private Single speed = 1;

        public void Draw(RenderWindow window)
        {
            //if (AttackFrequency % 10 == 0)
            //{
                RectangleShape projectileShape = new RectangleShape(new Vector2f(PROJECTILE_WIDTH,PROJECTILE_HEIGHT));
                projectileShape.Position = (new Vector2f(PositionX, PositionY));
                projectileShape.FillColor = GetColor(Type);
                window.Draw(projectileShape);
            //}
            AttackFrequency += 3;

        }

        public Color GetColor(CharacterType projType)
        {
            if (projType == CharacterType.Enemy)
            {
                return Color.Red;
            }
            if (projType == CharacterType.Hero)
            {
                return Color.Green;
            }
            return Color.Magenta;
        }

        public Projectile(Single positionX, Single positionY, CharacterType type)
        {
            Type = type;
            this.PositionX = positionX;
            this.PositionY = positionY;
            Speed = speed;
        }

        public bool Update(Single deltaT)
        {
            if (PositionY < Game.GAME_HEIGHT && PositionY > 0)
            {
                if (Type == CharacterType.Hero)
                {
                    PositionY -= Speed;
                    return false;
                }
                if (Type == CharacterType.Enemy)
                {
                    PositionY += Speed;
                    return false;
                }
            }
            return true;
        }
    }
    //</JCOTE>
}
