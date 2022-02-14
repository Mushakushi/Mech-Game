using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

/// <summary>
/// Utility IPhaseController that exposes functions to other classes
/// </summary>
[RequireComponent(typeof(CinemachineVirtualCamera))]
public class VirtualCameraExposer : MonoBehaviour, IPhaseController
{
    /// <summary>
    /// Virtual camera attached to this GameObject
    /// </summary>
    [SerializeField] [ReadOnly] private CinemachineVirtualCamera virtualCamera;

    /// <summary>
    /// Framing transposer attached to this virtual camera
    /// </summary>
    [SerializeField] [ReadOnly] private CinemachineFramingTransposer framingTransposer;

    public int group { get; set; }

    public Phase activePhase => Phase.All; 

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>(); 
    }

    /// <summary>
    /// Sets follow target to <paramref name="target"/>
    /// </summary>
    /// <param name="target">Follow target transform</param>
    public void SetFollowTarget(Transform target) => virtualCamera.m_Follow = target;

    /// <summary>
    /// Sets follow target to the battle group's boss
    /// </summary>

    /// <summary>
    /// Sets follow target offset to <paramref name="offset"/>
    /// </summary>
    /// <param name="offset">Follow target offset</param>
    private void SetFollowTargetOffset(Vector2 offset) => framingTransposer.m_TrackedObjectOffset = offset;

    /// <summary>
    /// Sets follow target offset y to <paramref name="offset"/>
    /// </summary>
    /// <param name="offset">Follow target offset y</param>
    public void SetFollowTargetOffsetY(float offset) => SetFollowTargetOffset(new Vector2(0, offset));

    /// <summary>
    /// Sets follow target offset x to <paramref name="offset"/>
    /// </summary>
    /// <param name="offset">Follow target offset x</param>
    public void SetFollowTargetOffsetX(float offset) => SetFollowTargetOffset(new Vector2(offset, 0));

    /// <summary>
    /// Sets follow target offset to Vector3.zero
    /// </summary>
    public void ResetFollowTargetOffset() => SetFollowTargetOffset(Vector2.zero);

    public void OnStart()
    {
        SetFollowTarget(this.GetManager().boss.transform);
        ResetFollowTargetOffset(); 
    }

    public void OnPhaseEnter()
    {
        Phase p = this.GetManagerPhase();

        if (p == Phase.Dialogue_Pre || p == Phase.Dialogue_Post)
            SetFollowTargetOffsetY(0.25f);
        else if (p == Phase.Player_Win)
            SetFollowTarget(this.GetManager().player.transform); 
    }

    public void OnPhaseUpdate() { }

    public void OnPhaseExit()
    {
        ResetFollowTargetOffset(); 
    }
}
