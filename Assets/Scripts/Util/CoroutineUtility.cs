using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtility 
{
    /// <summary>
    /// Waits for <paramref name="seconds"/> seconds then excutes optional action on complete
    /// </summary>
    /// <param name="seconds">Seconds to wait</param>
    /// <param name="onComplete">Optional action to be executed on complete</param>
    /// <returns></returns>
    public static IEnumerator WaitForSeconds(float seconds, Action onComplete = null)
    { 
        yield return new WaitForSeconds(seconds);; 
        if (onComplete != null) onComplete(); 
    }
}
