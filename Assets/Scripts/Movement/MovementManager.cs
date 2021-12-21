using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public Vector3 startPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPosFromStart(float vertical, float horizontal)
    {
        return startPos + (Vector3.up * vertical) + (Vector3.left * horizontal);
    }

    public Vector3 GetPosFromPos(Vector3 pos, float vertical, float horizontal)
    {
        return pos + (Vector3.up * vertical) + (Vector3.left * horizontal);
    }

    public Vector3 SmoothMove(Vector3 currentPos, Vector3 targetPos, float speed)
    {
        return Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
    }

    public bool PosRoughlyEqual(Vector3 currentPos, Vector3 targetPos)
    {
        return Vector3.Distance(currentPos, targetPos) < 0.001f;
    }
}
