using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [SerializeField] GameObject enemySoundCenterPrefab;

    float InstantiationTimer = 3f;
    float stepInterval = 3f;
    bool huntMode = false;
    float huntModeTime = 1f;
    float direction_x = 0, direction_y = 0;
    float movementSpeed = 0.026f;
    float soundInterval = 30f;

    Rigidbody2D rb;

    public AudioSource enemyCharge1;
    public AudioSource enemyCharge2;
    public AudioSource enemyCharge3;
    public AudioSource enemyCharge4;
    public AudioSource enemyStandBy1;
    public AudioSource enemyStandBy2;
    public AudioSource enemyStandBy3;
    public AudioSource enemyStandBy4;
    public AudioSource enemyStandBy5;
    public AudioSource enemyStandBy6;
    public AudioSource enemyStandBy7;

    public AudioSource enemyCharge5;
    public AudioSource enemyCharge6;
    public AudioSource enemyCharge7;
    public AudioSource enemyCharge8;


    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (huntMode)
        {
            stepInterval = 0.5f;
            huntModeTime -= Time.deltaTime;
            if (huntModeTime <= 0)
            {
                huntMode = false;
            }
            transform.Translate(direction_x, direction_y, 0);
        }
        else
        {
            stepInterval = 1f;
            direction_x = Random.Range(-0.01f, 0.01f);
            direction_y = Random.Range(-0.01f, 0.01f);
            transform.Translate(direction_x, direction_y, 0);

            soundInterval -= Time.deltaTime;
            if (soundInterval <= 0)
            {
                int track = Random.Range(0,7);
                if (track == 0) enemyStandBy1.Play();
                if (track == 1) enemyStandBy2.Play();
                if (track == 2) enemyStandBy3.Play();
                if (track == 3) enemyStandBy4.Play();
                if (track == 4) enemyStandBy5.Play();
                if (track == 5) enemyStandBy6.Play();
                if (track == 6) enemyStandBy7.Play();
                soundInterval = 30f;
            }
        }

        if(direction_x != 0 || direction_y != 0)
        {
            CreateSound();
        }

        rb.velocity = new Vector2(0f, 0f);
    }

    void CreateSound()
    {
        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            GameObject obj;
            obj = Instantiate(enemySoundCenterPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 2f);

            InstantiationTimer = stepInterval;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("soundCircle"))
        {
            huntMode = true;
            huntModeTime = 1f;

            int track = Random.Range(0,8);
            if (track == 0) enemyCharge1.Play();
            if (track == 1) enemyCharge2.Play();
            if (track == 2) enemyCharge3.Play();
            if (track == 3) enemyCharge4.Play();
            if (track == 4) enemyCharge5.Play();
            if (track == 5) enemyCharge6.Play();
            if (track == 6) enemyCharge7.Play();
            if (track == 7) enemyCharge8.Play();


            GameObject[] player = GameObject.FindGameObjectsWithTag("Player");

            float num = (player[0].transform.position[0] - gameObject.transform.position[0])*(player[0].transform.position[0] - gameObject.transform.position[0]) + 
                        (player[0].transform.position[1] - gameObject.transform.position[1])*(player[0].transform.position[1] - gameObject.transform.position[1]);
            
            num = Mathf.Sqrt(num);

            direction_x = (player[0].transform.position[0] - gameObject.transform.position[0]) * movementSpeed / num;
            direction_y = (player[0].transform.position[1] - gameObject.transform.position[1]) * movementSpeed / num;
        }
    }
}
