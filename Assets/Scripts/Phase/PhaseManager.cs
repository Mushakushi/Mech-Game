using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem; 

/// <summary>
/// Possible phases of battle 
/// </summary>
public enum Phase { All, Intro, Boss, Boss_Guard, Boss_Collapse, Player, Player_Win, Dialogue_Pre, Dialogue_Post, Mixed }

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
    /// <remarks>Must be unique</remarks>
    public int group { get; private set; }

    [Header("Required References")]
    [ReadOnly] public new Camera camera;
    [ReadOnly] public VirtualCameraExposer cameraExposer;
    [ReadOnly] public UIShaderOverlay uiShaderOverlay;
    [ReadOnly] public Boss boss;
    [ReadOnly] public Player player;
    [ReadOnly] public Jario jario; 

    /// <summary>
    /// Current phase of battle 
    /// </summary>
    public Phase phase { get; private set; }

    /// <summary>
    /// Every phase controller in the scene. Unity Serialization
    /// </summary>
    [SerializeField] [ReadOnly] private readonly List<IPhaseController> controllers = new List<IPhaseController>();

    /// <summary>
    /// Active phase controllers. Will recieve calls to start, update, and exit. Unity Serialization
    /// </summary>
    [SerializeField] [ReadOnly] private readonly List<IPhaseController> activeControllers = new List<IPhaseController>();

    /// <summary>
    /// This snippet allows for the static activeControllers to be serialized in Unity
    /// </summary>
    /// <remarks>Don't bother trying to debug clones in inspector.. doesn't serialize</remarks>
    #if UNITY_EDITOR
    [Space(15f)]
    [Header("Phase")]
    [ReadOnly] [InspectorName("Current Phase")] public Phase _phase;

    [Space(15f)]
    [ReadOnly] [SerializeField] [InspectorName("Controllers")] private List<GameObject> _controllers;

    [Space(15f)]
    [ReadOnly] [SerializeField] [InspectorName("Active Controllers")] private List<GameObject> _activeControllers;
    #endif

    /// <summary>
    /// List of default phase events
    /// </summary>
    [SerializeField] private readonly List<DefaultPhaseEvent> defaultPhaseEvents = new List<DefaultPhaseEvent>();

    /// <summary>
    /// Amount to transform new phase manager
    /// </summary>
    private const int width = 10;

    /// <summary>
    /// Initializes this group and gets every phase controller child
    /// </summary>
    public void OnAwake()
    {
        // Set group index, also equal to current player number
        group = BattleGroupManager.phaseManagers.Count() - 1;

        ScoreUtil.CreatePlayerScore(group);

        // get and cull camera
        // culling virtual cameras prevents cinemachine brain from autoblending on add
        camera = gameObject.GetComponentInChildren<Camera>();
        camera.cullingMask = ~((1 << 6) | (1 << 7) | (1 << 8) | (1 << 9)); // excludes all (hardcoded) culling layers
        int cull = group + 6;  // player cull layer starts at 6
        camera.cullingMask |= 1 << cull;  // include culling layer 

        // set ui camera overlay camera to this camera
        gameObject.GetComponentInChildren<Canvas>().worldCamera = camera;

        // Clear controllers
        controllers.Clear(); 

        // Get phase controller(s) attached to this object
        foreach (IPhaseController controller in GetComponents<IPhaseController>()) controllers.Add(controller);

        // Get all child phase controller(s)
        foreach (Transform child in GetAllChildren(transform))
        {
            if (child.GetComponent<IPhaseController>() is IPhaseController controller)
            {
                controller.group = group; 
                controllers.Add(controller);

                // Save required controllers
                // note usage of as instead of cast because it is possible to improperly tag a gameObject!
                switch (child.tag)
                {
                    case "MainCamera":
                        cameraExposer = controller as VirtualCameraExposer;
                        if (cameraExposer) cameraExposer.gameObject.layer = cull;
                        break; 
                    case "Boss":
                        boss = controller as Boss; 
                        break;
                    case "Player":
                        player = controller as Player;
                        break;
                    case "Jario":
                        jario = controller as Jario;
                        break;
                    case "UIShaderOverlay":
                        uiShaderOverlay = controller as UIShaderOverlay;
                        break; 
                }
            }
        }

        // Check for required controllers in scene
        if (!cameraExposer) Debug.LogError("Could not find child virtual camera tagged \"MainCamera\" with a VirtualCameraExposer script!");
        if (!boss) Debug.LogError("Could not find child GameObject tagged \"Boss\" with a Boss component!");
        if (!player) Debug.LogError("Could not find child GameObject tagged \"Player\" with a Player component!");
        if (!jario) Debug.LogError("You cannot escape Jario. Please add him to your list of dependents!");

        // Add default phase events
        foreach (DefaultPhaseEvent e in defaultPhaseEvents) AddRuntimePhaseController(e);
    }

    /// <summary>
    /// Adds this phase manager as a runtime phase manager if not player one, 
    /// who is not a runtime phase manager
    /// </summary>
    public void OnPlayerJoined()
    {
        if (PlayerInputManager.instance.playerCount > 1)
        {
            Debug.LogError($"Player {PlayerInputManager.instance.playerCount} joined."); 
            BattleGroupManager.AddRuntimePhaseManager(this);
            OnAwake();
            Start();
            transform.position += width * Vector3.right * group;
        }
    }

    // important to call controller.onStart after awake references to not break game!
    private void Start()
    {
        // Set first phase. Note: IPhaseController.OnPhaseEnter is called after OnStart...
        phase = Phase.Intro; 

        // Start all child phase controller(s)
        foreach (IPhaseController controller in controllers) controller.OnStart();

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
    public void AddRuntimePhaseController(IPhaseController controller)
    {
        controllers.Add(controller);
        controller.group = group; 
        controller.OnStart();
        TrySubscribePhaseController(controller); 
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
        //determines which phase to transition to
        switch (phase)
        {
            case Phase.Intro:
                EnterPhase(Phase.Dialogue_Post);
                break;
            case Phase.Player:
                EnterPhase(Phase.Boss_Guard);
                break;
            case Phase.Boss_Guard:
                EnterPhase(Phase.Dialogue_Pre);
                break; 
            case Phase.Dialogue_Pre:
                EnterPhase(Phase.Boss);
                break;
            case Phase.Boss:
                EnterPhase(Phase.Dialogue_Post);
                break;
            case Phase.Dialogue_Post:
                EnterPhase(Phase.Player);
                break;
            case Phase.Mixed:
            default:
                Debug.LogError("Phase switch is invalid!");
                break; 
        }
    }

    /// <summary>
    /// Exits the current phase and switches to <paramref name="phase"/> phase, interrupting the current flow, 
    /// subscribing controllers as neccessary
    /// </summary>
    /// <param name="phase">Phase to enter</param>
    /// <remarks>Phases only change when this function is called in an IPhaseController</remarks>
    public void EnterPhase(Phase phase)
    {
        UnsubscribeAll();
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
        if (controller.activePhase == phase || controller.activePhase == Phase.All)
        {
            activeControllers.Add(controller);
            controller.OnPhaseEnter();
            return true;
        }
        return false;
    }

}