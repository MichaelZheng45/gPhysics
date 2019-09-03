using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleCollisionResponder : MonoBehaviour
{
    // Start is called before the first frame update

    Rigidbody rb;
    Vector2 tVelocity;

    [SerializeField]
    float bounciness;
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        tVelocity = rb.velocity;
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        rb.velocity *= 0;

        rb.AddForce((collision.transform.up).normalized * (tVelocity.magnitude * bounciness));
    }
}
