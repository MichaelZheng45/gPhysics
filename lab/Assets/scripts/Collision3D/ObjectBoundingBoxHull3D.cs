using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectBoundingBoxHull3D : CollisionHull3D
{

    public ObjectBoundingBoxHull3D() : base(CollisionHullType3D.hull_obb) { }

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
    }

    public override bool TestCollisionVsCircle(CircleHull3D other, ref Collision3D c)
    {
		if (other.TestCollisionVsOBB(this, ref c))
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
		if(other.TestCollisionVsOBB(this,ref c))
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
        Vector3 thisMax, thisMin, otherMax, otherMin;
		Vector3 p1, p2, p3, p4, p5, p6, p7, p8;

		Vector3 thisPos = particle.position;
		Vector3 otherPosition = other.getParticle().position;
		float thisLength = particle.size.x * .5f;
		float thisHeight = particle.size.y * .5f;
		float thisWidth = particle.size.z * .5f;
		float otherLength = other.getParticle().size.x * .5f;
		float otherHeight = other.getParticle().size.y * .5f;
		float otherWidth = other.getParticle().size.z * .5f;

		p1 = particle.getTransformMatrix() * (thisPos + new Vector3(thisLength, thisHeight, thisWidth));
		p2 = particle.getTransformMatrix() * (thisPos + new Vector3(thisLength, thisHeight, -thisWidth));
		p3 = particle.getTransformMatrix() * (thisPos + new Vector3(thisLength, -thisHeight, thisWidth));
		p4 = particle.getTransformMatrix() * (thisPos + new Vector3(thisLength, -thisHeight, -thisWidth));
		p5 = particle.getTransformMatrix() * (thisPos + new Vector3(-thisLength, thisHeight, thisWidth));
		p6 = particle.getTransformMatrix() * (thisPos + new Vector3(-thisLength, thisHeight, -thisWidth));
		p8 = particle.getTransformMatrix() * (thisPos + new Vector3(-thisLength, -thisHeight, -thisWidth));
		p7 = particle.getTransformMatrix() * (thisPos + new Vector3(-thisLength, -thisHeight, thisWidth));


		p1 = other.getParticle().getTransformMatrixInv() * ( p1 - otherPosition);
		p2 = other.getParticle().getTransformMatrixInv() * ( p2 - otherPosition);
		p3 = other.getParticle().getTransformMatrixInv() * ( p3 - otherPosition);
		p4 = other.getParticle().getTransformMatrixInv() * ( p4 - otherPosition);
		p5 = other.getParticle().getTransformMatrixInv() * ( p5 - otherPosition);
		p6 = other.getParticle().getTransformMatrixInv() * ( p6 - otherPosition);
		p7 = other.getParticle().getTransformMatrixInv() * ( p7 - otherPosition);
		p8 = other.getParticle().getTransformMatrixInv() * ( p8 - otherPosition);

		p1 += otherPosition;
		p2 += otherPosition;
		p3 += otherPosition;
		p4 += otherPosition;
		p5 += otherPosition;
		p6 += otherPosition;
		p7 += otherPosition;
		p8 += otherPosition;

		thisMax = new Vector3(Mathf.Max(p1.x, p2.x, p3.x, p4.x, p5.x, p6.x, p7.x, p8.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y, p5.y, p6.y, p7.y, p8.y), Mathf.Max(p1.z, p2.z, p3.z, p4.z, p5.z, p6.z, p7.z, p8.z));
		thisMin = new Vector3(Mathf.Min(p1.x, p2.x, p3.x, p4.x, p5.x, p6.x, p7.x, p8.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y, p5.y, p6.y, p7.y, p8.y), Mathf.Min(p1.z, p2.z, p3.z, p4.z, p5.z, p6.z, p7.z, p8.z));

		otherMax = otherPosition + new Vector3(otherLength, otherHeight, otherWidth);
		otherMin = otherPosition + new Vector3(-otherLength, -otherHeight, -otherWidth);

        if ((thisMax.x >= otherMin.x && thisMax.y >= otherMin.y) && (otherMax.x >= thisMin.x && otherMax.y >= thisMin.y))
        {
            check1 = true;
        }
        else
        {
            check1 = false;
        }

		p1 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(otherLength, otherHeight, otherWidth));
		p2 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(otherLength, otherHeight, -otherWidth));	
		p3 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(otherLength, -otherHeight, otherWidth));
		p4 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(otherLength, -otherHeight, -otherWidth));
		p5 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(-otherLength, otherHeight, otherWidth));
		p6 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(-otherLength, otherHeight, -otherWidth));
		p7 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(-otherLength, -otherHeight, otherWidth));
		p8 = other.getParticle().getTransformMatrix() * (otherPosition + new Vector3(-otherLength, -otherHeight, -otherWidth));

		p1 = particle.getTransformMatrixInv() * (p1 - thisPos);
		p2 = particle.getTransformMatrixInv() * (p2 - thisPos);
		p3 = particle.getTransformMatrixInv() * (p3 - thisPos);
		p4 = particle.getTransformMatrixInv() * (p4 - thisPos);
		p5 = particle.getTransformMatrixInv() * (p5 - thisPos);
		p6 = particle.getTransformMatrixInv() * (p6 - thisPos);
		p7 = particle.getTransformMatrixInv() * (p7 - thisPos);
		p8 = particle.getTransformMatrixInv() * (p8 - thisPos);


		p1 += thisPos;
		p2 += thisPos;
		p3 += thisPos;
		p4 += thisPos;
		p5 += thisPos;
		p6 += thisPos;
		p7 += thisPos;
		p8 += thisPos;

		thisMax = new Vector3(Mathf.Max(p1.x, p2.x, p3.x, p4.x, p5.x, p6.x, p7.x, p8.x), Mathf.Max(p1.y, p2.y, p3.y, p4.y, p5.y, p6.y, p7.y, p8.y), Mathf.Max(p1.z, p2.z, p3.z, p4.z, p5.z, p6.z, p7.z, p8.z));
		thisMin = new Vector3(Mathf.Min(p1.x, p2.x, p3.x, p4.x, p5.x, p6.x, p7.x, p8.x), Mathf.Min(p1.y, p2.y, p3.y, p4.y, p5.y, p6.y, p7.y, p8.y), Mathf.Min(p1.z, p2.z, p3.z, p4.z, p5.z, p6.z, p7.z, p8.z));

		otherMax = otherPosition + new Vector3(otherLength, otherHeight, otherWidth);
		otherMin = otherPosition + new Vector3(-otherLength, -otherHeight, -otherWidth);

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
