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

string generate_Gravity()
{

}

string generate_Normal()
{

}

string generate_Sliding()
{

}

string generate_Friction_Static()
{

}

string generate_Frictioin_Kinetic()
{

}

string generate_Drag()
{

}
string generate_Spring()
{

}
string generate_Torque()
{

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