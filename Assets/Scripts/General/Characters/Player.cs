using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Player : Character
{

    public override string characterName => "";
    public override float maxHealth => 3; 
    public override float resistance => 1;
    public override Phase[] activePhases => new Phase[] { Phase.Player, Phase.Boss_Guard, Phase.Boss, Phase.Player_Win }; 

    [Header("Player Controls")]
    /// <summary>
    /// This player's input actions, automatically generated by PlayerControls.asset 
    /// </summary>
    private PlayerControls controller;
    
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

    /// <summary>
    /// 
    /// </summary>
    public bool allowQueueAction;

    /// <summary>
    /// 
    /// </summary>
    private IEnumerator queuedAction;

    /// <summary>
    /// 
    /// </summary>
    private float actionDelay;

    /// <summary>
    /// 
    /// </summary>
    public bool canAttack;

    /// <summary>
    /// 
    /// </summary>
    private enum ACTION_TYPE { Attack, Dodge, None }

    /// <summary>
    /// 
    /// </summary>
    private enum DODGE_DIRECTION { Left, Right }

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private ACTION_TYPE currentActionType;

    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private bool canRunInput;

    [Header("UI")]
    [SerializeField] private PlayerHealthSlider slider; 

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
    protected override void OnInitialize()
    {
        actionDelay = 0.5f;
        currentActionType = ACTION_TYPE.None;
        allowQueueAction = true;
        canRunInput = true;
        queuedAction = null;
    }

    protected override void PhaseEnterBehavior()
    {
        attack.Enable();
        switch (this.GetManagerPhase())
        {
            case Phase.Player:
                break;
            case Phase.Player_Win:
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
        if (this.GetManagerPhase() != Phase.Player_Win)
        {
            // input queueing 
            if (allowQueueAction)
            {
                if ((this.GetManagerPhase() == Phase.Player || this.GetManagerPhase() == Phase.Boss_Guard)
                    && (currentActionType == ACTION_TYPE.Attack || currentActionType == ACTION_TYPE.None)
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

            // run queue
            if (canRunInput && queuedAction != null)
            {
                StartCoroutine(queuedAction);
                queuedAction = null;
                canRunInput = false;
                StartCoroutine(CoroutineUtility.WaitForSeconds(actionDelay, () => canRunInput = true));
            }
        }
    }

    protected override void PhaseExitBehavior()
    {
        queuedAction = null;
        attack.Disable();
        DisableHitbox();
    }

    public override void OnHurtboxEnter(float damage)
    {
        base.OnHurtboxEnter(damage);
        slider.DepleteOneHealth();
    }

    protected override void OnHealthDeplete()
    {
        print("player defeated");
        //StartCoroutine(Scene.Load("Menu Scene")); 
    }

    private IEnumerator DoAttack()
    {
        animator.SetTrigger("Attack");
        yield return null;
    }

    private IEnumerator DoDodge(DODGE_DIRECTION direction)
    {
        animator.SetInteger("DodgeDirection", (int)direction);
        animator.SetTrigger("Dodge");
        yield return null;
    }
}
