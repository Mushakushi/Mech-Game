using System.Collections.Generic;

/// <summary>
/// Allows specifc phaseControllers to belong to more than one phase
/// </summary>
public static class PhaseGroup
{
    /// <summary>
    /// Returns PhaseManager.phase if <paramref name="activePhases"/> includes it, 
    /// Phase.Multiple otherwise
    /// </summary>
    /// <param name="activePhases">The IList (e.g. List or Array) of active Phases</param>
    public static Phase GetPhase(this IList<Phase> activePhases)
    {
        foreach (Phase p in activePhases)
            if (p == PhaseManager.phase)
                return PhaseManager.phase;
        return Phase.Mutiple;
    }
}
