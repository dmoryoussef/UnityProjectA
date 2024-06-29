using UnityEngine;

public class AttackStateA : AttackState
{
    private void Start()
    {
        stateName = "AttackStateA";
        attackDamage = 0.5f;
    }

    public override void Enter()
    {
        base.Enter();

        a.Play("HeroKnight_Attack1");

    }
}
