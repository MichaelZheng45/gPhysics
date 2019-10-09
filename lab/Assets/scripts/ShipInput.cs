﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
	particle2D mParticle;
	public float moveForce;
	public float rotForce;
    // Start is called before the first frame update
    void Start()
    {
		mParticle = GetComponent<particle2D>();
    }

	private void Update()
	{
		if (Input.GetKey(KeyCode.W))
		{
			moveUp();
		}
		if (Input.GetKey(KeyCode.D))
		{
			rotateRight();
		}
		if (Input.GetKey(KeyCode.S))
		{
			moveDown();
		}
		if (Input.GetKey(KeyCode.A))
		{
			rotateLeft();
		}
	}

	public void moveUp()
	{
		mParticle.AddForce(transform.up * moveForce);
	}

	public void moveDown()
	{
		mParticle.AddForce(-transform.up * moveForce);
	}

	public void rotateRight()
	{
		mParticle.AddForceRot(-rotForce);
	}

	public void rotateLeft()
	{
		mParticle.AddForceRot(rotForce);
	}
}
