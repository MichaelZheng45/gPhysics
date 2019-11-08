﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisAlignedBoundingBoxHull3D : CollisionHull3D
{

    // Start is called before the first frame update
    public AxisAlignedBoundingBoxHull3D() : base(CollisionHullType3D.hull_aabb) { }

    public float length, height, width;

    private void Update()
    {
    }

    public override bool TestCollisionVsCircle(CircleHull3D other, ref Collision3D c)
    {
        if(other.TestCollisionVsAABB(this,ref c))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision3D c)
    {
        //Check if maximum extent is greater than the others minimum extent FOR ALL AXIIS
        //1. create max extents of this and other particle
        //2. max = (center.x + length/2, center.y + height/2)
        //3. create min extents of this and other particle
        //4. min = (center.x - length/2, center.y - height/2)
        //5. check if this_max.x > other_min.x and this_max.y > other_min.y
        //6. check if other_max.x > this_min.x and other_max.y > this_min.y
        //7. if both checks is true then it passes

        Vector3 thisMax, thisMin, otherMax, otherMin;
        float thisLength = length * .5f;
        float thisHeight = height * .5f;
        float thisWidth = width * .5f;
        float otherLength = other.length * .5f;
        float otherHeight = other.height * .5f;
        float otherWidth = other.width * .5f;

        thisMax = new Vector3(particle.position.x + thisLength, particle.position.y + thisHeight, particle.position.z + thisWidth);
        otherMax = new Vector3(other.particle.position.x + otherLength, other.particle.position.y + otherHeight, other.particle.position.z + otherWidth);
        thisMin = new Vector3(particle.position.x - thisLength, particle.position.y - thisHeight, particle.position.z - thisWidth);
        otherMin = new Vector3(other.particle.position.x - otherLength, other.particle.position.y - otherHeight, other.particle.position.z - otherWidth);

        bool check1 = (thisMax.x >= otherMin.x && thisMax.y >= otherMin.y && thisMax.z >= otherMin.z);
        bool check2 = (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y && otherMax.z >= thisMin.z);

        if(check1 && check2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision3D c)
    {
        //do a vs AABB test for both objects rotated in each others space
        //if both pass, then there is a collision, otherwise no
        //first find max extents of OOB, perform aabb vs this
        //then, transform this into obb space, find max extents, aabb test again

        //1. create max and min extents of this particle
        //2. max = (center.x + length/2, center.y + height/2)
        //3. min = (center.x - length/2, center.y - height/2)
        //4. create max and min extents of other particle
        //4a. find all corner points of the box using length, height
        //4b. rotate all points around its rotation https://www.gamefromscratch.com/post/2012/11/24/GameDev-math-recipes-Rotating-one-point-around-another-point.aspx
        //4c. min = (min(x of all points), min(y of all points)) https://stackoverflow.com/questions/3231176/how-to-get-size-of-a-rotated-rectangle
        //4d. max = (max(x of all points), max(y of all points)
        //5. check if this_max.x > other_min.x and this_max.y > other_min.y
        //6. check if other_max.x > this_min.x and other_max.y > this_min.y
        //7. transform this center around the other's center using its transform matrix inverse
        //8. transform each point of the aab by the other's transform matrix inverse
        //8. find max and min extents of this by using Max(all points), Min(all points)
        //5. check if new this_max.x > other_min.x and new this_max.y > other_min.y
        //6. check if other_max.x > new this_min.x and other_max.y > new this_min.y
        //9. if all checks are true, then collision check passes

        bool check1, check2;
        Vector3 thisMax, thisMin, otherMax, otherMin;
        Vector3 p1, p2, p3, p4, p5, p6, p7, p8;

        Vector3 otherPosition = other.getParticle().position;
        float thisLength = length * .5f;
        float thisHeight = height * .5f;
        float thisWidth = width * .5f;
        float otherLength = other.length * .5f;
        float otherHeight = other.height * .5f;
        float otherWidth = other.width * .5f;
        //max and min of this position
        thisMax = new Vector3(particle.position.x + thisLength, particle.position.y + thisHeight, particle.position.z + thisWidth);
        thisMin = new Vector3(particle.position.x - thisLength, particle.position.y - thisHeight, particle.position.z - thisWidth);

        //find max and min of other
        //get all corner points and then rotate it
        p1 = particle.getTransformMatrixInv() * new Vector3(otherLength, otherHeight, otherWidth);
        p2 = particle.getTransformMatrixInv() * new Vector3(otherLength, otherHeight, -otherWidth);
        p3 = particle.getTransformMatrixInv() * new Vector3(otherLength, -otherHeight, otherWidth);
        p4 = particle.getTransformMatrixInv() * new Vector3(otherLength, -otherHeight, -otherWidth);
        p5 = particle.getTransformMatrixInv() * new Vector3(-otherLength, otherHeight, otherWidth);
        p6 = particle.getTransformMatrixInv() * new Vector3(-otherLength, otherHeight, -otherWidth);
        p7 = particle.getTransformMatrixInv() * new Vector3(-otherLength, -otherHeight, otherWidth);
        p8 = particle.getTransformMatrixInv() * new Vector3(-otherLength, -otherHeight, -otherWidth);

        //find max of all points
        otherMax = new Vector3(Mathf.Max(p1.x, p2.x, p3.x, p4.x, p5.x, p6.x, p7.x, p8.x) + otherPosition.x, Mathf.Max(p1.y, p2.y, p3.y, p4.y, p5.y, p6.y, p7.y, p8.y) + otherPosition.y, Mathf.Max(p1.z, p2.z, p3.z, p4.z, p5.z, p6.z, p7.z, p8.z) + otherPosition.z);
        otherMin = new Vector3(Mathf.Min(p1.x, p2.x, p3.x, p4.x, p5.x, p6.x, p7.x, p8.x) + otherPosition.x, Mathf.Min(p1.y, p2.y, p3.y, p4.y, p5.y, p6.y, p7.y, p8.y) + otherPosition.y, Mathf.Min(p1.z, p2.z, p3.z, p4.z, p5.z, p6.z, p7.z, p8.z) + otherPosition.z);

        //do check
        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMin.y && thisMax.z >= otherMin.z) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y && otherMax.z >= thisMin.z))
        {
            check1 = true;
        }
        else
        {
            check1 = false;
        }
        
        //for each corner, move it relative to the box then transform by the world matrix inverse. Finally add position back
        p1 = other.transform.localToWorldMatrix.inverse * (new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight) - otherPosition);
        p2 = other.transform.localToWorldMatrix.inverse * (new Vector2(particle.position.x + thisLength, particle.position.y - thisHeight) - otherPosition);
        p3 = other.transform.localToWorldMatrix.inverse * (new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight) - otherPosition);
        p4 = other.transform.localToWorldMatrix.inverse * (new Vector2(particle.position.x - thisLength, particle.position.y + thisHeight) - otherPosition);
        p1 += otherPosition;
        p2 += otherPosition;
        p3 += otherPosition;
        p4 += otherPosition;
        
        //get the extremes for min and max
        thisMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        thisMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        otherMax = new Vector2(otherPosition.x + otherLength, otherPosition.y + otherHeight);
        otherMin = new Vector2(otherPosition.x - otherLength, otherPosition.y - otherHeight);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMin.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check2 = true;
        }
        else
        {
            check2 = false;
        }
        if (check1 && check2)
        {
            return true;
        }
        return false;
    }

}