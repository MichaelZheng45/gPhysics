using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletTimer : MonoBehaviour
{
    [SerializeField]
    float bulletTime;
    float currentTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if(currentTime > bulletTime)
        {
            CollisionManager.Instance.removeOld(GetComponent<CollisionHull2D>());
            Destroy(gameObject);
        }
    }

}
