using UnityEngine;

public abstract class CollisionHull2D : MonoBehaviour
{
    public class Collision
    {
        public struct Contact
        {
            Vector2 point;
            Vector2 normal;
            float restitution;
            float penetration;
            public void setNew(Vector2 newPoint, Vector2 newNormal, float newRest, float newPenetration)
            {
                point = newPoint;
                normal = newNormal;
                restitution = newRest;
                penetration = newPenetration;
            }

            public Vector2 GetNormal()
            {
                return normal;
            }
            public Vector2 GetPoint()
            {
                return point;
            }
            public float GetRestitution()
            {
                return restitution;
            }
        }

        public CollisionHull2D a = null, b = null;
        public Contact[] contact = new Contact[4];
        public int contactCount = 0;
        public bool status = false;

        public float closingVelocity;

        public void resolve()
        {
            //From book page 120
            resolveVelocity();
            resolveInterpenetration();
        }

        Vector2 calcSeparatingVelocity()
        {
            //From book page 120
            Vector2 relativeVelocity = a.getParticle().posVelocity;

            if (b != null)
                relativeVelocity -= b.getParticle().posVelocity;

             return relativeVelocity;    

        }

        private void resolveVelocity() //Handles impulse calculations for this collision
        {
            //Follows psuedo code from book page 120-121

            Vector2 separatingVelocity = calcSeparatingVelocity();
            //May need to change
            if(separatingVelocity.magnitude == 0)
            {
                //Contact is either separating or stationary
                //No impulse is applied
                return;
            }

            Vector2 newSepVelocicity = separatingVelocity * contact[0].GetRestitution();
            Vector2 deltaVelocity = newSepVelocicity - separatingVelocity;

            float totalInverseMass = a.getParticle().getInverseMass();
            if (b != null)
                totalInverseMass += b.getParticle().getInverseMass();

  
            if (totalInverseMass <= 0) return;

            //Impulse to apply
            Vector2 impulse = deltaVelocity / totalInverseMass;
            Debug.Log(deltaVelocity.magnitude);

            //amount of impulse per unit to apply
            Vector2 impulsePerIMass = impulse.magnitude * contact[0].GetNormal();

            //Apply impulse to colliding bodies
            a.getParticle().posVelocity += impulsePerIMass * a.getParticle().getInverseMass();

            //if (b != null)
             //   b.getParticle().posVelocity += impulsePerIMass * -b.getParticle().getInverseMass();
        }

        private void resolveInterpenetration()
        {
            
        }
    }

    public enum CollisionHullType2D
    {
        hull_circle,
        hull_aabb,
        hull_obb
    }

    protected CollisionHull2D(CollisionHullType2D type_set)
    {
        type = type_set;
    }

    private CollisionHullType2D type { get; }
    public CollisionHullType2D getTypeHull()
    {
        return type;
    }

    public int getHullInt()
    {
        return (int)type;
    }

    protected particle2D particle { get; set; }
    public particle2D getParticle()
    {
        return particle;
    }

    public void setParticle(particle2D newParticle)
    {
        particle = newParticle;
    }

    void Start()
    {
        particle = GetComponent<particle2D>();
    }

    public static bool TestCollision(CollisionHull2D a, CollisionHull2D b, ref Collision c)
    {
        if(b.type == CollisionHullType2D.hull_circle)
        {
            a.TestCollisionVsCircle((CircleHull2D)b,ref c);
        }
        else if(b.type == CollisionHullType2D.hull_aabb)
        {
            a.TestCollisionVsAABB((AxisAlignedBoundingBoxHull2D)b, ref c);
        }
        else if (b.type == CollisionHullType2D.hull_obb)
        {
            a.TestCollisionVsOBB((ObjectBoundingBoxHull2D)b, ref c);
        }
            return false;
    }

    public abstract bool TestCollisionVsCircle(CircleHull2D other, ref Collision c);

    public abstract bool TestCollisionVsAABB(AxisAlignedBoundingBoxHull2D other, ref Collision c);

    public abstract bool TestCollisionVsOBB(ObjectBoundingBoxHull2D other, ref Collision c);

    protected Vector2 rotatePoint(Vector2 point, float rotation)
    {
        //http://polymathprogrammer.com/2008/09/01/cartesian-coordinates-and-transformation-matrices/
        //[cos(0)  -sin(0)]  [x]  = xCos(0) - ySin(0)
        //[sin(0)  cos(0)]   [y] = xSin(0) + yCos(0)
        float rad = rotation * Mathf.Deg2Rad;
        float x = point.x * Mathf.Cos(rad) - (point.y * Mathf.Sin(rad));
        float y = point.x * Mathf.Sin(rad) + point.y * Mathf.Cos(rad);

        return new Vector2(x, y);
    }
}
