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

public class particle2D : MonoBehaviour
{
    //step 1
    public Vector2 position, posVelocity, posAcceleration;
    public float rotation, rotVelocity, rotAcceleration;

    [SerializeField]
    rotationUpdate rotationMode = rotationUpdate.ROTATION_KINEMATIC;
    [SerializeField]
    positionUpdate positionMode= positionUpdate.POSITION_KINEMATIC;

    //lab 2 step 1
    public float startingMass;
    float mass, massInv;

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

    //force application lab 2 step 2
    Vector2 force;
    public void AddForce(Vector2 newForce)
    {
        //D'Alembert
        force += newForce;
    }

    void updateAcceleration()
    {
        //newtons 2nd law
        posAcceleration = force * massInv;
        force.Set(0.0f, 0.0f);
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
        transform.rotation = Quaternion.Euler(0,0,rotation);
        
        switch(positionMode)
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

        //lab 2 step 4
        // Vector2 f_gravity = mass * new Vector2(0.0f, -9.81f);
        //AddForce(f_gravity);
        AddForce(ForceGenerator.GenerateForce_Gravity(mass, -9.8f, Vector2.up));
    }

    public void resetData()
    {
       
        transform.position *= 0;

        position *= 0;
        posVelocity = new Vector2(1, 0);
        posAcceleration *= 0;

        rotation = 0;
        rotVelocity = 1;
        rotAcceleration = 0;

    }
}
