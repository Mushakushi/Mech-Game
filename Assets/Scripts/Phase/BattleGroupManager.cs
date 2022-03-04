using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.InputSystem;

// TODO - rename this class
[RequireComponent(typeof(PlayerInputManager))]
public class BattleGroupManager : MonoBehaviour
{
    /// <summary>
    /// Every Phase manager in scene
    /// </summary>
    public static List<PhaseManager> phaseManagers = new List<PhaseManager> { };

    // serializes phaseManagers in editor
    #if UNITY_EDITOR
    [ReadOnly] [SerializeField] private List<PhaseManager> _phaseManagers = new List<PhaseManager> { };
    #endif

    /// <summary>
    /// The current level scriptable object
    /// </summary>
    public static Level level;

    /// <summary>
    /// Prefab to spawn in unity
    /// </summary>
    [SerializeField] private GameObject playerPrefab;

    void Awake()
    {
        // set unity random seed
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);

        // enable or disable joining    
        PlayerInputManager inputMangager = GetComponent<PlayerInputManager>(); 
        if (GlobalSettings.isMultiplayerGame)
        {
            inputMangager.EnableJoining();
            // join player
            if (PlayerInputManager.instance.playerCount > 0) PlayerInput.Instantiate(playerPrefab); 
        }
        else inputMangager.DisableJoining();

        // delete after done debuging battle scene
        //LoadLevelData("Shoto");
        TranslatableTextManager.SetGameLang(LANGUAGE.English);

        // applies level data to scene 
        OnLoadLevel();

        // adds all current phaseManagers to managers
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PhaseManager"))
            phaseManagers.Add(g.GetComponent<PhaseManager>()); 

        // allows p to get references in awake after this 
        foreach (PhaseManager p in phaseManagers) p.OnAwake();
    }

    /// <summary>
    /// Called in this class by playerInputManager when a new player connects 
    /// </summary>
    /// <remarks>Convinient if we need another argument, worthless otherwise</remarks>
    public class PlayerConnectionEventArgs : EventArgs 
    {
        /// <summary>
        /// Number of players connected 
        /// </summary>
        public int playersConnected; 

        public PlayerConnectionEventArgs(int playersConnected)
        {
            this.playersConnected = playersConnected; 
        } 
    }

    /// <summary>
    /// Called by PlayerInputManager when a new player joins 
    /// </summary>
    public void OnPlayerJoined()
    {
        PlayerConnectionEventArgs e = new PlayerConnectionEventArgs(
            PlayerInputManager.instance.playerCount
            );

        // if not player 1 (who is not a runtime phase manager) or already initialized
        // checking player instance will prevent player 1 from running twice
        // is awake will prevent instance from running when it's already ran
        // (bc this is called on every prefab apparently)
        if (PlayerInputManager.instance.playerCount > 1)
        {
            if (GameObject.FindGameObjectsWithTag("PhaseManager")[e.playersConnected - 1].GetComponent<PhaseManager>()
                is PhaseManager p)
            {
                Debug.LogError($"Player {e.playersConnected} joined.");
                AddRuntimePhaseManager(p);
                p.OnAwake();
            }
        }

        // notify each phaseManager of join
        foreach (PhaseManager p in phaseManagers) p.OnPlayerJoined(e); 
    }

    /// <summary>
    /// Adds PhaseManager <paramref name="manager"/> manager to list of managers in scene
    /// </summary>
    /// <param name="manager">The Phase manager</param>
    public static void AddRuntimePhaseManager(PhaseManager manager)
    {
        phaseManagers.Add(manager);
    }

    private void Start()
    {
        AudioPlayer.PlayBGM(level.bgm);
    }

    #if UNITY_EDITOR
    private void Update()
    {
        // unity serialization
        _phaseManagers = phaseManagers;
    }
    #endif

    /// <summary>
    /// Loads Level ScriptableObject data from Scriptable Objects/Level Data/<paramref name="name"/>
    /// </summary>
    /// <param name="name">Name of level data field</param>
    public static void LoadLevelData(string name)
    {
        // get SO
        level = FileUtility.LoadFile<Level>($"{FileUtility.levelDataPath}/{name}");

        // initialize SO
        level.LoadReferences();
    }

    /// <summary>
    /// Applies level data to scene 
    /// </summary>
    private void OnLoadLevel()
    {
        // get rid of old static references 
        phaseManagers.Clear();

        // add boss script to boss 
        Type bossType = Type.GetType(level.bossName);
        if (bossType != null) GameObject.FindGameObjectWithTag("Boss").AddComponent(bossType);

        // change background 

        // etc...
    }

    // Wrapper for UnityEvent, allows it to access the static function
    public static Transform GetBossTransform(IPhaseController controller) => controller.GetBossTransform(); 
}