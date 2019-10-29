using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceGenerate3D
{
    public static Vector3 GenerateForce_Gravity(float particleMass, float gravityCoefficient, Vector3 worldUp)
    {
        Vector3 f_gravity = particleMass * gravityCoefficient * worldUp;
        return f_gravity;
    }

    public static Vector3 GenerateForce_normal(Vector3 f_gravity, Vector3 surfaceNormal_unit)
    {
        // f_normal = proj(f_gravity, surfaceNormal_unit)
        Vector3 f_normal = Vector3.Project(-f_gravity, surfaceNormal_unit);
        return f_normal;
    }

    public static Vector3 GenerateForce_sliding(Vector3 f_gravity, Vector3 f_normal)
    {
        // f_sliding = f_gravity + f_normal
        Vector3 f_sliding = f_gravity + f_normal;
        return f_sliding;
    }

    public static Vector3 GenerateForce_friction_static(Vector3 f_normal, Vector3 f_opposing, float frictionCoefficient_static)
    {
        // f_friction_s = -f_opposing if less than max, else -coeff*f_normal (max amount is coeff*|f_normal|)
        Vector3 f_friction_s;
        float max = frictionCoefficient_static * f_normal.magnitude;
        if (f_opposing.magnitude < max)
        {
            f_friction_s = -f_opposing;
        }
        else
        {
            f_friction_s = -max * f_opposing.normalized; //direction is probably -f_oposing
        }
        return f_friction_s;
    }

    public static Vector3 GenerateForce_friction_kinetic(Vector3 f_normal, Vector3 particleVelocity, float frictionCoefficient_kinetic)
    {
        // f_friction_k = -coeff*|f_normal| * unit(vel)
        Vector3 f_friction_k = -frictionCoefficient_kinetic * f_normal.magnitude * particleVelocity.normalized;
        return f_friction_k;
    }

    public static Vector3 GenerateForce_drag(Vector3 particleVelocity, Vector3 fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
    {
        // f_drag = (p * u^2 * area * coeff)/2, p = density, u= velocity  https://www.grc.nasa.gov/WWW/k-12/airplane/drageq.html
        Vector3 relativeVelocity = (particleVelocity - fluidVelocity);
        float velocityScale = relativeVelocity.magnitude;
        Vector3 f_drag = -relativeVelocity.normalized * (fluidDensity * (velocityScale * velocityScale) * objectArea_crossSection * objectDragCoefficient) * .5f;
        return f_drag;
    }

    public static Vector3 GenerateForce_spring(Vector3 particlePosition, Vector3 anchorPosition, float springRestingLength, float springStiffnessCoefficient)
    {
        // f_spring = -coeff*(spring length - spring resting length)
        Vector3 direction = (particlePosition - anchorPosition);
        Vector3 f_spring = -springStiffnessCoefficient * (direction.magnitude - springRestingLength) * direction.normalized;
        return f_spring;
    }

    public static Vector3 GenerateForce_Torque(Vector3 appliedForce, Vector3 centerOfMass, Vector3 pointOfForce)
    {
        //t= p * F
        Vector3 f_torque = Vector3.zero;

        Vector3 momentArm = pointOfForce - centerOfMass;

        f_torque = Vector3.Cross(momentArm, appliedForce);

        return f_torque;
    }
}
