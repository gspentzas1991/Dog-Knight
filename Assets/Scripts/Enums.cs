using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public enum CombatState
    {
        Approaching = 0,
        BackingAway = 1,
        Attacking = 2
    }

    public enum AttackVerticalDirection
    {
        Center = 0,
        Upwards = 1,
        Downwards = 2
    }

    public enum AttackHorizontalDirection
    {
        Center = 0,
        Front = 1
    }
}
