using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundCircle : MonoBehaviour
{
    [SerializeField] float DeletionTimer = 1f;

    [SerializeField] Transform soundPrefab;
    [SerializeField, Range(10,100)] int numPart = 10;
    [SerializeField, Range(0f,100f)] float speed = 1f;
    
    Rigidbody2D[] sounds;
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        sounds = new Rigidbody2D[numPart];
        for(int i = 0; i < sounds.Length; i++)
        {
            Transform sound = Instantiate(soundPrefab);

            sound.localPosition = new Vector3(0f, 0f, 0f);
            sound.SetParent(transform, false);

            sounds[i] = rb = sound.GetComponent<Rigidbody2D>();
            rb.velocity = new Vector2(Mathf.Sin(Mathf.PI * 2f * i / numPart) * speed, Mathf.Cos(Mathf.PI * 2f * i / numPart) * speed);

            GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("soundCircle");

            foreach (GameObject obj in otherObjects) {
                Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), sound.GetComponent<Collider2D>()); 
            }

            otherObjects = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject obj in otherObjects) {
                Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), sound.GetComponent<Collider2D>()); 
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DeletionTimer -= Time.deltaTime;
        if (DeletionTimer <= 0)
        {
            for(int i = 0; i < sounds.Length; i++)
            {
                sounds[i].velocity = new Vector2(0f, 0f);
            }
        }
    }
}
