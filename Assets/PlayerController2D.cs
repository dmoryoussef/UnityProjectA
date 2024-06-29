using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class PlayerController2D : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] public PlayerSO playerSO;

    [Header("Physics")]
    public bool bColliding;

    [Header("X-Axis")]
    public int nHorizontalMove;
    public bool bLeftWallGrab;
    public bool bRightWallGrab;
    public float currentSpeed;

    [Header("Y-Axis")]
    [SerializeField] bool bJumping;         //  current frames if jump force is being applied
    [SerializeField] bool bGrounded;        //  is on the ground - can jump, false if falling or jumping
    public float fJumptime;
   
    private List<Ray> UpRays;
    private List<Ray> DownRays;
    private List<Ray> LeftRays;
    private List<Ray> RightRays;

    private void Awake()
    {
        Physics2D.queriesStartInColliders = false;  //  for raycasting inside player collider2d, move to game manager script eventually

        Debug.Log("Loading " + gameObject.name);
       
        if (!GetComponent<Rigidbody2D>())
        {
            Debug.Log("Adding Rigidbody2D...");
            gameObject.AddComponent<Rigidbody2D>();
        }
        
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;                   //  prevent rotation of rigidbody
        rb.gravityScale = 1f;

        if (!GetComponent<BoxCollider2D>())
        {
            Debug.Log("Adding BoxCollider2D...");
            gameObject.AddComponent<BoxCollider2D>();
        }
    }                      
    private void Start()
    {
        DownRays = new List<Ray>();
        DownRays.Add(new Ray(new Vector2(transform.localScale.x / 2, 0), Vector2.down, 0.6f));
        DownRays.Add(new Ray(new Vector2(-transform.localScale.x / 2, 0), Vector2.down, 0.6f)); 
        DownRays.Add(new Ray(Vector2.zero, Vector2.down, 0.6f));

        LeftRays = new List<Ray>();
        LeftRays.Add(new Ray(Vector2.zero, Vector2.left, 0.6f));
        LeftRays.Add(new Ray(new Vector2(0, transform.localScale.y / 2), Vector2.left, 0.6f));
        LeftRays.Add(new Ray(new Vector2(0, -transform.localScale.y / 2), Vector2.left, 0.6f));


        RightRays = new List<Ray>();
        RightRays.Add(new Ray(Vector2.zero, Vector2.right, 0.6f));
        RightRays.Add(new Ray(new Vector2(0, transform.localScale.y / 2), Vector2.right, 0.6f));
        RightRays.Add(new Ray(new Vector2(0, -transform.localScale.y / 2), Vector2.right, 0.6f));

        UpRays = new List<Ray>();
        UpRays.Add(new Ray(new Vector2(transform.localScale.x / 2, 0), Vector2.up, 0.6f));
        UpRays.Add(new Ray(new Vector2(-transform.localScale.x / 2, 0), Vector2.up, 0.6f));
        UpRays.Add(new Ray(Vector2.zero, Vector2.up, 0.6f));

    }
    void updateOrientation()
    {
        if (rb.velocity.x > 0 && transform.localScale.x < 0)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

        if (rb.velocity.x < 0 && transform.localScale.x > 0)
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
    }

    void updateInput()
    {
        if (Input.GetKey(KeyCode.Escape))
            //  Application.Quit();
            EditorApplication.isPlaying = false;

        nHorizontalMove = (int)Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) && 
           (bGrounded || bLeftWallGrab || bRightWallGrab))
        {
            // start jump force timer
            fJumptime = 0;
            bJumping = true;
        }

        if (bJumping)
        {
            fJumptime += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) | (fJumptime > playerSO.fJumpMax))
        {
            bJumping = false;
        }
    }

    void handleRaycasts()
    {
        bGrounded = false;
        foreach (Ray ray in DownRays)
        {
            RaycastHit2D cast = ray.castRay(transform.position);
            if (cast.collider) bGrounded = true;
        }

        bLeftWallGrab = false;
        foreach(Ray ray in LeftRays)
        {
            RaycastHit2D cast = ray.castRay(transform.position);
            if (cast.collider) bLeftWallGrab = true;
        }

        bRightWallGrab = false;
        foreach (Ray ray in RightRays)
        {
            RaycastHit2D cast = ray.castRay(transform.position);
            if (cast.collider) bRightWallGrab = true;
        }

        foreach (Ray ray in UpRays)
        {
            RaycastHit2D cast = ray.castRay(transform.position);
           // if (cast.collider) bRightWallGrab = true;
        }
    }

    private void Update()
    {
        updateInput();
        updateOrientation();
    }

    private void FixedUpdate()
    {
        handleRaycasts();

        //  Y-Axis Stuff
        if (rb.velocity.y < 0)              
            rb.gravityScale = playerSO.fFastFallScale;
        else
            rb.gravityScale = playerSO.fDefaultGravity;

        if (rb.velocity.y < -playerSO.fMaxFallSpeed)
            rb.velocity = new Vector2(rb.velocity.x, -playerSO.fMaxFallSpeed);
       
        if (bJumping)
        {
            if (!bLeftWallGrab && !bRightWallGrab)
                //rb.velocity = rb.velocity + (Vector2.up * fJumpForce);
                rb.AddForce(Vector2.up * playerSO.fJumpForce, ForceMode2D.Impulse);
            
            if (bLeftWallGrab)
                //rb.velocity = rb.velocity + (Vector2.up * playerSO.fWallJumpUpForce) + (Vector2.right * playerSO.fWallJumpSideForce);
                rb.AddForce(Vector2.up * playerSO.fWallJumpUpForce + (Vector2.right * playerSO.fWallJumpSideForce), ForceMode2D.Impulse);

            if (bRightWallGrab)
                //rb.velocity = rb.velocity + (Vector2.up * playerSO.fWallJumpUpForce) + (Vector2.left * playerSO.fWallJumpSideForce);
                rb.AddForce(Vector2.up * playerSO.fWallJumpUpForce + (Vector2.left * playerSO.fWallJumpSideForce), ForceMode2D.Impulse);

        }

        //  X-Axis Stuff
        currentSpeed = Mathf.Abs(rb.velocity.x);
        if (currentSpeed < playerSO.fMaxSpeed) 
            rb.AddForce(new Vector2(nHorizontalMove, 0) * playerSO.fMoveForce);

        //  Handle Friction
        Vector2 currentVel = rb.velocity;
        if ((bLeftWallGrab || bRightWallGrab) && !bGrounded)
        {
            currentVel.y = currentVel.y * playerSO.fWallGrabFriction;
            rb.velocity = currentVel;
        }
        else
        {
            currentVel.x = currentVel.x * playerSO.fGroundFriction;
            if (Mathf.Abs(currentVel.x) < 0.1) currentVel.x = 0;
            rb.velocity = currentVel;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bColliding = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        bColliding = false;
    } 

    private void OnDrawGizmos()
    {
        if (DownRays != null && LeftRays != null && UpRays != null && RightRays != null)
        {
            foreach (Ray r in DownRays)
            {
                r.DebugDraw(transform.position, r.hasCollided() ? Color.red : Color.green);
            }
            foreach (Ray r in LeftRays)
            {
                r.DebugDraw(transform.position, r.hasCollided() ? Color.red : Color.green);
            }
            foreach (Ray r in RightRays)
            {
                r.DebugDraw(transform.position, r.hasCollided() ? Color.red : Color.green);
            }
            foreach (Ray r in UpRays)
            {
                r.DebugDraw(transform.position, r.hasCollided() ? Color.red : Color.green);
            }
        }
    }
}
