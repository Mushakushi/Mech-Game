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
    [SerializeField] private float travelLimit;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        hitbox = GetComponentInChildren<Hitbox>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>
    /// Set the values of this projectile. Should be run immediately after creation
    /// </summary>
    /// <param name="settings">Settings from AttackProjectileAsset scriptable object</param>
    /// <param name="spawnPos">Spawn position of the projectile</param>
    /// <param name="destination">Directional destination of projectile</param>
    /// <param name="speed">Speed of the projectile</param>
    public void SetValues(AttackProjectileAsset settings, Vector2 spawnPos, AttackDestination destination, float speed)
    {
        transform.position = spawnPos;

        travelLimit = GetTravelLimit(spawnPos, destination);

        animator.runtimeAnimatorController = settings.animations;
        this.destination = destination;
        this.speed = speed;

        HitboxProperties hitboxSettings = settings.hitboxProperties;
        hitbox.damage = hitboxSettings.damage;
        hitbox.boxCollider.size = hitboxSettings.size;
        hitbox.boxCollider.offset = hitboxSettings.offset;
        hitbox.boxCollider.enabled = true;
    }

    /// <summary>
    /// Get the location where the projectile should destroy itself
    /// </summary>
    /// <param name="spawnPos">Location this projectile spawned in</param>
    /// <param name="destination">Directional destination of attack</param>
    /// <returns></returns>
    private float GetTravelLimit(Vector2 spawnPos, AttackDestination destination)
    {
        switch (destination)
        {
            case AttackDestination.Down:
                return ProjectileManager.GetPosRelCamera(Vector2.down * 0.1f).y;
            case AttackDestination.Right:
                return ProjectileManager.GetPosRelCamera(Vector2.right * 1.1f).x;
            case AttackDestination.Left:
                return ProjectileManager.GetPosRelCamera(Vector2.left * 0.1f).x; // screen space is 0, 1
            default:
                return 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (destination)
        {
            case AttackDestination.Down:
                transform.position += ((Vector3.down * speed * Time.deltaTime) / 100);
                if (transform.position.y <= travelLimit)
                    DeleteProjectile();
                break;
            case AttackDestination.Right:
                transform.position += ((Vector3.right * speed * Time.deltaTime) / 100);
                if (transform.position.x >= travelLimit)
                    DeleteProjectile();
                break;
            case AttackDestination.Left:
                transform.position += ((Vector3.left * speed * Time.deltaTime) / 100);
                if (transform.position.x <= travelLimit)
                    DeleteProjectile();
                break;
            default:
                DeleteProjectile();
                break;
        }
    }

    public void OnEnterHurtbox()
    {
        hitbox.boxCollider.enabled = false;
        animator.SetTrigger("DestroyProjectile");
    }

    public void OnExitHurtbox()
    {
        
    }

    public void DeleteProjectile()
    {
        Destroy(gameObject);
    }
}
