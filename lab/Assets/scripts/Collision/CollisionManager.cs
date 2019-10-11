using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
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

    public List<CollisionHull2D> allCollisionObj;
	public CollisionHull2D shipCollisionHull;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CollisionHull2D obj in allCollisionObj)
        {
            checkCollision(obj, shipCollisionHull); //check player collision

            foreach (CollisionHull2D toCheck in allCollisionObj)
            {
                if(obj != toCheck && toCheck != null)
                    checkCollision(obj, toCheck);
            }
        }
    }

	public bool checkPlayerCollision()
	{
		bool notCollided = false;
		foreach (CollisionHull2D toCheck in allCollisionObj)
		{
			if(checkCollision(shipCollisionHull, toCheck) && toCheck.getParticle().typeOfParticle == particleType.ASTEROID)
			{
				notCollided = true;
			}
		}
		return notCollided;
	}

    bool checkCollision(CollisionHull2D a, CollisionHull2D b)
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
            //resolve collision
            col.resolve();
			return true;
        }
		return false;
    }

    public void addNew(GameObject gameObj)
    {
        allCollisionObj.Add(gameObj.GetComponent<CollisionHull2D>());
    }

    public void removeOld(CollisionHull2D gameObj)
    {
        allCollisionObj.Remove(gameObj);
    }
}
