using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class particle3D : MonoBehaviour
{
    //ParticleMODES
    //====================================================

    [HideInInspector]
    public rotationUpdate rotationMode = rotationUpdate.ROTATION_KINEMATIC;
    [HideInInspector]
    public positionUpdate positionMode = positionUpdate.POSITION_KINEMATIC;
    [HideInInspector]
    public InertiaTypes3D i_mode = InertiaTypes3D.BOX;


    //PHYSICAL DATA
    //====================================================
    public Vector3 position, posVelocity, posAcceleration;
    public Vector3  rotVelocity, rotAcceleration;

    Quaternion4D rotation;

    [HideInInspector]
    public Vector3 size;
    Matrix4x4 worldTransformMatrix, worldTransformMatrixInv;


    [HideInInspector]
    public float elasticity;

    //lab 2 step 1
    [HideInInspector]
    public float startingMass;
    float mass, massInv;

    //Center of mass
    [HideInInspector]
    public Vector3 centerOfMassLocal = new Vector3(0, 0, 0);
    Vector3 centerOfMassWorld;

    float coeff_static = .62f;
    float coeffc_kinetic = .48f;

    //FORCE DATA
    //====================================================
    //Position Force
    Vector3 force;

    //Rotational Force
    Matrix4x4 inertia, inertiaInv;
    Vector3 torque;

    //starting force data
    //====================================================
    [HideInInspector]
    public bool initialForce = false;
    [HideInInspector]
    public float initialForceMagnitude;
    [HideInInspector]
    public Vector3 initialDir;

    [HideInInspector]
    public bool gravityOn = false;
    [HideInInspector]
    public float gravityStrength = 0;


    public Matrix4x4 getInertia()
    {
        return inertia;
    }

    public void setInertia(Matrix4x4 newInertia)
    {
        inertia = newInertia;
        inertiaInv = InertiaGenerator3D.GenerateInertiaInverse(inertia);
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

    public Matrix4x4 getTransformMatrix()
    {
        return worldTransformMatrix;
    }

    public Matrix4x4 getTransformMatrixInv()
    {
        return worldTransformMatrixInv;
    }

    public int getPositionType()
    {
        return (int)positionMode;
    }

    public int getRotationType()
    {
        return (int)rotationMode;
    }

    public int getInertiaType()
    {
        return (int)i_mode;
    }

    public int getCollisionHullType()
    {
        CollisionHull3D hull;
        if (hull = GetComponent<CollisionHull3D>())
        {
            return hull.getHullInt();
        }
        else
        {
            return -1;
        }
    }

    //force application lab 2 step 2
    public void AddForce(Vector3 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    public void AddTorque(Vector3 newTorque)
    {
        torque += newTorque;
    }

	void updateAcceleration()
    {
        //newtons 2nd law
        posAcceleration = force * massInv;
        force.Set(0.0f, 0.0f, 0.0f);

        //torque
		Matrix4x4 rotMat = MatrixFunctions.getRotationMatrix(rotation);
		Matrix4x4 rotMatInv = MatrixFunctions.getInverseRotationMatrix(rotMat);

		rotAcceleration = (rotMat * inertiaInv * rotMatInv )* torque;
        torque = Vector3.zero;
    }

    //step 2
    void updatePositionEulerExplicit(float dt)
    {
        position += posVelocity * dt;
        posVelocity += posAcceleration * dt;
    }
    void updatePositionKinematic(float dt)
    {
        position += posVelocity * dt + (1 / 2) * posAcceleration * (dt * dt);
        posVelocity += posAcceleration * dt;
    }

    void updateRotationEulerExplicit(float dt)
    {
        rotation = Quaternion4D.addScaledVector(rotation, rotVelocity, dt);
        rotVelocity += rotAcceleration * dt;
    }

    void updateRotationKinematic(float dt)
    {
        rotationMode = rotationUpdate.ROTATION_EULER_EXPLICIT;
        //rotation += rotVelocity * dt + (1 / 2) * rotAcceleration * (dt * dt);
        rotVelocity += (rotAcceleration * dt);
    }

    public void updateInertia()
    {
        Matrix4x4 newInertia = Matrix4x4.identity;
        switch (i_mode)
        {
            case InertiaTypes3D.SPHERE:
                newInertia = InertiaGenerator3D.GenerateInertia_Sphere(mass, size.x);
                break;
            case InertiaTypes3D.HALLOW_SPHERE:
                newInertia = InertiaGenerator3D.GenerateInertia_Hallow_Sphere(mass, size.x);
                break;
            case InertiaTypes3D.BOX:
                newInertia = InertiaGenerator3D.GenerateInertia_Box(mass, size.x, size.y, size.z);
                break;
            case InertiaTypes3D.HALLOW_BOX:
                newInertia = InertiaGenerator3D.GenerateInertia_Hallow_Box(mass, size.x, size.y, size.z);
                break;
            default:
                break;
        }
        setInertia(newInertia);
    }

    // Start is called before the first frame update
    void Start()
    {
        Quaternion rot = transform.rotation;
        rotation = new Quaternion4D(rot.w,rot.x,rot.y,rot.z);

        AddForce(initialDir * initialForceMagnitude);
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
        transform.rotation = new Quaternion(rotation.x,rotation.y,rotation.z,rotation.w);

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

        //update world data
        worldTransformMatrix = MatrixFunctions.getTransformMatrix(rotation, position);
        worldTransformMatrixInv = MatrixFunctions.getTransformInverseMatrix(rotation, position);
        centerOfMassWorld = centerOfMassLocal + position;

        if(gravityOn)
        {
            AddForce(ForceGenerate3D.GenerateForce_Gravity(mass, gravityStrength, Vector3.up));
        }

        //accelerationUpdate
        updateAcceleration();
    }

    public void setBase(particle3D data)
    {
        position = data.position;
        posVelocity = data.posVelocity;
        posAcceleration = data.posAcceleration;
        rotVelocity = data.rotVelocity;
        rotAcceleration = data.rotAcceleration;
        rotation = data.rotation;
        worldTransformMatrix = data.worldTransformMatrix;
        worldTransformMatrixInv = data.worldTransformMatrixInv;

        rotationMode = data.rotationMode;
        positionMode = data.positionMode;
        i_mode = data.i_mode;

        elasticity = data.elasticity;

        startingMass = data.startingMass;
        mass = data.mass;
        massInv = data.massInv;

        coeff_static = data.coeff_static;
        coeffc_kinetic = data.coeffc_kinetic;

        force = data.force;

        centerOfMassLocal = data.centerOfMassLocal;
        centerOfMassWorld = data.centerOfMassWorld;

        inertia = data.inertia;
        inertiaInv = data.inertiaInv;
        torque = data.torque;

        size = data.size;

        //forcegenstuf

        initialForce = data.initialForce;
        initialForceMagnitude = data.initialForceMagnitude;
        initialDir = data.initialDir;
        gravityOn = data.gravityOn;
        gravityStrength = data.gravityStrength;
}
}
