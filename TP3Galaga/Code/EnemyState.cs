using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP3Galaga.Code
{
    //<SSpeichert>
    /// <summary>
    /// Enum qui comprend les actions possibles de l'ennemi :
    /// Attacking : Son état quand il essaie de foncer sur l'ennemi.
    /// Idle : Quand l'ennemi fait son mouvement latéral de gauche à droite.
    /// Spawning : lorsque l'ennemi est en train de réapparaitre en haut de l'écran.
    /// </summary>
    public enum EnemyState { Attacking, Idle, Spawing }
    //</SSpeichert>
}
