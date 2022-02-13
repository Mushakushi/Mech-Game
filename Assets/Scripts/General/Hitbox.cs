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
    [SerializeField] public float damage;
    
    // this is damaging the player when they hit the boss
    /// <summary>
    /// The Character this Hitbox is attached to.
    /// </summary>
    //private IHurtable self; 

    private void Start()
    {
        /*if (gameObject.GetComponentInParent<IHurtable>() is IHurtable owner)
            self = owner;
        else
            Debug.LogWarning("Hitbox has no owner. Ignore this if intended.");*/
    }

    /// <summary>
    /// Notifies entering GameObject other (that has a Character contained within the trigger layermask) of entrance
    /// </summary>
    /// <param name="other">Collider2D the GameObject is attached to</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        IHurtable hurtbox = GetIHurtboxOwner(other.gameObject);
        if (hurtbox != null)
        {
            hurtbox.OnHurtboxEnter(damage);
            Debug.LogError($"{transform.parent.name} hit {other.name} for {damage} damage"); 
        }   
        else
            print($"{transform.parent.name} failed to get character script from {other.name}");
        /*if (self != null)
            self.OnHurtboxEnter(damage);*/
    }

    /// <summary>
    /// Notifies exiting GameObject other (that has a Character contained within the trigger layermask) of exit
    /// </summary>
    /// <param name="other">Collider2D the GameObject is attached too</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        IHurtable hurtbox = GetIHurtboxOwner(other.gameObject);
        if (hurtbox != null)
            hurtbox.OnHurtboxExit();
        /*if (self != null)
            self.OnHurtboxExit();*/
    }

    /// <summary>
    /// Return IHurtboxOwner implementing script in other GameObject
    /// </summary>
    /// <param name="other">GameObject script may be attached to</param>
    /// <returns>Character script if found, null otherwise</returns>
    private IHurtable GetIHurtboxOwner(GameObject other)
    {
        if (layerMask != (layerMask | 1 << other.layer)) return null;
        if (other.GetComponent<IHurtable>() is IHurtable o) return o;
        else return null; 
    }
}
