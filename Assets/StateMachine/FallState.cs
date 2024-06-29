using UnityEngine;

public class FallState : BaseState
{
    private float x;

    private void Start()
    {
        stateName = "FallState";
    }

    public override void Enter()
    {
        base.Enter();

        a.Play("HeroKnight_Fall");

        rb.gravityScale = p.fFastFallScale;
    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();

        x = Input.GetAxisRaw("Horizontal") * p.fNotGroundedHorizontalForce;  // jumping horizontal force

        updateOrientation(x); 
    }

    public override void PhysicsTick()
    {
        base.PhysicsTick();

        if (p.bGrounded) // falling and just hit the ground
            Exit();

        // apply horizontal input force
        Vector2 force = new Vector2(x * p.fMoveForce, 0);
        if (Mathf.Abs(rb.velocity.x) < p.fMaxSpeed)
            rb.AddForce(force);
    }
}
