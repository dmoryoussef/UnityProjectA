using UnityEngine;

public class IdleAiState : BaseState
{
    [SerializeField] BaseState MoveToState;

    private void Start()
    {
        stateName = "Base";
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void ProcessTick()
    {
        base.ProcessTick();

    }

    public override void CheckNewState()
    {
        current = MoveToState;
        MoveToState.Enter();
    }
}
