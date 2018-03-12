using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3Galaga.Code
{
  class Program
  {
    static void Main( string[] args )
    {
      Game app = new Game( Game.GAME_NAME, Game.GAME_WIDTH, Game.GAME_HEIGHT);
      app.Run( );
    }
  }
}
