using UnityEngine;

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { Intro, Boss, Player, Dialogue_Pre, Dialogue_Post }
public class Combat : MonoBehaviour
{
    /// <summary>
    /// Boss in battle
    /// </summary>
    [SerializeField] public static Boss boss;

    /// <summary>
    /// Player in battle 
    /// </summary>
    [SerializeField] public static Player player;

    /// <summary>
    /// THE ref
    /// </summary>
    [SerializeField] public static Jario jario; 

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public static Phase phase { get; private set; }

    /// <summary>
    /// Gets references to player and boss in battle, then initializes the phase
    /// </summary>
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>();
        if (!boss) Debug.LogError("Could not find GameObject tagged \"Boss\" with a Boss component!");
        else boss.InitializeCharacter(); 

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (!player) Debug.LogError("Could not find GameObject tagged \"Player\" with a Player component!");
        else player.InitializeCharacter();

        jario = GameObject.FindGameObjectWithTag("Jario").GetComponent<Jario>();
        if (!jario) Debug.LogError("You cannot escape Jario. Please add him to the scene!");

        // reason why I removed the default start() is so that OnPhaseEnter() could be called after initialization ended
        InitializePhase(Phase.Intro); 
    }

    /// <summary>
    /// Updates current phase of battle, determines which phase belong to which objects
    /// </summary>
    private static void InitializePhase(Phase phase)
    {
        Debug.Log($"Phase switched to {phase}"); 
        Combat.phase = phase;

        IPhaseController controller; 
        switch (phase)
        {
            case Phase.Intro:
                controller = jario; 
                break;
            case Phase.Boss:
                controller = boss;  
                break;
            case Phase.Player:
                controller = player; 
                break;
            default:
                controller = null;
                break; 
        }
        if (controller != null) controller.OnPhaseEnter(); 
    }

    /// <summary>
    /// Determines which phase follows the current phase 
    /// </summary>
    public static void PhaseExit()
    {
        // definitiely want some checks here for having dialogue or no, I'm just assuming there's always
        // post dialogue for now
        switch (phase)
        {
            case Phase.Intro:
                InitializePhase(Phase.Player);
                break;
            case Phase.Player:
            case Phase.Boss:
                InitializePhase(Phase.Dialogue_Post);
                break;
            case Phase.Dialogue_Post:
                InitializePhase(Phase.Player);
                break;
        }
    }
}
