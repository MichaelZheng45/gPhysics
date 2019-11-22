#include "myunityplugin.h"

#include "forceGenerator.h"

ForceGenerator* inst = 0;

int InitFoo(int f_new)
{
	if (!inst)
	{
		inst = new ForceGenerator();
		return 1;
	}
	return 0;
}

int DoFoo(int bar)
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return 0;
}

string generate_Gravity(float particleMass, float gravityCoefficient, float worldUp[])
{
	if (inst)
	{
		Vector3D vWorldUp = Vector3D(worldUp[0], worldUp[1], worldUp[2]);

		Vector3D data = inst->generate_Gravity();

		//convert to string



		return 0;
	}
	return "failed";
}

string generate_Normal(float force_Gravity[], float surfaceNormal_unit[])
{
	if (inst)
	{
		Vector3D forceGravity = Vector3D(force_Gravity[0], force_Gravity[1], force_Gravity[2]);
		Vector3D surfaceNormal = Vector3D(surfaceNormal_unit[0], surfaceNormal_unit[1], surfaceNormal_unit[2]);
		forceGravity = forceGravity * 5;

		Vector3D data = inst->generate_Normal();

		//convert to string


		return 0;
	}
	return "failed";
}

string generate_Sliding()
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return "failed";
}

string generate_Friction_Static()
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return "failed";
}

string generate_Frictioin_Kinetic()
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return "failed";
}

string generate_Drag()
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return "failed";
}
string generate_Spring()
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return "failed";
}
string generate_Torque()
{
	if (inst)
	{
		//int result = inst->CheckVsCircle(bar);
		return 0;
	}
	return "failed";
}

int TermFoo()
{
	if (inst)
	{
		delete inst;
		inst = 0;
		return 1;
	}
	return 1;
}