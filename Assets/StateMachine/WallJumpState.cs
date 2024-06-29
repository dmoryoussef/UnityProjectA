using UnityEngine;

public class WallJumpState : BaseState
{
    [SerializeField] BaseState FallState;
    [SerializeField] BaseState WallGrabState; //    link to previous wall grab state in editor to loop

    private float fJumptime;
    private bool bJumping;
    private float dir;

    private void Start()
    {
        stateName = "WallJumpState";
    }

    public override void Enter()
    {
        base.Enter();

        a.Play("HeroKnight_Jump");
        rb.gravityScale = p.fDefaultGravity;
        bJumping = true;
        fJumptime = 0;
        dir = t.localScale.x;
    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();

        //x = Input.GetAxisRaw("Horizontal") * p.fNotGroundedHorizontalForce;  // jumping horizontal force

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
            rb.AddForce(Vector2.up * p.fWallJumpUpForce + (new Vector2(-dir, 0) * p.fWallJumpSideForce), ForceMode2D.Impulse);
        }

        // apply horizontal input force
        //Vector2 force = new Vector2(x * p.fMoveForce, 0);
        //if (Mathf.Abs(rb.velocity.x) < p.fMaxSpeed)
        //    rb.AddForce(force);

    }

    public override void CheckNewState()
    {
        base.CheckNewState();

        if ((bJumping == false)
           && (p.bRightWallGrab || p.bLeftWallGrab)
           && (current == null))
        {
            //  x = 0;
            current = WallGrabState;
            WallGrabState.Enter();
        }

        if ((bJumping == false)
            && (rb.velocity.y < 0)
            && (current == null)) // make sure current is null to prevent swapping between child states
        {
            current = FallState;
            FallState.Enter();
        }
    }

}
