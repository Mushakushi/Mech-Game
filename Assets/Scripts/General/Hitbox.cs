using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    /// <summary>
    /// Determines which hurtboxes should be damaged
    /// </summary>
    [SerializeField] private LayerMask layerMask = new LayerMask();

    /// <summary>
    /// Damage given to hurtboxes
    /// </summary>
    [SerializeField] private float damage;

    /// <summary>
    /// Notifies entering GameObject other (that has a Character contained within the trigger layermask) of entrance
    /// </summary>
    /// <param name="other">Collider2D the GameObject is attached to</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Character c = GetCharacterInOther(other.gameObject);
        if (c) c.OnHitboxEnter(damage);
        else print($"{transform.parent.name} failed to get character script from {other.name}");
    }

    /// <summary>
    /// Notifies exiting GameObject other (that has a Character contained within the trigger layermask) of exit
    /// </summary>
    /// <param name="other">Collider2D the GameObject is attached too</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        Character c = GetCharacterInOther(other.gameObject);
        if (c) c.OnHitboxExit();
    }

    /// <summary>
    /// Return Character script of GameObject other in layerMask
    /// </summary>
    /// <param name="other">GameObject script may be attached to</param>
    /// <returns>Character script if found, null otherwise</returns>
    private Character GetCharacterInOther(GameObject other)
    {
        if (layerMask != (layerMask | 1 << other.layer)) return null;
        return other.GetComponent<Character>(); 
    }
}