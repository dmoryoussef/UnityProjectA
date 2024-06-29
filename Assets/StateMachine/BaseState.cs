using UnityEngine;



public class BaseState : MonoBehaviour
{
    protected PlayerSO p;
    protected Transform t;
    protected Rigidbody2D rb;
    protected Animator a;

    protected EntityAIController c;

    public string stateName;
    public BaseState current;
    public bool done;

    private void Awake()
    {
        rb = transform.root.GetComponent<Rigidbody2D>();
        t = transform.root.GetComponent<Transform>();
        a = transform.root.GetComponentInChildren<Animator>();
        p = t.root.GetComponentInChildren<StateMachine>().p;
        c = t.root.GetComponentInChildren<EntityAIController>();
    }

    private void Start()
    {
        start();
        done = true;                                                             
    }

    public virtual void start()
    {
        //  run once at start
    }

    public virtual void ProcessTick() 
    {
        //  handle this AND current child
    }             

    public virtual void ProcessExclusiveTick()
    {
        // handle ONLY this when current child is null

    }

    public virtual void PhysicsTick() { }

    public virtual void CheckNewState() { }

    public virtual void Enter()  
    {
        done = false;
        current = null;

        a.speed = 1.0f;
        if (p.stateChanges)
            Debug.Log("Enter " + stateName);
    }

    public virtual void Exit()
    {
        done = true;
        if (p.stateChanges)
            Debug.Log("Exit " + stateName);
    }

    public void branchProcess() 
    {
        if (current)
            current.branchProcess();
        else
        {
            CheckNewState();
            if (!current)
                ProcessExclusiveTick();
        }
       
        ProcessTick();
    }

    public void branchPhysics() 
    {
        current?.branchPhysics();
        PhysicsTick();
    }
    
    protected void updateOrientation(float x)
    {
        if (t.localScale.x < 0 && x > 0) t.localScale = new Vector2(1, 1);
        if (t.localScale.x > 0 && x < 0) t.localScale = new Vector2(-1, 1);
    }


}
