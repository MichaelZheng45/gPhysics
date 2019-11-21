#ifndef  FORCEGENERATOR_H
#define FORCEGENERATOR_H

#include "vector3.h"

using namespace std;

class ForceGenerator
{
	Vector3D* mthisPos;
	float mRadius;

	public:
	ForceGenerator(Vector3D* thisPos, float radius_new = 0);
};
#endif 
