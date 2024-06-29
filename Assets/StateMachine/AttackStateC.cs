using UnityEngine;

public class AttackStateC : AttackState
{
    private void Start()
    {
        stateName = "AttackStateC";
        attackDamage = 3;
    }


    public override void Enter()
    {
        a.Play("HeroKnight_Attack3");
        base.Enter();

    }
}