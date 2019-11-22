#pragma once
 
#ifndef VECTOR3_H
#define VECTOR3_H

class Vector3D
{
	float x;
	float y;
	float z;

	public:
	Vector3D(float newX, float newY, float newZ) :x(newX), y(newY), z(newZ) {}
	Vector3D(Vector3D* data);

	float getX() { return x; }
	float getY() { return y; }
	float getZ() { return z; }
	void setX(float num) { x = num; }
	void setY(float num) { y = num; }
	void setZ(float num) { z = num; }

	static float Dot(Vector3D a, Vector3D b);
	static Vector3D Project(Vector3D a, Vector3D b);

	Vector3D operator+(Vector3D* vec);
	Vector3D operator-(Vector3D* vec);
	Vector3D operator*(Vector3D* vec);
	Vector3D operator*(float scalar);
};



#endif // !VECTOR3_H
