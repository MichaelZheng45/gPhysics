using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum rotationUpdate
{
    ROTATION_EULER_EXPLICIT,
    ROTATION_KINEMATIC
}

public enum positionUpdate
{
    POSITION_EULER_EXPLICIT,
    POSITION_KINEMATIC,
}

public enum forceMode
{
    FORCE_USER,
    FORCE_GRAVITY,
    FORCE_NORMAL,
    FORCE_SLIDING,
    FORCE_F_STATIC,
    FORCE_F_KINETIC,
    FORCE_DRAG,
    FORCE_SPRING
}
public class particle2D : MonoBehaviour
{
    //step 1
    public Vector2 position, posVelocity, posAcceleration;
    public float rotation, rotVelocity, rotAcceleration;

    [SerializeField]
    rotationUpdate rotationMode = rotationUpdate.ROTATION_KINEMATIC;
    [SerializeField]
    positionUpdate positionMode= positionUpdate.POSITION_KINEMATIC;
    [SerializeField]
    InertiaTypes i_mode = InertiaTypes.CIRCLE;

    [Range(0f, 1f)]
    public float elasticity;

    //lab 2 step 1
    public float startingMass;
    float mass, massInv;

    float coeff_static = .62f;
    float coeffc_kinetic = .48f;

    //Position Force
    Vector2 force;

    //Center of mass
    public Vector2 centOfMass = new Vector2(0.5f,0.5f);

    //Rotational Force
    float inertia, inertiaInv;
    float torque;

	//force bool activates
	public bool isGravity = false;
	public bool isDrag = false;

    public float getInertia()
    {
        return inertia;
    }

    public void setInertia(float newInertia)
    {
        inertia = newInertia;
        inertiaInv = inertia > 0.0f ? 1.0f / inertia : 0;
    }

    public void setMass(float newMass)
    {
        mass = newMass > 0.0f ? newMass : 0.0f;
        //mass = mathf.max(0,newMass);

        massInv = mass > 0.0f ? 1.0f / mass : 0;
    }

    public float getMass()
    {
        return mass;
    }

    public float getInverseMass()
    {
        return 1f / mass;
    }

    //force application lab 2 step 2
    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

	public void AddForceRot(float addRot)
	{
		Vector2 newForce = new Vector2(addRot,0);
		float nForce = ForceGenerator.GenerateForce_Torque(newForce, centOfMass, new Vector2(centOfMass.x, centOfMass.y * 2));
		AddTorque(nForce);
	}

    public void AddTorque(float newTorque)
    {
        torque += newTorque;
    }

    void updateAcceleration()
    {
        //newtons 2nd law
        posAcceleration = force * massInv;
        force.Set(0.0f, 0.0f);

        //torque
        rotAcceleration = torque * inertiaInv;
        torque = 0;
    }

    //step 2
    void updatePositionEulerExplicit(float dt)
    {
        position += posVelocity * dt ;
        posVelocity += posAcceleration * dt;
    }
    void updatePositionKinematic(float dt)
    {
        position += posVelocity * dt + (1 / 2) * posAcceleration * (dt * dt);
        posVelocity += posAcceleration * dt;
    }

    void updateRotationEulerExplicit(float dt)
    {
  
        rotation += rotVelocity * dt ;

        rotVelocity += rotAcceleration * dt;
    }

    void updateRotationKinematic(float dt)
    {
        rotation += rotVelocity * dt + (1 / 2) * rotAcceleration * (dt * dt);
        rotVelocity += (rotAcceleration * dt);
    }

    // Start is called before the first frame update
    void Start()
    {
        setMass(startingMass);

        switch (i_mode)
        {
            case InertiaTypes.RECTANGE:
                inertia = InertiaGenerator.GenerateInertia_Rectangle(mass, centOfMass.x * 2, centOfMass.y * 2);
                break;

            case InertiaTypes.CIRCLE:
                inertia = InertiaGenerator.GenerateInertia_Circle(mass, centOfMass.x);
                break;

            case InertiaTypes.RING:
                inertia = InertiaGenerator.GenerateInertia_Ring(mass, centOfMass.x, 0.4f);
                break;

            case InertiaTypes.ROD:
                inertia = InertiaGenerator.GenerateInertia_Rectangle(mass, centOfMass.x, centOfMass.y);
                break;
        }
        setInertia(inertia);

    }

	// Update is called once per frame
	void FixedUpdate()
	{
		float dt = Time.fixedDeltaTime;

		switch (rotationMode)
		{
			case rotationUpdate.ROTATION_EULER_EXPLICIT:
				updateRotationEulerExplicit(dt);
				break;
			case rotationUpdate.ROTATION_KINEMATIC:
				updateRotationKinematic(dt);
				break;
		}
		transform.rotation = Quaternion.Euler(0, 0, rotation);

		switch (positionMode)
		{
			case positionUpdate.POSITION_EULER_EXPLICIT:
				updatePositionEulerExplicit(dt);
				break;
			case positionUpdate.POSITION_KINEMATIC:
				updatePositionKinematic(dt);
				break;
		}
		transform.position = position;

		//accelerationUpdate
		updateAcceleration();

		Vector2 f_gravity = ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up);
		Vector2 f_normal = ForceGenerator.GenerateForce_normal(f_gravity, transform.up);
		Vector2 f_sliding = ForceGenerator.GenerateForce_sliding(f_gravity, f_normal);
		//Vector2 f_f_static = ForceGenerator.GenerateForce_friction_static(f_normal, p_force, coeff_static);
		Vector2 f_f_kinetic = ForceGenerator.GenerateForce_friction_kinetic(f_normal, posVelocity, coeffc_kinetic);
		Vector2 f_drag = ForceGenerator.GenerateForce_drag(posVelocity, Vector2.zero, 1.225f, 1, 5f);
		Vector2 f_spring = ForceGenerator.GenerateForce_spring(transform.position, Vector2.zero, .5f, 1.5f);

		if(isDrag)
		{
			AddForce(f_drag);
		}
	}
}
