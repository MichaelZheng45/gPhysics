using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager3D : MonoBehaviour
{
    public static CollisionManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Multiple Instances of collision managers");
        }
    }

    public List<CollisionHull3D> allCollisionObj;
	public CollisionHull3D shipCollisionHull;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CollisionHull3D obj in allCollisionObj)
        {
            checkCollision(obj, shipCollisionHull); //check player collision

            foreach (CollisionHull3D toCheck in allCollisionObj)
            {
                if(obj != toCheck && toCheck != null)
                    checkCollision(obj, toCheck);
            }
        }
    }

	public bool checkPlayerCollision()
	{
		bool notCollided = false;
		foreach (CollisionHull3D toCheck in allCollisionObj)
		{
			if(checkCollision(shipCollisionHull, toCheck) && toCheck.getParticle().typeOfParticle == particleType.ASTEROID)
			{
				notCollided = true;
			}
		}
		return notCollided;
	}

    bool checkCollision(CollisionHull3D a, CollisionHull3D b)
    {
        CollisionHull3D.Collision3D col = new CollisionHull3D.Collision3D();
        if(b.getTypeHull() == CollisionHull3D.CollisionHullType3D.hull_circle)
        {
            col.status = a.TestCollisionVsCircle((CircleHull3D)b, ref col);
        }
        else if(b.getTypeHull() == CollisionHull3D.CollisionHullType3D.hull_aabb)
        {
            col.status = a.TestCollisionVsAABB((AxisAlignedBoundingBoxHull3D)b, ref col);
        }
        else
        {
            col.status = a.TestCollisionVsOBB((ObjectBoundingBoxHull3D)b, ref col);
        }

        if(col.status) 
        {
            //resolve collision
            col.resolve();
			return true;
        }
		return false;
    }

    public void addNew(GameObject gameObj)
    {
        allCollisionObj.Add(gameObj.GetComponent<CollisionHull3D>());
    }

    public void removeOld(CollisionHull3D gameObj)
    {
        allCollisionObj.Remove(gameObj);
    }
}
