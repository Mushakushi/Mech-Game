using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IHitboxOwner
{
    /// <summary>
    /// Event that happens when this Owner's Hitbox enters a Character's Hurtbox
    /// </summary>
    void OnEnterHurtbox();

    /// <summary>
    /// Event that happens when this Owner's Hitbox exits a Character's Hurtbox
    /// </summary>
    void OnExitHurtbox();
}