using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField] public PlayerSO p;
    [SerializeField] public Transform t;
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Animator a;

    [SerializeField] public EntityAIController controller;

    [SerializeField] public BaseState RootState;

    private void Awake()
    {
        
    }

    void Start()
    {
        rb.freezeRotation = true;
        Physics2D.queriesStartInColliders = false;

        p.Init(t);

        RootState.Enter();
    }

    // Update is called once per frame
    void Update()
    {
        p.update(t.position);
        RootState.branchProcess();
        //  DebugCurrentState(RootState);
    }

    private void FixedUpdate()
    {
        RootState.branchPhysics();
    }

    private void DebugCurrentState(BaseState current)
    {
        if (current.current == null)
            Debug.Log(current.stateName);
        else
            DebugCurrentState(current.current);
    }


    public string getCurrentState(BaseState current)
    {
        if (current.current == null)
            return current.stateName;
        else return getCurrentState(current.current);
    }
}


//  TODO
//  bug fixes:
//  amplified jump forces when holding space
//  add exclusive physics function
//  displayed statename incorrect on attack3?

//  feature additions:
//  actual damage when attacking
//  enemy target demo

