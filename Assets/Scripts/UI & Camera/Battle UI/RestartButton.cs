using UnityEngine;

public class RestartButton : MonoBehaviour
{
    /// <summary>
    /// Reloads the current battle scene
    /// </summary>
    // TODO - show percent here, too
    public void RestartBattle()
    {
        if (!BattleGroupManager.level) return;
        StartCoroutine(Scene.Load("Battle Scene"));
    }
}
