using System.Data.Common;
using UnityEngine;

public class MoveState : BaseState
{
    [Header("Child States")]
    [SerializeField] BaseState JumpState;
    [SerializeField] BaseState BlockState;
    [SerializeField] BaseState AttackState;
    [SerializeField] BaseState FallState;
    [SerializeField] BaseState SlideState;

    public float x;
    public Vector2 force;

    private void Start()
    {
        stateName = "MoveState";
    }

    public override void Enter()
    {
        base.Enter();

        a.Play("HeroKnight_Run");
    }

    public override void ProcessTick()
    {
        if (current?.done == true)
        {
            Enter();
        }
    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();
        x = Input.GetAxisRaw("Horizontal");
    
        a.speed = (Mathf.Abs(rb.velocity.x) / p.fMaxSpeed) + 0.5f;

        // clamp to zero and end state
        if ((x == 0) &&
            (Mathf.Abs(rb.velocity.x) < 0.5f) &&
            (current == null))
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            Exit();
        }

        updateOrientation(rb.velocity.x);

    }

    public override void PhysicsTick()
    {
        base.PhysicsTick();

        // apply input force
        force = new Vector2(x * p.fMoveForce, 0);
        if (Mathf.Abs(rb.velocity.x) < p.fMaxSpeed)
            rb.AddForce(force);

        // apply friction
        Vector2 currentVel = rb.velocity;
        currentVel.x = currentVel.x * p.fGroundFriction;
        if (Mathf.Abs(currentVel.x) < 0.1) currentVel.x = 0;
            rb.velocity = currentVel;
    }

    public override void CheckNewState()
    {
        if (Input.GetKeyDown(KeyCode.Space) && p.bGrounded)
        {
            x = 0;
            current = JumpState;
            JumpState.Enter();
        }

        if (Input.GetMouseButtonDown(1))
        {
            x = 0;
            current = BlockState;
            BlockState.Enter();
        }

        if (rb.velocity.x > 0 && x < 0)
        {
            //current = SlideState;
            //SlideState.Enter();
        }

        if (Input.GetMouseButtonDown(0))
        {
            x = 0;
            current = AttackState;
            AttackState.Enter();
        }

        if (!p.bGrounded)
        {
            current = FallState;
            FallState.Enter();
        }

    }
}
