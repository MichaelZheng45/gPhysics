using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull3D : CollisionHull3D
{

    public ObjectBoundingBoxHull3D() : base(CollisionHullType3D.hull_obb) { }
    
    public float height, length, rotation;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
    }

    public override bool TestCollisionVsCircle(CircleHull3D other, ref Collision3D c)
    {
        Vector3 otherPos;
        //move circle center into box's space by multiplying by its world transform inverse
        otherPos = other.getParticle().position;
        Vector3 rotatedPos = transform.localToWorldMatrix.inverse * (otherPos - particle.position);
        rotatedPos += particle.position;

        //find the closest point of on the rectangle to the circle
        float newX = Mathf.Clamp(rotatedPos.x, particle.position.x - length * .5f, particle.position.x + length * .5f);
        float newY = Mathf.Clamp(rotatedPos.y, particle.position.y - height * .5f, particle.position.y + height * .5f);
        Vector3 closestPoint = new Vector2(newX, newY);

        //act like it is now a circle, calculate "radius"
        Vector3 rectangleDiff = particle.position - closestPoint;
        float rectangleToClosest = Vector2.Dot(rectangleDiff, rectangleDiff);

        //calculate distance between particles
        Vector2 objDiff = rotatedPos - particle.position;
        float particleDistance = Vector2.Dot(objDiff, objDiff);

        //find the sum radii
        float sumRadii = other.radius + Mathf.Sqrt(rectangleToClosest);
        sumRadii *= sumRadii;

        //compare
        if (particleDistance <= sumRadii)
            return true;
        else
            return false;
    }

    public override bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision3D c)
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
        Vector3 p1, p2, p3, p4;

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

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMin.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check1 = true;
        }
        else
        {
            check1 = false;
        }

        //for each corner, move it relative to the box then transform by the world matrix inverse. Finally add position back
        p1 = transform.localToWorldMatrix.inverse * (new Vector3(otherPosition.x + otherLength, otherPosition.y + otherHeight) - particle.position);
        p2 = transform.localToWorldMatrix.inverse * (new Vector3(otherPosition.x + otherLength, otherPosition.y - otherHeight) - particle.position);
        p3 = transform.localToWorldMatrix.inverse * (new Vector3(otherPosition.x - otherLength, otherPosition.y - otherHeight) - particle.position);
        p4 = transform.localToWorldMatrix.inverse * (new Vector3(otherPosition.x - otherLength, otherPosition.y + otherHeight) - particle.position);
        p1 += particle.position;
        p2 += particle.position;
        p3 += particle.position;
        p4 += particle.position;

        //get the extremes for min and max
        otherMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        otherMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        thisMax = new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        thisMin = new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);
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

    public override bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision3D c)
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
        Vector3 p1, p2, p3, p4;

        Vector3 otherPosition = other.getParticle().position;
        float thisLength = length * .5f;
        float thisHeight = height * .5f;
        float otherLength = other.length * .5f;
        float otherHeight = other.height * .5f;

        //get all corner points and then rotate it
        p1 = rotatePoint(new Vector2(thisLength, thisHeight), rotation) + particle.position;
        p2 = rotatePoint(new Vector2(thisLength, -thisHeight), rotation) + particle.position;
        p3 = rotatePoint(new Vector2(-thisLength, -thisHeight), rotation) + particle.position;
        p4 = rotatePoint(new Vector2(-thisLength, thisHeight), rotation) + particle.position;

        //for each corner, move it relative to the box then transform by the world matrix inverse. Finally add position back
        p1 = other.transform.localToWorldMatrix.inverse * (p1 - otherPosition);
        p2 = other.transform.localToWorldMatrix.inverse * (p2 - otherPosition);
        p3 = other.transform.localToWorldMatrix.inverse * (p3 - otherPosition);
        p4 = other.transform.localToWorldMatrix.inverse * (p4 - otherPosition);

        p1 += otherPosition;
        p2 += otherPosition;
        p3 += otherPosition;
        p4 += otherPosition;

        thisMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        thisMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        otherMax = new Vector2(otherPosition.x + otherLength, otherPosition.y + otherHeight);
        otherMin = new Vector2(otherPosition.x - otherLength, otherPosition.y - otherHeight);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMin.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
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

        //for each corner, move it relative to the box then transform by the world matrix inverse. Finally add position back
        p1 = transform.localToWorldMatrix.inverse * (p1 - particle.position);
        p2 = transform.localToWorldMatrix.inverse * (p2 - particle.position);
        p3 = transform.localToWorldMatrix.inverse * (p3 - particle.position);
        p4 = transform.localToWorldMatrix.inverse * (p4 - particle.position);

        p1 += particle.position;
        p2 += particle.position;
        p3 += particle.position;
        p4 += particle.position;

        otherMax = new Vector2(Mathf.Max(p1.x, p2.x, p3.x, p4.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y));
        otherMin = new Vector2(Mathf.Min(p1.x, p2.x, p3.x, p4.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y));

        thisMax = new Vector2(particle.position.x + thisLength, particle.position.y + thisHeight);
        thisMin = new Vector2(particle.position.x - thisLength, particle.position.y - thisHeight);

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
