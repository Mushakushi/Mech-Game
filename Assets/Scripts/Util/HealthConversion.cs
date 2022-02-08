using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HealthConversion 
{
    /// <summary>
    /// Determines how many shakes, jario counts, etc. are performed based on current and maximum health bar values.
    /// </summary>
    /// <param name="current">Current health bars</param>
    /// <param name="maximum">Maximum health bars</param>
    /// <returns></returns>
    public static int ConvertBarsToCount(int current, int maximum)
    {
        if (current == 0) return 0; // defeated
        else if (current == 1) return 9; // one left
        else if (current <= maximum / 2) return 8; // less than or equal to half
        else if (current < maximum) return 5; // less than maximum
        else return 0; // current >= maximum
    }
}