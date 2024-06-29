using UnityEngine;


public class MoveToState : BaseState
{
    [Header("Children")]
    [SerializeField] BaseState MoveState;

    [SerializeField] Vector2 targetLocation;
    float x;

    private void Start()
    {
        stateName = "MoveToState";
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void ProcessTick()
    {
        base.ProcessTick();
        targetLocation.y = t.position.y;

        targetLocation.x = c.target.position.x;

        if (Vector2.Distance(t.position, targetLocation) < 3)
        {
            Exit();
        }
    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();
            
        x = t.position.x > targetLocation.x ? -1 : 1;

    }

    public override void PhysicsTick()
    {
        base.PhysicsTick();

        if (Vector2.Distance(t.position, targetLocation) > 3)
            rb.AddForce(new Vector2(x, 0) * p.fMoveForce);

        // apply friction
        Vector2 currentVel = rb.velocity;
        currentVel.x = currentVel.x * p.fGroundFriction;
        if (Mathf.Abs(currentVel.x) < 0.1) currentVel.x = 0;
        rb.velocity = currentVel;
    }
}
