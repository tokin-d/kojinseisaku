using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bound : MonoBehaviour
{
    Vector2 velocity;

    // Update is called once per frame
    void Update()
    {
        GetComponent<Rigidbody2D>().velocity = velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityNext
            = Vector2.Reflect(velocity, collision.contacts[0].normal);
        velocity = velocityNext;
    }

}
