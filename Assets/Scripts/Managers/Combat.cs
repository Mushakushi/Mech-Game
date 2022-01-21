using UnityEngine;

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { Boss, Player }
public class Combat : MonoBehaviour
{
    /// <summary>
    /// Player in battle 
    /// </summary>
    [SerializeField] private static Player player;

    /// <summary>
    /// Boss in battle
    /// </summary>
    [SerializeField] private static Boss boss; 

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public static Phase phase { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player) Debug.LogError("Could not find GameObject tagged \"Player\" with a Player component!");
        else player.Start(); 

        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        if (!boss) Debug.LogError("Could not find GameObject tagged \"Boss\" with a Boss component!");
        else boss.Start(); 

        ChangePhase(Phase.Player); 
    }

    /// <summary>
    /// Updates current phase of battle, determines which phase belong to which objects
    /// </summary>
    public static void ChangePhase(Phase phase)
    {
        Combat.phase = phase;

        switch (phase)
        {
            case Phase.Boss:
                boss.OnPhaseEnter(); 
                break;
            case Phase.Player:
                player.OnPhaseEnter(); 
                break; 
        }
    }
}
