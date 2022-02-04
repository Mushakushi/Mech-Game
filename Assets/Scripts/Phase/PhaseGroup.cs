using System.Collections.Generic;

/// <summary>
/// Allows specifc phaseControllers to belong to more than one phase
/// </summary>
public static class PhaseGroup
{
    /// <summary>
    /// Returns the controller's manager's phase if <paramref name="activePhases"/> includes it, 
    /// Phase.Multiple otherwise
    /// </summary>
    /// <param name="activePhases">The IList (e.g. List or Array) of active Phases</param>
    public static Phase GetPhaseFromCollection(this IPhaseController controller, IList<Phase> activePhases)
    {
        foreach (Phase p in activePhases)
        {
            Phase m = controller.GetManagerPhase();
            if (p == m) return m;
        }
        return Phase.Multiple;
    }
}
