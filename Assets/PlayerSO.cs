using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "PlayerSO")]
public class PlayerSO : ScriptableObject
{
    [Header("Physics")]
    [SerializeField] public float fGroundFriction = 0.95f;
    [SerializeField] public float fWallGrabFriction = 0.5f;
    [Header("Horizontal")]
    [SerializeField] public float fMoveForce = 30f;
    [SerializeField] public float fMaxSpeed = 10f;
    [Header("Vertical")]
    [SerializeField] public float fJumpForce = 1.5f;
    [SerializeField] public float fJumpMax = .2f;
    [SerializeField] public float fFastFallScale = 5f;
    [SerializeField] public float fDefaultGravity = 3f;
    [SerializeField] public float fMaxFallSpeed = 20f;
    [SerializeField] public float fNotGroundedHorizontalForce = 0.6f;

    [Header("Wall Grab")]
    [SerializeField] public float fWallJumpUpForce = 8f;
    [SerializeField] public float fWallJumpSideForce = 10f;

    public bool bGrounded;
    public bool bLeftWallGrab;
    public bool bRightWallGrab;

    [Header("Attack")]
    [SerializeField] public float fAttackWindow;

    [Header("Debugging")]
    [SerializeField] public bool showRays;
    [SerializeField] public bool stateChanges;

    private List<Ray> UpRays;
    private List<Ray> DownRays;
    private List<Ray> LeftRays;
    private List<Ray> RightRays;

    public void Init(Transform t)
    {
        DownRays = new List<Ray>();
        DownRays.Add(new Ray(new Vector2(t.localScale.x / 2, 0), Vector2.down, 0.6f));
        DownRays.Add(new Ray(new Vector2(-t.localScale.x / 2, 0), Vector2.down, 0.6f));
        DownRays.Add(new Ray(Vector2.zero, Vector2.down, 0.6f));

        LeftRays = new List<Ray>();
        LeftRays.Add(new Ray(Vector2.zero, Vector2.left, 0.6f));
        LeftRays.Add(new Ray(new Vector2(0, t.localScale.y / 2), Vector2.left, 0.6f));
        LeftRays.Add(new Ray(new Vector2(0, -t.localScale.y / 2), Vector2.left, 0.6f));


        RightRays = new List<Ray>();
        RightRays.Add(new Ray(Vector2.zero, Vector2.right, 0.6f));
        RightRays.Add(new Ray(new Vector2(0, t.localScale.y / 2), Vector2.right, 0.6f));
        RightRays.Add(new Ray(new Vector2(0, -t.localScale.y / 2), Vector2.right, 0.6f));

        UpRays = new List<Ray>();
        UpRays.Add(new Ray(new Vector2(t.localScale.x / 2, 0), Vector2.up, 0.6f));
        UpRays.Add(new Ray(new Vector2(-t.localScale.x / 2, 0), Vector2.up, 0.6f));
        UpRays.Add(new Ray(Vector2.zero, Vector2.up, 0.6f));
    }

    public void update(Vector2 p)
    {
        handleRaycasts(p);
        if (showRays)
            DebugDrawRays(p);
    }

    void handleRaycasts(Vector2 p)
    {
        bGrounded = false;
        foreach (Ray ray in DownRays)
        {
            RaycastHit2D cast = ray.castRay(p);
            if (cast.collider) bGrounded = true;
        }

        bLeftWallGrab = false;
        foreach (Ray ray in LeftRays)
        {
            RaycastHit2D cast = ray.castRay(p);
            if (cast.collider) bLeftWallGrab = true;
        }

        bRightWallGrab = false;
        foreach (Ray ray in RightRays)
        {
            RaycastHit2D cast = ray.castRay(p);
            if (cast.collider) bRightWallGrab = true;
        }

        foreach (Ray ray in UpRays)
        {
            RaycastHit2D cast = ray.castRay(p);
        }
    }


    void DebugDrawRays(Vector2 p)
    {
        foreach (Ray r in DownRays)
        {
            r.DebugDraw(p, r.hasCollided() ? Color.red : Color.green);
        }
        foreach (Ray r in LeftRays)
        {
            r.DebugDraw(p, r.hasCollided() ? Color.red : Color.green);
        }
        foreach (Ray r in RightRays)
        {
            r.DebugDraw(p, r.hasCollided() ? Color.red : Color.green);
        }
        foreach (Ray r in UpRays)
        {
            r.DebugDraw(p, r.hasCollided() ? Color.red : Color.green);
        }
    }
}
