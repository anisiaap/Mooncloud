using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("soundCircle"))
        {
            // var speed = lastVelocity.magnitude;
            // var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            // rb.velocity = direction * Mathf.Max(speed, 0f);
        }
    }
}
