using UnityEngine;

public class JumpState : BaseState
{
    [Header("Child States")]
    [SerializeField] BaseState FallState;
    [SerializeField] BaseState WallGrabState;

    private bool bJumping;
    private float fJumptime;

    private float x;

    private void Start()
    {
        stateName = "JumpState";
    }

    public override void Enter()
    {
        base.Enter();
        a.Play("HeroKnight_Jump");

        rb.gravityScale = p.fDefaultGravity;

        bJumping = true;
        fJumptime = 0;
        x = 0;
    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();

        x = Input.GetAxisRaw("Horizontal") * p.fNotGroundedHorizontalForce;  // jumping horizontal force

        if (rb.velocity.y < 0 && current == null) // make sure current is null to prevent swapping between child states
        {
            current = FallState;
            FallState.Enter();
        }
        
        if (p.bLeftWallGrab || p.bRightWallGrab)
        {
            if (WallGrabState)
            {
                x = 0;
                current = WallGrabState;
                WallGrabState.Enter();
            }
        }
    }

    public override void ProcessTick()
    {
        base.ProcessTick();

        updateOrientation(rb.velocity.x);

        if (current?.done == true)
        {
            Exit();
        }

        if (bJumping)
        {
            fJumptime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) | (fJumptime > p.fJumpMax))
        {
            bJumping = false;
        }
    }

    public override void PhysicsTick()
    {
        base.PhysicsTick();

        if (bJumping)
        {
            rb.AddForce(Vector2.up * p.fJumpForce, ForceMode2D.Impulse);
        }

        // apply horizontal input force
        Vector2 force = new Vector2(x * p.fMoveForce, 0);
        if (Mathf.Abs(rb.velocity.x) < p.fMaxSpeed)
            rb.AddForce(force);


    }
}
