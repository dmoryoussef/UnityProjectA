using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderScript : MonoBehaviour
{
    [SerializeField] bool bTouchingLadder;
    [SerializeField] bool bClimbing;

    [SerializeField] Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (bTouchingLadder)
        {
            if (Input.GetKey(KeyCode.W))
                bClimbing = true;
            else
                bClimbing = false;
        }
    }

    private void FixedUpdate()
    {
        //if (bClimbing)
        //{
        //    rb.gravityScale = 0;
        //    rb.AddForce(Vector2.up * 20);
        //}
        //else
        //{
        //    rb.gravityScale = 1;    
        //}
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            bTouchingLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            bTouchingLadder = false;
            bClimbing = false;
        }
    }
}
