using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MenuEventHandler : MonoBehaviour
{
    public void LoadScene(string scene) => StartCoroutine(Scene.Load(scene)); 
}
