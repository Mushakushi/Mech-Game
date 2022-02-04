using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    /// The current level
    /// </summary>
    public static Level level;

    void Awake()
    {
        foreach (GameObject g in GameObject.FindGameObjectsWithTag("PhaseManager"))
            phaseManagers.Add(g.GetComponent<PhaseManager>());

        // TODO - update this when we have more than one level
        level = new Level() { name = "Lobstobotomizer" };

        // unity serialization
        _phaseManagers = phaseManagers;
    }

    public void AddPhaseManager(PhaseManager pm)
    {
        phaseManagers.Add(pm);
    }

    // Wrapper for UnityEvent, allows it to access the static function
    public static Transform GetBossTransform(IPhaseController controller) => controller.GetBossTransform(); 
}