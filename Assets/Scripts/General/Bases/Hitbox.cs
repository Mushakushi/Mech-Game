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
    /// Wrapper class for CheckTriggerEnter
    /// </summary>
    /// <param name="other">GameObject the Collider2D is attached to</param>
    private void OnTriggerEnter2D(Collider2D other) => CheckTriggerEnter(other.gameObject);

    /// <summary>
    /// Checks if entering GameObject other has a Character contained within the trigger layermask and damages it
    /// </summary>
    /// <param name="other">Collider2D collision</param>
    private void CheckTriggerEnter(GameObject other)
    {
        Character c = GetCharacterInOther(other);
        if (c) c.OnGetHit(damage);
        else print($"failed to get character script from {other.name}");
    }

    /// <summary>
    /// Wrapper class for CheckTrigerExit
    /// </summary>
    /// <param name="other">GameObject the Collider2D is attached to</param>
    private void OnTriggerExit2D(Collider2D other) => CheckTriggerExit(other.gameObject);

    /// <summary>
    /// Checks if exiting GameObject other has a Character contained within the trigger layermask and disables damaging
    /// </summary>
    /// <param name="other"></param>
    private void CheckTriggerExit(GameObject other)
    {
        Character c = GetCharacterInOther(other); 
        if (c)
        {
            // TODO: Possibly add onGetHitExit?
            c.isHit = false; 
        }
    }

    /// <summary>
    /// Return Character script of GameObject other in layerMask
    /// </summary>
    /// <param name="other">GameObject script may be attached to</param>
    /// <returns>Character script if found, null otherwise</returns>
    private Character GetCharacterInOther(GameObject other)
    {
        if (layerMask != (layerMask | 1 << other.layer)) return null;

        foreach (MonoBehaviour script in other.GetComponents<MonoBehaviour>())
        {
            if (script is Character c) return c; 
        }
        return null; 
    }
}
