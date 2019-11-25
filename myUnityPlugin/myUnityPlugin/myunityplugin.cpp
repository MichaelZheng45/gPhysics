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

		Vector3D data = inst->generate_Gravity(particleMass, gravityCoefficient, vWorldUp);

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

		Vector3D data = inst->generate_Normal(forceGravity, surfaceNormal);

		//convert to string


		return 0;
	}
	return "failed";
}

string generate_Sliding(float force_Gravity[], float force_Normal[])
{
	if (inst)
	{
		Vector3D forceGravity = Vector3D(force_Gravity[0], force_Gravity[1], force_Gravity[2]);
		Vector3D forceNormal = Vector3D(force_Normal[0], force_Normal[1], force_Normal[2]);

		Vector3D data = inst->generate_Sliding(forceGravity, forceNormal);
		
		return 0;
	}
	return "failed";
}

string generate_Friction_Static(float force_Normal[], float force_opposing[], float frictionCoeff)
{
	if (inst)
	{
		Vector3D forceNormal = Vector3D(force_Normal[0], force_Normal[1], force_Normal[2]);
		Vector3D forceOpposing = Vector3D(force_opposing[0], force_opposing[1], force_opposing[2]);

		Vector3D data = inst->generate_Friction_Static(forceNormal, forceOpposing, frictionCoeff);

		return 0;
	}
	return "failed";
}

string generate_Frictioin_Kinetic(float force_Normal[], float particleVel[], float frictionCoeff)
{
	if (inst)
	{
		Vector3D forceNormal = Vector3D(force_Normal[0], force_Normal[1], force_Normal[2]);
		Vector3D particleVelocity = Vector3D(particleVel[0], particleVel[1], particleVel[2]);
		
		Vector3D data = inst->generate_Friction_Kinetic(forceNormal, particleVelocity, frictionCoeff);
		return 0;
	}
	return "failed";
}

string generate_Drag(float particleVel[], float fluidVel[], float fluidDensity, float crossSection,float objectDragCoeff)
{
	if (inst)
	{
		Vector3D particleVelocity = Vector3D(particleVel[0], particleVel[1], particleVel[2]);
		Vector3D fluidVelocity = Vector3D(fluidVel[0], fluidVel[1], fluidVel[2]);

		Vector3D data = inst->generate_Drag(particleVelocity, fluidVelocity, fluidDensity, crossSection, objectDragCoeff);
		return 0;
	}
	return "failed";
}
string generate_Spring(float particlePos[], float anchorPos[], float springLength, float springCoeff)
{
	if (inst)
	{
		Vector3D particlePosition = Vector3D(particlePos[0], particlePos[1], particlePos[2]);
		Vector3D anchorPosition = Vector3D(anchorPos[0], anchorPos[1], anchorPos[2]);

		Vector3D data = inst->generate_Spring(particlePosition, anchorPosition, springLength, springCoeff);
		return 0;
	}
	return "failed";
}
string generate_Torque(float appliedForce[], float centOfMass[], float pointForce[])
{
	if (inst)
	{
		Vector3D forceApplied = Vector3D(appliedForce[0], appliedForce[1], appliedForce[2]);
		Vector3D centerOfMass = Vector3D(centOfMass[0], centOfMass[1], centOfMass[2]);
		Vector3D pointOfForce = Vector3D(pointForce[0], pointForce[1], pointForce[2]);
		Vector3D data = inst->generate_Torque(forceApplied, centerOfMass, pointOfForce);
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