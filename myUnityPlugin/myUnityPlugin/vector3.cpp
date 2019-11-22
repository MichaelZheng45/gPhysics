#include "vector3.h"
#include <cmath>

Vector3D::Vector3D()
{
	x = 0;
	y = 0;
	z = 0;
}

Vector3D::Vector3D(Vector3D* data)
{
	x = data->x;
	y = data->y;
	z = data->z;
}

Vector3D Vector3D::operator+(const Vector3D vec)
{
	x += vec.x;
	y += vec.y;
	z += vec.z;
	return this;
}

Vector3D Vector3D::operator-(const Vector3D vec)
{
	x -= vec.x;
	y -= vec.y;
	z -= vec.z;
	return *this;
}

Vector3D Vector3D::operator*=(const Vector3D vec)
{
	//do later
}

Vector3D* Vector3D::operator*(float scalar)
{
	x *= scalar;
	y *= scalar;
	z *= scalar;
}

Vector3D Vector3D::operator/(float scalar)
{
	x /= scalar;
	y /= scalar;
	z /= scalar;
}

Vector3D Vector3D::operator-()
{
	x = -x;
	y = -y;
	z = -z;
}

float Vector3D::Dot(Vector3D a, Vector3D b)
{
	return ((a.x * b.x) + (a.y * b.y) + (a.z * b.z));
}

Vector3D Vector3D::Cross(Vector3D vec1, Vector3D vec2)
{
	float a = (vec1.y * vec2.z) - (vec1.z * vec2.y),
		  b = (vec1.z * vec2.x) - (vec1.x * vec2.z),
		  c = (vec1.x * vec2.y) - (vec1.y * vec2.x);

	return Vector3D(a, b, c);
}

//Projects Vector b onto Vector a
Vector3D Vector3D::Project(Vector3D a, Vector3D b)
{
	return b * Dot(a, b);
}

//DOES NOT CHANGE VECTOR, JUST GIVES A NORMALIZED ONE
Vector3D Vector3D::Normalized()
{
	float mag = this->Magnitude();
	Vector3D normal = Vector3D(x / mag, y / mag, z / mag);
	return normal;
}

float Vector3D::Magnitude()
{
	float a = pow(x, 2),
		  b = pow(y, 2),
		  c = pow(z, 2);
	float highonpotenuse = sqrt(a + b + c);

	return highonpotenuse;
}