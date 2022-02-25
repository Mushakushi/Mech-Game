using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using static CoroutineUtility; 

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

    /// <summary>
    /// POV aimer 
    /// </summary>
    [SerializeField] [ReadOnly] private CinemachinePOV aim; 

    public int group { get; set; }

    /// <summary>
    /// Default orthographic size
    /// </summary>
    [SerializeField] [Range(1, 2)] private float defaultZoom;

    /// <summary>
    /// Lerp amount of camera rotation
    /// </summary>
    [SerializeField] [Range(0, 0.5f)] private float rotationSpeed; 

    /// <summary>
    /// Amount to offset the offset of follow target y 
    /// </summary>
    private float offsetY;

    /// <summary>
    /// Amount to offset the zoom
    /// </summary>
    private float zoomOffset;

    public Phase activePhase => Phase.All; 

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        framingTransposer = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        aim = virtualCamera.GetCinemachineComponent<CinemachinePOV>(); 
    }

    public void OnStart()
    {
        FollowBoss();
        SetZoomOffset(0); 
        ResetZoom(); 
        ResetFollowTargetOffset();
    }

    public void OnPhaseEnter()
    {
        switch(this.GetManagerPhase())
        {
            case Phase.Dialogue_Pre:
            case Phase.Dialogue_Post:
                SetFollowTargetOffsetY(0.25f);
                Zoom(-.15f);
                break;
            case Phase.Player:
                SetFollowTargetOffsetY(0.1f);
                StartCoroutine(SetRotationX(10f, rotationSpeed));
                break;
            case Phase.Player_Win:
                FollowTarget(this.GetManager().player.transform);
                break;
        }
    }

    public void OnPhaseUpdate() { }

    public void OnPhaseExit()
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Dialogue_Pre:
            case Phase.Dialogue_Post:
                ResetZoom();
                break;
            case Phase.Player:
                StartCoroutine(ResetRotationX(rotationSpeed));
                break;
        }
        ResetFollowTargetOffset();
        FollowBoss(); 
    }

    /// <summary>
    /// Sets follow target to <paramref name="target"/>
    /// </summary>
    /// <param name="target">Follow target transform</param>
    public void FollowTarget(Transform target) => virtualCamera.m_Follow = target;

    /// <summary>
    /// Sets follow target to the battle group's boss
    /// </summary>
    public void FollowBoss() => FollowTarget(this.GetBossTransform());

    /// <summary>
    /// Sets follow target offset to <paramref name="offset"/>
    /// </summary>
    /// <param name="offset">Follow target offset</param>
    private void SetFollowTargetOffset(Vector2 offset) => framingTransposer.m_TrackedObjectOffset = offset;

    /// <summary>
    /// Sets follow target offset x to <paramref name="offset"/>
    /// </summary>
    /// <param name="offset">Follow target offset x</param>
    public void SetFollowTargetOffsetX(float offset) => SetFollowTargetOffset(new Vector2(offset, 0));

    /// <summary>
    /// Sets follow target offset y to <paramref name="offset"/>
    /// </summary>
    /// <param name="offset">Follow target offset y</param>
    public void SetFollowTargetOffsetY(float offset) => SetFollowTargetOffset(new Vector2(0, offset + offsetY));

    /// <summary>
    /// Offsets follow target offset y (and applies it)
    /// </summary>
    public void SetOffsetY(float offset)
    {
        offsetY = offset;
        SetFollowTargetOffsetY(framingTransposer.m_TrackedObjectOffset.y);
    }


    // TODO - lerp these

    /// <summary>
    /// Sets follow target offset to Vector3.zero
    /// </summary>
    public void ResetFollowTargetOffset() => SetFollowTargetOffset(Vector2.up * offsetY);

    /// <summary>
    /// Sets zoom to <paramref name="zoom"/>
    /// </summary>
    public void SetZoom(float zoom) => virtualCamera.m_Lens.OrthographicSize = zoom + zoomOffset;

    /// <summary>
    /// Adds <paramref name="delta"/> to current zoom
    /// </summary>
    public void Zoom(float delta) => SetZoom(virtualCamera.m_Lens.OrthographicSize + delta);

    /// <summary>
    /// Offsets zoom amount (and applies it)
    /// </summary>
    public void SetZoomOffset(float offset)
    {
        zoomOffset = offset;
        SetZoom(virtualCamera.m_Lens.OrthographicSize); 
    }

    /// <summary>
    /// Sets zoom to default zoom
    /// </summary>
    public void ResetZoom() => SetZoom(defaultZoom);

    /// <summary>
    /// Sets vertical axis aim to rotation progressively
    /// </summary>
    /// <param name="rotation">Degrees to rotate along the x axis</param>
    /// <param name="step">Timestep to linerally interpolate between the two rotations</param>
    public IEnumerator SetRotationX(float rotation, float step)
    {
        yield return Lerp(aim.m_VerticalAxis.Value, rotation, step, (x) => aim.m_VerticalAxis.Value = x);
    }

    /// <summary>
    /// Sets rotation across x axis to zero
    /// </summary>
    /// <param name="step">Timestep to linerally interpolate between the two rotations</param>
    public IEnumerator ResetRotationX(float step) => SetRotationX(0, step); 
}
