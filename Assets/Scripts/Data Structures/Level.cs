using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Boss Level Data", menuName = "Create New Boss Level Data")]
public class Level : ScriptableObject
{
    /// <summary>
    /// Name of the boss in this level
    /// </summary>
    /// <remarks>Used to identify file names</remarks>
    public new string name;

    public Level(string name)
    {
        this.name = base.name;
    }
}
