using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtility 
{
    public static IEnumerator WaitForSeconds(float seconds, Action onComplete)
    { 
        yield return new WaitForSeconds(seconds);; 
        onComplete(); 
    }
}
