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
        Vector2 thisPos = particle.position, 
                otherPos = other.getParticle().position;
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
        //Check if maximum extent is greater than the others minimum extent FOR ALL AXIIS
        //1. create max extents of this and other particle
        //2. max = (center.x + length/2, center.y + height/2)
        //3. create min extents of this and other particle
        //4. min = (center.x - length/2, center.y - height/2)
        //5. check if this_max.x > other_min.x and this_max.y > other_min.y
        //6. check if other_max.x > this_min.x and other_max.y > this_min.y
        //7. if both checks is true then it passes

        Vector2 thisMax, thisMin, otherMax, otherMin;
        float thisLength = length * .5f;
        float thisHeight = height * .5f;
        float otherLength = other.length * .5f;
        float otherHeight = other.height * .5f;

        thisMax = new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        otherMax = new Vector2(other.particle.position.x + otherLength, other.particle.position.y + otherHeight);
        thisMin = new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);
        otherMin = new Vector2(other.particle.position.x - otherLength, other.particle.position.y - otherHeight);

        bool check1 = (thisMax.x >= otherMin.x && thisMax.y >= otherMax.y);
        bool check2 = (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y);

        if(check1 && check2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other)
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
        thisMax = new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        thisMin = new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);

        //find max and min of other
        //get all corner points and then rotate it
        p1 = rotatePoint(new Vector2(otherLength, otherHeight), other.rotation);
        p2 = rotatePoint(new Vector2(otherLength, -otherHeight), other.rotation);
        p3 = rotatePoint(new Vector2(-otherLength, -otherHeight), other.rotation);
        p4 = rotatePoint(new Vector2(-otherLength, otherHeight), other.rotation);

        //find max of all points
        otherMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x) + otherPosition.x, Mathf.Max(p1.y, p2.y, p3.y, p4.y) + otherPosition.y);
        otherMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x) + otherPosition.x, Mathf.Min(p1.y, p2.y, p3.y, p4.y) + otherPosition.y);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMax.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check1 = true;
        }
        else
        {
            check1 = false;
        }

        p1 = other.transform.localToWorldMatrix.inverse * new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        p2 = other.transform.localToWorldMatrix.inverse * new Vector2(particle.position.x + thisLength, particle.position.y - thisHeight);
        p3 = other.transform.localToWorldMatrix.inverse * new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);
        p4 = other.transform.localToWorldMatrix.inverse * new Vector2(particle.position.x - thisLength, particle.position.y + thisHeight);

        thisMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        thisMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        otherMax = new Vector2(otherPosition.x + otherLength, otherPosition.y + otherHeight);
        otherMin = new Vector2(otherPosition.x - otherLength, otherPosition.y - otherHeight);
        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMax.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check2 = true;
        }
        else
        {
            check2 = false;
        }

        if(check1 && check2)
        {
            return true;
        }
        return false;
    }

}