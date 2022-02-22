using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

[RequireComponent(typeof(PlayerInput))]
public class Player : Character
{
    public override string characterName => "";
    public override float maxHealth => 3;
    public override float resistance => 1;
    public override Phase[] activePhases => new Phase[] { Phase.Player, Phase.Boss_Guard, Phase.Boss, Phase.Player_Win };

    [Header("Player Controls")]

    /// <summary>
    /// Pressed DodgeLeft this frame
    /// </summary>
    private bool dodgeLeft;

    /// <summary>
    /// Pressed DodgeRight this frame
    /// </summary>
    private bool dodgeRight;

    /// <summary>
    /// Pressed DodgeDown this frame
    /// </summary>
    private bool dodgeDown;

    /// <summary>
    /// Pressed Attack this frame
    /// </summary>
    private bool attack;

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
        GetComponent<PlayerInput>().onActionTriggered += PoolAction;
    }

    private void Start()
    {
        // set player input virtual camera
        GetComponent<PlayerInput>().camera = this.GetManager().camera;
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
        ClearPooledActions(); 
        DisableHurtbox();
        switch (this.GetManagerPhase())
        {
            case Phase.Player:
                break;
            case Phase.Player_Win:
                animator.SetTrigger("Win");
                break;
            case Phase.Boss:
                EnableHurtbox();
                break;
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
                    && attack)
                {
                    queuedAction = DoAttack();
                }
                else if (currentActionType == ACTION_TYPE.Dodge || currentActionType == ACTION_TYPE.None)
                {
                    if (dodgeLeft)
                    {
                        queuedAction = DoDodge(DODGE_DIRECTION.Left);
                    }
                    else if (dodgeRight)
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
        ClearPooledActions(); 
    }

    /// <summary>
    /// Pools input from player input
    /// </summary>
    public void PoolAction(InputAction.CallbackContext context)
    {
        if (context.action.triggered)
        {
            switch (context.action.name)
            {
                case "Tap":
                case "Attack":
                    attack = true;
                    break;
                case "DodgeLeft":
                    dodgeLeft = true;
                    break;
                case "DodgeRight":
                    dodgeRight = true;
                    break;
                case "DodgeDown":
                    dodgeDown = true;
                    break;
                default:
                    throw new System.Exception($"Action {context.action.name} is unhandeled!");
            }
        }
        else if (context.action.name == "Swipe")
        {
            if (context.control is TouchControl t && t.phase.ReadUnprocessedValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 delta = t.delta.ReadValue();

                // moving more downwards than upwards
                if (delta.y > delta.x && delta.y < 0) dodgeDown = true;
                else if (delta.x < 0) dodgeLeft = true;
                else if (delta.x > 0) dodgeRight = true; 
            }
        }
    }

    /// <summary>
    /// Clears pooled input
    /// </summary>
    /// <remarks>Should be cleared at the end of frame</remarks>
    public void ClearPooledActions()
    {
        dodgeLeft = false;
        dodgeRight = false;
        dodgeDown = false;
        attack = false; 
    }

    protected override void PhaseExitBehavior()
    {
        queuedAction = null;
        DisableHitbox();
    }

    public override void OnHitboxEnter(float damage)
    {
        base.OnHitboxEnter(damage);
        slider.DepleteOneHealth();
        new ScoreData(damageTaken: damage).AddToPlayerScore(group);
    }

    public override void OnEnterHurtbox()
    {
        DisableHitbox();
        Debug.LogError("hitbox disabled");
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
