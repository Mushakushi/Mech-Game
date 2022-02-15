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
    
    /// <summary>
    /// The Character this Hitbox is attached to.
    /// </summary>
    private IHitboxOwner hitboxOwner;

    public BoxCollider2D boxCollider;

    private void Awake()
    {
        boxCollider = gameObject.GetComponent<BoxCollider2D>();
        if (gameObject.GetComponentInParent<IHitboxOwner>() is IHitboxOwner owner)
            hitboxOwner = owner;
        else
            Debug.LogWarning("Hitbox has no owner. Ignore this if intended.");
    }

    /// <summary>
    /// Notifies entering GameObject other (that has a Character contained within the trigger layermask) of entrance
    /// </summary>
    /// <param name="other">Collider2D the GameObject is attached to</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Character hurtbox = GetCharacter(other.gameObject);
        if (hurtbox != null)
        {
            hurtbox.OnHitboxEnter(damage);
            Debug.LogError($"{transform.parent.name} hit {other.name} for {damage} damage");
            if (hitboxOwner != null)
                hitboxOwner.OnEnterHurtbox();
        }   
        else
            print($"{transform.parent.name} failed to get character script from {other.name}");
    }

    /// <summary>
    /// Notifies exiting GameObject other (that has a Character contained within the trigger layermask) of exit
    /// </summary>
    /// <param name="other">Collider2D the GameObject is attached too</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        Character hurtbox = GetCharacter(other.gameObject);
        if (hurtbox != null)
        {
            hurtbox.OnHitboxExit();
            if (hitboxOwner != null)
                hitboxOwner.OnExitHurtbox();
        }
    }

    /// <summary>
    /// Return IHurtboxOwner implementing script in other GameObject
    /// </summary>
    /// <param name="other">GameObject script may be attached to</param>
    /// <returns>Character script if found, null otherwise</returns>
    private Character GetCharacter(GameObject other)
    {
        if (layerMask != (layerMask | 1 << other.layer)) return null;
        if (other.GetComponent<Character>() is Character o) return o;
        else return null; 
    }
}
