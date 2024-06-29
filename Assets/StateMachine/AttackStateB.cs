using UnityEngine;

public class AttackStateB : AttackState
{
    private void Start()
    {
        stateName = "AttackStateB";
        attackDamage = 1.0f;
    }

    public override void Enter()
    {
        base.Enter();
        a.Play("HeroKnight_Attack2");

    }
}
