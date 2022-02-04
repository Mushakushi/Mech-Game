using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEventHandler : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(AsyncLoadBattleScene());
    }

    IEnumerator AsyncLoadBattleScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Battle Scene");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        Debug.Log("Scene loaded :)");

        gameObject.SetActive(false);
    }
}
