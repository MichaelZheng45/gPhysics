using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
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
    public CollisionHull2D other;

    void Start()
    {
        particle = GetComponent<particle2D>();
    }

    private void Update()
    {
        if(other.type == CollisionHullType2D.hull_obb)
        {
            if(TestCollisionVsOBB((ObjectBoundingBoxHull2D)other))
            {
                GetComponent<MeshRenderer>().material = green;
            }
            else
            {
                GetComponent<MeshRenderer>().material = red;
            }
        }
        else if (other.type == CollisionHullType2D.hull_aabb)
        {
            if (TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)other))
            {
                GetComponent<MeshRenderer>().material = green;
            }
            else
            {
                GetComponent<MeshRenderer>().material = red;
            }
        }
        else
        {
            if (TestCollisionVsCircle((CircleHull2D)other))
            {
                GetComponent<MeshRenderer>().material = green;
            }
            else
            {
                GetComponent<MeshRenderer>().material = red;
            }
        }
    }

    public static bool TestCollision(CollisionHullType2D a, CollisionHullType2D b)
    {

        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D other);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other);

    protected Vector2 rotatePoint(Vector2 point, float rotation)
    {
        //get matrix and rotate it 4 times
        //http://polymathprogrammer.com/2008/09/01/cartesian-coordinates-and-transformation-matrices/
        //[cos(0)  -sin(0)]  [x]  = xCos(0) - ySin(0)
        //[sin(0)  cos(0)]   [y] = xSin(0) + yCos(0)
        float rad = rotation * Mathf.Deg2Rad;
        float x = point.x * Mathf.Cos(rad) - (point.y * Mathf.Sin(rad));
        float y = point.x * Mathf.Sin(rad) + point.y * Mathf.Cos(rad);
        
        return new Vector2(x, y);
    }
}
