//represent this incredible engine
#include "ForceGenerator.h"

ForceGenerator::ForceGenerator()
{}

Vector3D ForceGenerator::generate_Gravity(float mass, float gravCoeff, Vector3D worldUp)
{
	Vector3D grav = worldUp;
	grav = grav * mass;
	grav = grav * gravCoeff;
	return grav;
}

Vector3D ForceGenerator::generate_Normal(Vector3D f_gravity, Vector3D surfaceNomal_unit)
{
	Vector3D f_Normal = f_Normal.Project(-f_gravity, surfaceNomal_unit);
}

Vector3D ForceGenerator::generate_Sliding(Vector3D f_gravity, Vector3D f_normal) 
{
	return f_gravity + f_normal;
}

Vector3D ForceGenerator::generate_Friction_Static(Vector3D f_normal, Vector3D f_opposing, float frictionCoefficient_static)
{

}

Vector3D ForceGenerator::generate_Friction_Kinetic(Vector3D f_normal, Vector3D particleVelocity, float frictionCoefficient_kinetic)
{

}

Vector3D ForceGenerator::generate_Drag(Vector3D particleVelocity, Vector3D fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient)
{

}

Vector3D ForceGenerator::generate_Spring(Vector3D particlePosition, Vector3D anchorPosition, float springRestingLength, float springStiffnessCoefficient)
{

}

Vector3D ForceGenerator::generate_Torque(Vector3D appliedForce, Vector3D centerOfMass, Vector3D pointOfForce)
{

}