#ifndef  FORCEGENERATOR_H
#define FORCEGENERATOR_H

#include "vector3.h"

using namespace std;

class ForceGenerator
{
	public:
	ForceGenerator();

	Vector3D* generate_Gravity(float mass, float gravCoeff, Vector3D worldUp);
	Vector3D* generate_Normal(Vector3D f_gravity, Vector3D surfaceNomal_unit);
	Vector3D* generate_Sliding(Vector3D f_gravity, Vector3D f_normal);
	Vector3D* generate_Friction_Static(Vector3D f_normal, Vector3D f_opposing, float frictionCoefficient_static);
	Vector3D* generate_Friction_Kinetic(Vector3D f_normal, Vector3D particleVelocity, float frictionCoefficient_kinetic);
	Vector3D* generate_Drag(Vector3D particleVelocity, Vector3D fluidVelocity, float fluidDensity, float objectArea_crossSection, float objectDragCoefficient);
	Vector3D* generate_Spring(Vector3D particlePosition, Vector3D anchorPosition, float springRestingLength, float springStiffnessCoefficient);
	Vector3D* generate_Torque(Vector3D appliedForce, Vector3D centerOfMass, Vector3D pointOfForce);
};
#endif 
