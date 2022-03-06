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
        yield return new WaitForSeconds(seconds);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Waits for <paramref name="seconds"/> seconds then excutes optional action on complete, ignoring timeScale
    /// </summary>
    /// <param name="seconds">Seconds to wait</param>
    /// <param name="onComplete">Optional action to be executed on complete</param>
    /// <remarks>Still needs to be wrapped with StartCoroutine</remarks>
    public static IEnumerator WaitForSecondsRealtime(float seconds, Action onComplete = null)
    {
        yield return new WaitForSecondsRealtime(seconds);
        onComplete?.Invoke();
    }

    /// <summary>
    /// Linerally interpolates between lhs and rhs by time
    /// </summary>
    /// <param name="lhs">Origin value</param>
    /// <param name="rhs">Target value</param>
    /// <param name="step">Timestep to interpolate. Must be greater than zero.</param>
    /// <param name="value">Call back to update value</param>
    /// <returns></returns>
    public static IEnumerator Lerp(float lhs, float rhs, float step, Action<float> value)
    {
        // clamp
        Mathf.Clamp01(step);

        // could yield break here as well
        if (step <= 0) throw new Exception($"Time {step} is less than or equal to zero!"); 

        // lerp
        for (float i = 0; i <= 1; i += step)
        {
            value(Mathf.Lerp(lhs, rhs, i));
            yield return null; 
        }
    }
}
