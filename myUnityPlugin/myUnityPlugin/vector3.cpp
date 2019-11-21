#include "vector3.h"

Vector3D::Vector3D(Vector3D* data)
{
	x = data->x;
	y = data->y;
	z = data->z;
}

Vector3D Vector3D::operator+(Vector3D* vec)
{
	x += vec->x;
	y += vec->y;
	z += vec->z;
	return *this;
}

Vector3D Vector3D::operator-(Vector3D* vec)
{
	x -= vec->x;
	y -= vec->y;
	z -= vec->z;
	return *this;
}

Vector3D Vector3D::operator*(Vector3D* vec)
{
	//do later
}

Vector3D Vector3D::operator*(float scalar)
{
	x *= scalar;
	y *= scalar;
	z *= scalar;
}