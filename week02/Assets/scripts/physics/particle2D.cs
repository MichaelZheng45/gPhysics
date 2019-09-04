using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum rotationUpdate
{
    ROTATION_EULER_EXPLICIT,
    ROTATION_KINEMATIC
}

public enum positionUpdate
{
    POSITION_EULER_EXPLICIT,
    POSITION_KINEMATIC
}

public class particle2D : MonoBehaviour
{
    //step 1
    public Vector2 position, posVelocity, posAcceleration;
    public float rotation, rotVelocity, rotAcceleration;

    [SerializeField]
    rotationUpdate rotationMode;
    [SerializeField]
    positionUpdate positionMode;

    //step 2
    void updatePositionEulerExplicit(float dt)
    {
        //x(t + dt) = x(t) + v(t)dt
        //Euler's Method
        //F(t+dt) = F(t) + f(t)dt
        //               + (dF/dt)dt
        position += posVelocity * dt;

        //**** more to do here ****
        //v(t*dt) = v(t) + a(t)dt
        posVelocity += posAcceleration * dt;
    }
    void updatePositionKinematic(float dt)
    {
        //x(t+dt) = x(t) + v(t)dt + (1/2) * a(t) * dt^2
        position += posVelocity * dt + (1 / 2) * posAcceleration * (dt * dt);
        posVelocity += posAcceleration * dt;
    }

    void updateRotationEulerExplicit(float dt)
    {
  
        rotation += rotVelocity * dt;


        rotVelocity += rotAcceleration * dt;
    }

    void updateRotationKinematic(float dt)
    {
        rotation += rotVelocity * dt + (1 / 2) * rotAcceleration * (dt * dt);
        rotVelocity += rotAcceleration * dt;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;
        /*
        updatePositionEulerExplicit(Time.fixedDeltaTime);
        transform.position = position;

        //step 4
        posAcceleration.x = -Mathf.Sin(Time.fixedTime);

        */
        //step 3

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

        posAcceleration.x = -Mathf.Sin(Time.fixedTime);
        rotAcceleration = -Mathf.Sin(Time.fixedTime);
    }
}
