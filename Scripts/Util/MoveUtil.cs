using UnityEngine;

public static class MoveUtil
{
    public static Vector3 GetPosFromPos(Vector3 pos, float horizontal, float vertical)
    {
        return pos + (Vector3.up * vertical) + (Vector3.right * horizontal);
    }

    public static Vector3 SmoothMove(Vector3 currentPos, Vector3 targetPos, float speed)
    {
        return Vector3.MoveTowards(currentPos, targetPos, speed * Time.deltaTime);
    }

    public static bool PosRoughlyEqual(Vector3 currentPos, Vector3 targetPos)
    {
        return Vector3.Distance(currentPos, targetPos) < 0.1f;
    }

    public static bool PosWithinRange(Vector3 currentPos, Vector3 targetPos, float range)
    {
        return Vector3.Distance(currentPos, targetPos) <= range;
    }
}
