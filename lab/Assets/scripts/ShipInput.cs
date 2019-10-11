using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipInput : MonoBehaviour
{
	particle2D mParticle;
    ShipHandler shipController;
	public float moveForce;
	public float rotForce;
    // Start is called before the first frame update
    void Start()
    {
		mParticle = GetComponent<particle2D>();
        shipController = GetComponent<ShipHandler>();
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            shipController.FireWeapon();
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
		mParticle.AddForceRot(rotForce); //pushing it right x
	}

	public void rotateLeft()
	{
		mParticle.AddForceRot(-rotForce); //pushing it left -x
	}
}
