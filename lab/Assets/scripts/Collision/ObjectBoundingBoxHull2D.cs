using System.Collections;
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
        rotation = transform.rotation.z;
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
        //see AABB
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
        p1 = rotatePoint(new Vector2(otherLength, thisHeight), rotation) + particle.position;
        p2 = rotatePoint(new Vector2(otherLength, -thisHeight), rotation) + particle.position;
        p3 = rotatePoint(new Vector2(-otherLength, -thisHeight), rotation) + particle.position;
        p4 = rotatePoint(new Vector2(-otherLength, thisHeight), rotation) + particle.position;

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
            check2 = true;
        }
        else
        {
            check2 = false;
        }
        //check1 
        return false;
    }
}
