using UnityEngine;

public class IdleState : BaseState
{
    [Header("Child States")]
    [SerializeField] BaseState JumpState;
    [SerializeField] BaseState MoveState;
    [SerializeField] BaseState AttackState;
    [SerializeField] BaseState BlockState;
    [SerializeField] BaseState FallState;

    private void Start()
    {     
        stateName = "Idle State";
    }

    public override void Enter()
    {
        base.Enter();
        a.Play("HeroKnight_Idle");

    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();

        if (rb.velocity.magnitude != 0)
            rb.velocity = Vector2.zero;
    }

    public override void ProcessTick()
    {
        if (current?.done == true)
            Enter();
    }

    public override void CheckNewState()
    {
        // Input state changes
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            current = MoveState;
            MoveState.Enter();
        }

        if (Input.GetKeyDown(KeyCode.Space) && p.bGrounded)
        {
            current = JumpState;
            JumpState.Enter();
        }

        if (Input.GetMouseButtonDown(0))
        {
            current = AttackState;
            AttackState.Enter();
        }

        if (Input.GetMouseButtonDown(1))
        {
            current = BlockState;
            BlockState.Enter();
        }

        //  Non Input state changes
        if (!p.bGrounded)
        {
            current = FallState;
            FallState.Enter();
        }
    }
}
