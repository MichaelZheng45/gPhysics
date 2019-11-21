#pragma once
#ifndef MYUNITYPLUGIN_H
#define MYUNITYPLUGIN_H

#include "lib.h"



#ifdef __cplusplus
extern "C"
{


#else


#endif //___cplusplus

MYUNITYPLUGIN_SYMBOL int InitFoo(int f_new);
MYUNITYPLUGIN_SYMBOL int DoFoo(int bar);
MYUNITYPLUGIN_SYMBOL int TermFoo();


#ifdef __cplusplus
}


#else


#endif //___cplusplus



#endif // !MYUNITYPLUGIN_H
