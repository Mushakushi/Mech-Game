using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

interface IHurtable
{
    /// <summary>
    /// Event that happens when this Owner's Hitbox enters a Character's Hurtbox
    /// </summary>
    /// <param name="damage">Damage to be taken</param>
    void OnHurtboxEnter(float damage);

    /// <summary>
    /// Event that happens when this Owner's Hitbox exits a Character's Hurtbox
    /// </summary>
    void OnHurtboxExit();
}