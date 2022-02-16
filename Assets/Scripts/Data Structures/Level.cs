using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static FileUtility; 

[CreateAssetMenu(fileName = "Boss Level Data", menuName = "Boss Level Data")]
public class Level : ScriptableObject
{
    /// <summary>
    /// Name of the boss in this level
    /// </summary>
    /// <remarks>Used to identify file names</remarks>
    public string bossName;

    /// <summary>
    /// Background sprite
    /// </summary>
    [ReadOnly] public Sprite background;

    /// <summary>
    /// Background music 
    /// </summary>
    [ReadOnly] public BGM bgm; 

    public Level(string name)
    {
        bossName = base.name;
    }

    /// <summary>
    /// Initializes data in level
    /// </summary>
    /// <remarks>When a SO is referenced, the constructor is not called</remarks>
    public void LoadReferences()
    {
        Texture2D tex = LoadFile<Texture2D>($"Art/{name}/{name}_Background");
        background = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(.5f, .5f));

        bgm = new BGM(
            LoadFile<AudioClip>($"Audio/Music/{name}_Intro"),
            LoadFile<AudioClip>($"Audio/Music/{name}_Loop")
            );
    }
}
