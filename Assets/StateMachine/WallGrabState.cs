using UnityEngine;

public class WallGrabState : BaseState
{
    [Header("Child States")]
    [SerializeField] BaseState FallState;
    [SerializeField] BaseState WallJumpState;

    private void Start()
    {
        stateName = "WallGrabState";
    }


    public override void Enter()
    {
        base.Enter();

        a.Play("HeroKnight_WallSlide");

        updateOrientation(rb.velocity.x);
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public override void PhysicsTick()
    {
        base.PhysicsTick();

        if (current == null)
        {
            Vector2 currentVel = rb.velocity;
            currentVel.y = currentVel.y * p.fWallGrabFriction;
            rb.velocity = currentVel;
        }

        if (p.bGrounded) // falling and just hit the ground
            Exit();
    }

    public override void CheckNewState()
    {
        base.CheckNewState();

        if (!p.bRightWallGrab && !p.bLeftWallGrab)  // sliding off platform - transition to free fall
        {
            current = FallState;
            FallState.Enter();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            current = WallJumpState;
            WallJumpState.Enter();
        }

    }

}
