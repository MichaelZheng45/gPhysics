#ifndef  FORCEGENERATOR_H
#define FORCEGENERATOR_H

#include "vector3.h"

using namespace std;

class ForceGenerator
{
	public:
	ForceGenerator();

	Vector3D* generate_Gravity();
	Vector3D* generate_Normal();
	Vector3D* generate_Sliding();
	Vector3D* generate_Friction_Static();
	Vector3D* generate_Frictioin_Kinetic();
	Vector3D* generate_Drag();
	Vector3D* generate_Spring();
	Vector3D* generate_Torque();
};
#endif 
