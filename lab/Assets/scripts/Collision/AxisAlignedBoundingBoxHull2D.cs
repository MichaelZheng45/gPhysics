using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxHull2D : CollisionHull2D
{

    // Start is called before the first frame update
    public AxisAlignedBoundingBoxHull2D() : base(CollisionHullType2D.hull_aabb) { }

    public float length, height;
   

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override bool TestCollisionVsCircle(CircleHull2D other)
    {
        //See circle
        return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
    {
        //Check if maximum extent is greater than the others minimum extent FOR ALL AXIIS
        //passes is so

        return false;
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        //do a vs AABB test for both objects rotated in each others space
        //if both pass, then there is a collision, otherwise no

        //first find max extents of OOB, perform aabb vs this
        //then, transform this into obb space, find max extents, aabb test again

        return false;
    }
}