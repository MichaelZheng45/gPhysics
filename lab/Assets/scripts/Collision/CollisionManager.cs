using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public List<CollisionHull2D> allCollisionObj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CollisionHull2D obj in allCollisionObj)
        {
            obj.GetComponent<Renderer>().material = obj.red;
            foreach (CollisionHull2D toCheck in allCollisionObj)
            {
                if(obj != toCheck)
                    checkCollision(obj, toCheck);
            }
        }
    }

    void checkCollision(CollisionHull2D a, CollisionHull2D b)
    {
        CollisionHull2D.Collision col = new CollisionHull2D.Collision();
        bool check = false;
        if(b.getTypeHull() == CollisionHull2D.CollisionHullType2D.hull_circle)
        {
            check = a.TestCollisionVsCircle((CircleHull2D)b, ref col);
        }
        else if(b.getTypeHull() == CollisionHull2D.CollisionHullType2D.hull_aabb)
        {
            check = a.TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b, ref col);
        }
        else
        {
            check = a.TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref col);
        }

        if(check) 
        {
            a.GetComponent<Renderer>().material = a.green;

            //do collision resolve
        }
    }


}
