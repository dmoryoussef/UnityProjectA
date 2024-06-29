using UnityEngine;

public class SlideState : BaseState
{
    float x;

    public override void Enter()
    {
        base.Enter();

        a.Play("SlideDust");
    }

    public override void ProcessTick()
    {
        base.ProcessTick();

        x = Input.GetAxisRaw("Horizontal");

        if (rb.velocity.x < 0 && x < 0)
            done = true;
    }
}
