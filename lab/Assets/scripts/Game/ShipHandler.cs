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

    public GameObject bullet;
    float bulletForce = 200;
    float bulletSpawnOffset = 1.3f;

	public Text timer;
	float currentTime = 0;
    void Start()
    {
		currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        checkCollision();
	
        if (currentHealth <= 0)
        {
			timer.text = "You Are Dead";
        }
		else
		{
			currentTime += Time.deltaTime;
			timer.text = currentTime.ToString();
		}
    }

    void checkCollision()
    {
        if (colliderManager.checkPlayerCollision())
        {
            currentHealth--;
            healthbar.value = currentHealth;
        }
    }

    public void FireWeapon()
    {
        GameObject newBullet = Instantiate(bullet, transform.position + (transform.up * bulletSpawnOffset), transform.rotation);
        particle2D newBulletParticle = newBullet.GetComponent<particle2D>();
        CollisionHull2D colliderHull2D = newBullet.GetComponent<CollisionHull2D>();

        colliderHull2D.setParticle(newBulletParticle);
        newBulletParticle.setBase(transform.position + (transform.up * bulletSpawnOffset), transform.rotation.eulerAngles.z);
        newBulletParticle.AddForce(transform.up * bulletForce);
        colliderManager.addNew(newBullet);
    }
}
