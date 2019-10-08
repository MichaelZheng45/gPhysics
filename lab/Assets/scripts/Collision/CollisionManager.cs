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
        if(b.getTypeHull() == CollisionHull2D.CollisionHullType2D.hull_circle)
        {
            col.status = a.TestCollisionVsCircle((CircleHull2D)b, ref col);
        }
        else if(b.getTypeHull() == CollisionHull2D.CollisionHullType2D.hull_aabb)
        {
            col.status = a.TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b, ref col);
        }
        else
        {
            col.status = a.TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref col);
        }

        if(col.status) 
        {
            a.GetComponent<Renderer>().material = a.green;
            //resolve collision
            col.resolve();
        }
    }
}
