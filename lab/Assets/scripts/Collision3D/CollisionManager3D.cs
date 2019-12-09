using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager3D : MonoBehaviour
{
    public static CollisionManager3D Instance { get; private set; }

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

	public Material red;
	public Material Green;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach(CollisionHull3D obj in allCollisionObj)
        {
            foreach (CollisionHull3D toCheck in allCollisionObj)
            {
                if(obj != toCheck && toCheck != null && obj != toCheck)
                    checkCollision(obj, toCheck);
            }
        }
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
			a.gameObject.GetComponent<MeshRenderer>().material = Green;
            //resolve collision
           // col.resolve();
			return true;
        }
		a.gameObject.GetComponent<MeshRenderer>().material = red;
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

    public void removeOld(GameObject gameObj)
    {
        allCollisionObj.Remove(gameObj.GetComponent<CollisionHull3D>());
    }

	public List<particle3D> getAllParticles()
	{
		List<particle3D> pList = new List<particle3D>();
		foreach (CollisionHull3D obj in allCollisionObj)
		{
			pList.Add(obj.getParticle());
		}
		return pList;
	}
}
