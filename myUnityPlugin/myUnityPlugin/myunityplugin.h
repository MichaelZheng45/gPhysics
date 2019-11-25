#pragma once
#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "lib.h"
#include <string>

using namespace std;

#ifdef __cplusplus
extern "C"
{


#else


#endif //___cplusplus

MYUNITYPLUGIN_SYMBOL int InitFoo(int f_new);
MYUNITYPLUGIN_SYMBOL int DoFoo(int bar);
MYUNITYPLUGIN_SYMBOL string generate_Gravity(float particleMass, float gravityCoefficient, float worldUp[]);
MYUNITYPLUGIN_SYMBOL string generate_Normal(float force_Gravity[], float surfaceNormal_unit[]);
MYUNITYPLUGIN_SYMBOL string generate_Sliding();
MYUNITYPLUGIN_SYMBOL string generate_Friction_Static();
MYUNITYPLUGIN_SYMBOL string generate_Frictioin_Kinetic();
MYUNITYPLUGIN_SYMBOL string generate_Drag();
MYUNITYPLUGIN_SYMBOL string generate_Spring();
MYUNITYPLUGIN_SYMBOL string generate_Torque();
MYUNITYPLUGIN_SYMBOL int TermFoo();


#ifdef __cplusplus
}


#else


#endif //___cplusplus



#endif // !MYUNITYPLUGIN_H
