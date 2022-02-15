using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System; 

public class BattleGroupManager : MonoBehaviour
{
    /// <summary>
    /// Every Phase manager in scene
    /// </summary>
    public static List<PhaseManager> phaseManagers = new List<PhaseManager> { };

    // serializes phaseManagers in editor
    #if UNITY_EDITOR
    [SerializeField] [ReadOnly] private List<PhaseManager> _phaseManagers = new List<PhaseManager> { };
    #endif

    /// <summary>
    /// The current level scriptable object
    /// </summary>
    public static Level level;

    void Awake()
    {
        // delete after done debuging battle scene
        LoadLevelData("Lobstobotomizer");

        // applies level data to scene 
        OnLoadLevel();

        // adds all current phaseManagers to managers
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PhaseManager"))
            phaseManagers.Add(g.GetComponent<PhaseManager>()); 

        // allows p to get references in awake after this 
        foreach (PhaseManager p in phaseManagers) p.OnAwake(); 
    }

    /// <summary>
    /// Adds PhaseManager <paramref name="manager"/> manager to list of managers in scene
    /// </summary>
    /// <param name="manager">The Phase manager</param>
    public static void AddRuntimePhaseManager(PhaseManager manager)
    {
        phaseManagers.Add(manager);
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
        level = FileUtility.LoadFile<Level>($"Scriptable Objects/Level Data/{name}");
    }

    /// <summary>
    /// Applies level data to scene 
    /// </summary>
    private void OnLoadLevel()
    {
        // get rid of old references 
        phaseManagers.Clear();

        // add boss script to boss 
        Type bossType = Type.GetType(level.bossName);
        if (bossType != null) GameObject.FindGameObjectWithTag("Boss").AddComponent(bossType); 

        // change music 

        // change background 

        // etc...
    }

    // Wrapper for UnityEvent, allows it to access the static function
    public static Transform GetBossTransform(IPhaseController controller) => controller.GetBossTransform(); 
}