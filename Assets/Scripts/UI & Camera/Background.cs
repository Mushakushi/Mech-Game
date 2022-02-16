using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Background : MonoBehaviour
{
    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = BattleGroupManager.level.background;


        // TODO - scale this properly us SO
        transform.localScale = new Vector2(1.25f, 1.25f); 
    }
        
}
