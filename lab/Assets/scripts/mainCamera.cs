using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainCamera : MonoBehaviour
{

    [SerializeField]
    bool followPlayer;
    [SerializeField]
    Transform playerTransform;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(followPlayer)
        {
            transform.position = new Vector3(playerTransform.position.x,playerTransform.position.y, -30);
        }

    }
}
