using UnityEngine;

public abstract class CollisionHull3D : MonoBehaviour
{
    public class Collision3D
    {
        public struct Contact3D
        {
            Vector3 point;
            Vector3 normal;
            float restitution;
            float penetration;
            public void setNew(Vector3 newPoint, Vector3 newNormal, float newRest, float newPenetration)
            {
                point = newPoint;
                normal = newNormal;
                restitution = newRest;
                penetration = newPenetration;
            }

            public Vector3 GetNormal()
            {
                return normal;
            }
            public Vector3 GetPoint()
            {
                return point;
            }
            public float GetRestitution()
            {
                return restitution;
            }
        }

        public CollisionHull3D a = null, b = null;
        public Contact3D[] contact = new Contact3D[4];
        public int contactCount = 0;
        public bool status = false;

        public float closingVelocity;

        public void resolve()
        {
            //From book page 120
            resolveVelocity();
            resolveInterpenetration();
        }

        Vector3 calcSeparatingVelocity()
        {
            //From book page 120
            Vector3 relativeVelocity = a.getParticle().posVelocity;

            if (b != null)
                relativeVelocity -= b.getParticle().posVelocity;

             return relativeVelocity;    

        }

        private void resolveVelocity() //Handles impulse calculations for this collision
        {
            //Follows psuedo code from book page 120-121

            Vector3 separatingVelocity = calcSeparatingVelocity();
            //May need to change
            if(separatingVelocity.magnitude == 0)
            {
                //Contact is either separating or stationary
                //No impulse is applied
                return;
            }

            Vector3 newSepVelocicity = separatingVelocity * contact[0].GetRestitution();
            Vector3 deltaVelocity = newSepVelocicity - separatingVelocity;

            float totalInverseMass = a.getParticle().getInverseMass();
            if (b != null)
                totalInverseMass += b.getParticle().getInverseMass();

  
            if (totalInverseMass <= 0) return;

            //Impulse to apply
            Vector3 impulse = deltaVelocity / totalInverseMass;
            Debug.Log(deltaVelocity.magnitude);

            //amount of impulse per unit to apply
            Vector3 impulsePerIMass = impulse.magnitude * contact[0].GetNormal();

            //Apply impulse to colliding bodies
            a.getParticle().posVelocity += impulsePerIMass * a.getParticle().getInverseMass();

            //if (b != null)
             //   b.getParticle().posVelocity += impulsePerIMass * -b.getParticle().getInverseMass();
        }

        private void resolveInterpenetration()
        {
            
        }
    }

    public enum CollisionHullType3D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    protected CollisionHull3D(CollisionHullType3D type_set)
    {
        type = type_set;
    }

    private CollisionHullType3D type { get; }
    public CollisionHullType3D getTypeHull()
    {
        return type;
    }

    public int getHullInt()
    {
        return (int)type;
    }

    protected particle3D particle { get; set; }
    public particle3D getParticle()
    {
        return gameObject.GetComponent<particle3D>();
    }

    public void setParticle(particle3D newParticle)
    {
        particle = newParticle;
    }

    void Start()
    {
        particle = GetComponent<particle3D>();
    }

    public static bool TestCollision(CollisionHull3D a, CollisionHull3D b, ref Collision3D c)
    {
        if(b.type == CollisionHullType3D.hull_circle)
        {
            a.TestCollisionVsCircle((CircleHull3D)b,ref c);
        }
        else if(b.type == CollisionHullType3D.hull_aabb)
        {
            a.TestCollisionVsAABB((AxisAlignedBoundingBoxHull3D)b, ref c);
        }
        else if (b.type == CollisionHullType3D.hull_obb)
        {
            a.TestCollisionVsOBB((ObjectBoundingBoxHull3D)b, ref c);
        }
            return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull3D other, ref Collision3D c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull3D other, ref Collision3D c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull3D other, ref Collision3D c);
}
