#include "myunityplugin.h"

#include "circleCheck.h"

CircleCollisionCheck* inst = 0;

int InitFoo(int f_new)
{
	if (!inst)
	{
		inst = new CircleCollisionCheck(f_new);
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