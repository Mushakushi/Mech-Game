using UnityEngine;

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { Boss, Player }
public class Combat : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Boss boss;

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public static Phase phase { get; private set; }

    /// <summary>
    /// Updates current phase of battle 
    /// </summary>
    public static void ChangePhase(Phase phase)
    {
        Combat.phase = phase; 
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
    }

    

    public void DisablePlayerAttack()
    {
        player.canAttack = false;
    }

    public void EnablePlayerAttack()
    {
        player.canAttack = true;
    }
}
