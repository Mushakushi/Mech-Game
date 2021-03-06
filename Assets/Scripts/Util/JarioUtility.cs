using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO - move this over to jario, give jario a  public variable everyone will read!

public static class JarioUtility 
{
    /// <summary>
    /// Determines how many shakes, jario counts, etc. are performed based on current and maximum health bar values of boss
    /// </summary>
    /// <param name="controller">Controller in the same group as boss</param>
    public static int GetCounts(this IPhaseController controller)
    {
        int current = controller.GetManager().boss.healthBars;
        int maximum = controller.GetManager().boss.maxHealthBars;

        if (current <= 0) return -1; // defeated
        else if (current < maximum / 4) return 9; // one left
        else if (current <= maximum / 2) return 8; // less than or equal to half
        else if (current < maximum) return 5; // less than maximum
        else return 3; // current >= maximum
    }
}