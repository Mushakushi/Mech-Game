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
    /// <remarks>Still needs to be wrapped with StartCoroutine</remarks>
    public static IEnumerator WaitForSeconds(float seconds, Action onComplete = null)
    {
        Debug.Log('y'); 
        yield return new WaitForSeconds(seconds);;
        onComplete?.Invoke();
    }
}
