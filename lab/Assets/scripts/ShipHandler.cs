using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShipHandler : MonoBehaviour
{

	// Start is called before the first frame update
	public Slider healthbar;
	float maxHealth = 100;
	float currentHealth;

	public CollisionManager colliderManager;
    void Start()
    {
		currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(colliderManager.checkPlayerCollision())
		{
			currentHealth--;
			healthbar.value = currentHealth;
		}

		if (currentHealth <= 0)
		{

		}
    }
}
