using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scene
{
    /// <summary>
    /// Loads scene with <paramref name="name"/> name
    /// </summary>
    /// <param name="name">Name of the scene to load</param>
    /// <returns></returns>
    public static IEnumerator Load(string name)
    {
        Debug.Log($"Start scene {name} load"); 
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.LogError("Scene loaded :)"); 
    }
}
