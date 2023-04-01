using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundCollision : MonoBehaviour
{
    [SerializeField] GameObject trace;

    Rigidbody2D rb;

    Vector3 lastVelocity;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Wall"))
        {
            var speed = lastVelocity.magnitude;
            var direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

            rb.velocity = direction * Mathf.Max(speed, 0f);

            GameObject obj;
            obj = Instantiate(trace, transform.position + new Vector3(0f,0f,-1f), Quaternion.identity);
            Destroy(obj, 4f);
        }

        if(collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
