﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class particle3D : MonoBehaviour
{
    public particleType typeOfParticle;
    public Vector3 position, posVelocity, posAcceleration;
    public Vector3  rotVelocity, rotAcceleration;
    Quaternion4D rotation;
    Matrix4x4 worldTransformMatrix, worldTransformMatrixInv;

    [SerializeField]
    rotationUpdate rotationMode = rotationUpdate.ROTATION_KINEMATIC;
    [SerializeField]
    positionUpdate positionMode = positionUpdate.POSITION_KINEMATIC;
    [SerializeField]
    InertiaTypes3D i_mode = InertiaTypes3D.BOX;

    [Range(0f, 1f)]
    public float elasticity;

    //lab 2 step 1
    public float startingMass;
    float mass, massInv;

    float coeff_static = .62f;
    float coeffc_kinetic = .48f;

    //Position Force
    Vector3 force;

    //Center of mass
    public Vector3 centerOfMassLocal = new Vector3(0,0);
    public Vector3 centerOfMassWorld;

    //Rotational Force
    Matrix4x4 inertiaLocal, inertiaWorld, inertiaInv;
    Vector3 torque;

    //force bool activates
    public bool isGravity = false;
    public bool isDrag = false;

    public Matrix4x4 getInertia()
    {
        return inertiaLocal;
    }

    public void setInertia(Matrix4x4 newInertia)
    {
        inertiaLocal = newInertia;
       // inertiaInv = inertia > 0.0f ? 1.0f / inertia : 0;
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
        rotAcceleration = inertiaInv * torque;
        torque = Vector3.zero; ;
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
        //rotation += rotVelocity * dt + (1 / 2) * rotAcceleration * (dt * dt);
        rotVelocity += (rotAcceleration * dt);
    }

    // Start is called before the first frame update
    void Start()
    {
        Quaternion rot = transform.rotation;
        rotation = new Quaternion4D(rot.w,rot.x,rot.y,rot.z);
        setMass(startingMass);

        switch (i_mode)
        {

        }
        setInertia(inertiaLocal);

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
        worldTransformMatrix = Quaternion4D.getTransformMatrix(rotation, position);
        centerOfMassWorld = centerOfMassLocal + position;


        //accelerationUpdate
        updateAcceleration();
    }

    public void setBase(Vector3 nPosition, Quaternion4D nRotation)
    {
        rotation = nRotation;
        position = nPosition;
    }
}
