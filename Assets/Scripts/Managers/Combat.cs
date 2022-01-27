using UnityEngine;
using System.Collections.Generic;
using System.Linq; 

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { Intro, Boss, Player, Dialogue_Pre, Dialogue_Post, Invalid }

[RequireComponent(typeof(DialogueController))]
public class Combat : MonoBehaviour
{
    /// <summary>
    /// The current level
    /// </summary>
    public static Level level; 

    /// <summary>
    /// Boss in battle
    /// </summary>
    public static Boss boss;

    /// <summary>
    /// Player in battle 
    /// </summary>
    public static Player player;

    /// <summary>
    /// THE ref
    /// </summary>
    public static Jario jario;

    /// <summary>
    /// Every phase observer in the scene
    /// </summary>
    private static List<IPhaseObserver> observers = new List<IPhaseObserver>();

    /// <summary>
    /// Active phase observers. Will recieve calls to start, update, and (TODO) exit
    /// </summary>
    private static List<IPhaseObserver> activeObservers = new List<IPhaseObserver>(); 

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public static Phase phase { get; private set; }

    /// <summary>
    /// Updates references in scene
    /// </summary>
    void Start()
    {
        // TODO - update this when we have more than one level
        level = new Level() { name = "Lobstobotomizer" }; 

        // Get every phase controller 
        // TODO - Optomize searching
        foreach (GameObject g in (GameObject[])FindObjectsOfType(typeof(GameObject)))
        {
            if (g.GetComponent<IPhaseObserver>() is IPhaseObserver o)
            {
                observers.Add(o);
                o.OnStart();

                // Save required controllers
                switch (g.tag)
                {
                    case "Boss":
                        boss = (Boss)o; 
                        break;
                    case "Player":
                        player = (Player)o;
                        break;
                    case "Jario":
                        jario = (Jario)o;
                        break; 
                }
            }
        }

        // Check for required controllers in scene
        if (!boss) Debug.LogError("Could not find GameObject tagged \"Boss\" with a Boss component!");
        if (!player) Debug.LogError("Could not find GameObject tagged \"Player\" with a Player component!");
        if (!jario) Debug.LogError("You cannot escape Jario. Please add him to the scene!");

        // Start phase
        InitializePhase(Phase.Intro); 
    }

    /// <summary>
    /// Add phase observers to Combat during runtime
    /// </summary>
    /// <param name="observer">Observer to add</param>
    public static void AddPhaseObserver(IPhaseObserver observer)
    {
        observers.Add(observer);
        observer.OnStart();
        if (observer.activePhase == phase) SubscribePhaseObserver(observer); 
    }

    /// <summary>
    /// Updates current phase of battle, determines which phase belong to which objects
    /// </summary>
    private static void InitializePhase(Phase phase)
    {
        Debug.Log($"Phase switched to {phase}"); 
        Combat.phase = phase;

        // could also be achieved with linq, I just think this is easier
        foreach (IPhaseObserver observer in observers)
        {
            if (observer.activePhase == phase)
            {
                observer.OnPhaseEnter();
                SubscribePhaseObserver(observer); 
            }
        }
                
    }

    /// <summary>
    /// Adds phase controller to list of active controllers
    /// </summary>
    private static void SubscribePhaseObserver(IPhaseObserver observer)
    {
        activeObservers.Add(observer); 
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
                InitializePhase(Phase.Dialogue_Post);
                break;
            case Phase.Player:
                InitializePhase(Phase.Dialogue_Pre);
                break; 
            case Phase.Boss:
                InitializePhase(Phase.Dialogue_Post);
                break;
            case Phase.Dialogue_Pre:
                InitializePhase(Phase.Boss);
                break;
            case Phase.Dialogue_Post:
                InitializePhase(Phase.Player);
                break;
            case Phase.Invalid:
            default:
                Debug.LogError("Phase switch is invalid!");
                break; 
        }
    }

    /// <summary>
    /// Updates current Phase observers
    /// </summary>
    private void Update()
    {
        foreach (IPhaseObserver o in activeObservers) o.OnPhaseUpdate();    
    }
}
