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

        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PhaseManager"))
            phaseManagers.Add(g.GetComponent<PhaseManager>());

        foreach (PhaseManager p in phaseManagers) p.OnStart(); 

        // unity serialization
        _phaseManagers = phaseManagers;
    }

    public void AddPhaseManager(PhaseManager pm)
    {
        phaseManagers.Add(pm);
    }

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
        // add boss script to boss 
        Type type = Type.GetType(BattleGroupManager.level.bossName);
        if (type != null)
        {
            GameObject.FindGameObjectWithTag("Boss").AddComponent
        }
    }

    // Wrapper for UnityEvent, allows it to access the static function
    public static Transform GetBossTransform(IPhaseController controller) => controller.GetBossTransform(); 
}