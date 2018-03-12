using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Policy;
using SFML;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace TP3Galaga.Code
{
  public class Game
  {
    /// <summary>
    /// Représente le concept de partie
    /// </summary>
    ///
    public const string GAME_NAME = "Galaga2016";
    public const int GAME_WIDTH = 512;
    public const int GAME_HEIGHT = 512;
    public const int FRAME_LIMIT = 60;
    private RenderWindow window = null;
    private Color backgroundColor = Color.Black;
    private Font gameFont = new Font("Data\\Arts\\Font\\Symtext.ttf");
    //fréquence de tir des missiles.
    private int AttackFrequency = 0;
    //infos sur le joueur (nom et score)
    private string playerName = "";
    private int score = 0;
    //état de la partie.
    private GameState gameState = GameState.SplashScreen;
    //instance du hero.
    private Hero HeroPlayer = null;
    private Leaderboard leaderboard = null;
    //<SSpeichert>
    private Level level = null;
    private StringTable stringTable = null;
    static private Language lang = Language.English;
    static public Language GetLanguage { get { return lang; } }
    //</SSpeichert>
    private List<Projectile> projectiles = new List<Projectile>();
    //liste de étoiles à afficher.
    private List<Particle> particles = new List<Particle>();
    private List<Projectile> projectilesToRemove = new List<Projectile>(); 
    //<SSpeichert>
    public List<Projectile> Projectiles { get { return projectiles; } set { projectiles = value; } }
    //</SSpeichert>
    
    /// <summary>
    /// Se déclanche quand une touche est appuyée.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnKeyPressed( object sender, KeyEventArgs e )
    {
        if(e.Code == Keyboard.Key.Escape)
        window.Close();
        if (e.Code == Keyboard.Key.Left)
        {
            HeroPlayer.Update(Keyboard.Key.Left);
        }
        if (e.Code == Keyboard.Key.Right)
        {
            HeroPlayer.Update(Keyboard.Key.Right);
        }
        //<JCOTE>
        //tir de missiles avec la touche espace.
        if (e.Code == Keyboard.Key.Space)
        {
            if (AttackFrequency%2==0)
            {
                projectiles.Add(new Projectile(HeroPlayer.XPosition + (HeroPlayer.HeroWidth/2), HeroPlayer.YPosition, CharacterType.Hero));
            }
        }
        //Entrer pour commencer la partie.
        if (e.Code == Keyboard.Key.Return)
        {
            if (playerName != "" && playerName.Length >= 3 && gameState != GameState.Leaderboard)
            {
                Logger.GetInstance().Log(DateTime.Now.ToString()+" - "+"Début d'une nouvelle partie");
                gameState = GameState.Normal;
            }
        }
        //pour revenir en arrière lorsque le joueur entre son nom.
        if (e.Code == Keyboard.Key.BackSpace)
        {
            if (playerName.Length >= 1)
            {
                playerName = playerName.Substring(0,playerName.Length - 1);
            }

        }
        //</JCOTE>
        //<SSPEICHERT>
        //Mettre le jeu en français
        if (e.Code == Keyboard.Key.F4)
        {
            lang = Language.Francais;

        }
        //Mettre le jeu en anglais
        if (e.Code == Keyboard.Key.F5)
        {
            lang = Language.English;

        }
        //Mettre le jeu en japonais
        if (e.Code == Keyboard.Key.F6)
        {
            //lang = Language.にほんご;

        }
        //</SSPEICHERT>
    }
    //<JCOTE>
    /// <summary>
    /// texte entrée par le joueur lors du choix du nom dans le splashScreen.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    void OnTextEntered(object sender, TextEventArgs e)
    {
        if (gameState == GameState.SplashScreen)
        {
            if (playerName.Length < 10)
            {
                //n'accepte que les lettres de l'alphabet.
                //http://stackoverflow.com/questions/8321871/how-to-make-a-textbox-accept-only-alphabetic-characters
                if (System.Text.RegularExpressions.Regex.IsMatch(e.Unicode, "^[a-zA-Z]"))
                {
                    playerName += e.Unicode;
                }  
            }
        }
        
    }
    //</JCOTE>
    /// <summary>
    /// Contructeur de la partie
    /// </summary>
    /// <param name="windowTitle"></param>
    /// <param name="width">largeur de la fenêtre</param>
    /// <param name="height">hauteur de la fenêtre</param>
    public Game( string windowTitle, uint width, uint height )
    {
      window = new RenderWindow( new SFML.Window.VideoMode( width, height ), windowTitle, Styles.Titlebar );
      window.KeyPressed += new EventHandler<KeyEventArgs>( OnKeyPressed );
      //<JCOTE>
      window.TextEntered += new EventHandler<TextEventArgs>( OnTextEntered );
      //</JCOTE>
      window.SetFramerateLimit( FRAME_LIMIT );
      //<SSpeichert>
      level = new Level();
      stringTable = StringTable.GetInstance();
      //</SSpeichert>
      //<JCOTE>
      HeroPlayer = new Hero();
      leaderboard = new Leaderboard();

      //répartition des étoiles.
      Random rnd = new Random();
      for (int i = 0; i < 51; i++)
      {
          int speed = rnd.Next(2, 10);
          int positionX = rnd.Next(0, GAME_WIDTH);
          int positionY = rnd.Next(0, GAME_HEIGHT);
          particles.Add(new Particle(speed,positionX,positionY));
      }
      //</JCOTE>
    }

    /// <summary>
    /// affiche la partie selon l'état du jeu ( splashScreen, normal, leaderboard ).
    /// </summary>
    public void Run( )
    {
        float deltaT = 1.0f/(float) FRAME_LIMIT;
        window.SetActive( );
        while ( window.IsOpen )
        {
            //<JCOTE>
            InitGame();
            
            //Affichage de l'écran de démarrage.
            while (gameState == GameState.SplashScreen)
            {
                window.Clear(backgroundColor);
                window.DispatchEvents();
                DrawSplashScreen();
                window.Display();
            }
            //Affichage de la partie.
            while (gameState == GameState.Normal)
            {
                window.Clear( backgroundColor );
                window.DispatchEvents( );
                Update(deltaT);
                DrawNormal();          
                window.Display( );
            }
            //Affichage du leaderboard.
            while (gameState == GameState.Leaderboard)
            {
                window.Clear(backgroundColor);
                window.DispatchEvents();
                EndGame();
                window.Display();
            }
            //</JCOTE>
        }
    }

    /// <summary>
    /// déssine la partie a l'écran.
    /// </summary>
    private void DrawNormal( )
    {
        //<JCOTE>
        //affiche le joueur.
        HeroPlayer.Draw(window);
        //affiche les projectiles
        foreach (Projectile projectile in projectiles)
        {
           projectile.Draw(window); 
        }
        //affiche les étoiles.
        foreach (Particle item in particles)
        {
           item.Draw(window);
        }

        //Affichage du score.
        Text scoreText = new Text("", gameFont);
        scoreText.CharacterSize = 18;
        scoreText.Position = new Vector2f(2, 1);
        scoreText.DisplayedString = StringTable.GetInstance().GetValue(lang, "ID_SCORE") +" : "+ score;
        scoreText.Color = Color.White;
        window.Draw(scoreText);
        //</JCOTE>

        //<SSpeichert>
        level.Draw(window);
        //</SSpeichert>
    }

    /// <summary>
    /// Met à jour la partie.
    /// </summary>
    /// <param name="deltaT"></param>
    private void Update(float deltaT)
    {
        //<JCOTE>
        AttackFrequency++;
        foreach (Projectile projectile in projectiles.ToList())
        {
            //vérifie si les projectiles sont à l'extérieur de la limite de la fenêtre en même temps de le mettre à jour.
            bool isOut = false;
            isOut = projectile.Update(deltaT);
            if (isOut)
            {
                projectiles.Remove(projectile);
            }
        }
        foreach (Particle particle in particles)
        {
            //vérifie si les l'étoiles sont à l'extérieur de la limite de la fenêtre en même temps de le mettre à jour.
            bool isOut = false;
            isOut = particle.Update(deltaT);
            if (isOut)
            {
                particle.RestartParticle();
            }
        }
        //vérifie si tous les ennemies sont mort.
        if (level.EnemyCount==0)
        {
            leaderboard.InsertScore(score, playerName);
            gameState = GameState.Leaderboard;
        }
        //</JCOTE>
        //<SSpeichert>
        level.Update(HeroPlayer.XPosition);
        
        // Gestions des collisions entre les projectiles et les ennemis
        foreach (Enemy enemy in level.ListOfEnemies)
        {
            //  // Pour chaque grain
            foreach (Projectile projectile in projectiles)
            {
                // Si le grain touche à la pan (utilisez la méthode CheckIntersectionBetweenRectangle)

                RectangleShape r1 = new RectangleShape(new Vector2f(Projectile.PROJECTILE_WIDTH, Projectile.PROJECTILE_HEIGHT));
                r1.Position = new Vector2f(projectile.PositionX, projectile.PositionY);

                RectangleShape r2 = new RectangleShape(new Vector2f(Enemy.ENEMY_WIDTH, Enemy.ENEMY_HEIGHT));
                r2.Position = new Vector2f(enemy.PositionX, enemy.PositionY);

                if (CheckIntersectionBetweenRectangle(r1, r2))
                {
                    projectilesToRemove.Add(projectile);
                    level.ListOfEnemiesToRemove.Add(enemy);
                    //<JCOTE>
                    //level.EnemyCount--;
                    //score incrémenté à chaques fois que l'ont tue un ennemi.
                    score += 500;
                    //</JCOTE>
                }
            }
        }


        foreach (Projectile projectile in projectilesToRemove)
        {
            projectiles.Remove(projectile);
        }
        foreach (Enemy enemy in level.ListOfEnemiesToRemove)
        {
            level.ListOfEnemies.Remove(enemy);
        }


        // Gestions des collisions entre le héros et les ennemis
        foreach (Enemy enemy in level.ListOfEnemies)
        {

            // Si un ennemi touche à un héros

            RectangleShape r1 = new RectangleShape(new Vector2f(HeroPlayer.HeroWidth, HeroPlayer.HeroHeight));
            r1.Position = new Vector2f(HeroPlayer.XPosition, HeroPlayer.YPosition);

            RectangleShape r2 = new RectangleShape(new Vector2f(Enemy.ENEMY_WIDTH, Enemy.ENEMY_HEIGHT));
            r2.Position = new Vector2f(enemy.PositionX, enemy.PositionY);

            if (CheckIntersectionBetweenRectangle(r1, r2))
            {
                //<JCOTE>
                //si le joueur perd une vie ou si la partie se termine.
                if (HeroPlayer.GetnbLifeRemain > 0)
                {
                    HeroPlayer.RetrivePlayerLife();
                    level.ListOfEnemiesToRemove.Add(enemy);
                    //level.EnemyCount--;
                }
                else if(HeroPlayer.GetnbLifeRemain==0)
                {
                    Logger.GetInstance().Log(DateTime.Now.ToString() + " - " + "Fin de la partie");
                    leaderboard.InsertScore(score, playerName);
                    gameState= GameState.Leaderboard;
                }
                //</JCOTE>
            }

        }

        foreach (Projectile projectile in level.ListOfEnemyProjectiles.ToList())
        {

            // Si un projectile touche à un héros

            RectangleShape r1 = new RectangleShape(new Vector2f(HeroPlayer.HeroWidth, HeroPlayer.HeroHeight));
            r1.Position = new Vector2f(HeroPlayer.XPosition, HeroPlayer.YPosition);

            RectangleShape r2 = new RectangleShape(new Vector2f(Projectile.PROJECTILE_WIDTH, Projectile.PROJECTILE_HEIGHT));
            r2.Position = new Vector2f(projectile.PositionX, projectile.PositionY);

            if (CheckIntersectionBetweenRectangle(r1, r2))
            {
                //<JCOTE>
                //si le joueur perd une vie ou si la partie se termine.
                if (HeroPlayer.GetnbLifeRemain > 0)
                {
                    HeroPlayer.RetrivePlayerLife();
                    level.ListOfEnemyProjectiles.Remove(projectile);
                }
                else
                {
                    Logger.GetInstance().Log(DateTime.Now.ToString() + " - " + "Fin de la partie");
                    leaderboard.InsertScore(score, playerName);
                    gameState = GameState.Leaderboard;
                }
                //</JCOTE> 
            }
        }

        foreach (Enemy enemy in level.ListOfEnemiesToRemove)
        {
            level.ListOfEnemies.Remove(enemy);
        }
        //</SSpeichert>

    }
    //<SSpeichert>
    public void EnemyShot()
    {
        //Projectiles.Add(new Projectile(float enemyPositionX, float enemyPositionY + 16, CharacterType.Enemy));
    }
    //</SSpeichert>
    private static bool CheckIntersectionBetweenRectangle( RectangleShape r1, RectangleShape r2 )
    {
      float xInter1 = ( r1.Position.X + r1.Size.X - r2.Position.X );
      float xInter2 = ( ( r2.Position.X + r2.Size.X ) - r1.Position.X );
      bool xCollide = ( xInter1 >= 0 ) && ( xInter2 >= 0 );

      float yInter1 = ( r1.Position.Y + r1.Size.Y - r2.Position.Y );
      float yInter2 = ( ( r2.Position.Y + r2.Size.Y ) - r1.Position.Y );
      bool yCollide = ( yInter1 >= 0 ) && ( yInter2 >= 0 );

      return ( xCollide && yCollide );
    }
    //<JCOTE>
    /// <summary>
    /// Dessine l'écran de démarrage.
    /// </summary>
    private void DrawSplashScreen()
    {
        //text
        Text txtPlayerName = new Text("",gameFont);
        txtPlayerName.CharacterSize = 18;
        txtPlayerName.Position = new Vector2f(32, GAME_HEIGHT-124);
        txtPlayerName.DisplayedString = StringTable.GetInstance().GetValue(lang, "ID_ENTER_YOUR_NAME") + playerName;
        txtPlayerName.Color = Color.White;

        //sprite du logo galaga
        Texture logoTexture = new Texture("Data\\Arts\\Galaga_logo.tga");
        Sprite logoSprite = new Sprite(logoTexture);
        logoSprite.Position = new Vector2f(32,64);

        //affichage
        window.Draw(logoSprite);
        window.Draw(txtPlayerName);
        
    }
    //</JCOTE>
    /// <summary>
    /// Gère la fin de partie.
    /// </summary>
    private void EndGame()
    {
        leaderboard.Draw(window,gameFont);
    }
    /// <summary>
    /// initialise la partie.
    /// </summary>
    private void InitGame()
    {
        playerName = "";
        score = 0;
    }

    private void LoadLevel(int level)
    {
          
    }
    /// <summary>
    /// ajoute un projectile dans la liste de projectiles.
    /// </summary>
    /// <param name="projectile"></param>
    public void AddProjectile(Projectile projectile)
    {
        projectiles.Add(projectile);
    }

  }
}
