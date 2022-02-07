using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuEventHandler : MonoBehaviour
{
    public void StartGame()
    {
        StartCoroutine(Scene.Load("Battle Scene"));
    }
}
