using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Scene
{
    /// <summary>
    /// Loads scene with <paramref name="name"/> name
    /// </summary>
    /// <param name="name">Name of the scene to load</param>
    /// <remarks>
    /// Never run this outside of a StartCoroutine() call,
    /// this includes calling other methods that do (e.g. foo() => bar()
    /// where bar() calls StartCoroutine(Load(...));). Will crash unity build otherwise!
    /// </remarks>
    public static IEnumerator Load(string name)
    {
        System.Diagnostics.Debug.WriteLine("Started scene load"); 
        Debug.Log($"Start scene {name} load"); 
        AsyncOperation aop = SceneManager.LoadSceneAsync(name);

        // first 90% is loading the scene
        aop.allowSceneActivation = true; 

        // Wait until the asynchronous scene loads
        while (!aop.isDone)
        {
            System.Diagnostics.Debug.WriteLine($"Loading at {aop.progress}");
            yield return new WaitForSeconds(0.1f);
        }

        Debug.LogError("Scene loaded :)");
        System.Diagnostics.Debug.WriteLine("Started succesfully loaded!");
    }
}
