using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

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
    /// Every phase controller in the scene
    /// </summary>
    [SerializeField] private static List<IPhaseController> controllers; 

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public static Phase phase { get; private set; }

    /// <summary>
    /// Updates references in scene
    /// </summary>
    void Start()
    {
        // Get every phase controller 
        foreach (GameObject g in (GameObject[])FindObjectsOfType(typeof(GameObject)))
            if (g.GetComponent<IPhaseController>() is IPhaseController c)
                AddPhaseController(c); 

        // Check for required controllers in scene
        if (!GameObject.FindGameObjectWithTag("Boss").GetComponent<Boss>()) Debug.LogError("Could not find GameObject tagged \"Boss\" with a Boss component!");
        if (!GameObject.FindGameObjectWithTag("Player").GetComponent<Player>()) Debug.LogError("Could not find GameObject tagged \"Player\" with a Player component!");
        if (!GameObject.FindGameObjectWithTag("Jario").GetComponent<Jario>()) Debug.LogError("You cannot escape Jario. Please add him to the scene!");

        // Start phase
        InitializePhase(Phase.Intro); 
    }

    public static void AddPhaseController(IPhaseController controller)
    {
        controllers.Add(controller);
        controller.OnStart(); 
    }

    /// <summary>
    /// Exits the current phase
    /// </summary>
    public static void ExitPhase()
    {
        //determines which phase to transition to
        switch (phase)
        {
            case Phase.Intro:
                InitializePhase(Phase.Player);
                break;
            case Phase.Dialogue_Pre:
                InitializePhase(Phase.Boss);
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

    /// <summary>
    /// Updates current phase of battle, determines which phase belong to which objects
    /// </summary>
    private static void InitializePhase(Phase phase)
    {
        Debug.Log($"Phase switched to {phase}"); 
        Combat.phase = phase;

        foreach (IPhaseController c in controllers.Select(x => x.activePhase).ToList().Where(x => x == phase).)
        {
            
        }
    }

    
}
