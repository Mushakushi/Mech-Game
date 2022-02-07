using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PhaseManagerAccess
{
    /// <summary>
    /// Gets the phase manager associated with this <paramref name="controller"/>
    /// </summary>
    /// <param name="controller">The controller</param>
    /// <returns>The PhaseManager associated with this <paramref name="controller"/></returns>
    public static PhaseManager GetManager(this IPhaseController controller)
    {
        if (controller.group < 0 || controller.group >= BattleGroupManager.phaseManagers.Count)
            throw new System.Exception($"Controller {controller.GetType()}'s group, " +
                $"{controller.group}, is out of bounds!"); 
        return BattleGroupManager.phaseManagers[controller.group];
    }

    /// <summary>
    /// Gets the phase associated with this <paramref name="controller"/>'s manager
    /// </summary>
    /// <param name="controller">The controller</param>
    /// <returns>The Phase associated with this <paramref name="controller"/>'s manager</returns>
    public static Phase GetManagerPhase(this IPhaseController controller) => GetManager(controller).phase;

    /// <summary>
    /// Exits the current phase in group
    /// </summary>
    /// <param name="controller">The controller</param>
    public static void ExitPhase(this IPhaseController controller)
    {
        Debug.LogError($"{controller.GetManagerPhase()} phase exited by {controller.gameObject.name}"); 
        GetManager(controller).ExitPhase();
    }

    /// <summary>
    /// Exits the current phase in group to <paramref name="targetPhase"/> phase, interrupting the current flow
    /// </summary>
    /// <param name="controller">The controller</param>
    public static void ExitPhase(this IPhaseController controller, Phase targetPhase)
    {
        Debug.LogError($"{controller.GetManagerPhase()} phase exited to {targetPhase} phase by {controller.gameObject.name}");
        GetManager(controller).ExitPhase(targetPhase);
    }

    /// <summary>
    /// Gets the Boss's transform associated with this <paramref name="controller"/>'s group
    /// </summary>
    /// <param name="controller">The controller</param>
    /// <returns>Boss's transform</returns>
    public static Transform GetBossTransform(this IPhaseController controller) => GetManager(controller).boss.transform;
}