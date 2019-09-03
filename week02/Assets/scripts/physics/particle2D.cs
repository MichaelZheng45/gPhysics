using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particle2D : MonoBehaviour
{
    //step 1
    public Vector2 position, velocity, acceleration;

    //step 2
    void updatePositionEulerExplicit(float dt)
    {
        //x(t + dt) = x(t) + v(t)dt
        //Euler's Method
        //F(t+dt) = F(t) + f(t)dt
        //               + (dF/dt)dt
        position += velocity * dt;

        //**** more to do here ****
        //v(t*dt) = v(t) + a(t)dt
        velocity += acceleration * dt;
    }
    void updatePositionKinematic(float dt)
    {

    }

    void updateRotationEulerExplicit(float dt)
    {

    }

    void updateRotationKinematic(float dt)
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        //step 3
        updatePositionEulerExplicit(Time.fixedDeltaTime);
        transform.position = position;

        //step 4
        acceleration.x = -Mathf.Sin(Time.fixedTime);
    }
}
