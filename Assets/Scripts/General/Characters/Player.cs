using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Player : Character
{
    [Header("Player Controls")]
    /// <summary>
    /// This player's input actions, automatically generated by PlayerControls.asset 
    /// </summary>
    private PlayerControls controller;

    /// <summary>
    /// Used to pool input action
    /// </summary>
    
    /// <summary>
    /// Dodge left button input action 
    /// </summary>
    private InputAction dodgeLeft;

    /// <summary>
    /// Dodge right button input action 
    /// </summary>
    private InputAction dodgeRight;

    /// <summary>
    /// Attack button input action
    /// </summary>
    private InputAction attack; 

    // TODO - document this 
    public bool allowQueueAction;
    private IEnumerator queuedAction;
    private Coroutine delayCoroutine = null;

    public bool canAttack;

    private enum ACTION_TYPE { Attack, Dodge, None }
    [SerializeField] private ACTION_TYPE currentActionType;
    [SerializeField] private float actionDelay;

    private enum DODGE_DIRECTION { Left, Right }

    private void Awake()
    {
        // get player input actions
        controller = new PlayerControls();

        // sets variable to true when respective input action is performed via delegate
        dodgeLeft = controller.Player.DodgeLeft;
        dodgeRight = controller.Player.DodgeRight; 
        attack = controller.Player.Attack; 
    }

    /// <summary>
    /// Enable input actions
    /// </summary>
    private void OnEnable()
    {
        dodgeLeft.Enable();
        dodgeRight.Enable();
        attack.Enable(); 
    }

    /// <summary>
    /// Disable input actions
    /// </summary>
    private void OnDisable()
    {
        dodgeLeft.Disable();
        dodgeRight.Disable();
        attack.Disable(); 
    }

    /// <summary>
    /// Initializies player values
    /// </summary>
    /// <returns>Phases this player belongs to</returns>
    protected override IList<Phase> InitializeCharacter()
    {
        health = 100.0f;
        damage = 5.0f;
        resistance = 0f;

        actionDelay = 0.4f;
        currentActionType = ACTION_TYPE.None;
        allowQueueAction = true;
        queuedAction = null;

        //triggerLayerMask.SetLayerMask(LayerMask.GetMask("Player Attack"));

        return new Phase[]{ Phase.Player, Phase.Boss, Phase.Boss_Defeat }; 
    }

    protected override void PhaseEnterBehavior()
    {
        switch (this.GetManagerPhase())
        {
            case Phase.Player:
                attack.Enable();
                EnableHitbox();
                break;
            case Phase.Boss_Defeat:
                animator.SetTrigger("Win"); 
                break;
            case Phase.Boss:
            default:
                break; 

        }
    }

    private void Update()
    {
        //print(attack.WasPressedThisFrame());
        //print(dodgeLeft.WasPressedThisFrame());
        //print(dodgeRight.WasPressedThisFrame());
    }

    protected override void PhaseUpdateBehavior() 
    {
        if (this.GetManagerPhase() != Phase.Boss_Defeat)
        {
            // input queueing 
            if (allowQueueAction)
            {
                if (this.GetManagerPhase() == Phase.Player && (currentActionType == ACTION_TYPE.Attack || currentActionType == ACTION_TYPE.None)
                    && attack.WasPressedThisFrame())
                {
                    queuedAction = DoAttack();
                }
                else if (currentActionType == ACTION_TYPE.Dodge || currentActionType == ACTION_TYPE.None)
                {
                    if (dodgeLeft.WasPressedThisFrame())
                    {
                        queuedAction = DoDodge(DODGE_DIRECTION.Left);
                    }
                    else if (dodgeRight.WasPressedThisFrame())
                    {
                        queuedAction = DoDodge(DODGE_DIRECTION.Right);
                    }
                }
            }
        }
        

        // run queue
        if (delayCoroutine == null && queuedAction != null)
        {
            StartCoroutine(queuedAction);
            //allowQueueAction = false; (case in point)
            queuedAction = null;
            delayCoroutine = StartCoroutine(WaitActionDelay());
        }
    }

    protected override void PhaseExitBehavior()
    {
        queuedAction = null;
        attack.Disable();
        DisableHitbox();
    }

    public IEnumerator WaitActionDelay()
    {
        yield return new WaitForSecondsRealtime(actionDelay);
        delayCoroutine = null;
    }

    protected override void OnHealthDeplete()
    {
        print("player defeated"); 
    }

    private IEnumerator DoAttack()
    {
        animator.SetTrigger("Attack");
        yield return null;
    }

    private IEnumerator DoDodge(DODGE_DIRECTION direction)
    {
        animator.SetInteger("Dodge Direction", (int)direction);
        animator.SetTrigger("Dodge");
        yield return null;
    }
}
