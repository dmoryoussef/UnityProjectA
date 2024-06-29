using UnityEngine;

public class BlockState : BaseState
{
    private void Start()
    {
        stateName = "BlockState";
    }

    public override void Enter()
    {
        base.Enter();

        a.Play("HeroKnight_BlockIdle");

    }

    public override void ProcessTick()
    {
        base.ProcessTick();

        if (Input.GetMouseButtonUp(1))
        {
            Exit();
        }
    }
}
