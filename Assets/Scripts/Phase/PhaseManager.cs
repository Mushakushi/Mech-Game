using UnityEngine;
using UnityEngine.Events; 
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { Intro, Boss, Boss_Collapse, Boss_Defeat, Player, Dialogue_Pre, Dialogue_Post, Multiple }

/// <summary>
/// Manages the phase behavior of every child phase controller
/// </summary>
[System.Serializable]
[RequireComponent(typeof(DialogueController))]
public class PhaseManager : MonoBehaviour
{
    /// <summary>
    /// The index of this battle's group
    /// </summary>
    public int group { get; private set; }

    [Header("Required References")]
    /// <summary>
    /// Boss in battle
    /// </summary>
    [ReadOnly] public Boss boss;

    /// <summary>
    /// Player in battle 
    /// </summary>
    [ReadOnly] public Player player;

    /// <summary>
    /// THE ref
    /// </summary>
    [ReadOnly] public Jario jario;

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public Phase phase { get; private set; }

    /// <summary>
    /// Every phase controller in the scene
    /// </summary>
    [SerializeField] [ReadOnly] private List<IPhaseController> controllers = new List<IPhaseController>();

    /// <summary>
    /// Active phase controllers. Will recieve calls to start, update, and exit
    /// </summary>
    [SerializeField] [ReadOnly] private List<IPhaseController> activeControllers = new List<IPhaseController>();

    /// <summary>
    /// This snippet allows for the static activeControllers to be serialized in Unity
    /// </summary>
    #if UNITY_EDITOR
    [Space(15f)]
    [Header("Phase")]
    [ReadOnly] [InspectorName("Current Phase")] public Phase _phase;

    [Space(15f)]
    [ReadOnly] [SerializeField] [InspectorName("Active Controllers")] private List<GameObject> _controllers;

    [Space(15f)]
    [ReadOnly] [SerializeField] [InspectorName("Active Controllers")] private List<GameObject> _activeControllers;
    #endif

    /// <summary>
    /// List of default phase events
    /// </summary>
    [SerializeField] private List<DefaultPhaseEvent> defaultPhaseEvents = new List<DefaultPhaseEvent>();

    #region DEFAULT PHASE EVENTS CLASS
    // IMPORTANT - make sure you save to version control when REFACTORING because changing Unity's
    // reserialization can actually delete all of the events without undo! 
    // Please don't be like me :) (really be careful...)

    /// <summary>
    /// Phase Controller that publishes events
    /// </summary>
    [System.Serializable]
    public sealed class DefaultPhaseEvent : IPhaseController
    {
        // only accessible to this class doesn't matter what this is
        [HideInInspector] public GameObject gameObject => null;

        /// <summary>
        /// The group this controller belongs to
        /// </summary>
        public int group { get; set; }

        /// <summary>
        /// Phase(s) in which this gameObject belongs to
        /// </summary>
        [SerializeField] public List<Phase> activePhases;

        /// <summary>
        /// Returns PhaseManager.Phase if activePhases contains PhaseManager.Phase. Phase.Invalid otherwise
        /// </summary>
        [HideInInspector] public Phase activePhase { get => this.GetPhaseFromCollection(activePhases); }

        [Header("OnPhaseEnter()")]
        /// <summary>
        /// Event associated with Phase enter
        /// </summary>
        [SerializeField] private PhaseEnter phaseEnter = new PhaseEnter { };
        [System.Serializable] private class PhaseEnter : UnityEvent { }

        [Header("OnPhaseUpdate()")]
        /// <summary>
        /// Event associated with Phase update
        /// </summary>
        [SerializeField] private PhaseUpdate phaseUpdate = new PhaseUpdate { };
        [System.Serializable] private class PhaseUpdate : UnityEvent { }

        [Header("OnPhaseExit()")]
        /// <summary>
        /// Event associated with Phase exit
        /// </summary>
        [SerializeField] private PhaseExit phaseExit = new PhaseExit { };
        [System.Serializable] private class PhaseExit : UnityEvent { }

        /// <summary>
        /// What happens when this controller is added as a default Phase event
        /// </summary>
        public void OnStart() { }

        /// <summary>
        /// Sets enter trigger
        /// </summary>
        public void OnPhaseEnter()
        {
            phaseEnter.Invoke();
        }

        /// <summary>
        /// Sets update trigger
        /// </summary>
        public void OnPhaseUpdate()
        {
            phaseUpdate.Invoke();
        }

        /// <summary>
        /// Sets exit trigger
        /// </summary>
        public void OnPhaseExit()
        {
            phaseExit.Invoke();
        }
    }
    #endregion

    /// <summary>
    /// Initializes this group and gets every phase controller child
    /// </summary>
    void Start()
    {
        // Set group index 
        group = 0; 

        // Add default phase events 
        foreach (DefaultPhaseEvent e in defaultPhaseEvents) AddPhaseController(e);

        // Get phase controller attached to this object
        if (GetComponent<IPhaseController>() is IPhaseController selfController)
        {
            controllers.Add(selfController);
            selfController.OnStart();
        }

        // Get all child phase controllers
        foreach (Transform child in GetAllChildren(transform))
        {
            if (child.GetComponent<IPhaseController>() is IPhaseController controller)
            {
                controllers.Add(controller);
                controller.OnStart();

                // Save required controllers
                switch (child.tag)
                {
                    case "Boss":
                        boss = (Boss)controller; 
                        break;
                    case "Player":
                        player = (Player)controller;
                        break;
                    case "Jario":
                        jario = (Jario)controller;
                        break; 
                }
            }
        } 

        // Check for required controllers in scene
        if (!boss) Debug.LogError("Could not find child GameObject tagged \"Boss\" with a Boss component!");
        if (!player) Debug.LogError("Could not find child GameObject tagged \"Player\" with a Player component!");
        if (!jario) Debug.LogError("You cannot escape Jario. Please add him to your list of dependents!");

        // Start phase
        EnterPhase(Phase.Intro); 
    }

    /// <summary>
    /// Recursively gets all children (including children of children)
    /// </summary>
    /// <param name="children">Parent of children</param>
    /// <returns>Children of this gameObject</returns>
    private List<Transform> GetAllChildren(Transform parent)
    {
        List<Transform> children = new List<Transform> { }; 

        // Get every child
        foreach (Transform child in parent) // This is a built in feature ... go figure
        {
            children.Add(child); 
        }

        // Get children of child
        foreach (Transform child in children.ToList())
        {
            if (child.childCount > 0) children.AddRange(GetAllChildren(child)); 
        }

        return children; 
    }

    /// <summary>
    /// Add phase controller to PhaseManager during runtime
    /// </summary>
    /// <param name="controller">Controller to add</param>
    public void AddPhaseController(IPhaseController controller)
    {
        controllers.Add(controller);
        controller.group = group; 
        controller.OnStart();
        TrySubscribePhaseController(controller); 
    }

    /// <summary>
    /// Enters phase and tries to subscribe each controller to current phase
    /// </summary>
    /// <param name="phase">Phase to enter</param>
    private void EnterPhase(Phase phase)
    {
        this.phase = phase;
        foreach (IPhaseController controller in controllers) TrySubscribePhaseController(controller);
    }

    /// <summary>
    /// Adds phase controller to list of active phase controllers if its active phase is equal to the current phase
    /// </summary>
    /// <param name="controller">Controller to try to add</param>
    /// think of this as a private delegate
    private bool TrySubscribePhaseController(IPhaseController controller)
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

        // unity serialization 
        _phase = phase;
        _controllers = controllers.Select(x => x.gameObject).ToList(); 
        _activeControllers = activeControllers.Select(x => x.gameObject).ToList(); 
    }

    /// <summary>
    /// Unsubscribes all active controllers
    /// </summary>
    private void UnsubscribeAll()
    {
        // unsubscribe all active controllers
        foreach (IPhaseController controller in controllers) controller.OnPhaseExit();
        activeControllers.Clear();
    }

    /// <summary>
    /// Exits the current phase and switches to the next phase according to a predefined map
    /// </summary>
    /// <remarks>Phases only change when this function is called in an IPhaseController</remarks>
    public void ExitPhase()
    {
        UnsubscribeAll(); 

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
            case Phase.Multiple:
            default:
                Debug.LogError("Phase switch is invalid!");
                break; 
        }
    }

    /// <summary>
    /// Exits the current phase and switches to <paramref name="targetPhase"/> phase, interrupting the current flow
    /// </summary>
    /// <remarks>Phases only change when this function is called in an IPhaseController</remarks>
    public void ExitPhase(Phase targetPhase)
    {
        UnsubscribeAll(); 
        EnterPhase(targetPhase);
    }

}