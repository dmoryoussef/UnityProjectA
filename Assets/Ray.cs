using UnityEngine;

public class Ray
{
    Vector2 position;
    Vector2 direction;
    float length;
    bool collided;

    public Ray(Vector2 pos, Vector2 dir, float len)
    {
        position = pos;
        direction = dir;
        length = len;
        collided = false;
    }

    public bool hasCollided()
    {
        return collided;
    }

    public RaycastHit2D castRay(Vector2 pos)
    {
        RaycastHit2D cast = Physics2D.Raycast(position + pos, direction, length);
        collided = cast.collider;
        return cast;
    }

    public void DebugDraw(Vector2 pos, Color c)
    {
        Debug.DrawLine(position + pos, position + pos + direction * length, c);
    }
}
