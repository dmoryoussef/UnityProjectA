using UnityEngine;

public class DamageController : MonoBehaviour
{
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            AttackState state = collision.gameObject.GetComponent<AttackState>();
            if (state != null)
            {
                rb.AddForce(state.attackVector());
            }
        }
        
    }
}
