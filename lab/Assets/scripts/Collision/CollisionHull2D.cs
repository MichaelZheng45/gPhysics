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
    private CollisionHullType2D type { get; }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        type = type_set;
    }

    protected particle2D particle { get; set; }

    public particle2D getParticle()
    {
        return particle;
    }

    void Start()
    {
        particle = GetComponent<particle2D>();
    }

    public static bool TestCollision(CollisionHullType2D a, CollisionHullType2D b)
    {

        return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D other);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other);

}
