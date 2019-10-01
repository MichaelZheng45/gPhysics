using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float restitution;
        }

        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        public Vector2 closingVelocity;
    }

    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        type = type_set;
    }

    private CollisionHullType2D type { get; }
    public CollisionHullType2D getTypeHull()
    {
        return type;
    }

    protected particle2D particle { get; set; }
    public particle2D getParticle()
    {
        return particle;
    }

    public Material red;
    public Material green;

    void Start()
    {
        particle = GetComponent<particle2D>();
    }

    private void Update()
    {
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
        if(b.type == CollisionHullType2D.hull_circle)
        {
            a.TestCollisionVsCircle((CircleHull2D)b,ref c);
        }
        else if(b.type == CollisionHullType2D.hull_aabb)
        {
            a.TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b, ref c);
        }
        else if (b.type == CollisionHullType2D.hull_obb)
        {
            a.TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref c);
        }
            return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D other, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c);

    protected Vector2 rotatePoint(Vector2 point, float rotation)
    {
        //http://polymathprogrammer.com/2008/09/01/cartesian-coordinates-and-transformation-matrices/
        //[cos(0)  -sin(0)]  [x]  = xCos(0) - ySin(0)
        //[sin(0)  cos(0)]   [y] = xSin(0) + yCos(0)
        float rad = rotation * Mathf.Deg2Rad;
        float x = point.x * Mathf.Cos(rad) - (point.y * Mathf.Sin(rad));
        float y = point.x * Mathf.Sin(rad) + point.y * Mathf.Cos(rad);

        return new Vector2(x, y);
    }
}
