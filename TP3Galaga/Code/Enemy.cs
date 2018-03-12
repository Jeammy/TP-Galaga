using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TP3Galaga.Code
{
    public class Enemy
    {
        //<SSPEICHERT>
        /// <summary>
        /// Représente tout ce qui est relié à la gestion interne d'un ennemi (son AI, ses collisions etc...).
        /// </summary>

        //Tableau 1D contenant toutes les textures qui peuvent être assignée au sprite dans le constructeur.
        private Texture[] enemyTexture = new Texture[4] { new Texture("Data\\Arts\\PurpleGalaxian.tga"), new Texture("Data\\Arts\\BlueGalaxian2.tga"), new Texture("Data\\Arts\\YellowGalaxian.tga"), new Texture("Data\\Arts\\RedGalaxian.tga") }; 
       
        //Sprite de l'ennemi. Il sera assigné dans le constructeur.
        private Sprite enemySprite = null;

        //Hauteur de la "hitbox" de collision de l'ennemi.
        public const float ENEMY_HEIGHT = 32;

        //Largeur de la "hitbox" de collision de l'ennemi.
        public const float ENEMY_WIDTH = 32;

        //Position en X et Y de l'ennemi.
        private float positionX = 0.0f;
        private float positionY = 0.0f;

        //La position en X et Y du point de départ de l'ennemi (utilisé pour les mouvements latéraux de gauche à droite).
        private float initPosX = 0.0f;
        private float initPosY = 0.0f;

        //Position en X et Y ou l'ennemi devra réapparaître après avoir tenté de foncer sur le héros. 
        private float respawnPosX = 0.0f;
        private float respawnPosY = 0.0f;  
        
        //Propriétés C# qui permettent de contrôller l'assignation et l'obtention des positions en X et Y via l'extérieur de cette classe.
        public float PositionX 
        { 
            get { return positionX; } 
            set 
            {
                if (value >= 0.0f && value < Game.GAME_WIDTH)
                    this.positionX = value;
                else
                    throw new ArgumentOutOfRangeException("X", "Valeur invalide qui doit être positive");
            } 
        }
        public float PositionY 
        { 
            get { return positionY; } 
            set { this.positionY = value; } 
        }
                                          
        //Propriétés c# qui déterminer les limites gauches et droites du déplacement latéral de l'ennemi.
        public float RangeLeftPosX
        {
            get { return initPosX - 32; }
        }

        public float RangeRightPosX
        {
            get { return initPosX + 32; }
        }

        //Fréquence d'attaque assignée à l'ennemi         
        private int enemyAttackFrequency = 0;
        
        //Booléen qui détermine si le joueur peut bouger à gauche.
        private bool canMoveLeft = true;

        //Propriété C# qui permet d'obtenir le booléen moveLeft.
        public bool CanMoveLeft
        {
            get { return canMoveLeft; }
        }

        //Représente l'action que l'ennemi est en train de faire.
        private EnemyState enemyState = EnemyState.Idle;





        /// <summary>
        /// Constructeur de la classe Enemy.
        /// </summary>
        /// <param name="enemyType">L'entier qui représente le type d'ennemi.</param>
        /// <param name="positionX">La position en X de l'ennemi.</param>
        /// <param name="positionY">La position en Y de l'ennemi.</param>
        /// <param name="enemyAttackFrequency">La fréquence de tir de l'ennemi.</param>
        /// <returns>Aucune valeur de retour</returns>
        public Enemy(int enemyType, float positionX, float positionY, int enemyAttackFrequency)
        {

            //On assigne un sprite à l'ennemi selon la valeur de enemyType entré en paramètre.
            if (enemyType == 1)
            {
                enemySprite = new Sprite(enemyTexture[0]); 
            }
            else if (enemyType == 2)
            {
                enemySprite = new Sprite(enemyTexture[1]);
            }
            else if (enemyType == 3)
            {
                enemySprite = new Sprite(enemyTexture[2]);
            } 
            else if (enemyType == 4)
            {
                enemySprite = new Sprite(enemyTexture[3]);
            }
            
            this.positionX = positionX;
            this.positionY = positionY;  
            this.enemyAttackFrequency = enemyAttackFrequency;
            initPosX = positionX;
            initPosY = positionY;
            respawnPosX = positionX;
            respawnPosY = positionY;
            this.enemyAttackFrequency = enemyAttackFrequency;       
        }

        /// <summary>
        /// La fonction Update de l'ennemi a plusieurs rôles, car elle gère les différents états
        /// d'un ennemi, soit son déplacement latéral de gauche à droite, son tir ainsi que sa réapparition 
        /// en haut de l'écran, afin qu'il revienne à sa position initiale.
        /// </summary>
        /// <param name="heroPositionX">La position en X du héros.</param>
        /// <param name="randomShot">Une deuxième fréquence de tir ajoutée, afin qu'il y ait moins d'ennemis par colonnes qui tirent.</param>
        /// <param name="enemyAttackFrequency">La probabilité que l'ennemi tente de foncer sur le héros.</param>
        /// <returns>La fonction retourne vrai si l'ennemi est face au héros (en position X).</returns>
        public bool Update(int heroPositionX, int randomShot, int randomTackle) 
        {
            //On décrémente enemyAttackFrequency à chaque mise à jour. Si enemyAttackFrequency < 0, un tir vers l'ennemi sera fait (voir plus bas).
            enemyAttackFrequency -= 1;

            //Probabilité que l'ennemi tente de foncer sur le héros.
            if (randomTackle == 1)
            {
                enemyState = EnemyState.Attacking;
            }

            //Si l'ennemi est en train de bouger de gauche à droite
            if (enemyState == EnemyState.Idle)
            {
                //Gestion du tir
                if (enemyAttackFrequency < 0 && positionX == heroPositionX && randomShot == 1)
                {
                    return true;
                }
                //Gestion du mouvement                                                             
                if (canMoveLeft == true)
                {
                    MoveLeft();
                }
                else
                {
                    MoveRight();
                }
                //Gestion du mouvement latéral, pour que l'ennemi change de direction.
                if (RangeRightPosX <= positionX)
                {
                    canMoveLeft = true;
                }
                else if (RangeLeftPosX >= positionX)
                {
                    canMoveLeft = false;
                } 
            } 
            // Si l'ennemi essaie de foncer sur le héros.
            if (enemyState == EnemyState.Attacking )    
            {
                PositionY++;

                if (PositionY > Game.GAME_HEIGHT)
                {   
                    enemyState = EnemyState.Spawing;
                    PositionY = 0;
                }
            }
            //Si l'ennemi est en train de revenir à sa position de base.
            if (enemyState == EnemyState.Spawing)
            {
                PositionY++;

                if (PositionY == initPosY)
                {
                    PositionX = respawnPosX;
                    PositionY = respawnPosY;
                    enemyState = EnemyState.Idle;
                }
            }
            return false;
        }

        /// <summary>
        /// La fonction MoveLeft de l'ennemi permet de le faite bouger vers la gauche.
        /// </summary>
        /// <returns>Aucun retour.</returns>
        public void MoveLeft()
        {
            PositionX--;
            respawnPosX--;
        }

        /// <summary>
        /// La fonction MoveRight de l'ennemi permet de le faite bouger vers la droite.
        /// </summary>
        /// <returns>Aucun retour.</returns>
        public void MoveRight()
        {
            PositionX++;
            respawnPosX++;
        }



        /// <summary>
        /// La fonction Draw de l'ennemi permet d'afficher le sprite de l'ennemi à sa position.
        /// </summary>
        /// <param name="window">Le rendu en fenêtre de la fenêtre.</param>
        /// <returns>Aucun retour.</returns>
        public void Draw(RenderWindow window)
        {
            enemySprite.Position = new Vector2f(positionX, positionY);
            window.Draw(enemySprite);
        }
        //</SSPEICHERT>
    }
}
