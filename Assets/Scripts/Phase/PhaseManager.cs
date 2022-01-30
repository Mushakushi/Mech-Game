using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System; 

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { Intro, Boss, Player, Dialogue_Pre, Dialogue_Post, Mutiple }

[RequireComponent(typeof(DialogueController))]
public class PhaseManager : MonoBehaviour
{
    [Header("Required References")]
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
    /// Current phase of battle 
    /// </summary>
    public static Phase phase { get; private set; }

    /// <summary>
    /// Every phase controller in the scene
    /// </summary>
    private static List<IPhaseController> controllers = new List<IPhaseController>();

    /// <summary>
    /// Active phase controllers. Will recieve calls to start, update, and (TODO) exit
    /// </summary>
    public static List<IPhaseController> activeControllers = new List<IPhaseController>();

    /// <summary>
    /// This snippet allows for the static activeControllers to be serialized in Unity
    /// </summary>
    #if UNITY_EDITOR
    [Header("Phase")]
    [ReadOnly] [InspectorName("Current Phase")] public Phase _phase;

    [Space(15f)]
    [ReadOnly] [SerializeField] [InspectorName("Active Controllers")] private List<GameObject> _activeControllers;
    #endif

    /// <summary>
    /// List of default phase events
    /// </summary>
    [SerializeField] List<DefaultPhaseEvent> defaultPhaseEvents = new List<DefaultPhaseEvent>(); 

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
            if (g.GetComponent<IPhaseController>() is IPhaseController o)
            {
                controllers.Add(o);
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

        // Add default phase events
        foreach (DefaultPhaseEvent e in defaultPhaseEvents) controllers.Add(e); 

        // Start phase
        EnterPhase(Phase.Intro); 
    }

    /// <summary>
    /// Add phase controller to PhaseManager during runtime
    /// </summary>
    /// <param name="controller">Controller to add</param>
    public static void AddPhaseController(IPhaseController controller)
    {
        controllers.Add(controller);
        controller.OnStart();
        TrySubscribePhaseController(controller); 
    }

    /// <summary>
    /// Enters phase and tries to subscribe each controller to current phase
    /// </summary>
    /// <param name="phase">Phase to enter</param>
    private static void EnterPhase(Phase phase)
    {
        PhaseManager.phase = phase;

        // could also be achieved with linq, I just think this is easier
        foreach (IPhaseController controller in controllers)
        {
            TrySubscribePhaseController(controller);
        }
    }

    /// <summary>
    /// Adds phase controller to list of active phase controllers if its active phase is equal to the current phase
    /// </summary>
    /// <param name="controller">Controller to try to add</param>
    /// think of this as a private delegate
    private static bool TrySubscribePhaseController(IPhaseController controller)
    {
        if (controller.activePhase == phase)
        {
            activeControllers.Add(controller);
            controller.OnPhaseEnter();
            return true; 
        }
        return false; 
    }

    /// <summary>
    /// Updates subscribed Phase controllers
    /// </summary>
    private void Update()
    {
        foreach (IPhaseController o in activeControllers.ToList()) o.OnPhaseUpdate();

        // Serialization helper, TODO - better way to serialize this data
        _phase = phase;
        _activeControllers = activeControllers.Select(x => x.gameObject).Where(x => x != null).ToList(); 
    }

    /// <summary>
    /// Exits the current phase and switches to the next phase according to a predefined map
    /// </summary>
    // Note - phases only change when this function is called in an IPhaseController
    public static void ExitPhase()
    {
        // unsubscribe all active controllers
        foreach (IPhaseController controller in controllers) controller.OnPhaseExit(); 
        activeControllers.Clear(); 

        //determines which phase to transition to
        switch (phase)
        {
            case Phase.Intro:
                EnterPhase(Phase.Dialogue_Post);
                break;
            case Phase.Player:
                EnterPhase(Phase.Dialogue_Pre);
                break; 
            case Phase.Boss:
                EnterPhase(Phase.Dialogue_Post);
                break;
            case Phase.Dialogue_Pre:
                EnterPhase(Phase.Boss);
                break;
            case Phase.Dialogue_Post:
                EnterPhase(Phase.Player);
                break;
            case Phase.Mutiple:
            default:
                Debug.LogError("Phase switch is invalid!");
                break; 
        }
    }
}