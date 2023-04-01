using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2;

    float InstantiationTimer = 1f;
    float stepInterval = 1f;
    bool IsSneaking = false;

    [SerializeField] GameObject soundCenterPrefab;
    [SerializeField] GameObject clapSoundCenterPrefab;
    [SerializeField] GameObject sneakSoundCenterPrefab;
    [SerializeField] GameObject playerTrace;


    public AudioSource clap;
    public AudioSource concrete_step1;
    public AudioSource concrete_step2;
    public AudioSource concrete_step3;
    

    void Start()
    {
       clap = GetComponent<AudioSource>();
       concrete_step1 = GetComponent<AudioSource>();
       concrete_step2 = GetComponent<AudioSource>();
       concrete_step3 = GetComponent<AudioSource>();
    
    }


    // Update is called once per frame
    void Update()
    {
        float direction_x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float direction_y = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;;
        transform.Translate(direction_x, direction_y, 0);

        if(direction_x != 0 || direction_y != 0)
        {
            CreateSound();
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            clap.Play();
            GameObject obj = Instantiate(clapSoundCenterPrefab, transform.position, Quaternion.identity);
            Destroy(obj, 3f);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = 1;
            IsSneaking = true;
            stepInterval = 0.5f;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = 2;
            IsSneaking = false;
            stepInterval = 1f;
        }
    }

    void CreateSound()
    {
        InstantiationTimer -= Time.deltaTime;
        if (InstantiationTimer <= 0)
        {
            GameObject obj;
            if(IsSneaking)
            {
                obj = Instantiate(sneakSoundCenterPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                obj = Instantiate(soundCenterPrefab, transform.position, Quaternion.identity);
            }
            InstantiationTimer = stepInterval;
            Destroy(obj, 2f);

            obj = Instantiate(playerTrace, transform.position + new Vector3(0f,0f,-1f), Quaternion.identity);
            Destroy(obj, 3f);
        }
    }
}
