using System;
using UnityEngine;

public class AttackState : BaseState
{
    [Header("Child States")]
    [SerializeField] BaseState NextAttackState;

    [SerializeField] string AttackAnimation;

    protected CircleCollider2D hitBox;
    protected float attackDamage;

    protected float duration;

    public override void Enter()
    {
        base.Enter();
        a.speed = 1.0f;
        duration = 0.0f;
    }

    public override void ProcessTick()
    {
        base.ProcessTick();
        duration += Time.deltaTime;

        if (current?.done == true)
            Exit();

        if (a.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            Exit();
        
        if (duration >= a.GetCurrentAnimatorStateInfo(0).length)
        {
            if (hitBox != null)
            {
                deleteHitBox();
            }
        }
    }

    public override void ProcessExclusiveTick()
    {
        base.ProcessExclusiveTick();

        // chain to next attack animation, if input is at specific window
        if (Input.GetMouseButtonDown(0))
        {
            if (a.GetCurrentAnimatorStateInfo(0).normalizedTime > p.fAttackWindow)
            {
                if (NextAttackState != null)
                {
                    deleteHitBox();
                    current = NextAttackState;
                    NextAttackState.Enter();
                }
            }

            //else
            //    Exit();   // cant delete collider
        }

        //  create hit box that does damage to other entities here:
        if ((hitBox == null) && (a.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2f) && (a.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.9f))
        {
            createHitBox();
        }

      
    }

    private void createHitBox()
    {
        hitBox = gameObject.AddComponent<CircleCollider2D>();
        hitBox.offset = new Vector2(1, 0.4f);
        hitBox.isTrigger = true;
    }

    private void deleteHitBox()
    {
        if (hitBox != null)
        {
            hitBox.enabled = false;
            Destroy(hitBox);
            Destroy(GetComponent<CircleCollider2D>());
            hitBox = null;
        }
    }

    public override void Exit()
    {
        base.Exit();
        deleteHitBox();
    }

    public Vector2 attackVector()
    {
        return direction() * attackDamage * 100;
    }


    public Vector2 direction()
    {
        return new Vector2(t.localScale.x, 0);
    }

    public float getDamage()
    {
        return attackDamage;
    }
}
