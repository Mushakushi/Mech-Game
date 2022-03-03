using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// TODO - every interactable ui element must have a collider/ be raycastable
// cursor would check overlap and get relevant components from element
// (e.g. button would get their onclick() event fired)

[RequireComponent(typeof(PlayerInput), typeof(Animator))]
public class Cursor : MonoBehaviour
{
    /// <summary>
    /// Delta movement
    /// </summary>
    private Vector2 delta;

    /// <summary>
    /// Animator attached to this gameObject
    /// </summary>
    [SerializeField] [ReadOnly] private Animator animator; 

    /// <summary>
    /// Speed of movement
    /// </summary>
    /// <remarks>Adjust as neccessary to calculate for deltaTime</remarks>
    [SerializeField] [Min(250)] private float magnitude;

    private void Awake()
    {
        animator = GetComponent<Animator>(); 
    }

    // move
    private void FixedUpdate()
    {
        transform.position += (Vector3)delta * magnitude * Time.deltaTime;
        // check for overlap && input 
        // do stuff with ui
    }
    
    // get movement, called in input manager
    private void OnMove(InputValue value)
    {
        delta = value.Get<Vector2>();
    }
}
