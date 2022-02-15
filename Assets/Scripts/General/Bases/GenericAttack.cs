using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(SpriteRenderer))]
public class GenericAttack : MonoBehaviour, IHitboxOwner
{
    [SerializeField] private AttackDestination destination;
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private Animator animator;
    [SerializeField] private Hitbox hitbox;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponentInChildren<Hitbox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetValues(AttackProjectileAsset settings, AttackDestination destination, float speed)
    {
        animator.runtimeAnimatorController = settings.animations;
        this.destination = destination;
        this.speed = speed;

        HitboxProperties hitboxSettings = settings.hitboxProperties;
        hitbox.damage = hitboxSettings.damage;
        hitbox.boxCollider.size = hitboxSettings.size;
        hitbox.boxCollider.offset = hitboxSettings.offset;
        hitbox.boxCollider.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        switch (destination)
        {
            case AttackDestination.Down:
                transform.position += ((Vector3.down * speed) / 100);
                break;
            case AttackDestination.Right:
                transform.position += ((Vector3.right * speed) / 100);
                break;
            case AttackDestination.Left:
                transform.position += ((Vector3.left * speed) / 100);
                break;
            default:
                break;
        }
    }

    public void OnEnterHurtbox()
    {
        animator.SetTrigger("DestroyProjectile");
    }

    public void OnExitHurtbox()
    {
        
    }

    public void DeleteProjectile()
    {
        Destroy(this);
    }
}
