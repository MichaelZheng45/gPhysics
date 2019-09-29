﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull2D : CollisionHull2D
{

    public ObjectBoundingBoxHull2D() : base(CollisionHullType2D.hull_obb) { }

    public float height, length, rotation;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        rotation = transform.eulerAngles.z;
        Debug.Log("doing");
        if (other.getTypeHull() == CollisionHullType2D.hull_obb)
        {
            if (TestCollisionVsOBB((ObjectBoundingBoxHull2D)other))
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
            if (TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)other))
            {
                GetComponent<MeshRenderer>().material = green;
            }
            else
            {
                GetComponent<MeshRenderer>().material = red;
            }
        }
    }

    public override bool TestCollisionVsCircle(CircleHull2D other)
    {
        Vector2 thisPos, otherPos;
        //move circle center into box's space by multiplying by its world transform inverse
        Vector4 rotatedPos = other.transform.localToWorldMatrix.inverse * new Vector4(particle.position.x, particle.position.y, 0, 0);
        otherPos = new Vector2(rotatedPos.x, rotatedPos.y);
        thisPos = other.getParticle().position;

        //find the closest point of on the rectangle to the circle
        float newX = Mathf.Clamp(otherPos.x, thisPos.x - length * 0.5f, thisPos.x + length / 2);
        float newY = Mathf.Clamp(otherPos.y, thisPos.y - height * 0.5f, thisPos.x + height / 2);
        Vector2 closestPoint = new Vector2(newX, newY);

        //get "radius" of this ractangle
        Vector2 rectangleDiff = thisPos - closestPoint;
        float rectangleToClosest = Vector2.Dot(rectangleDiff, rectangleDiff);

        //calculate distance between particles
        Vector2 objDiff = thisPos - otherPos;
        float particleDistance = Vector2.Dot(rectangleDiff, rectangleDiff);

        //find the sum radii
        float sumRadii = other.radius + Mathf.Sqrt(rectangleToClosest);
        sumRadii *= sumRadii;

        //compare
        if (particleDistance <= sumRadii)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other)
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
        Vector2 thisMax, thisMin, otherMax, otherMin;
        Vector2 p1, p2, p3, p4;

        Vector2 otherPosition = other.getParticle().position;
        float thisLength = length * .5f;
        float thisHeight = height * .5f;
        float otherLength = other.length * .5f;
        float otherHeight = other.height * .5f;

        //max and min of this position
        otherMax = new Vector2(otherPosition.x + otherLength, otherPosition.y + otherHeight);
        otherMin = new Vector2(otherPosition.x - otherLength, otherPosition.y - otherHeight);

        //find max and min of other
        //get all corner points and then rotate it
        p1 = rotatePoint(new Vector2(thisLength, thisHeight), rotation);
        p2 = rotatePoint(new Vector2(thisLength, -thisHeight), rotation);
        p3 = rotatePoint(new Vector2(-thisLength, -thisHeight), rotation);
        p4 = rotatePoint(new Vector2(-thisLength, thisHeight), rotation);

        //find max of all points
        thisMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x) + particle.position.x, Mathf.Max(p1.y, p2.y, p3.y, p4.y) + particle.position.y);
        thisMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x) + particle.position.x, Mathf.Min(p1.y, p2.y, p3.y, p4.y) + particle.position.y);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMax.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check1 = true;
        }
        else
        {
            check1 = false;
        }

        p1 = transform.localToWorldMatrix.inverse * new Vector2(otherPosition.x + otherLength, otherPosition.y + otherHeight);
        p2 = transform.localToWorldMatrix.inverse * new Vector2(otherPosition.x + otherLength, otherPosition.y - otherHeight);
        p3 = transform.localToWorldMatrix.inverse * new Vector2(otherPosition.x - otherLength, otherPosition.y - otherHeight);
        p4 = transform.localToWorldMatrix.inverse * new Vector2(otherPosition.x - otherLength, otherPosition.y + otherHeight);

        otherMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        otherMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        thisMax = new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        thisMin = new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);
        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMax.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
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

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
    {
        //Transform this into others space and do AABB vs OBB
        //transform other into this space and do AABB vs OBB
        //If both tests pass, collision occurs otherwise no collision

        //1. do AABB check with this rotated around other
        //2. find all corner points of this box using length and height
        //3. rotate the points of this box with its rotation
        //4. transform the points using the transform inverse of the other
        //5. min = (min(x of all points), min(y of all points))
        //6. max = (max(x of all points), max(y of all points)
        //7. create max and min extents of other particle
        //8. max = (center.x + length/2, center.y + height/2)
        //9. min = (center.x - length/2, center.y - height/2)
        //10. check if this_max.x > other_min.x and this_max.y > other_min.y
        //11. check if other_max.x > this_min.x and other_max.y > this_min.y 
        //12. do AABB check with other rotated around this
        //13. find all corner points of other box using length and height
        //14. rotate the points of other box with its rotation
        //15. transform the points using the transform inverse of this
        //16. min = (min(x of all points), min(y of all points))
        //17. max = (max(x of all points), max(y of all points)
        //18. create max and min extents of this particle
        //19. max = (center.x + length/2, center.y + height/2)
        //20. min = (center.x - length/2, center.y - height/2)
        //21. check if this_max.x > other_min.x and this_max.y > other_min.y
        //22. check if other_max.x > this_min.x and other_max.y > this_min.y 

        bool check1, check2;
        Vector2 thisMax, thisMin, otherMax, otherMin;
        Vector2 p1, p2, p3, p4;

        Vector2 otherPosition = other.getParticle().position;
        float thisLength = length * .5f;
        float thisHeight = height * .5f;
        float otherLength = other.length * .5f;
        float otherHeight = other.height * .5f;

        //get all corner points and then rotate it
        p1 = rotatePoint(new Vector2(thisLength, thisHeight), rotation) + particle.position;
        p2 = rotatePoint(new Vector2(thisLength, -thisHeight), rotation) + particle.position;
        p3 = rotatePoint(new Vector2(-thisLength, -thisHeight), rotation) + particle.position;
        p4 = rotatePoint(new Vector2(-thisLength, thisHeight), rotation) + particle.position;

        p1 = other.transform.localToWorldMatrix.inverse * p1;
        p2 = other.transform.localToWorldMatrix.inverse * p2;
        p3 = other.transform.localToWorldMatrix.inverse * p3;
        p4 = other.transform.localToWorldMatrix.inverse * p4;

        thisMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        thisMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        otherMax = new Vector2(otherPosition.x + otherLength, otherPosition.y + otherHeight);
        otherMin = new Vector2(otherPosition.x - otherLength, otherPosition.y - otherHeight);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMax.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check1 = true;
        }
        else
        {
            check1 = false;
        }

        //get all corner points and then rotate it
        p1 = rotatePoint(new Vector2(otherLength, otherHeight), rotation) + otherPosition;
        p2 = rotatePoint(new Vector2(otherLength, -otherHeight), rotation) + otherPosition;
        p3 = rotatePoint(new Vector2(-otherLength, -otherHeight), rotation) + otherPosition;
        p4 = rotatePoint(new Vector2(-otherLength, otherHeight), rotation) + otherPosition;

        p1 = transform.localToWorldMatrix.inverse * p1;
        p2 = transform.localToWorldMatrix.inverse * p2;
        p3 = transform.localToWorldMatrix.inverse * p3;
        p4 = transform.localToWorldMatrix.inverse * p4;

        otherMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        otherMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        thisMax = new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        thisMin = new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMax.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check2 = true;
        }
        else
        {
            check2 = false;
        }
        Debug.Log("check2: " + check2);
        if (check1 && check2)
        {
            return true;
        }
        return false;
    }
}
